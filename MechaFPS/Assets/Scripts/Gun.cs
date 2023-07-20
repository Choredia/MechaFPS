using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float pistolDamage = 5f;
    [SerializeField] float heavyDamage = 15f;

    [SerializeField] private float fireRate= 2f;
    private float nextTimeToFire = 0f;

    private Camera cam;
    private Animator animator;

    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject fleshEffect;

    private Transform firePoint;
    public Transform pistolFirePoint;
    public Transform heavyFirePoint;
    public float bulletSpeed = 10f;

    public int magazineSize = 25;
    public TextMeshProUGUI ammunitionDisplay;
    public GameObject reloadWarning;
    private int bulletsShot;
    int bulletsLeft;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        bulletsLeft = magazineSize - bulletsShot;

        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText("Ammo:" + bulletsLeft + " / " + magazineSize);
        }

        SelectedWeapon();
        GatherInput();

    }

    private void GatherInput()
    {
        if (bulletsLeft != 0)
        {
            
            if (damage == heavyDamage)
            {
                if (Time.time >= nextTimeToFire && Input.GetButton("Fire1"))
                {
                    animator.SetBool("Shooting", true);
                    FireBullet();
                    nextTimeToFire = Time.time + 1f / fireRate;
                }
                else
                {
                    animator.SetBool("Shooting", false);
                }
                
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    animator.SetBool("Shooting", true);
                    FireBullet();
                }
                else
                {
                    animator.SetBool("Shooting", false);
                }
            }

        }
        else
        {
            reloadWarning.SetActive(true);
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {

            animator.SetBool("Reloading", true);
            animator.SetBool("Shooting", false);

            Reload();
        }
        else
        {
            animator.SetBool("Reloading", false);

        }


    }
    private void Reload()
    {
        reloadWarning.SetActive(false);
        
        bulletsShot = 0;
        bulletsLeft = magazineSize;
        
    }
    private void SelectedWeapon()
    {
        switch (WeaponSwitch.selectedWeapon)
        {
            case 0:
                damage = pistolDamage;
                firePoint = pistolFirePoint;
                break;
            case 1:
                damage = heavyDamage;
                firePoint = heavyFirePoint;
                break;
        }
    }

    private void FireBullet()
    {
        bulletsShot++;
        muzzleFlash.Play();
        
        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = bullet.transform.forward * bulletSpeed;

        Invoke("DisableBullet", 2f); 
    }

    private void DisableBullet()
    {
        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.SetActive(false);
    }    
}
