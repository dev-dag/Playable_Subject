using UnityEngine;

public interface IContainer
{
    /// <summary>
    /// 아이템을 보관하는 함수
    /// </summary>
    /// <param name="newItem"></param>
    public void SetItem(Item newItem);

    /// <summary>
    /// 보관중인 아이템을 반환하는 함수
    /// </summary>
    public Item GetItem();

    public GameObject GetGameObject();
}