using UnityEngine;
using System.Collections.Generic;
using System;

public class CharacterStatsHandler : MonoBehaviour
{
    // 기본스탯과 추가스탯들을 계산해서 최종 스탯을 계산하는 로직이 있음
    // 근데 지금은 그냥 기본스탯만

    [SerializeField] private CharacterStat baseStat;

    public CharacterStat CurrentStat { get; private set; }

    public List<CharacterStat> statModifiers = new List<CharacterStat>();

    private void Awake()
    {
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat() // baseStat -> Upgrade -> CurStat
    {
        AttackSO attackSO = null;
        if (baseStat.attackSO != null)
        {
            attackSO = Instantiate(baseStat.attackSO);
        }

        CurrentStat = new CharacterStat { attackSO = attackSO }; // 현재능력치로 되돌리기 위해 현재 가지고 있는 attackSO의 정보를 넣음
        CurrentStat.statsChangeType = baseStat.statsChangeType;
        CurrentStat.maxHealth = baseStat.maxHealth;
        CurrentStat.speed = baseStat.speed;

    }
}
