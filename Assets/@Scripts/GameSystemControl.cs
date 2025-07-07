using UnityEngine;

public class GameSystemControl : MonoBehaviour
{
    public ColorMaterialScriptableObject colorMaterialTable;

    [Space(30f)]
    [LunaPlaygroundField(fieldTitle: "Time Limit")]
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private InputControl inputControl;

    private float endTime = 0.0f;
    private GameState state = GameState.Wait;

    public void InitialzieAndStart()
    {
        endTime = Time.time + timeLimit;
        state = GameState.Playing;

        // 블럭 초기화

        // 입력 모듈 초기화
        if (inputControl.IsInit == false)
        {
            inputControl.Init(this);
        }

        inputControl.Enable();
    }

    private void Reset()
    {
        state = GameState.Wait;
    }
}
