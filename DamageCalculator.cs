using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(float baseDamage, float acidDamage, float fireDamage, float electricDamage, float toxicDamage, float critChance, float critMultiplier, WeaponStatsSO.WeaponElementType weaponElementType, EnemyStatsSO enemy)
    {
        float totalDamage = baseDamage;

        // Apply weapon element adjustment (20% bonus/reduction)
        totalDamage *= ApplyElementalAdjustment(enemy, weaponElementType, 0.2f);

        // Calculate elemental damage contributions (1% stat = 1% damage)
        float acidDamageCalc = acidDamage;
        float fireDamageCalc = fireDamage;
        float electricDamageCalc = electricDamage;
        float toxicDamageCalc = toxicDamage;

        // Apply elemental stat adjustments (10% bonus/reduction)
        acidDamageCalc *= ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Acid, 0.1f);
        fireDamageCalc *= ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Fire, 0.1f);
        electricDamageCalc *= ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Electric, 0.1f);
        toxicDamageCalc *= ApplyElementalAdjustment(enemy, WeaponStatsSO.WeaponElementType.Toxic, 0.1f);


        if (enemy.enemyType == EnemyStatsSO.EnemyType.Hybrid)
        {
            acidDamageCalc = 0;
            fireDamageCalc = 0;
            electricDamageCalc = 0;
            toxicDamageCalc = 0;
        }

        totalDamage += acidDamageCalc + fireDamageCalc + electricDamageCalc + toxicDamageCalc;

        float critChanceCalc = critChance / 100; 
        if (Random.value <= critChanceCalc)
        {
            totalDamage *= critMultiplier; 
            Debug.Log("Critical Hit! Damage multiplied.");
        }

        return totalDamage;
    }

    private static float ApplyElementalAdjustment(EnemyStatsSO enemy, WeaponStatsSO.WeaponElementType elementType, float adjustmentFactor)
    {
        float adjustedDamage = 1.0f;

        switch (enemy.enemyType)
        {
            case EnemyStatsSO.EnemyType.Parasite:
                if (elementType == WeaponStatsSO.WeaponElementType.Fire)
                {
                    adjustedDamage += adjustmentFactor;
                }
                else if (elementType == WeaponStatsSO.WeaponElementType.Toxic)
                {
                    adjustedDamage -= adjustmentFactor;
                }
                break;
            case EnemyStatsSO.EnemyType.Armored:
                if (elementType == WeaponStatsSO.WeaponElementType.Acid)
                {
                    adjustedDamage += adjustmentFactor;
                }
                else if (elementType == WeaponStatsSO.WeaponElementType.Fire)
                {
                    adjustedDamage -= adjustmentFactor;
                }
                break;
            case EnemyStatsSO.EnemyType.Mesh:
                if (elementType == WeaponStatsSO.WeaponElementType.Electric)
                {
                    adjustedDamage += adjustmentFactor;
                }
                else if (elementType == WeaponStatsSO.WeaponElementType.Fire)
                {
                    adjustedDamage -= adjustmentFactor;
                }
                break;
            case EnemyStatsSO.EnemyType.heavyArmored:
                adjustedDamage -= 0.2f; // Consistent 0.8x damage multiplier
                break;
            case EnemyStatsSO.EnemyType.Hybrid:
                break;
            default:
                adjustedDamage = 1.0f;
                break;
        }

        return adjustedDamage;
    }
}