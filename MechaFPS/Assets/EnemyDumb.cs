using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDumb : MonoBehaviour
{
    public Transform target;
    private EnemyReferences enemyReferences;

    private float pathUpdateDeadLine;
    private float shootingDistance;
    private float BlendEnemySpeed;
    private float bulletSpeed = 10f;
    public Transform firePoint;
    public Transform firePoint1;
    [SerializeField] Transform mainCam;
    

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
        
    }
    void Start()
    {
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;
    }

    void Update()
    {

        


        if (target != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;


            if (inRange) 
            {
                LookAtTheTarget();
                FireBullet();

            }
            else
            {
                UpdatePath();
                
            }

            enemyReferences.animator.SetBool("EnemyShooting", inRange);
        }
       
        enemyReferences.animator.SetFloat("EnemySpeed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
        

    }
    private void FireBullet()
    {


        GameObject bullet = ObjectPool.Instance.GetEnemy();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);

        if (firePoint1 != null)
        {
            GameObject bullet1 = ObjectPool.Instance.GetEnemy();
            bullet1.transform.position = firePoint1.position;
            bullet1.transform.rotation = transform.rotation;
            bullet1.SetActive(true);

            Rigidbody bulletRigidbody1 = bullet1.GetComponent<Rigidbody>();
            Vector3 direction1 = (mainCam.position) - bullet1.transform.position;
            bulletRigidbody1.velocity = direction1.normalized * bulletSpeed;
        }

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (mainCam.position) - bullet.transform.position;
        bulletRigidbody.velocity = direction.normalized * bulletSpeed;

        Invoke("DisableBullet", 2f);
    }

    private void DisableBullet()
    {
        GameObject bullet = ObjectPool.Instance.GetEnemy();
        bullet.SetActive(false);
    }


    private void LookAtTheTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private void UpdatePath()
    {
        Debug.Log("Updating Path");
        pathUpdateDeadLine = Time.time + enemyReferences.pathUpdateDelay;
        enemyReferences.navMeshAgent.SetDestination(target.position);
    }
    


}
