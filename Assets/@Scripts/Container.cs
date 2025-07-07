using UnityEngine;

/// <summary>
/// 아이템을 넣어둘 수 있는 컨테이너.
/// </summary>
public class Container : MonoBehaviour, IContainer
{
    public ObjectColor Color { get; private set; }
    public int LineIndex { get; private set; } = -1;
    public int ContainerIndex { get; private set; } = -1;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Vector3 itemOffset;

    private Item item;

    public void Reset()
    {
        Color = ObjectColor.None;
        LineIndex = -1;
        ContainerIndex = -1;
        item?.Return(); // 아이템을 풀에 반환
        item = null;
    }

    /// <summary>
    /// 초기화 함수.
    /// </summary>
    /// <param name="material">컨테이너를 렌더링하기 위해 사용할 머터리얼</param>
    /// <param name="lineIndex">ContainerLine의 인덱스</param>
    /// <param name="containerIndex">ContainerLine에서 이 컨테이너의 인덱스</param>
    public void Init(ColorMaterial colorMaterial, int lineIndex, int containerIndex)
    {
        LineIndex = lineIndex;
        ContainerIndex = containerIndex;

        meshRenderer.material = colorMaterial.material;
        Color = colorMaterial.color;
    }

    /// <summary>
    /// 컨테이너에 아이템을 넣는 함수
    /// </summary>
    public void SetItem(Item newItem)
    {
        item = newItem;

        if (item != null)
        {
            item.SetPosition(transform.position + itemOffset); // 아이템의 위치를 컨테이너 위치로 설정

#if UNITY_EDITOR
            item.container = this; // 디버그용
#endif
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetLineIndex()
    {
        return LineIndex;
    }
}