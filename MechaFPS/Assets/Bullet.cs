using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;


    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Çarpma olayý gerçekleþtiðinde bu metot çalýþýr

            // Çarpma noktasýný bulma

            
            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 collisionPoint = contactPoint.point;

            GameObject impactedObject = collision.gameObject;
            Target targetScript = impactedObject.GetComponent<Target>();

            // "target" scriptini çalýþtýr
            if (targetScript != null && collision.gameObject.CompareTag("Enemy"))
            {
                targetScript.TakeDamage(20f); // Target scriptindeki bir iþlevi çaðýrabilirsiniz
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
