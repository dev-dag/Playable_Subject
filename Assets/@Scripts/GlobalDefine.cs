using UnityEngine;

/// <summary>
/// 레이어 이름을 정의하는 구조체
/// </summary>
public struct LayerNameDefine
{
    public static readonly string CONTAINER_LAYER_NAME = "Container";
    public static readonly string HOLDING_CONTAINER_LAYER_NAME = "HoldingContainer";
}

/// <summary>
/// 컬러 - 머터리얼 매핑을 위한 스트럭쳐
/// </summary>
[System.Serializable]
public struct ColorMaterial
{
    public ObjectColor color;
    public Material material;
}

/// <summary>
/// 게임에 존재하는 오브젝트 머터리얼의 색상
/// </summary>
public enum ObjectColor
{
    None,
    Red,
    Green,
    Blue,
    Pink,
    Orange,
    Mint
}

/// <summary>
/// 게임의 상태
/// </summary>
public enum GameState
{
    Wait,
    Playing,
    Pause,
    Fail,
    Clear,
}