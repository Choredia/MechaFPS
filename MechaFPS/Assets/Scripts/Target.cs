using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50f;
    [SerializeField] private float sightRange;
    [SerializeField] private bool playerInSightRange;
    [SerializeField] private LayerMask Player;
    [SerializeField] EnemyDumb enemyScript;

    private Animator animator;
    private AudioSource audioSource;

    private bool isDead = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);

        if (playerInSightRange ) 
        {
            if (enemyScript != null)
            {
                enemyScript.enabled = true;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;

        health -= amount;
        animator.SetBool("EnemyHurt", true);

        if (health <= 0f)
        {
            animator.SetBool("EnemyShooting", false);
            isDead = true;
            Die();
        }
    }
    void Die()
    {

        Destroy(gameObject);
        /*animator.SetBool("EnemyDeath", true);
        Destroy(enemyScript);
        Destroy(audioSource);
        */
    }
}
