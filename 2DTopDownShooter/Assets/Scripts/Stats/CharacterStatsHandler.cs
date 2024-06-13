using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CharacterStatsHandler : MonoBehaviour
{
    // 기본스탯과 추가스탯들을 계산해서 최종 스탯을 계산하는 로직이 있음
    // 근데 지금은 그냥 기본스탯만

    [SerializeField] private CharacterStat baseStat;

    public CharacterStat CurrentStat { get; private set; } = new();

    public List<CharacterStat> statModifiers = new List<CharacterStat>();

    private readonly float MinAttackDelay = 0.03f;
    private readonly float MinAttackPower = 0.5f;
    private readonly float MinAttackSize = 0.4f;
    private readonly float MinAttackSpeed = 0.1f;

    private readonly float MinSpeed = 0.8f;

    private readonly int MinMaxHealth = 5; // Minumum size of char max health

    private void Awake()
    {
        UpdateCharacterStat();

        if (baseStat.attackSO != null)
        {
            baseStat.attackSO = Instantiate(baseStat.attackSO);
            CurrentStat.attackSO = Instantiate(baseStat.attackSO);

        }
    }

    private void UpdateCharacterStat() // baseStat -> Upgrade -> CurStat
    {
        ApplyStatModifier(baseStat); // to set base stat

        foreach (CharacterStat stat in statModifiers.OrderBy(o => o.statsChangeType))
        {
            ApplyStatModifier(stat);
        }

    }



    private void ApplyStatModifier(CharacterStat modifier)
    {
        Func<float, float, float> operation = modifier.statsChangeType switch
        {
            StatsChangeType.Add => (current, change) => current + change,
            StatsChangeType.Multiple => (current, change) => current * change,
            StatsChangeType.Override => (current, change) => change,
            _ => (current, change) => change // default type
        };

        UpdateBasicStats(operation, modifier); // makeChange
        UpdateAttackStats(operation, modifier);
        if (CurrentStat.attackSO is RangedAttackSO currentRanged && modifier.attackSO is RangedAttackSO newRanged) // if both is ranged SO Data
        {
            UpdateRangedAttackStats(operation, currentRanged, newRanged);
        }

    }

    private void UpdateRangedAttackStats(Func<float, float, float> operation, RangedAttackSO currentRanged, RangedAttackSO newRanged)
    {
        currentRanged.multipleProjectilesAngle = operation(currentRanged.multipleProjectilesAngle, newRanged.multipleProjectilesAngle);
        currentRanged.spread = operation(currentRanged.spread, newRanged.spread);
        currentRanged.duration = operation(currentRanged.duration, newRanged.duration);
        currentRanged.numberOfProjectilesPerShot = Mathf.CeilToInt(operation(currentRanged.numberOfProjectilesPerShot, newRanged.numberOfProjectilesPerShot));
        currentRanged.projectileColor = UpdateColor(operation, currentRanged.projectileColor, newRanged.projectileColor);

    }

    private Color UpdateColor(Func<float, float, float> operation, Color current, Color modifier)
    {
        return new Color(
            operation(current.r, modifier.r),
            operation(current.g, modifier.g),
            operation(current.g, modifier.g),
            operation(current.a, modifier.a));
    }

    private void UpdateAttackStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        if (CurrentStat.attackSO == null || modifier.attackSO == null) return;

        var currentAttack = CurrentStat.attackSO;
        var newAttack = modifier.attackSO;

        currentAttack.delay = Mathf.Max(operation(currentAttack.delay, newAttack.delay), MinAttackDelay);
        currentAttack.power = Mathf.Max(operation(currentAttack.power, newAttack.power), MinAttackPower);
        currentAttack.size = Mathf.Max(operation(currentAttack.size, newAttack.size), MinAttackPower);
        currentAttack.speed = Mathf.Max(operation(currentAttack.speed, newAttack.speed), MinAttackPower);

    }

    private void UpdateBasicStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        CurrentStat.maxHealth = Math.Max((int)operation(CurrentStat.maxHealth, modifier.maxHealth), MinMaxHealth);
        CurrentStat.speed = Math.Max((int)operation(CurrentStat.speed, modifier.speed), MinSpeed);

    }

    public void AddStatModifier(CharacterStat modifier)
    {
        statModifiers.Add(modifier);
        UpdateCharacterStat();
    }

    public void RemoveStatModifier(CharacterStat statModifier)
    {
        statModifiers.Remove(statModifier);
        UpdateCharacterStat();
    }


}
