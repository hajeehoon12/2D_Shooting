using System;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    // 실제로 이동이 일어날 컴포넌트

    private TopDownController controller;
    private Rigidbody2D movementRigidbody;
    private CharacterStatsHandler characterStatsHandler;

    private Vector2 movementDirection = Vector2.zero; // 이동안하는 경우 0으로 초기화

    private void Awake() // 자기 내부 값 선언 할때 Awake를 주로 사용
    {

        controller = GetComponent<TopDownController>();  // controller랑 TopdownController랑 같은 게임오브젝트 안에 있다는 가정
        movementRigidbody = GetComponent<Rigidbody2D>();
        characterStatsHandler = GetComponent<CharacterStatsHandler>();

    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 direction)
    {
        movementDirection = direction; // direction 방향으로 이동
    }


    private void FixedUpdate()
    {
        // FixedUpdate는 물리 업데이트 관련 이기에 Rigidbody 값 변경 부분을 포함함
        ApplyMoveMent(movementDirection);
    }


    private void ApplyMoveMent(Vector2 direction)
    {
        direction = direction * characterStatsHandler.CurrentStat.speed; // speed 
        movementRigidbody.velocity = direction;
    }


}