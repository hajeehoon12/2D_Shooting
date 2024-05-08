using UnityEngine;

public enum StatsChangeType // stat 스탯증가 적용 방식
{ 
    Add, // 0
    Multiple, // 1
    Override // 2
}

[System.Serializable] // 데이터 폴더용으로 스크립트 작업용
public class CharacterStat
{
    public StatsChangeType statsChangeType;
    [Range(1, 100)] public int maxHealth;
    [Range(1f, 20f)] public float speed;
    public AttackSO attackSO;
}