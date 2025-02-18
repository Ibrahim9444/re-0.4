using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponStatsSO;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponStatsSO weaponStatsSO;
    public List<ModSO> mods = new List<ModSO>();

    private float currentBaseDamage;
    private float currentFireRate;
    private float currentReloadSpeed;
    private int currentClipSize;
    private float currentAcidDamage;
    private float currentFireDamage;
    private float currentElectricDamage;
    private float currentToxicDamage;
    private float currentElementTriggerChance;
    private float currentCritChance;
    private float currentCritMultiplier;
    private float currentSpreadAngle;
    private int currentBulletsPerShot;

    private bool bypassArmor;
    private bool doubleDamageToArmor;
    private bool electricChain;
    private bool stunEnemies;
    private bool areaDamage;
    private bool healAllies;

    private int currentAmmo;
    private bool isReloading = false;

    private float nextFireTime = 0f;
    private bool statsChanged = false;

    public ModManger modManger;
    public Transform firePoint;
    public GameObject projectilePrefab;

    private EnemyStatsSO targetEnemyStats;

    void Start()
    {
        firePoint = transform.Find("FirePoint");
        modManger = GetComponent<ModManger>();
        WeaponReference();
        ApplyMods();
        currentAmmo = currentClipSize;
        Debug.Log("Starting Ammo: " + currentAmmo);

        // Set target enemy stats for testing (you might want to set this dynamically in your game)
        var enemyController = Object.FindFirstObjectByType<EnemyController>();  // Use the recommended method
        if (enemyController != null)
        {
            targetEnemyStats = enemyController.enemyStats;
            Debug.Log("Target enemy stats set to: " + targetEnemyStats.enemyName);
        }
        else
        {
            Debug.LogWarning("No enemy found to set target enemy stats.");
        }
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Debug.Log("Attempting to shoot...");
            Shoot();
            nextFireTime = Time.time + (1f / currentFireRate);
        }

        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (statsChanged)
        {
            WeaponReference();
            statsChanged = false;
        }
    }

    public void OnStatsChanged()
    {
        statsChanged = true;
    }

    public void Shoot()
    {
        // ... (Other code in Shoot)

        float damage = DamageCalculator.CalculateDamage(
            currentBaseDamage,
            currentAcidDamage,
            currentFireDamage,
            currentElectricDamage,
            currentToxicDamage,
            currentCritChance,
            currentCritMultiplier,
            weaponStatsSO.weaponElementType,
            targetEnemyStats);

        if (weaponStatsSO.weaponType == WeaponType.Shotgun)
        {
            for (int i = 0; i < currentBulletsPerShot; i++)
            {
                float angleOffset = Random.Range(-currentSpreadAngle, currentSpreadAngle);
                Quaternion spreadRotation = Quaternion.Euler(0, angleOffset, 0);
                var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation * spreadRotation);
                projectile.GetComponent<ProjectileController>().SetDamage(damage); // Pass damage
            }
            currentAmmo--;
        }
        else
        {
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<ProjectileController>().SetDamage(damage); // Pass damage
            currentAmmo--;
        }
        Debug.Log("Bullets left: " + currentAmmo);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(currentReloadSpeed);

        currentAmmo = currentClipSize;
        isReloading = false;
        Debug.Log("Reloaded Ammo: " + currentAmmo);
    }

    void WeaponReference()
    {
        currentBaseDamage = weaponStatsSO.baseDamage;
        currentFireRate = weaponStatsSO.fireRate;
        currentReloadSpeed = weaponStatsSO.reloadSpeed;
        currentClipSize = weaponStatsSO.clipSize;

        currentAcidDamage = weaponStatsSO.acidDamage;
        currentFireDamage = weaponStatsSO.fireDamage;
        currentElectricDamage = weaponStatsSO.electricDamage;
        currentToxicDamage = weaponStatsSO.toxicDamage;

        currentElementTriggerChance = weaponStatsSO.elementTriggerChance;
        currentCritChance = weaponStatsSO.critChance;
        currentCritMultiplier = weaponStatsSO.critMultiplier;

        bypassArmor = weaponStatsSO.bypassArmor;
        doubleDamageToArmor = weaponStatsSO.doubleDamageToArmor;
        electricChain = weaponStatsSO.electricChain;
        stunEnemies = weaponStatsSO.stunEnemies;
        areaDamage = weaponStatsSO.areaDamage;
        healAllies = weaponStatsSO.healAllies;
        currentSpreadAngle = weaponStatsSO.spreadAngle;
        currentBulletsPerShot = weaponStatsSO.bulletsPerShot;
    }

    public void ApplyElementalModEffects(ModSO mod, int level)
    {
        if (mod.modType == ModType.Elemental)
        {
            currentFireDamage += mod.fireDamageBoostLevels[level];
            currentToxicDamage += mod.toxicDamageBoostLevels[level];
            currentAcidDamage += mod.acidDamageBoostLevels[level];
            currentElectricDamage += mod.electricDamageBoostLevels[level];
        }
    }

    public void ApplyBasicModEffects(ModSO mod, int level)
    {
        if (mod.modType == ModType.Basic)
        {
            currentBaseDamage += mod.baseDamageBoostLevels[level];
            currentFireRate += mod.FireRateBoostLevels[level];
            currentClipSize += mod.clipSizeBoostLevels[level];
            currentCritChance += mod.critChanceBoostLevels[level];
            currentCritMultiplier += mod.critMultiplierBoostLevels[level];


            float reloadMultiplier = 1f - (mod.ReloadSpeedBoostLevels[level] / 100f);
            float oldReloadSpeed = currentReloadSpeed;
            currentReloadSpeed *= reloadMultiplier;
            currentReloadSpeed = Mathf.Max(currentReloadSpeed, 0.1f);

            Debug.Log($"[Reload Debug] Old Reload Speed: {oldReloadSpeed} sec | Modifier: {mod.ReloadSpeedBoostLevels[level]}% | New Reload Speed: {currentReloadSpeed} sec");
            Debug.Log($"Applying Basic Mod Effects: Damage Boost: {mod.baseDamageBoostLevels[level]}, New Base Damage: {currentBaseDamage}");
            Debug.Log($"[Reload Debug] Old Reload Speed: {oldReloadSpeed} sec | Modifier: {mod.ReloadSpeedBoostLevels[level]}% | New Reload Speed: {currentReloadSpeed} sec");
        }
    }

    public void ApplyHybridModEffects(ModSO mod, int level)
    {
        if (mod.modType == ModType.Hybrid)
        {
            currentBaseDamage += mod.baseDamageBoostLevels[level];
            currentFireRate += mod.FireRateBoostLevels[level];
            currentReloadSpeed += mod.ReloadSpeedBoostLevels[level];
            currentClipSize += mod.clipSizeBoostLevels[level];
            currentFireDamage += mod.fireDamageBoostLevels[level];
            currentToxicDamage += mod.toxicDamageBoostLevels[level];
            currentAcidDamage += mod.acidDamageBoostLevels[level];
            currentElectricDamage += mod.electricDamageBoostLevels[level];
            currentCritChance += mod.critChanceBoostLevels[level];
            currentCritMultiplier += mod.critMultiplierBoostLevels[level];

        }
    }

    public void RemoveElementalModEffects(ModSO mod, int level)
    {
        currentFireDamage -= mod.fireDamageBoostLevels[level];
        currentToxicDamage -= mod.toxicDamageBoostLevels[level];
        currentAcidDamage -= mod.acidDamageBoostLevels[level];
        currentElectricDamage -= mod.electricDamageBoostLevels[level];
    }

    public void RemoveBasicModEffects(ModSO mod, int level)
    {
        currentBaseDamage -= mod.baseDamageBoostLevels[level];
        currentFireRate -= mod.FireRateBoostLevels[level];
        currentReloadSpeed -= mod.ReloadSpeedBoostLevels[level];
        currentClipSize -= mod.clipSizeBoostLevels[level];
        currentCritChance += mod.critChanceBoostLevels[level];
        currentCritMultiplier += mod.critMultiplierBoostLevels[level];

    }

    public void RemoveHybridModEffects(ModSO mod, int level)
    {
        currentBaseDamage -= mod.baseDamageBoostLevels[level];
        currentFireRate -= mod.FireRateBoostLevels[level];
        currentReloadSpeed -= mod.ReloadSpeedBoostLevels[level];
        currentClipSize -= mod.clipSizeBoostLevels[level];
        currentFireDamage -= mod.fireDamageBoostLevels[level];
        currentToxicDamage -= mod.toxicDamageBoostLevels[level];
        currentAcidDamage -= mod.acidDamageBoostLevels[level];
        currentElectricDamage -= mod.electricDamageBoostLevels[level];
        currentCritChance += mod.critChanceBoostLevels[level];
        currentCritMultiplier += mod.critMultiplierBoostLevels[level];

    }

    public void ApplyMods()
    {
        foreach (var mod in modManger.activeMods)
        {
            if (mod.modType == ModType.Basic)
            {
                ApplyBasicModEffects(mod, mod.currentLevel);
            }
            else if (mod.modType == ModType.Elemental)
            {
                ApplyElementalModEffects(mod, mod.currentLevel);
            }
            else if (mod.modType == ModType.Hybrid)
            {
                ApplyHybridModEffects(mod, mod.currentLevel);
            }
        }
    }

    // Method to set target enemy stats
    public void SetTargetEnemyStats(EnemyStatsSO enemyStats)
    {
        targetEnemyStats = enemyStats;
    }
}