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

    [LunaPlaygroundField(fieldTitle: "Restartable Count")]
    public int restartableCount = 3; // 재시작 가능한 횟수
    [LunaPlaygroundField(fieldTitle: "Try Count")]
    public int playCount = 0;

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
        playCount++;
        GameSystemControl.InitialzieAndStart();
    }

    /// <summary>
    /// 게임을 실패 시키는 함수
    /// </summary>
    public void FailGame()
    {
        if (playCount <= restartableCount)
        {
            endCardControl.ShowEndCard(true);
        }
        else
        {
            endCardControl.ShowEndCard(false);
        }
    }

    /// <summary>
    /// 게임을 성공 시키는 함수
    /// </summary>
    public void ClearGame()
    {
        if (playCount <= restartableCount)
        {
            endCardControl.ShowEndCard(true);
        }
        else
        {
            endCardControl.ShowEndCard(false);
        }
    }
}
