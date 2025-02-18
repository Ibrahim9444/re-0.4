
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "ScriptableObjects/WeaponStats")]
public class WeaponStatsSO : ScriptableObject
{

    [Header("Weapon Element")]
    // **New - Element Type** 
    // Allows selection of a single element type to trigger stats later
    public WeaponElementType weaponElementType;
    public WeaponType weaponType;

    [Header("Basic Stats")]
    public string weaponName;
    public float baseDamage;
    public int clipSize;
    public float fireRate;
    public float reloadSpeed;
    public float maxPenetrations;
    [Header("If ShotGun")]
    public float spreadAngle; // for shotguns
    public int bulletsPerShot;// for shotguns

    [Header("Elemental Stats")]
    public float acidDamage;
    public float fireDamage;
    public float electricDamage;
    public float toxicDamage;

    [Header("Chances")]
    public float elementTriggerChance; // Chance for element to activate  
    public float critChance;
    public float critMultiplier;

    [Header("Other Stats")]
    public bool hasSpecialAbility;
    public string specialAbilityDescription;

    [Header("WeaponElementEffect")]
    // **New - Elemental Effect Types**
    public bool bypassArmor;
    public bool doubleDamageToArmor;
    public bool electricChain;
    public bool stunEnemies;
    public bool areaDamage;
    public bool healAllies;



    public enum WeaponType
    {
        Pistol,
        Shotgun,
        LMG,
        AssaultRifle,
        SniperRifle,
        rpg,
        grenadeLauncher,
    }

    public enum WeaponElementType
    {
        None,   // No element
        Acid,   // Acid element
        Fire,   // Fire element
        Electric, // Electric element
        Toxic   // Toxic element
    }
}
// rebuild check point

