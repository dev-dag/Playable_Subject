using System.Collections.Generic;
using UnityEngine;

public class GameSystemControl : MonoBehaviour
{
    public ColorMaterialScriptableObject colorMaterialTable;

    [Space(30f)]
    [LunaPlaygroundField(fieldTitle: "Time Limit")]
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private InputControl inputControl;

    [SerializeField] private List<ContainerLine> containerLines;

    private Dictionary <int, ContainerLine> containerLineDictionary = new Dictionary<int, ContainerLine>();
    private float endTime = 0.0f;
    private GameState state = GameState.Wait;

    public void InitialzieAndStart()
    {
        containerLineDictionary.Clear();
        endTime = Time.time + timeLimit;
        state = GameState.Playing;

        // 컨테이너 초기화
        MakeContainer();


        // 입력 모듈 초기화
        if (inputControl.IsInit == false)
        {
            inputControl.Init(this);
        }

        inputControl.Enable();
    }

    /// <summary>
    /// 컨테이너를 생성하는 함수
    /// </summary>
    private void MakeContainer()
    {
        int containerLineIndex = 0;

        List<ObjectColor> colorTable = new List<ObjectColor>()
        {
            ObjectColor.Red,
            ObjectColor.Green,
            ObjectColor.Blue,
            ObjectColor.Yellow,
            ObjectColor.Mint,
            ObjectColor.Pink,
            ObjectColor.Orange
        };

        foreach (var containerLine in containerLines)
        {
            var randomColorIndex = Random.Range(0, colorTable.Count);
            var color = colorTable[randomColorIndex];

            containerLine.Init(containerLineIndex, color);
            containerLineDictionary.Add(containerLineIndex, containerLine);

            colorTable.RemoveAt(randomColorIndex); // 사용한 색상을 랜덤 테이블에서 제거
        }
    }
}
