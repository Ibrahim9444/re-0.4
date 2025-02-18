using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/New Enemy")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Enemy Type")]
    // Use this enum to define the type of the enemy
    public EnemyType enemyType;

    [Header("Basic Stats")]
    public string enemyName;
    public float Health;
    public float Armor;
    public float MovementSpeed;
    public float BaseDamage;

    [Header("Enemy Elemental Stats")]
    // Resistances & Weaknesses
    public float acidResistance;
    public float fireResistance;
    public float electricResistance;
    public float toxicResistance;

    public enum EnemyType
    {
        Parasite,  // Weak to fire
        Armored,   // Weak to acid
        Mesh,      // Weak to electric
        Hybrid,     // only take base damage
        heavyArmored // takes 0.8 damage from everything
    }
}
