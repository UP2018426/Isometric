using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int health;
    public float detectionRange;
    private NavMeshAgent agent;

    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = (transform.position - player.transform.position).magnitude;

        if (distanceToPlayer < detectionRange)
        {
            agent.destination = player.position;
        }
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if(health <= 0)
        {
            Debug.Log("Enemy is dead");
            Destroy(this.gameObject, 0.2f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
