using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent; // Action 은 무조건 void 만 반환해야함 아니면 false 처리
    public event Action<Vector2> OnLookEvent;
    public event Action OnAttackEvent;

    protected bool isAttacking { get; set; }

    private float timeSinceLastAttack = float.MaxValue;


    private void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        //Todo: Magic Number
        if (timeSinceLastAttack < 0.2f)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else if(isAttacking && timeSinceLastAttack >= 0.2f)
        {
            timeSinceLastAttack = 0f;
            CallAttackEvent();

        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction); // ?. Null 이면 실행안함, 아니면 이어서함 (안전한 코딩)
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }
    private void CallAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }


}
