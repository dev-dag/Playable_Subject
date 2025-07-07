using System;
using System.Collections.Generic;
using UnityEngine;

public class ContainerLine : MonoBehaviour
{
    public int LineIndex { get; private set; }
    public ObjectColor Color { get; private set; }

    [SerializeField] private List<Container> containers;
    private Dictionary<int, Container> containerDictioanry = new Dictionary<int, Container>();

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(int lineIndex, ObjectColor color)
    {
        containerDictioanry.Clear();

        LineIndex = lineIndex;
        Color = color;

        int index = 0;

        foreach (var container in containers)
        {
            container.Init(GameFlowControl.Instance.GameSystemControl.colorMaterialTable.GetColorMaterial(color).Value, LineIndex, index); // 컨테이너에 인덱스 및 컬러 할당
            containerDictioanry.Add(index, container); // 컨테이너를 딕셔너리에 추가
            index++;
        }
    }
}