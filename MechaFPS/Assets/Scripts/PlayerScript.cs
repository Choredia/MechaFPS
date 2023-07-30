using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Image splatterImage;
    [SerializeField] private Image hurtImage;
    [SerializeField] private Image deathImage;
    [SerializeField] private Image lastImage;
    [SerializeField] private float hurtTimer = 0.1f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float fadeInDuration = 1f; // Alfa deðerinin artýþ süresini ayarlamak için deðiþkeni ekleyin.
    [SerializeField] private float fadeOutDuration = 1f; // Alfa deðerinin azalýþ süresini ayarlamak için deðiþkeni ekleyin.

    private float full = .4f;
    private bool fade = true;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;


    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float jumpHeight = 5f;
    private float gravity = -9.81f;

    private AudioSource audioSource;
    private Vector3 movement;
    private Vector3 velocity;
    private bool isGrounded;
    private bool crouching;
    private float xValue;
    private float zValue;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;

    private float maxPlayerHealth = 100f;
    private float currentPlayerHealth;
    private float lerpSpeed;
    private bool isAlive = true;
    [SerializeField] PlayerLook look;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentPlayerHealth = maxPlayerHealth;
        audioSource = GetComponent<AudioSource>();
        hurtImage.enabled = false;
        lastImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + currentPlayerHealth + "%";
        lerpSpeed = 3f * Time.deltaTime;

        Movement();

        Jump();

        Sprint();

        Crouch();

        Heal();

        HealthBarFiller();

        HealthBarColorChanger();
    }

    private void Heal()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isAlive) { return; }
            currentPlayerHealth += 20;
            if (currentPlayerHealth > maxPlayerHealth)
            {
                currentPlayerHealth = maxPlayerHealth;
            }
        }
    }

    private void HealthBarColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (currentPlayerHealth / maxPlayerHealth));
        healthBar.color = healthColor;
    }

    private void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentPlayerHealth / maxPlayerHealth, lerpSpeed);
    }

    private void Movement()
    {
        if (!isAlive) return;
        xValue = Input.GetAxisRaw("Horizontal");
        zValue = Input.GetAxisRaw("Vertical");

        movement = transform.right * xValue + transform.forward * zValue;

        if (movement != Vector3.zero)
        {
            characterController.Move(movement.normalized * playerSpeed * Time.deltaTime);
            playerAnimator.SetFloat("Speed", 0.7f);
            playerAnimator.SetBool("isMoving", true);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
        else
        {
            audioSource.Stop();
            if (!crouching)
            {
                playerAnimator.SetBool("isMoving", false);
            }

        }

    }

    private void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            playerAnimator.SetBool("Jump", false);
        }
        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            playerSpeed = sprintSpeed;
            playerAnimator.SetFloat("Speed", 1f);
        }
        else
        {
            playerSpeed = walkSpeed;
            playerAnimator.SetFloat("Speed", 0f);
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerSpeed = crouchSpeed;
            crouching = true;
            characterController.center = new Vector3(0, 0.04f, 0);
            characterController.radius = 0.78f;
            characterController.height = 1.5f;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            playerSpeed = walkSpeed;
            crouching = false;
            characterController.center = new Vector3(0, 0.33f, 0);
            characterController.radius = 0.5f;
            characterController.height = 2.3f;
            playerAnimator.SetBool("isCrouching", crouching);
        }

    }
    private void UpdateHealth()
    {
        Color splatterAlpha = splatterImage.color;
        splatterAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        splatterImage.color = splatterAlpha;
        if (currentPlayerHealth > 0)
        {
            splatterImage.enabled = true;
            StartCoroutine(FadeOutSplatterImage(1f));
        }
    }

    IEnumerator FadeOutSplatterImage(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        Color startColor = splatterImage.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);

            splatterImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Color finalColor = splatterImage.color;
        finalColor.a = 0f;
        splatterImage.color = finalColor;
        splatterImage.enabled = false;
    }
    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }
    public void TakeDamage(float amount)
    {
        if (!isAlive)
            return;

        currentPlayerHealth -= amount;

        if (currentPlayerHealth > 0)
        {
            StartCoroutine(HurtFlash());
            UpdateHealth();
        }
        else if (currentPlayerHealth <= 0f)
        {
            hurtImage.enabled = true;

            isAlive = false;
            Die();
        }


    }
    void Die()
    {
        playerAnimator.SetBool("Death", true);
        hurtImage.enabled = true;
       

        StartCoroutine(FadeInDeathImage()); // Ölme anýnda ölüm efekti için "deathImage" nesnesinin alfasýný artýran bir iþlev çaðýrýn.
    }

    IEnumerator FadeInDeathImage()
    {
        while (fade) 
        {
            hurtImage.enabled = true;

            deathImage.enabled = true;
            float elapsedTime = 0f;

            // Fading In (Artýþ)
            while (elapsedTime < fadeInDuration)
            {
                float alpha = Mathf.Lerp(0f, full, elapsedTime / fadeInDuration);
                deathImage.color = new Color(deathImage.color.r, deathImage.color.g, deathImage.color.b, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Fading Out (Azalýþ)
            while (elapsedTime < (fadeInDuration + fadeOutDuration))
            {
                float alpha = Mathf.Lerp(full, 0f, (elapsedTime - fadeInDuration) / fadeOutDuration);
                deathImage.color = new Color(deathImage.color.r, deathImage.color.g, deathImage.color.b, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (full >= 0.6f)
            {
                fade = false;
                StartCoroutine(FadeToBlack());
            }
            else
            {
                full += 0.1f;
                deathImage.enabled = false; // Efekt tamamlandýktan sonra nesneyi devre dýþý býrakýn.
            }
        }


    }

    IEnumerator FadeToBlack()
    {
        // Ekrana geçiþ için beklemek istediðiniz süreyi burada ayarlayabilirsiniz (örneðin, 5 saniye için: yield return new WaitForSeconds(5f);)

        lastImage.enabled = true;
        float elapsedTime = 0f;

        // Tamamen siyah hale gelene kadar ekrana geçiþ yapýn
        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutDuration);
            lastImage.color = new Color(lastImage.color.r, lastImage.color.g, lastImage.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Eðer buraya ulaþýlýrsa oyunu yeniden baþlatmak veya farklý bir iþlem yapmak isteyebilirsiniz.
    }

}
