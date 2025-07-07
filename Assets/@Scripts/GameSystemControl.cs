using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 시스템의 로직을 제어하는 클래스
/// </summary>
public class GameSystemControl : MonoBehaviour
{
    public ColorMaterialScriptableObject colorMaterialTable;

    [Space(30f)]
    [LunaPlaygroundField(fieldTitle: "Time Limit")]
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private InputControl inputControl;

    [Space(30f)]
    [SerializeField] private MyObjectPool.ObjectPool itemPool;
    [SerializeField] private List<ContainerLine> containerLines;
    [SerializeField] private SharedContainer sharedContainer;

    private Dictionary <int, ContainerLine> containerLineDictionary = new Dictionary<int, ContainerLine>();
    private float endTime = 0.0f;
    private GameState state = GameState.Wait;

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Re Init"))
        {
            InitialzieAndStart();
        }
    }
#endif

    private void Reset()
    {
        foreach (var containerLine in containerLines)
        {
            containerLine.Reset(); // 컨테이너 라인 초기화
        }

        sharedContainer.GetItem()?.Return();

        containerLineDictionary.Clear();
        endTime = 0f;
        state = GameState.Wait;
        inputControl.Disable();
    }

    /// <summary>
    /// 게임을 초기화하고 시작하는 함수
    /// </summary>
    public void InitialzieAndStart()
    {
        Reset();

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
    /// 아이템을 특정 컨테이너로 이동시키는 함수
    /// </summary>
    /// <param name="from">원래 아이템이 있던 컨테이너</param>
    /// <param name="to">이동시킬 컨테이너</param>
    /// <returns>to에 아이템이 있으면 false반환.</returns>
    public bool MoveItem(IContainer from, IContainer to)
    {
        if (to.GetItem() != null)
        {
            from.SetItem(from.GetItem());
            return false;
        }
        else if (from.GetItem() == null)
        {
            return false;
        }

        to.SetItem(from.GetItem()); // to에 아이템 설정
        from.SetItem(null);

        if (CheckLine(to.GetLineIndex()))
        {
            Debug.Log("성공");
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
                if (container.GetItem()?.color != lineColor)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}
