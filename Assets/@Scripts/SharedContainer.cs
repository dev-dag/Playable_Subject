using UnityEngine;

/// <summary>
/// 피라미드 맨 윗줄에 있는 완성되지 않는 여분의 컨테이너를 제어하는 클래스
/// </summary>
public class SharedContainer : MonoBehaviour
{
    [SerializeField] private Vector3 itemOffset;

    private Item item;

    /// <summary>
    /// 컨테이너에 아이템을 넣는 함수
    /// </summary>
    public void SetItem(Item newItem)
    {
        item = newItem;
        item.SetPosition(transform.position + itemOffset); // 아이템의 위치를 컨테이너 위치로 설정
    }
}