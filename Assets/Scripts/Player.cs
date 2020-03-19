using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    private Rigidbody                   rig;
    private bool                        isHiddingPlaceBehind = false;
    private bool                        isHiddingPlaceBefore = false;

    private int                         jump = 0;



    [Header("Movements")]
    [SerializeField] [Range(0f, 20f)] private float      speed = 0f;
    [SerializeField] [Range(100f, 1000f)] private float jumpForce = 0f;
    [SerializeField] [Range(0, 10)] private int        numberOfJump = 0;

    [Header("Characteristics")]
    [SerializeField] private float      life = 0f;

    [Header("Ground Checker")]
    [SerializeField] private Transform  groundedLeft = null;
    [SerializeField] private Transform  groundedRight = null;

    [Header("Hidding Place Checker")]
    [SerializeField] private Transform  hiddingBehind = null;
    [SerializeField] private Transform  hiddingBefore = null;

    [Header("Features")]
    [SerializeField] private bool       hiddenBefore = false;
    [SerializeField] private bool       hiddenBehind = false;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (life > 0f) // If player is alive
        {
            HiddingPlaceCheck();
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal") * speed; // Used to move player
        float Vertical = Input.GetAxis("Vertical");             // Used to hide ourself in different parts

        if (!hiddenBefore && !hiddenBehind)
        {
            transform.Translate(Vector3.right * Horizontal * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && (jump < numberOfJump - 1 || GroundCheck() && numberOfJump > 0))
            {
                rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z); // TODO: maybe if velocity y > 0 keep actual + new else if velocity < 0 reset to 0
                rig.AddForce(Vector3.up * jumpForce);
                jump++;
            }

            if (GroundCheck())
                jump = 0;        
        }

        if (Vertical >= 0.75f || Vertical <= -0.75f)
            HidePlayer(Vertical);
    }

    private void HidePlayer(float Vertical)
    {
        if (!hiddenBefore && !hiddenBehind)
        {
            if (Vertical >= 0.75f && isHiddingPlaceBehind)
            {
                hiddenBehind = true;
                transform.Translate(new Vector3(0f, 0f, 4f));
            }
            else if (Vertical <= -0.75f && isHiddingPlaceBefore)
            {
                hiddenBefore = true;
                transform.Translate(new Vector3(0f, 0f, -4f));
            }
        }

        if (hiddenBefore)
        {
            if (Vertical >= 0.75f)
            {
                hiddenBefore = false;
                transform.Translate(new Vector3(0f, 0f, 4f));
            }
        }

        if (hiddenBehind)
        {
            if (Vertical <= -0.75f)
            {
                hiddenBehind = false;
                transform.Translate(new Vector3(0f, 0f, -4f));
            }
        }
    }

    private bool GroundCheck()
    {
        RaycastHit hitLeft;
        RaycastHit hitRight;

        bool isGroundedLeft = false;
        bool isGroundedRight = false;

        if (Physics.Raycast(groundedLeft.position, transform.TransformDirection(-Vector3.up), out hitLeft))
            isGroundedLeft = false;
        else
            isGroundedLeft = true;

        if (Physics.Raycast(groundedRight.position, transform.TransformDirection(-Vector3.up), out hitRight))
            isGroundedRight = false;
        else
            isGroundedRight = true;

        if (isGroundedRight || isGroundedLeft)
            return true;
        else
            return false;
    }

    private void HiddingPlaceCheck()
    {
        RaycastHit hitBehind;
        RaycastHit hitBefore;

        if (Physics.Raycast(hiddingBefore.position, transform.TransformDirection(-Vector3.forward), out hitBefore))
        {
            if (hitBefore.collider.gameObject.CompareTag("HiddingPlace"))
                isHiddingPlaceBefore = true;
            else
                isHiddingPlaceBefore = false;
        }
        else
            isHiddingPlaceBefore = false;

        if (Physics.Raycast(hiddingBehind.position, transform.TransformDirection(Vector3.forward), out hitBehind))
        {
            if (hitBehind.collider.gameObject.CompareTag("HiddingPlace"))
                isHiddingPlaceBehind = true;
            else
                isHiddingPlaceBehind = false;
        }
        else
            isHiddingPlaceBehind = false;
    }
}