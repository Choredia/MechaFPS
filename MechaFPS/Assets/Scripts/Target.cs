using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    private Animator animator;
    private bool isDead = false;
    [SerializeField] EnemyDumb enemyScript;
    private AudioSource audioSource;
    public float sightRange;
    public bool playerInSightRange;

    public LayerMask Player;


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
