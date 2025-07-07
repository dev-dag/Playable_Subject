using UnityEngine;

/// <summary>
/// 게임의 흐름을 제어하는 클래스이자, 모든 코드의 시작점.
/// </summary>
public class GameFlowControl : SingleTon<GameFlowControl>
{
    public GameSystemControl GameSystemControl { get => gameSystemControl; }

    [SerializeField] private EndCardControl endCardControl;
    [SerializeField] private GameSystemControl gameSystemControl;

    private bool isNeedTutorial = true;

    protected override void Awake()
    {
        base.Awake();

        InitalizeGame();
    }

    /// <summary>
    /// 게임을 초기화하는 함수
    /// </summary>
    public void InitalizeGame()
    {
        GameSystemControl.InitialzieAndStart();
    }

    /// <summary>
    /// 게임을 일시 정지하는 함수
    /// </summary>
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
