using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 컨테이너의 집합을 제어하는 클래스
/// </summary>
public class ContainerLine : MonoBehaviour
{
    public bool IsCleared { get; private set; } = false; // 라인이 클리어 되었는지 여부
    public int LineIndex { get; private set; } = -1;
    public ObjectColor Color { get; private set; }
    public Dictionary<int, FixedContainer> containerDictioanry = new Dictionary<int, FixedContainer>();

    [SerializeField] private List<FixedContainer> containers;

    public float time1 = 0.5f;
    public Ease ease1 = Ease.Linear;
    public float time2 = 0.25f;
    public Ease ease2 = Ease.Linear;
    public float time3 = 0.1f;
    public Ease ease3 = Ease.Linear;

    private bool isPlayingAnimation = false;

    public void Reset()
    {
        foreach (var container in containers)
        {
            container.Reset();
        }

        IsCleared = false;
    }

    private void Update()
    {
        if (isPlayingAnimation) // 애니메이션 중에 아이템이 따라오도록 설정
        {
            foreach (var container in containers)
            {
                if (container.Item != null)
                {
                    container.Item.SetPosition(container.transform.position + container.ItemOffset); // 아이템의 위치를 컨테이너 위치로 설정
                }
            }
        }
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(int lineIndex, ObjectColor color)
    {
        Reset();

        this.gameObject.SetActive(true); // 컨테이너 라인을 활성화

        containerDictioanry.Clear();

        LineIndex = lineIndex;
        Color = color;

        var yDelta = GameFlowControl.Instance.GameSystemControl.ContainerYDelta;
        transform.localPosition = new Vector3(0f, yDelta * lineIndex, 0f);

        int index = 0;

        foreach (var container in containers)
        {
            var colorMaterialPair = GameFlowControl.Instance.GameSystemControl.colorMaterialTable.GetColorMaterial(color);

            if (colorMaterialPair != null)
            {
                container.Init(colorMaterialPair.Value, LineIndex, index); // 컨테이너에 인덱스 및 컬러 할당
                containerDictioanry.Add(index, container); // 컨테이너를 딕셔너리에 추가
                index++;
            }
            else
            {
                Debug.LogError($"컬러 머터리얼 참조 실패", this);
            }
        }
    }

    /// <summary>
    /// 아래쪽 라인이 클리어 되었을 때, 라인 전체를 애니메이션 하는
    /// </summary>
    public void Fall()
    {
        var yDelta = GameFlowControl.Instance.GameSystemControl.ContainerYDelta;

        isPlayingAnimation = true;

        transform.DOMoveY(transform.position.y - yDelta, time1).SetEase(ease1).OnComplete(() =>
        {
            yDelta /= 2f;

            transform.DOMoveY(transform.position.y + yDelta, time2).SetEase(ease2).OnComplete(() =>
            {
                transform.DOMoveY(transform.position.y - yDelta, time3).SetEase(ease3).OnComplete(() =>
                {
                    isPlayingAnimation = false;

                    foreach (var container in containers)
                    {
                        if (container.Item != null)
                        {
                            container.Item.SetPosition(container.transform.position + container.ItemOffset); // 아이템의 위치를 컨테이너 위치로 설정
                        }
                    }
                });
            });
        });
    }

    /// <summary>
    /// 라인을 성공 처리하는 함수
    /// </summary>
    public void Clear()
    {
        foreach (var container in containers)
        {
            container.Item?.Return(); // 컨테이너에 있는 아이템을 풀에 반환
            container.Reset(); // 컨테이너 초기화
        }

        IsCleared = true;
        gameObject.SetActive(false); // 라인 전체 비활성화
    }
}