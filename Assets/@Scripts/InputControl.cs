using UnityEngine;

/// <summary>
/// 입력을 처리하는 클래스
/// </summary>
public class InputControl : MonoBehaviour
{
    public bool IsInit { get; private set; } = false;
    public bool IsActive { get; private set; } = false;

    private GameSystemControl gameSystemControl;

    private void Update()
    {
        if (IsActive == false)
        {
            return;
        }

        // 입력 감지
    }

    /// <summary>
    /// 모듈을 초기화하는 함수
    /// </summary>
    public void Init(GameSystemControl gameSystemControl)
    {
        this.gameSystemControl = gameSystemControl;
        IsInit = true;
    }

    /// <summary>
    /// 입력 모듈을 활성화하는 함수
    /// </summary>
    public void Enable()
    {
        IsActive = true;
    }

    /// <summary>
    /// 입력 모듈을 비활성화하는 함수
    /// </summary>
    public void Disable()
    {
        IsActive = false;
    }
}