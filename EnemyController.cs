
using UnityEngine;
using static EnemyStatsSO;

public class EnemyController : MonoBehaviour
{
    public EnemyStatsSO enemyStats;
    public EnemyType enemyType;
    [Header("Enemy Stats")]
    private string enemyCurrentName;
    private float enemyCurrentHealth;
    private float enemyCurrentArmor;
    private float enemyCurrentMovementSpeed;
    private float enemyCurrentBaseDamage;
    [Header("Enemy Elemental Stats")]
    private float acidResistance;
    private float fireResistance;
    private float electricResistance;
    private float toxicResistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        References();
        enemyCurrentHealth = enemyStats.Health;
        enemyCurrentArmor = enemyStats.Armor;
        Debug.Log("Enemy Type: " + enemyStats.enemyType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage) // Accept float damage
    {
        if (enemyCurrentArmor > 0)
        {
            float damageToArmor = Mathf.Min(damage, enemyCurrentArmor);
            enemyCurrentArmor -= damageToArmor;
            damage -= damageToArmor;
            Debug.Log($"Damage absorbed by armor: {damageToArmor}. Remaining Armor: {enemyCurrentArmor}");
        }

        if (damage > 0)
        {
            enemyCurrentHealth -= damage;
            Debug.Log($"Damage taken: {damage}. Remaining Health: {enemyCurrentHealth}");
        }

        if (enemyCurrentHealth <= 0)
        {
            Debug.Log("Enemy destroyed.");
            Destroy(gameObject);
        }
    }


    void References()
    {
        // References to the enemy stats
        enemyCurrentName = enemyStats.enemyName;
        enemyCurrentHealth = enemyStats.Health;
        enemyCurrentArmor = enemyStats.Armor;
        enemyCurrentMovementSpeed = enemyStats.MovementSpeed;
        enemyCurrentBaseDamage = enemyStats.BaseDamage;
        acidResistance = enemyStats.acidResistance;
        fireResistance = enemyStats.fireResistance;
        electricResistance = enemyStats.electricResistance;
        toxicResistance = enemyStats.toxicResistance;
    }

}
// rebuild check point