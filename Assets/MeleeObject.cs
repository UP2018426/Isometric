using UnityEngine;

public class MeleeObject : MonoBehaviour
{
    [SerializeField] private int damage;
    public Collider meleeCollider;
    private bool isAttacking;
    private Animator animator;
    private float swingTime;
    private float meleeTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("The enemy " + other.name + " was hit by " + transform.name);
                EnemyController enemy = other.transform.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
            }
        }
    }

    private void Awake()
    {
        meleeCollider = GetComponent<Collider>();

        animator = GetComponent<Animator>();

        meleeCollider.enabled = false;

        swingTime = GetClipLength("MeleeSwing");
    }

    public float GetClipLength(string clipName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning("Animation clip " + clipName + " not found.");
        return 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            meleeTimer += Time.deltaTime;
        }

        if(meleeTimer >= swingTime)
        {
            isAttacking = false;
            OnAttackEnd();
            meleeTimer = 0f;
        }
    }

    public void OnAttackBegin()
    {
        isAttacking = true;
        meleeCollider.enabled = true;

        animator.Play("MeleeSwing");
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        meleeCollider.enabled = false;
    }
}
