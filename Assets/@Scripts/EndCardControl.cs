using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// End Card를 제어하는 클래스
/// </summary>
public class EndCardControl : MonoBehaviour
{
    [SerializeField] private TMP_Text playNowTmp;
    [SerializeField] private TMP_Text restartTmp;
    [SerializeField] private GameObject restartButtonObject;

    [LunaPlaygroundField(fieldTitle:"Play Now Button's Text")]
    public string playNowText;
    [LunaPlaygroundField(fieldTitle: "Restart Button's Text")]
    public string restartText;
    [LunaPlaygroundField(fieldTitle: "Download Page Link")]
    public string downloadPageUrl;

    /// <summary>
    /// 엔드카드를 노출시키는 함수
    /// </summary>
    /// <param name="canRestart">true일 때, 다운로드 버튼과 함께 다시 시작도 제공함.</param>
    public void ShowEndCard(bool canRestart)
    {
        this.gameObject.SetActive(true);

        playNowTmp.text = playNowText;
        restartTmp.text = restartText;

        restartButtonObject.SetActive(canRestart);
    }

    /// <summary>
    /// 게임을 재시작하는 함수 - 버튼에 이벤트로 연결됨.
    /// </summary>
    public void RestartButtonClickEventHandler()
    {
        this.gameObject.SetActive(false);

        GameFlowControl.Instance.InitalizeGame();
    }

    /// <summary>
    /// 다운로드 페이지를 로드하는 함수 - 버튼에 이벤트로 연결됨.
    /// </summary>
    public void DownloadButtonClickEventHandler()
    {
        Application.OpenURL(downloadPageUrl);
    }
}