using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float pistolDamage = 5f;
    [SerializeField] float rifleDamage = 10f;
    [SerializeField] float heavyDamage = 15f;

    [SerializeField] float range = 100f;

    [SerializeField] float rifleFireRate = 15f;
    [SerializeField] float heavyFireRate = 10f;



    public float impactForce = 30f;



    private float nextTimeToFire = 0f;

    private Camera cam;
    private Animator animator;

    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject fleshEffect;

    public Transform firePoint;
    public float bulletSpeed = 10f;


    public int magazineSize = 25;
    public TextMeshProUGUI ammunitionDisplay;
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
            ammunitionDisplay.SetText(bulletsLeft + " / " + magazineSize);
        }

        SelectedWeapon();


        if (Input.GetButtonDown("Fire1") )
        {
            if (bulletsLeft == 0) 
            { Debug.Log("mermikalmadý");
                return; }
            FireBullet();

            //Shoot();
        }

    }

    private void SelectedWeapon()
    {
        switch (WeaponSwitch.selectedWeapon)
        {
            case 0:
                damage = pistolDamage;
                break;
            case 1:
                damage = rifleDamage;
                break;
            case 2:
                damage = heavyDamage;
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

        Invoke("DisableBullet", 2f); // Örneðin, 2 saniye sonra mermiyi devre dýþý býrakmak için Invoke kullanabilirsiniz.
        
    }

    private void DisableBullet()
    {
        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.SetActive(false);
    }


   

    

   




    
    
}
