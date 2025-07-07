using DG.Tweening;
using UnityEngine;

/// <summary>
/// 피라미드 맨 윗줄에 있는 완성되지 않는 여분의 컨테이너를 제어하는 클래스
/// </summary>
public class SharedContainer : Container
{
    public float time1 = 0.5f;
    public Ease ease1 = Ease.Linear;
    public float time2 = 0.25f;
    public Ease ease2 = Ease.Linear;
    public float time3 = 0.1f;
    public Ease ease3 = Ease.Linear;

    private bool isPlayingAnimation = false;

    public void Reset()
    {
        isPlayingAnimation = false;
        Item?.Return();
        Item = null;
    }

    private void Update()
    {
        if (isPlayingAnimation) // 애니메이션 중에 아이템이 따라오도록 설정
        {
            Item.SetPosition(transform.position + ItemOffset); // 아이템의 위치를 컨테이너 위치로 설정
        }
    }

    /// <summary>
    /// 아래쪽 라인이 클리어 되었을 때, 라인 전체를 애니메이션 하는
    /// </summary>
    public void Fall()
    {
        float yDelta = GameFlowControl.Instance.GameSystemControl.ContainerYDelta;

        isPlayingAnimation = true;

        transform.DOMoveY(transform.position.y - yDelta, time1).SetEase(ease1).OnComplete(() =>
        {
            yDelta /= 2f;

            transform.DOMoveY(transform.position.y + yDelta, time2).SetEase(ease2).OnComplete(() =>
            {
                transform.DOMoveY(transform.position.y - yDelta, time3).SetEase(ease3).OnComplete(() =>
                {
                    isPlayingAnimation = false;
                });
            });
        });
    }

    public override int GetLineIndex()
    {
        return -1; // SharedContainer는 라인에 속하지 않으므로 -1 반환
    }
}