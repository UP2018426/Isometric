using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if(health <= 0)
        {
            Debug.Log("Enemy is dead");
            Destroy(this.gameObject, 1f);
        }
    }
}
