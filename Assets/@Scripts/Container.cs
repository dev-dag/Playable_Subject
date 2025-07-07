using UnityEngine;

public abstract class Container : MonoBehaviour
{
    public Item Item { get; protected set; }
    public Vector3 ItemOffset { get => itemOffset; }

    [SerializeField] private Vector3 itemOffset;

    /// <summary>
    /// 아이템을 보관하는 함수
    /// </summary>
    /// <param name="newItem"></param>
    public virtual void SetItem(Item newItem)
    {
        Item = newItem;

        if (Item != null)
        {
            Item.SetPosition(transform.position + itemOffset); // 아이템의 위치를 컨테이너 위치로 설정

#if UNITY_EDITOR
            Item.container = this; // 디버그용
#endif
        }
    }

    /// <summary>
    /// 소속된 라인의 인덱스를 반환하는 함수
    /// </summary>
    /// <returns>GameSystemControl에 의해 할당된 라인 인덱스 값</returns>
    public abstract int GetLineIndex();
}