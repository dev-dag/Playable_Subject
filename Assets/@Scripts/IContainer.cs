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

    /// <summary>
    /// 소속된 라인의 인덱스를 반환하는 함수
    /// </summary>
    /// <returns>GameSystemControl에 의해 할당된 라인 인덱스 값</returns>
    public int GetLineIndex();

    /// <summary>
    /// 게임 오브젝트를 반환하는 함수
    /// </summary>
    public GameObject GetGameObject();
}