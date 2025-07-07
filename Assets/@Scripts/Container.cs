using UnityEngine;

/// <summary>
/// 아이템을 넣어둘 수 있는 컨테이너.
/// </summary>
public class Container : MonoBehaviour
{
    public int LineIndex { get; private set; }
    public int ContainerIndex { get; private set; }

    [SerializeField] private MeshRenderer meshRenderer;

    private Item item;
    private ObjectColor color;

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
        color = colorMaterial.color;
    }

    /// <summary>
    /// 컨테이너에 아이템을 넣는 함수
    /// </summary>
    /// <param name="newItem">넣을 아이템</param>
    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public void Reset()
    {
        item = null;
        color = ObjectColor.None;
    }
}