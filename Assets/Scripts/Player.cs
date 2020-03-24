using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    private Rigidbody                                       rig;
    private int                                             jump = 0;
    private bool                                            isJumping = false;
    private bool                                            inShadow;
    private                                                 MeshRenderer meshPlayer;
    private List<Vector3>                                   shadowPos = new List<Vector3>();

    [Header("Movements")]
    [SerializeField] [Range(0f, 200f)] private float        speed = 0f;
    [SerializeField] [Range(100f, 1000f)] private float     jumpForce = 0f;
    [SerializeField] [Range(0, 10)] private int             numberOfJump = 0;

    [Header("Characteristics")]
    [SerializeField] private float                          life = 0f;

    [Header("Ground Checker")]
    [SerializeField] private Transform                      groundedLeft = null;
    [SerializeField] private Transform                      groundedRight = null;

    [Header("Features")]
    [SerializeField] private Transform                      directionalLight = null;

    [Header("Shadow and Light")]
    [Tooltip("Time in sec to be completely Black")]
    [SerializeField] private float                          TimerInShadow = 2f;
    [Tooltip("Time in sec to be completely White")]
    [SerializeField] private float                          TimerInLight = 6f;

    [HideInInspector] public bool umbrella;

    public float                                            colorPlayer;


    public float                                            courage = 0;
    
    


    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        SetBasicShadowPos();
        meshPlayer = gameObject.GetComponent<MeshRenderer>();
    }
    
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            umbrella = !umbrella;
        }

        if (life > 0f) // If player is alive
        {
            PlayerMovement();
            inShadow = CheckShadow();
            ColorOfPlayer();
        }
    }

    private void PlayerMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal") * speed; // Used to move player
        float Vertical = Input.GetAxis("Vertical") * speed;     // Used to hide ourself in different parts

        rig.velocity = new Vector3(Horizontal * Time.deltaTime, rig.velocity.y , Vertical * Time.deltaTime);
        //transform.Translate(Vector3.right * Horizontal * Time.deltaTime);
        //transform.Translate(Vector3.forward * Vertical * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && (jump < numberOfJump - 1 || GroundCheck() && numberOfJump > 0) && !isJumping)
        {
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z); // TODO: maybe if velocity y > 0 keep actual + new else if velocity < 0 reset to 0
            rig.AddForce(Vector3.up * jumpForce);
            jump++;
        }

        if (GroundCheck())
            jump = 0;
    }

    private bool CheckShadow()
    {
        RaycastHit[] hits;

        bool check = true;

        for (int i = 0; i < shadowPos.Count; i++)
        {
            hits = Physics.RaycastAll(transform.position - shadowPos[i], -directionalLight.forward);
            if (hits.Length > 0)
            {

                for (int j = 0; j < hits.Length; j++)
                {
                    if (!hits[j].collider.gameObject.CompareTag("Wall"))
                    {
                        Debug.DrawRay(transform.position - shadowPos[i], hits[j].point - (transform.position - shadowPos[i]), Color.green);
                    }
                    else
                    {
                        if (hits.Length < 2)
                        {
                            check = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                check = false;
            }
               
        }

        return check;
    }

    public void SetBasicShadowPos()
    {
        shadowPos.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        shadowPos.Add(new Vector3(0.5f , -0.5f, -0.5f));
        shadowPos.Add(new Vector3(0.5f , -0.5f,  0.5f));
        shadowPos.Add(new Vector3(-0.5f, -0.5f,  0.5f));

        shadowPos.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        shadowPos.Add(new Vector3(0.5f , 0.5f, -0.5f));
        shadowPos.Add(new Vector3(0.5f , 0.5f,  0.5f));
        shadowPos.Add(new Vector3(-0.5f, 0.5f,  0.5f));
    }

    private bool GroundCheck()
    {
        RaycastHit hitLeft;
        RaycastHit hitRight;

        bool isGroundedLeft = false;
        bool isGroundedRight = false;

        if (Physics.Raycast(groundedLeft.position, transform.TransformDirection(-Vector3.up), out hitLeft, 1f))
            isGroundedLeft = true;
        else
            isGroundedLeft = false;

        if (Physics.Raycast(groundedRight.position, transform.TransformDirection(-Vector3.up), out hitRight, 1f))
            isGroundedRight = true;
        else
            isGroundedRight = false;

        if (isGroundedRight || isGroundedLeft)
            return true;
        else
            return false;
    }

    private void ColorOfPlayer()
    {

        if (meshPlayer.material.color.r <= 1)
        {
            if (inShadow == false)
            {
                colorPlayer += 1 / TimerInLight * Time.deltaTime;
                meshPlayer.material.color = new Color(colorPlayer, colorPlayer, colorPlayer, 255);
            }
        }
        if (meshPlayer.material.color.r >= 0)
        {
            if (inShadow == true)
            { 
                colorPlayer -= 1 / TimerInShadow * Time.deltaTime;
                meshPlayer.material.color = new Color(colorPlayer, colorPlayer, colorPlayer, 255);
            }
        }

        if(colorPlayer > 1) // if he get this value he die 
        {
            // death of player 
            colorPlayer = 1;
            meshPlayer.material.color = new Color(1, 1, 1, 255);
        }
        if (colorPlayer < 0f)
        {
            colorPlayer = 0;
            meshPlayer.material.color = new Color(0, 0, 0, 255);
        }

    }

    public void Lose()
    {

    }
}