using UnityEngine;

/// <summary>
/// 컨테이너에 들어갈 아이템 클래스
/// </summary>
public class Item : PoolingObject
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Vector3 offset;

#if UNITY_EDITOR
    public Container container;
#endif

    public int ID { get; private set; } = -1;
    public ObjectColor color { get; private set; }

    /// <summary>
    /// 초기화 함수.
    /// </summary>
    public void Init(int id, ObjectColor color)
    {
        ID = id;

        var colorMaterialPair = GameFlowControl.Instance.GameSystemControl.colorMaterialTable.GetColorMaterial(color);
        if (colorMaterialPair != null)
        {
            meshRenderer.material = colorMaterialPair.Value.material;
            this.color = color;
        }
    }

    /// <summary>
    /// 위치를 설정하는 함수
    /// </summary>
    public void SetPosition(Vector3 position)
    {
        transform.position = position + offset;
    }

    public override void OnReturn()
    {
        base.OnReturn();

        color = ObjectColor.None; // 아이템 색상 초기화
        ID = -1; // 아이템 ID 초기화

#if UNITY_EDITOR
        container = null;
#endif
    }
}