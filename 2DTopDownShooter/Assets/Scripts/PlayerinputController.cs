using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerinputController : TopDownController
{
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main; // mainCamera 태그가 붙어있는 카메라를 가져온다.
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized; // value로 입력받은 값을 가져와서 정규화 시켜서 moveInput에 넣음
        CallMoveEvent(moveInput);
        // 실제움직이는 처리는 여기서 하는게 아니라 PlayerMovement에서 함
    }

    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();             // 매개변수 value로 마우스 포인터가 가르키는 위치 정보값 가져옴
        Vector2 worldPos = camera.ScreenToWorldPoint(newAim); // screen상에서 찍은거기에 world 좌표로 변경
        newAim = (worldPos - (Vector2)transform.position).normalized; // world좌표로 변경된 값에서 현재 위치 값 빼주고 정규화해서 보는 방향을 정규벡터화시킴

        CallLookEvent(newAim);
    }

    public void OnFire(InputValue value)
    {
        isAttacking = value.isPressed;
    }

}
