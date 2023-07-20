using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;


    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float jumpHeight = 5f;
    private float gravity = -9.81f;

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

        HealthBarFiller();

        HealthBarColorChanger();
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
        xValue = Input.GetAxisRaw("Horizontal");
        zValue = Input.GetAxisRaw("Vertical");

        movement = transform.right * xValue + transform.forward * zValue;

        if (movement != Vector3.zero)
        {
            // Karakter hareket ediyor
            characterController.Move(movement.normalized * playerSpeed * Time.deltaTime);
            playerAnimator.SetFloat("Speed", 0.7f);
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
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
    public void TakeDamage(float amount)
    {
        if (!isAlive)
            return;

        currentPlayerHealth -= amount;


        if (currentPlayerHealth <= 0f)
        {
            isAlive = false;
            Die();
        }

        void Die()
        {
            playerAnimator.SetBool("Death", true);
        }
    }
}
