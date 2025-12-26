using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float despawnTime = 10f; // This is how long the object will take to despawn;

    [SerializeField] int damage; // This is the damage that the projectile will deal to the enemy it hits

    private void Start()
    {
        Destroy(this.gameObject, despawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("is not player");
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("is enemy");
                EnemyController enemy = other.transform.GetComponent<EnemyController>();

                enemy.TakeDamage(damage);
            }
        }
    }
}
