using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Camera cam;
    private Animator animator;
    private Transform firePoint;

    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject fleshEffect;
    [SerializeField] private GameObject reloadWarning;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pistolAudio;
    [SerializeField] private AudioClip heavyAudio;

    [SerializeField] private Transform pistolFirePoint;
    [SerializeField] private Transform heavyFirePoint;

    [SerializeField] private TextMeshProUGUI ammunitionDisplay;

    [SerializeField] private float damage;
    [SerializeField] private float pistolDamage = 5f;
    [SerializeField] private float heavyDamage = 15f;
    [SerializeField] private float fireRate= 2f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int magazineSize = 25;

    private float nextTimeToFire = 0f;

    private int bulletsShot;
    private int bulletsLeft;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
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
                HeavyFire();

            }
            else
            {
                PistolFire();
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

    private void PistolFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("Shooting", true);
            audioSource.Stop();
            audioSource.PlayOneShot(pistolAudio);
            
            FireBullet();
           
        }
        else
        {
            animator.SetBool("Shooting", false);
        }
    }

    private void HeavyFire()
    {
        if (Input.GetButton("Fire1")&&Time.time >= nextTimeToFire)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(heavyAudio);
            }
            animator.SetBool("Shooting", true);
            FireBullet();
            nextTimeToFire = Time.time + 1f / fireRate;
        }
        else
        {
            animator.SetBool("Shooting", false);

        }
        if (Input.GetButtonUp("Fire1"))
        {
            audioSource.Stop();
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
