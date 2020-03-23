using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Player player;
    private List<Vector3> posPlayer;
    private float initialTimeFollow;
    private float timeFollow = 0.2f;
    // Start is called before the first frame update

    private Vector3 posCam;
    private float posX;
    private bool dirRight = true;
    [Header("Location of Camera")]
    [SerializeField] [Range(0f, 10f)] private float speedCam = 3f;
    [SerializeField] [Range(0f, 2f)] private float limitOfCam = 0.7f;
    void Start()
    {
        initialTimeFollow = timeFollow;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        posPlayer = new List<Vector3>();
        posX = player.transform.position.x + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
        timeFollow -= Time.deltaTime;
        if(timeFollow < 0)
        {
            timeFollow = initialTimeFollow;
        }

        float Horizontal = Input.GetAxis("Horizontal") * speedCam;

        if (Horizontal > 0 && dirRight == false)
        {
            dirRight = true;
        }
        if (Horizontal < 0 && dirRight == true)
        {
            dirRight = false;
        }

        if(dirRight == true)
        {
            if (posX < limitOfCam)
            {
                posX += Horizontal * Time.deltaTime;
                if (Horizontal == 0)
                { 
                    posX += speedCam/2 * Time.deltaTime;
                }                  
            }
        }
        if (dirRight == false)
        {
            if (posX > -limitOfCam)
            {
                posX += Horizontal * Time.deltaTime;
                if (Horizontal == 0)
                {
                    posX -= speedCam/2 * Time.deltaTime;
                }
            }
        }



        posCam.x = player.transform.position.x + posX;
        posCam.y = player.transform.position.y;
        posCam.z = player.transform.position.z;
        transform.position = new Vector3(player.transform.position.x + posX, player.transform.position.y + 4.5f, player.transform.position.z - 9.5f);
        transform.LookAt(posCam);



    }
}
