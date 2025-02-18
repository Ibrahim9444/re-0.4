using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    public float lifeTime;
    private float currentLifeTime;
    private float damage;
    private WeaponStatsSO weaponStats;  // Reference to WeaponStatsSO

    void Start()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

        if (currentLifeTime <= 0)
        {
            destroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage); // Pass damage
            destroyProjectile();
        }
    }

    public void SetDamage(float damage) // Remove WeaponStatsSO parameter
    {
        this.damage = damage;
    }

    void destroyProjectile()
    {
        Destroy(gameObject);
    }
}