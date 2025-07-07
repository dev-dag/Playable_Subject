using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    public int ID { get; private set; }
    public ObjectColor color { get; private set; }

    /// <summary>
    /// 초기화 함수.
    /// </summary>
    public void Init(int id, int containerID, ObjectColor color)
    {
        ID = id;
        this.color = color;
    }

    /// <summary>
    /// 위치를 설정하는 함수
    /// </summary>
    public void SetPosition(Vector3 position)
    {
        transform.position = position + offset;
    }
}