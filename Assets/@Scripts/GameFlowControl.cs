using UnityEngine;

/// <summary>
/// 게임의 흐름을 제어하는 클래스
/// </summary>
public class GameFlowControl : SingleTon<GameFlowControl>
{
    public GameSystemControl GameSystemControl { get => gameSystemControl; }

    [SerializeField] private EndCardControl endCardControl;
    [SerializeField] private GameSystemControl gameSystemControl;

    private bool isNeedTutorial = true;

    public void InitalizeGame()
    {

    }

    public void PauseGame()
    {

    }

    /// <summary>
    /// 게임을 실패 시키는 함수
    /// </summary>
    public void FailGame()
    {

    }

    /// <summary>
    /// 게임을 성공 시키는 함수
    /// </summary>
    public void ClearGame()
    {

    }
}
