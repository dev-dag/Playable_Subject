using UnityEngine;

/// <summary>
/// 컨테이너에 들어갈 아이템 클래스. 아이템은 하이어러키 상에서 컨테이너의 자식으로 설정되지 않음. (최적화를 위함)
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