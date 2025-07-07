using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 입력을 처리하는 클래스
/// </summary>
public class InputControl : MonoBehaviour
{
    public bool IsInit { get; private set; } = false;
    public bool IsActive { get; private set; } = false;

    [SerializeField] private float raycastDistance = 10f;

    private GameSystemControl gameSystemControl;
    private Container holdingContainer;

    private void Update()
    {
        if (IsActive == false)
        {
            return;
        }

        if (Input.touchSupported) // 터치 입력 처리
        {
            TouchInputProc();
        }
        else // 마우스 입력 처리
        {
            MouseInputProc();
        }
    }

    /// <summary>
    /// 모듈을 초기화하는 함수
    /// </summary>
    public void Init(GameSystemControl gameSystemControl)
    {
        this.gameSystemControl = gameSystemControl;
        IsInit = true;
    }

    /// <summary>
    /// 입력 모듈을 활성화하는 함수
    /// </summary>
    public void Enable()
    {
        IsActive = true;
    }

    /// <summary>
    /// 입력 모듈을 비활성화하는 함수
    /// </summary>
    public void Disable()
    {
        IsActive = false;
    }

    /// <summary>
    /// 마우스 입력 처리
    /// </summary>
    private void MouseInputProc()
    {
        var mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0)) // Begin
        {
            // 마우스 위치에서 월드 좌표로 레이 캐스트 (Container Layer)
            var ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, raycastDistance, LayerMask.GetMask(LayerNameDefine.CONTAINER_LAYER_NAME))) // Container Layer 캐스트
            {
                // 히트 오브젝트가 IContainer인터페이스를 구현하는 컴포넌트를 가지고 있는지 확인
                var hitContainer = hit.collider.GetComponent<Container>();
                if (hitContainer != null && hitContainer.Item != null) // 아이템을 가지고 있는 컨테이너 인 경우에만 캐싱함.
                {
                    holdingContainer = hitContainer; // holdingContainer에 캐싱
                    holdingContainer.gameObject.layer = LayerMask.NameToLayer(LayerNameDefine.HOLDING_CONTAINER_LAYER_NAME); // 다른 오브젝트를 레이 캐스트가 발견할 수 있게 레이어 임시 변경
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) // End
        {
            if (holdingContainer == null) // 홀딩중인 컨테이너가 없으면 아니면 처리하지 않음.
            {
                return;
            }

            // 마우스 위치에서 월드 좌표로 레이 캐스트 (Container Layer)
            var ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, raycastDistance, LayerMask.GetMask(LayerNameDefine.CONTAINER_LAYER_NAME)))
            {
                var hitContainer = hit.collider.GetComponent<Container>();
                if (hitContainer != null && hitContainer != holdingContainer)
                {
                    gameSystemControl.MoveItem(holdingContainer, hitContainer); // holdingContainer의 아이템을 hitContainer로 이동 시도
                }
                else
                {
                    holdingContainer.SetItem(holdingContainer.Item); // 원래 위치로 이동
                }
            }
            else
            {
                holdingContainer.SetItem(holdingContainer.Item); // 원래 위치로 이동
            }

            holdingContainer.gameObject.layer = LayerMask.NameToLayer(LayerNameDefine.CONTAINER_LAYER_NAME);
            holdingContainer = null; // holdingContainer를 초기화
        }
        else if (Input.GetMouseButton(0)) // Holding
        {
            if (holdingContainer == null) // 홀딩중인 컨테이너가 없으면 아니면 처리하지 않음.
            {
                return;
            }

            var ray = Camera.main.ScreenPointToRay(mousePos);
            Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, -3f));

            if (plane.Raycast(ray, out float distance))
            {
                var worldPos = ray.GetPoint(distance);
                holdingContainer.Item.SetPosition(worldPos); // holding의 위치를 업데이트
            }
        }
    }

    /// <summary>
    /// 터치 입력 처리
    /// </summary>
    private void TouchInputProc()
    {
        if (Input.touchCount == 0)
        {
            return; // 터치가 없으면 처리하지 않음
        }

        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
            {
                // 터치 위치에서 월드 좌표로 레이 캐스트 (Container Layer)
                var ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out var hit, raycastDistance, LayerMask.GetMask(LayerNameDefine.CONTAINER_LAYER_NAME))) // Container Layer 캐스트
                {
                    // 히트 오브젝트가 IContainer인터페이스를 구현하는 컴포넌트를 가지고 있는지 확인
                    var hitContainer = hit.collider.GetComponent<Container>();
                    if (hitContainer != null && hitContainer.Item != null) // 아이템을 가지고 있는 컨테이너 인 경우에만 캐싱함.
                    {
                        holdingContainer = hitContainer; // holdingContainer에 캐싱
                        holdingContainer.gameObject.layer = LayerMask.NameToLayer(LayerNameDefine.HOLDING_CONTAINER_LAYER_NAME); // 다른 오브젝트를 레이 캐스트가 발견할 수 있게 레이어 임시 변경
                    }
                }

                break;
            }
            case TouchPhase.Moved:
            {
                if (holdingContainer == null) // 홀딩중인 컨테이너가 없으면 아니면 처리하지 않음.
                {
                    break;
                
                }

                var ray = Camera.main.ScreenPointToRay(touch.position);
                Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, -3f));

                if (plane.Raycast(ray, out float distance))
                {
                    var worldPos = ray.GetPoint(distance);
                    holdingContainer.Item.SetPosition(worldPos); // holding의 위치를 업데이트
                }

                break;
            }
            case TouchPhase.Ended:
            {
                // 터치 위치에서 월드 좌표로 레이 캐스트 (Container Layer)
                var ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out var hit, raycastDistance, LayerMask.GetMask(LayerNameDefine.CONTAINER_LAYER_NAME))) // Container Layer 캐스트
                {
                    // 히트 오브젝트가 IContainer인터페이스를 구현하는 컴포넌트를 가지고 있는지 확인
                    var hitContainer = hit.collider.GetComponent<Container>();
                    if (hitContainer != null && hitContainer != holdingContainer)
                    {
                        gameSystemControl.MoveItem(holdingContainer, hitContainer); // holdingContainer의 아이템을 hitContainer로 이동 시도
                    }
                    else
                    {
                        holdingContainer.SetItem(holdingContainer.Item); // 원래 위치로 이동
                    }
                }
                else
                {
                    holdingContainer.SetItem(holdingContainer.Item); // 원래 위치로 이동
                }

                holdingContainer.gameObject.layer = LayerMask.NameToLayer(LayerNameDefine.CONTAINER_LAYER_NAME);
                holdingContainer = null; // holdingContainer를 초기화

                break;
            }
        }
    }
}