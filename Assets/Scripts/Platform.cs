using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private LevelManager levelManager;

    public bool platformMove = false;
    public bool dirX = false, dirY = false, dirZ = false;
    public float platformSpeed = 0.1f;
    public float platformDist = 1f;
    public AnimationCurve moveCurve = null;
    public bool startDirLeft = false;
    private float distParcouru = 0f;
    
    public bool platformTurn = false;
    public float turnSpeed = 20f;
    public bool timer = false;
    public float time = 2f;
    
    public bool platformTurnAround = false;
    public Vector3 pointPos = Vector3.zero;
    public float turnAroundSpeed = 20f;

    public bool platformDestroy = false;
    public float destroyTimer = 2f;
    public bool platformRespawn = false;
    public float respawnTimer = 2f;

    private Vector3 initialPos;
    private bool direction = false;

    private float initialTime;

    private bool platAtDestroy = false;
    private float initialDestroyTime;
    private float initialRespawnTime;

    public bool bumper = false;
    private bool playerOnBumper = false;
    public float bumperForce = 500;

    public bool isActive = false;

    private Player player;
    private Rigidbody rig;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rig = player.GetComponent<Rigidbody>();
        initialPos = gameObject.transform.position;
        initialTime = time;
        initialDestroyTime = destroyTimer;
        initialRespawnTime = respawnTimer;
        distParcouru = 0.5f;
        direction = startDirLeft;
    }

    void Update()
    {
        if (isActive && !levelManager.pause)
        {
            DestroyPlatform();
            BumperPlatform();

            // platform move on axes Y or X ;

            if (platformMove == true)
            {
                if (dirX == true)
                {
                    if (gameObject.transform.position.x > initialPos.x + platformDist )    
                        direction = true;                  
                    if (gameObject.transform.position.x < initialPos.x  - platformDist)                   
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

                if (dirZ == true)
                {
                    if (gameObject.transform.position.z > initialPos.z + platformDist)
                        direction = true;
                    if (gameObject.transform.position.z < initialPos.z - platformDist)
                        direction = false;

                    if (direction == false)
                        gameObject.transform.Translate(new Vector3(0, 0, platformSpeed * Time.deltaTime));
                    if (direction == true)
                        gameObject.transform.Translate(new Vector3(0, 0, -platformSpeed * Time.deltaTime));
                }
            }

            // platform turn with a timer
            if (platformTurn == true)
            {
                if (timer == false)
                    gameObject.transform.Rotate(new Vector3(0, 0, turnSpeed * Time.deltaTime));
                else
                {
                    time -= Time.deltaTime;

                    if (time < 0)
                    {
                        gameObject.transform.Rotate(new Vector3(0, 0, turnSpeed));
                        time = initialTime;
                    }
                }
            }

            // platform Turn Around a point
            if (platformTurnAround == true)
            {
                gameObject.transform.RotateAround(pointPos, new Vector3(0, 0, 1), turnAroundSpeed * Time.deltaTime);
                gameObject.transform.Rotate(new Vector3(0, 0, -turnAroundSpeed * Time.deltaTime));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (platformDestroy == true)
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

        if (collision.gameObject.CompareTag("Player"))
        {
            if (bumper)
            {
                playerOnBumper = true;
            }
            player.transform.parent = transform;
        }

        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bumper)
            {
                playerOnBumper = false;
            }
            player.transform.parent = null;
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

            if (platformRespawn)
            {
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

    private void BumperPlatform()
    {
        if(playerOnBumper)
        {
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z); 
            rig.AddForce(Vector3.up * bumperForce);   
        }


        if (rig.velocity.y > bumperForce / 53 && bumper)
        {
            rig.velocity = new Vector3(rig.velocity.x, bumperForce / 53, rig.velocity.z);
        }

    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Platform)), CanEditMultipleObjects]
public class PlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (Platform)target;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Is Active", GUILayout.Width(130));
        script.isActive = EditorGUILayout.Toggle(script.isActive);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Moving Platform", GUILayout.Width(130));
        script.platformMove = EditorGUILayout.Toggle(script.platformMove);
        GUILayout.EndHorizontal();

        if (script.platformMove)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("DirX", GUILayout.Width(35));
            script.dirX = EditorGUILayout.Toggle(script.dirX, GUILayout.Width(30));
            GUILayout.Label("DirY", GUILayout.Width(35));
            script.dirY = EditorGUILayout.Toggle(script.dirY, GUILayout.Width(30));
            GUILayout.Label("DirZ", GUILayout.Width(35));
            script.dirZ = EditorGUILayout.Toggle(script.dirZ);
            GUILayout.EndHorizontal();

            if (!script.dirX && !script.dirY && !script.dirZ)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                EditorGUILayout.HelpBox("Choose one direction at least", MessageType.Error);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Starting Negative Direction", GUILayout.Width(130));
            script.startDirLeft = EditorGUILayout.Toggle(script.startDirLeft);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Speed", GUILayout.Width(120));
            script.platformSpeed = EditorGUILayout.FloatField(script.platformSpeed);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Platform Distance", GUILayout.Width(120));
            script.platformDist = EditorGUILayout.FloatField(script.platformDist);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Speed Curve", GUILayout.Width(120));
            script.moveCurve = EditorGUILayout.CurveField(script.moveCurve);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("You need to fill the curve between 0 and 1 in X axis", MessageType.Info);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Turning Platform", GUILayout.Width(130));
        script.platformTurn = EditorGUILayout.Toggle(script.platformTurn);
        GUILayout.EndHorizontal();

        if (script.platformTurn)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Speed", GUILayout.Width(120));
            script.turnSpeed = EditorGUILayout.FloatField(script.turnSpeed);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Timer", GUILayout.Width(120));
            script.timer = EditorGUILayout.Toggle(script.timer);
            GUILayout.EndHorizontal();

            if (script.timer)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Time", GUILayout.Width(120));
                script.time = EditorGUILayout.FloatField(script.time);
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Turn Around Platform", GUILayout.Width(130));
        script.platformTurnAround = EditorGUILayout.Toggle(script.platformTurnAround);
        GUILayout.EndHorizontal();

        if (script.platformTurnAround)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            script.pointPos = EditorGUILayout.Vector3Field("Position point", script.pointPos);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Speed", GUILayout.Width(120));
            script.turnAroundSpeed = EditorGUILayout.FloatField(script.turnAroundSpeed);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Destroyable Platform", GUILayout.Width(130));
        script.platformDestroy = EditorGUILayout.Toggle(script.platformDestroy);
        GUILayout.EndHorizontal();

        if (script.platformDestroy)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Destroy Timer", GUILayout.Width(120));
            script.destroyTimer = EditorGUILayout.FloatField(script.destroyTimer);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Respawn Platform", GUILayout.Width(120));
            script.platformRespawn = EditorGUILayout.Toggle(script.platformRespawn);
            GUILayout.EndHorizontal();

            if (script.platformRespawn)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label("Respawn Timer", GUILayout.Width(120));
                script.respawnTimer = EditorGUILayout.FloatField(script.respawnTimer);
                GUILayout.EndHorizontal();
            }
        }

        // bumper
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Bumper Platform", GUILayout.Width(130));
        script.bumper = EditorGUILayout.Toggle(script.bumper);
        GUILayout.EndHorizontal();

        if(script.bumper)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Bumper Force", GUILayout.Width(120));
            script.bumperForce = EditorGUILayout.FloatField(script.bumperForce);
            GUILayout.EndHorizontal();
        }

    }
}

#endif