using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 게임 시스템의 로직을 제어하는 클래스
/// </summary>
public class GameSystemControl : MonoBehaviour
{
    public float ContainerYDelta { get => containerYDelta; }

    public ColorMaterialScriptableObject colorMaterialTable;

    [Space(30f)]
    [LunaPlaygroundField(fieldTitle: "Time Limit")]
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private InputControl inputControl;

    [Space(30f)]
    [SerializeField] private MyObjectPool.ObjectPool itemPool;
    [SerializeField] private List<ContainerLine> containerLines;
    [SerializeField] private SharedContainer sharedContainer;
    [SerializeField] private float containerYDelta = 1.02f; // 컨테이너 Y축 크기

    [Space(30f)]
    [SerializeField] private ParticleSystem lineClearParticle;

    [Space(30f)]
    [SerializeField] private TMP_Text timerTmp;

    private Dictionary <int, ContainerLine> containerLineDictionary = new Dictionary<int, ContainerLine>();
    private float endTime = 0.0f;
    private GameState gameState = GameState.Wait;

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Restart"))
        {
            InitialzieAndStart();
        }

        if (GUI.Button(new Rect(10, 50, 100, 30), "Clear"))
        {
            GameFlowControl.Instance.ClearGame();
        }

        if (GUI.Button(new Rect(10, 90, 100, 30), "Fail"))
        {
            GameFlowControl.Instance.FailGame();
        }
    }
#endif

    private void Reset()
    {
        foreach (var containerLine in containerLines)
        {
            containerLine.Reset(); // 컨테이너 라인 초기화
        }

        sharedContainer.Reset();

        containerLineDictionary.Clear();
        endTime = 0f;
        inputControl.Disable();
    }

    private void Update()
    {
        if (gameState is GameState.Fail or GameState.Clear)
        {
            return;
        }
        else if (Time.time >= endTime)
        {
            gameState = GameState.Fail;
            timerTmp.text = "0s"; // 타이머가 끝나면 0으로 표시
            inputControl.Disable();
            GameFlowControl.Instance.FailGame();
        }
        else
        {
            timerTmp.text = $"{(endTime - Time.time).ToString("F2")}s"; // 남은 시간 설정
        }
    }

    /// <summary>
    /// 게임을 초기화하고 시작하는 함수
    /// </summary>
    public void InitialzieAndStart()
    {
        Reset();

        gameState = GameState.Playing;

        containerLineDictionary.Clear();
        endTime = Time.time + timeLimit;
        timerTmp.text = $"{timeLimit}s"; // 타이머 초기화

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
    /// 아이템을 특정 컨테이너로 이동시키는 함수
    /// </summary>
    /// <param name="from">원래 아이템이 있던 컨테이너</param>
    /// <param name="to">이동시킬 컨테이너</param>
    /// <returns>to에 아이템이 있으면 false반환.</returns>
    public bool MoveItem(Container from, Container to)
    {
        if (to.Item != null)
        {
            from.SetItem(from.Item);
            return false;
        }
        else if (from.Item == null)
        {
            return false;
        }

        to.SetItem(from.Item); // to에 아이템 설정
        from.SetItem(null);

        int lineIndex = to.GetLineIndex();

        if (CheckLine(lineIndex))
        {
            lineClearParticle.transform.position = containerLineDictionary[lineIndex].transform.position - Vector3.forward * 1f;
            lineClearParticle.Play();

            containerLineDictionary[lineIndex].Clear(); // 라인 클리어 처리

            for (int fallLineIndex = lineIndex + 1; fallLineIndex < containerLineDictionary.Count; fallLineIndex++) // 더 위에 있는 라인 추락 처리
            {
                containerLineDictionary[fallLineIndex].Fall();
            }

            sharedContainer.Fall(); // 공유 컨테이너도 추락 처리

            bool isEveryContainerCleared = true;

            foreach (var containerLine in containerLines)
            {
                if (containerLine.IsCleared == false)
                {
                    isEveryContainerCleared = false;
                    break;
                }
            }

            if (isEveryContainerCleared)
            {
                gameState = GameState.Clear;
                GameFlowControl.Instance.ClearGame(); // 모든 컨테이너가 클리어되면 게임 클리어 처리
            }
        }

        return true;
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
            ObjectColor.Mint,
            ObjectColor.Pink,
            ObjectColor.Orange
        };

        Debug.Assert(containerLines.Count == colorTable.Count, "컨테이너와 컬러 테이블의 색상 수량은 1:1이어야 함.");

        // 컨테이너 라인 초기화
        foreach (var containerLine in containerLines)
        {
            var randomColorIndex = Random.Range(0, colorTable.Count);
            var color = colorTable[randomColorIndex];

            containerLine.Init(containerLineIndex, color);
            containerLineDictionary.Add(containerLineIndex, containerLine);
            colorTable.RemoveAt(randomColorIndex); // 사용한 색상을 랜덤 테이블에서 제거

            containerLineIndex++;
        }

        // 공유 컨테이너 위치 설정
        sharedContainer.transform.localPosition = Vector3.up * containerLines.Count * ContainerYDelta;

        // 아이템 생성
        List<Item> tempItemList = new List<Item>();
        int itemID = 0;

        foreach (var containerLine in containerLines)
        {
            for (int createIndex = 0; createIndex < containerLine.containerDictioanry.Count; createIndex++)
            {
                var newItem = itemPool.Burrow<Item>();
                newItem.Init(itemID, containerLine.Color);
                tempItemList.Add(newItem);

                itemID++;
            }
        }

        // 컨테이너에 랜덤 아이템 할당
        foreach (var containerLine in containerLines)
        {
            foreach (var container in containerLine.containerDictioanry.Values)
            {
                int randomIndex = Random.Range(0, tempItemList.Count);
                var randomItem = tempItemList[randomIndex];

                container.SetItem(randomItem);
                tempItemList.RemoveAt(randomIndex); // 사용한 아이템을 리스트에서 제거
            }
        }
    }

    /// <summary>
    /// 한 줄이 같은 색으로 채워졌는지 체크하는 함수
    /// </summary>
    /// <param name="lineIndex">컨테이너 라인 인덱스</param>
    /// <returns>한 줄이 모두 같은색이면 true 반환</returns>
    private bool CheckLine(int lineIndex)
    {
        if (containerLineDictionary.TryGetValue(lineIndex, out var containerLine))
        {
            var containerDictioanry = containerLine.containerDictioanry;
            var lineColor = containerLine.Color;

            foreach (var container in containerDictioanry.Values)
            {
                if (container.Item?.color != lineColor)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}
