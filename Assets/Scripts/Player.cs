using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    private Rigidbody                   rig;
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

    //[Header("Features")]

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (life > 0f) // If player is alive
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal") * speed; // Used to move player
        float Vertical = Input.GetAxis("Vertical");             // Used to hide ourself in different parts


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

}