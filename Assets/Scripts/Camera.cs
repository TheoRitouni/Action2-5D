using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Camera : MonoBehaviour
{
    private Player player;

    private float initialTimeFollow;
    private Vector3 PosPLayerDelay;
    private Vector3 latencyDir;

    private Vector3 posCam;
    private float posX;
    private bool dirRight = true;

    [Header("Location of Camera")]
    [SerializeField] private bool cameraFront = false;
    [SerializeField] [Range(0f, 10f)] private float speedCam = 3f;
    [SerializeField] [Range(0f, 2f)] private float limitOfCam = 0.7f;
    [Space]
    [SerializeField] private bool latency = false;
    [SerializeField] private float smoothness = 0.2f;
    [SerializeField] [Range(0f, 5f)] private float speedLatency = 1f;

    void Start()
    {
        initialTimeFollow = smoothness;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        posX = player.transform.position.x + 1f;
        PosPLayerDelay = player.transform.position;
    }

    void Update()
    {
        if (latency == true)
        {
            smoothness -= Time.deltaTime;
            if (smoothness < 0 )
            {
                smoothness = initialTimeFollow;
                latencyDir = player.transform.position - PosPLayerDelay;
            }
        }

        if (cameraFront == true)
        {
            float Horizontal = Input.GetAxis("Horizontal") * speedCam;

            if (Horizontal > 0 && dirRight == false)
            {
                dirRight = true;
            }
            if (Horizontal < 0 && dirRight == true)
            {
                dirRight = false;
            }

            if (dirRight == true)
            {
                if (posX < limitOfCam)
                {
                    posX += Horizontal * Time.deltaTime;
                    if (Horizontal == 0)
                    {
                        posX += speedCam / 2 * Time.deltaTime;
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
                        posX -= speedCam / 2 * Time.deltaTime;
                    }
                }
            }
        }

        if (latency == true && cameraFront == false)
        {
            PosPLayerDelay += latencyDir * Time.deltaTime * speedLatency ;
            transform.position = new Vector3(PosPLayerDelay.x , PosPLayerDelay.y + 4.5f, PosPLayerDelay.z - 9.5f);
            transform.LookAt(PosPLayerDelay);
        }
        if(cameraFront == true && latency == false)
        {
            posCam.x = player.transform.position.x + posX;
            posCam.y = player.transform.position.y;
            posCam.z = player.transform.position.z;
            transform.position = new Vector3(posCam.x, player.transform.position.y + 4.5f, player.transform.position.z - 9.5f);
            transform.LookAt(posCam);
        }

        if(latency == true && cameraFront == true)
        {
            PosPLayerDelay += latencyDir * Time.deltaTime * speedLatency;
            posCam.x = PosPLayerDelay.x + posX;
            posCam.y = PosPLayerDelay.y;
            posCam.z = PosPLayerDelay.z;
            transform.position = new Vector3(PosPLayerDelay.x + posX, PosPLayerDelay.y + 4.5f, PosPLayerDelay.z - 9.5f);
            transform.LookAt(posCam);
        }
    }
}
