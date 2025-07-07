using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 컨테이너의 집합을 제어하는 클래스
/// </summary>
public class ContainerLine : MonoBehaviour
{
    public int LineIndex { get; private set; } = -1;
    public ObjectColor Color { get; private set; }
    public Dictionary<int, Container> containerDictioanry = new Dictionary<int, Container>();

    [SerializeField] private List<Container> containers;

    public void Reset()
    {
        foreach (var container in containers)
        {
            container.Reset();
        }
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(int lineIndex, ObjectColor color)
    {
        this.gameObject.SetActive(true); // 컨테이너 라인을 활성화

        containerDictioanry.Clear();

        LineIndex = lineIndex;
        Color = color;

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
}