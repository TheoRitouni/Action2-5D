using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private bool platformMove = false;
    [SerializeField] private bool dirX = false, dirY = false;
    [SerializeField] private float platformSpeed = 0.1f;
    [SerializeField] private float platformDist = 1f; 
    [Space]
    [SerializeField] private bool platformTurn = false;
    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private bool timer = false;
    [SerializeField] private float time = 2f;
    [Space]
    [SerializeField] private bool platformTurnAround = false;
    [SerializeField] Vector3 pointPos;
    [SerializeField] private float turnAroundSpeed = 20f;
    [Space]
    [SerializeField] private bool platformDestroy = false;
    [SerializeField] private float destroyTimer = 2f;
    [SerializeField] private float respawnTimer = 2f;

    private Vector3 initialPos;
    private bool direction = false;

    private float initialTime;

    private bool platAtDestroy = false;
    private float initialDestroyTime;
    private float initialRespawnTime;

    

    // Start is called before the first frame update
    void Start()
    {
        initialPos = gameObject.transform.position ;
        initialTime = time;
        initialDestroyTime = destroyTimer;
        initialRespawnTime = respawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyPlatform();

        // platform move on axes Y or X ;

        if (platformMove == true)
        {
            if (dirX == true)
            {
                if (gameObject.transform.position.x > initialPos.x + platformDist)
                    direction = true;
                if (gameObject.transform.position.x < initialPos.x - platformDist)
                    direction = false;

                if (direction == false)
                    gameObject.transform.Translate(new Vector3(platformSpeed * Time.deltaTime, 0, 0));
                if (direction == true)
                    gameObject.transform.Translate(new Vector3(-platformSpeed * Time.deltaTime, 0, 0));
            }

            if (dirY == true)
            {
                if (gameObject.transform.position.y > initialPos.y + platformDist)
                    direction = true;
                if (gameObject.transform.position.y < initialPos.y - platformDist)
                    direction = false;

                if (direction == false)
                    gameObject.transform.Translate(new Vector3(0, platformSpeed * Time.deltaTime, 0));
                if (direction == true)
                    gameObject.transform.Translate(new Vector3(0, -platformSpeed * Time.deltaTime, 0));
            }
        }

        // platform turn with a timer
        if(platformTurn == true)
        {
            if (timer == false)
                gameObject.transform.Rotate(new Vector3(0, 0, turnSpeed * Time.deltaTime));
            else
            {
                time -= Time.deltaTime ;

                if(time < 0)
                {
                    gameObject.transform.Rotate(new Vector3(0, 0, turnSpeed));
                    time = initialTime;
                }
            }
        }

        // platform Turn Around a point
        if(platformTurnAround == true)
        {
            gameObject.transform.RotateAround(pointPos, new Vector3(0, 0, 1), turnAroundSpeed * Time.deltaTime);
            gameObject.transform.Rotate(new Vector3(0, 0, -turnAroundSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(platformDestroy == true)
        {
            // destroy if you have a contact with platform
            if (platAtDestroy == false)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    platAtDestroy = true;
                }
            }
        }
    }

    private void DestroyPlatform()
    {
        if (platformDestroy == true)
        {
            // destroy if you have a contact with platform
            if (platAtDestroy == true)
            {
                destroyTimer -= Time.deltaTime;
                if (destroyTimer < 0)
                {
                    platAtDestroy = false;
                    destroyTimer = initialDestroyTime;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }

            // respawn platform
            if (gameObject.GetComponent<MeshRenderer>().enabled == false)
            {
                respawnTimer -= Time.deltaTime;
                if (respawnTimer < 0)
                {
                    respawnTimer = initialRespawnTime;
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
    }
}
