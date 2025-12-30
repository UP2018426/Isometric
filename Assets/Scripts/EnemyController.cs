using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int health;
    public float detectionRange;
    private NavMeshAgent agent;

    private Transform player;
    private ThirdPersonController playerController;

    [Header("Attack Variables")]
    [SerializeField] private int attackDamage; 
    [SerializeField] public float attackRange;
    [Range(-1f,1f), SerializeField] private float maxAttackAngle;
    [SerializeField] private float windupSpeed;
    private float windingTimer;
    [SerializeField] private float attackCooldownTime; // The time between an attack ending, and a new one begining
    private float cooldownTimer;

    private bool preparingToAttack;

    [SerializeField] private Color telegraphColour;
    private MeshRenderer meshRenderer;
    private Color baseColour;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<ThirdPersonController>();

        meshRenderer = GetComponent<MeshRenderer>();
        baseColour = meshRenderer.material.color;
    }

    void Update()
    {
        float distanceToPlayer = (transform.position - player.transform.position).magnitude;

        if (distanceToPlayer < detectionRange)
        {
            agent.destination = player.position;
        }

        // Handle telegraphing and attacking

        cooldownTimer += Time.deltaTime;

        if (distanceToPlayer < attackRange && attackCooldownTime < cooldownTimer)
        {
            preparingToAttack = true;

            // Stop AI agent movement as it attacks
            agent.isStopped = true;
        }

        if (preparingToAttack)
        {
            windingTimer += windupSpeed * Time.deltaTime;

            if (windingTimer >= 1)
            {
                OnAttack(distanceToPlayer);
            }
        }
        else
        {
            windingTimer -= windupSpeed * Time.deltaTime;
        }

        windingTimer = Mathf.Clamp01(windingTimer);

        meshRenderer.material.color = Color.Lerp(baseColour, telegraphColour, windingTimer);
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

    void OnAttack(float distanceToPlayer)
    {
        // Reset telegraphing variables
        preparingToAttack = false;
        windingTimer = 0;

        // Re-enable movement
        agent.isStopped = false;

        // Calculate the Dot Product (Angle without left/right) to the player
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, toPlayer);

        // If the player is within our attack angle AND the player is within out attacking distance...
        if (dot >= maxAttackAngle && distanceToPlayer < attackRange)
        {
            Debug.Log("Hit player");

            // Deal damage to the player
            playerController.TakeDamage(attackDamage);
        }

        // Reset the cooldown timer to stop an immediate attack
        cooldownTimer = 0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
