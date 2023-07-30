using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject enemyHitEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        { 
            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 collisionPoint = contactPoint.point;

            GameObject impactedObject = collision.gameObject;
            Target targetScript = impactedObject.GetComponent<Target>();

            // "target" scriptini çalýþtýr
            if (targetScript != null && collision.gameObject.CompareTag("Enemy"))
            {
                targetScript.TakeDamage(20f); 
                GameObject enemyHitGO = Instantiate(enemyHitEffect, collisionPoint, Quaternion.LookRotation(contactPoint.normal));
                Destroy(enemyHitGO, 2f);
            }
            else
            {
                GameObject impactGO = Instantiate(hitEffect, collisionPoint, Quaternion.LookRotation(contactPoint.normal));
                Destroy(impactGO, 2f);
            }
;
            this.gameObject.SetActive(false);
        }
    }
}
