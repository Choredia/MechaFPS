using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;



    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            // Çarpma olayý gerçekleþtiðinde bu metot çalýþýr

            // Çarpma noktasýný bulma


            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 collisionPoint = contactPoint.point;

            GameObject impactedObject = collision.gameObject;
            PlayerScript playerScript = impactedObject.GetComponent<PlayerScript>();

            // "target" scriptini çalýþtýr
            if (playerScript != null && collision.gameObject.CompareTag("Player"))
            {
                
                playerScript.TakeDamage(10f); // Target scriptindeki bir iþlevi çaðýrabilirsiniz
                
            }
            else
            {
                GameObject impactGO = Instantiate(hitEffect, collisionPoint, Quaternion.LookRotation(contactPoint.normal));
                Destroy(impactGO, 2f);
            }
;




            this.gameObject.SetActive(false);

            // Çarpýþma noktasýný kullanarak iþlemler yapabilirsiniz
        }
        // Çarpýþma noktasýný kullanarak iþlemler yapabilirsiniz
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tetikleyici olay gerçekleþtiðinde bu metot çalýþýr

        // Çarpma noktasýný bulma
        Vector3 collisionPoint = other.ClosestPoint(transform.position);

        // Çarpýþma noktasýný kullanarak iþlemler yapabilirsiniz
    }
}
