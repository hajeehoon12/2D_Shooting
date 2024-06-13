using UnityEngine;

public class PickedUpHeal : PickUpItem
{
    [SerializeField] int healValue = 10;

    protected override void OnPickedUp(GameObject go)
    {
        HealthSystem healthSystem = go.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(healValue);
    }


}