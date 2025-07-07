using UnityEngine;

/// <summary>
/// 정답 색상이 정해진, 아이템을 넣어둘 수 있는 컨테이너.
/// </summary>
public class FixedContainer : Container
{
    public ObjectColor Color { get; private set; }
    public int LineIndex { get; private set; } = -1;
    public int ContainerIndex { get; private set; } = -1;

    [SerializeField] private MeshRenderer meshRenderer;

    public void Reset()
    {
        Color = ObjectColor.None;
        LineIndex = -1;
        ContainerIndex = -1;
        Item?.Return(); // 아이템을 풀에 반환
        Item = null;
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

    public override int GetLineIndex()
    {
        return LineIndex;
    }
}