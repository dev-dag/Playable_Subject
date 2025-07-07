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

        to.SetItem(from.GetItem()); // to에 아이템 설정
        from.SetItem(null);

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

            Debug.Log($"컨테이너 라인 초기화 : 인덱스 = {containerLine.LineIndex}, 컬러 = {containerLine.Color}");

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

                Debug.Log($"아이템 생성 : 아이템 ID = {itemID}, 아이템 컬러 = {newItem.color.ToString()}");

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

                Debug.Log($"컨테이너에 아이템 랜덤 할당 : 아이템 ID = {randomItem.ID}, 아이템 컬러 = {randomItem.color.ToString()}, 컨테이너 인덱스 = {container.ContainerIndex}, 라인 인덱스 = {container.LineIndex}");

                tempItemList.RemoveAt(randomIndex); // 사용한 아이템을 리스트에서 제거
            }
        }
    }
}
