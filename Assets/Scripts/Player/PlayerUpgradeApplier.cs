using UnityEngine;

public class PlayerUpgradeApplier : MonoBehaviour
{
    [SerializeField]
    private WeaponShooter weaponShooter;

    [SerializeField]
    private TopDownMover topDownMover;

    [SerializeField]
    private PlayerStats playerStats;

    public bool Apply(LevelUpOptionSO option)
    {
        if(option == null)
        {
            return false;
        }

        if(option.effectType == LevelUpEffectType.DamagePlusPercent)
        {
            if(weaponShooter != null)
            {
                weaponShooter.MultiplyDamage(1.0f + option.value / 100.0f);
            }
            return true;
        }

        if(option.effectType == LevelUpEffectType.FireIntervalMinusPercent)
        {
            if(weaponShooter != null)
            {
                weaponShooter.MultiplyFireInterval(1.0f - option.value / 100.0f);
            }
            return true;
        }

        if(option.effectType == LevelUpEffectType.MoveSpeedPlusPercent)
        {
            if(topDownMover != null)
            {
                topDownMover.MultiplyMoveSpeed(1.0f + option.value / 100.0f);
            }
            return true;
        }

        if(option.effectType == LevelUpEffectType.MaxHpPlusFlat)
        {
            if(playerStats != null)
            {
                playerStats.AddMaxHp((int)option.value);
            }
            return true;
        }

        return false;
    }
}
