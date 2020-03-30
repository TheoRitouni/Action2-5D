using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private LevelManager levelManager;

    private Rigidbody                                       rig;
    private int                                             jump = 0;
    private bool                                            isJumping = false;
    private                                                 MeshRenderer meshPlayer;
    private List<Vector3>                                   shadowPos = new List<Vector3>();

    [Header("Movements")]
    [SerializeField] [Range(0f, 1000f)] private float        speed = 0f;
    [SerializeField] [Range(100f, 1000f)] private float     jumpForce = 0f;
    [SerializeField] [Range(0, 10)] private int             numberOfJump = 0;

    [Header("Ground Checker")]
    [SerializeField] private Transform                      groundedLeft = null;
    [SerializeField] private Transform                      groundedRight = null;

    [Header("Features")]
    [SerializeField] private Transform                      directionalLight = null;

    [Header("Shadow and Light")]
    [Tooltip("Time in sec to be completely Black")]
    [SerializeField] private float                          timerInShadow = 2f;
    [Tooltip("Time in sec to be completely White")]
    [SerializeField] private float                          timerInLight = 6f;


    private bool umbrella = false;
    [Space]
    [Header("Umbrella")]
    [SerializeField] private GameObject umbrel = null;
    [SerializeField] private float timerUmbrella = 1f;
    [SerializeField] private PlayerColorBar barPlayer;
    [SerializeField] private UmbrellaColorBar barUmbrella;
    [SerializeField] [Range(1f,2f)] private float fallOfPlaner = 1.2f;
    [SerializeField] [Range(1f, 7f)] private float speedOfPlaner = 2f;
    private float initialTimerUmbrella = 0f;
    private bool planer = false;
    private bool umbrellaJump = false;
    [Space]


    public bool                                             inShadow;

    [HideInInspector] public float                          colorPlayer;
    private float                                           courage = 0f;
    public float Courage { 
        get { return courage; } 
        set { if (value > maxCourage) courage = maxCourage; else courage = value; barUmbrella.RefreshBar(); } 
    }

    public float maxCourage = 0f;

    [Header("Camera Zoom")]
    [SerializeField] private float distanceToRoof = 2f;
    private bool roofAbovePlayer = false;
    public bool RoofOnPlayer { get { return roofAbovePlayer; } }

    private bool squat = false;
    private float sizeSquat = 0.5f;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        SetBasicShadowPos();
        meshPlayer = gameObject.GetComponent<MeshRenderer>();
        initialTimerUmbrella = timerUmbrella;
        Courage = 0;
    }
    
    void Update()
    {
        if (!levelManager.dead && !levelManager.pause) // If player is alive
        {
            if (!squat)
            {
                UmbrellaActiveOrNot();
            }
            PlayerMovement();
            PlayerSquat();
            inShadow = CheckShadow();
            ColorOfPlayer();
            RoofAbovePLayer();
        }
    }

    private void PlayerMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal") * speed; // Used to move player
        float Vertical = Input.GetAxis("Vertical") * speed;     // Used to hide ourself in different parts

        rig.velocity = new Vector3(Horizontal * Time.deltaTime, rig.velocity.y , Vertical * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && (jump < numberOfJump - 1 || GroundCheck() && numberOfJump > 0) && !isJumping)
        {
            if(jump == 0) // umbrella off if you jump
            {
                umbrellaJump = true;
            }
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z); // TODO: maybe if velocity y > 0 keep actual + new else if velocity < 0 reset to 0
            rig.AddForce(Vector3.up * jumpForce);
            jump++;
        }

        if (GroundCheck())
        {
            jump = 0;
        }
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
        if (squat == false)
        {
            if (shadowPos.Count != 0)
            {
                shadowPos.Clear();
            }
            shadowPos.Add(new Vector3(-0.49f, -0.49f, -0.49f));
            shadowPos.Add(new Vector3(0.49f, -0.49f, -0.49f));
            shadowPos.Add(new Vector3(0.49f, -0.49f, 0.49f));
            shadowPos.Add(new Vector3(-0.49f, -0.49f, 0.49f));

            shadowPos.Add(new Vector3(-0.49f, 0.49f, -0.49f));
            shadowPos.Add(new Vector3(0.49f, 0.49f, -0.49f));
            shadowPos.Add(new Vector3(0.49f, 0.49f, 0.49f));
            shadowPos.Add(new Vector3(-0.49f, 0.49f, 0.49f));
        }
        else
        {
            if (shadowPos.Count != 0)
            {
                shadowPos.Clear();
            }
            shadowPos.Add(new Vector3(-0.49f, -0.49f + sizeSquat/2, -0.49f));
            shadowPos.Add(new Vector3(0.49f, -0.49f + sizeSquat/2, -0.49f));
            shadowPos.Add(new Vector3(0.49f, -0.49f + sizeSquat/2, 0.49f));
            shadowPos.Add(new Vector3(-0.49f, -0.49f + sizeSquat/2, 0.49f));

            shadowPos.Add(new Vector3(-0.49f, 0.49f - sizeSquat/2, -0.49f));
            shadowPos.Add(new Vector3(0.49f, 0.49f - sizeSquat/2, -0.49f));
            shadowPos.Add(new Vector3(0.49f, 0.49f - sizeSquat/2, 0.49f));
            shadowPos.Add(new Vector3(-0.49f, 0.49f - sizeSquat/2, 0.49f));
        }
    }

    private bool GroundCheck()
    {
        RaycastHit hitLeft;
        RaycastHit hitRight;

        bool isGroundedLeft = false;
        bool isGroundedRight = false;

        if (Physics.Raycast(groundedLeft.position, transform.TransformDirection(-Vector3.up), out hitLeft, 0.51f))
            isGroundedLeft = true;
        else
            isGroundedLeft = false;

        if (Physics.Raycast(groundedRight.position, transform.TransformDirection(-Vector3.up), out hitRight, 0.51f))
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
                colorPlayer += 1 / timerInLight * Time.deltaTime;
                meshPlayer.material.color = new Color(colorPlayer, colorPlayer, colorPlayer, 255);
                barPlayer.RefreshBar();
            }
        }
        if (meshPlayer.material.color.r >= 0)
        {
            if (inShadow == true)
            { 
                colorPlayer -= 1 / timerInShadow * Time.deltaTime;
                meshPlayer.material.color = new Color(colorPlayer, colorPlayer, colorPlayer, 255);
                barPlayer.RefreshBar();
            }
        }

        if(colorPlayer > 1) // if he get this value he die 
        {
            colorPlayer = 1;
            meshPlayer.material.color = new Color(1, 1, 1, 255);
            barPlayer.RefreshBar();
            Lose();
        }
        if (colorPlayer < 0f)
        {
            colorPlayer = 0;
            meshPlayer.material.color = new Color(0, 0, 0, 255);
            barPlayer.RefreshBar();
        }

    }

    public void Lose()
    {
        levelManager.dead = true;
    }

    public void UmbrellaActiveOrNot()
    {
        if (timerUmbrella > 0)
        {
            timerUmbrella -= Time.deltaTime;
        }
        if ((Input.GetAxis("RightTrigger") > 0 && timerUmbrella < 0 ) || (umbrellaJump == true && umbrella == true))
        {
            if(umbrellaJump == true)
            {
                umbrellaJump = false;
            }

            timerUmbrella = initialTimerUmbrella;
            umbrella = !umbrella;
            umbrel.SetActive(umbrella);

            if(umbrella == true )
            {
                speed = speed / 2;
            }
            else
            {
                // reset plane if you tunr off umbrella in air 
                if(planer == true)
                {
                    planer = false;
                    speed = speed / speedOfPlaner;
                }
                umbrel.transform.rotation = Quaternion.Euler(0, 0, 0);
                umbrel.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
                speed = speed * 2;

            }
        }

        // Planer
        if (umbrella == true && GroundCheck() == false)
        {
            rig.velocity = new Vector3(rig.velocity.x, rig.velocity.y / fallOfPlaner, rig.velocity.z);
            if (planer == false)
            {
                planer = true;
                speed = speed * speedOfPlaner;
            }
        }
        if (planer == true && GroundCheck() == true)
        {
            planer = false;
            speed = speed / speedOfPlaner;
        }
    }

    private void RoofAbovePLayer()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,Vector3.up, out hit, distanceToRoof))
        {
            if (!hit.collider.gameObject.CompareTag("Umbrella"))
            {
                roofAbovePlayer = true;
                Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
            }
            else
            {
                roofAbovePlayer = false;
            }
        }
        else
        {
            roofAbovePlayer = false;
        }

    }

    private void PlayerSquat()
    {
        if(Input.GetButtonDown("CircleButton"))
        {
            Transform saveParent = gameObject.transform.parent;
            gameObject.transform.parent = null;
            // manage umbrella with squat
            if (umbrella == true)
            {
                timerUmbrella = initialTimerUmbrella;
                umbrella = !umbrella;
                umbrel.SetActive(umbrella);

                if (planer == true)
                {
                    planer = false;
                    speed = speed / speedOfPlaner;
                }

                umbrel.transform.rotation = Quaternion.Euler(0, 0, 0);
                umbrel.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
                speed = speed * 2;
            }

            // squat
            if (gameObject.transform.localScale.y > sizeSquat)
            {
               
                gameObject.transform.localScale = new Vector3(transform.localScale.x , sizeSquat, transform.localScale.z);
                gameObject.transform.Translate(0, - sizeSquat / 2, 0);
                squat = true;
                SetBasicShadowPos();
                speed = speed / 2;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
                squat = false;
                SetBasicShadowPos();
                speed = speed * 2;
            }

            gameObject.transform.parent = saveParent;
        }
    }
}