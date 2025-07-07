using UnityEngine;

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
    Yellow,
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