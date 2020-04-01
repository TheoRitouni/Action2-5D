using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private LevelManager levelManager;

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
    [SerializeField] [Range(0f, 5f)] private float limitOfCam = 0.7f;
    [Space]
    [SerializeField] private bool latency = false;
    [SerializeField] private float smoothness = 0.2f;
    [SerializeField] [Range(0f, 5f)] private float speedLatency = 1f;
    //private float moveDur = 0f;
    //[SerializeField] private float TimeLatency = 6f;
    //private float initSpeedLatency = 0f;
    //[SerializeField] private AnimationCurve curveLatency;

    [Header("Zoom of Camera")]
    [SerializeField] [Range(1f, 3f)] private float powerZoom = 2f;
    [SerializeField] [Range(1f, 20f)] private float speedZoom = 5f;
    private float distCamY = 4.5f;
    private float distCamZ = 9.5f;
    private float initialDistCamY;
    private float initialDistCamZ;


    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        initialTimeFollow = smoothness;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        posX = player.transform.position.x + limitOfCam;
        PosPLayerDelay = player.transform.position;
        initialDistCamY = distCamY;
        initialDistCamZ = distCamZ;
        //initSpeedLatency = speedLatency;
    }

    void Update()
    {
        // curve for smoothness //

        //if (PosPLayerDelay.magnitude < player.transform.position.magnitude + 0.01 && PosPLayerDelay.magnitude > player.transform.position.magnitude - 0.01)
        //{
        //    moveDur = 0;
        //}
        //moveDur += Time.deltaTime;
        //speedLatency = curveLatency.Evaluate(moveDur/ TimeLatency) * initSpeedLatency;
        
        // ========= // 
        if (!levelManager.dead && !levelManager.pause)
        { 
            if(player.RoofOnPlayer)
            {
                if(distCamY > initialDistCamY / powerZoom)
                {
                    distCamY -= speedZoom  / distCamZ * Time.deltaTime;
                }
                if (distCamZ > initialDistCamZ / powerZoom)
                {
                    distCamZ -= speedZoom  / distCamY * Time.deltaTime;
                }
            }
            else
            {
                if (distCamY < initialDistCamY)
                {
                    distCamY += speedZoom / distCamZ * Time.deltaTime;
                }
                if (distCamZ < initialDistCamZ)
                {
                    distCamZ += speedZoom / distCamY * Time.deltaTime;
                }
            }

            if (latency == true)
            {
                smoothness -= Time.deltaTime;
                if (smoothness < 0)
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
                transform.position = new Vector3(PosPLayerDelay.x , PosPLayerDelay.y + distCamY, PosPLayerDelay.z - distCamZ);
                transform.LookAt(PosPLayerDelay);
            }
            else if(cameraFront == true && latency == false)
            {
                posCam.x = player.transform.position.x + posX;
                posCam.y = player.transform.position.y;
                posCam.z = player.transform.position.z;
                transform.position = new Vector3(posCam.x, player.transform.position.y + distCamY, player.transform.position.z - distCamZ);
                transform.LookAt(posCam);
            }
            else if (latency == true && cameraFront == true)
            {
                PosPLayerDelay += latencyDir * Time.deltaTime * speedLatency;
                posCam.x = PosPLayerDelay.x + posX;
                posCam.y = PosPLayerDelay.y;
                posCam.z = PosPLayerDelay.z;
                transform.position = new Vector3(PosPLayerDelay.x + posX, PosPLayerDelay.y + distCamY, PosPLayerDelay.z - distCamZ);
                transform.LookAt(posCam);
            }

        }


    }
}
