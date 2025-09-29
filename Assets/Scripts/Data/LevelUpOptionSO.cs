using UnityEngine;

public enum LevelUpEffectType
{
    DamagePlusPercent,
    FireIntervalMinusPercent,
    MoveSpeedPlusPercent,
    MaxHpPlusFlat
}

[CreateAssetMenu(fileName = "LevelUpOption", menuName = "Game/LevelUpOption")]
public class LevelUpOptionSO : ScriptableObject
{
    public string title;
    public string description;
    public Sprite Icon;
    public LevelUpEffectType effectType;
    public float value = 10.0f;
}
