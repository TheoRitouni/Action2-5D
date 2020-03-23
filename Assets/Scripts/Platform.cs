using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool platformMove = false;
    public bool dirX = false, dirY = false;
    public float platformSpeed = 0.1f;
    public float platformDist = 1f;
    public AnimationCurve moveCurve = null;
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
    public float respawnTimer = 2f;

    private Vector3 initialPos;
    private bool direction = false;

    private float initialTime;

    private bool platAtDestroy = false;
    private float initialDestroyTime;
    private float initialRespawnTime;

    
    void Start()
    {
        initialPos = gameObject.transform.position;
        initialTime = time;
        initialDestroyTime = destroyTimer;
        initialRespawnTime = respawnTimer;
        distParcouru = 0.5f;
    }

    void Update()
    {
        DestroyPlatform();

        // platform move on axes Y or X ;

        if (platformMove == true)
        {
            if (dirX == true)
            {
                if (gameObject.transform.position.x >= initialPos.x + platformDist / 2)
                {
                    direction = true;
                }
                if (gameObject.transform.position.x <= initialPos.x - platformDist / 2)
                {
                    direction = false;
                }

                if (direction == false)
                {
                    float curve = moveCurve.Evaluate(distParcouru / (initialPos.x + platformDist / 2));
                    gameObject.transform.Translate(new Vector3(platformSpeed * curve * Time.deltaTime, 0, 0));
                    distParcouru = transform.position.x - platformDist / 2 - initialPos.x;
                }
                if (direction == true)
                {
                    Debug.Log(distParcouru / (initialPos.x - platformDist / 2));
                    float curve = moveCurve.Evaluate(distParcouru / (initialPos.x - platformDist / 2));
                    gameObject.transform.Translate(new Vector3(-platformSpeed * curve * Time.deltaTime, 0, 0));
                    distParcouru = initialPos.x - platformDist / 2 - transform.position.x;
                }

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

[CustomEditor(typeof(Platform)), CanEditMultipleObjects]
public class PlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (Platform)target;

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
            script.dirY = EditorGUILayout.Toggle(script.dirY);
            GUILayout.EndHorizontal();

            if (!script.dirX && !script.dirY)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                EditorGUILayout.HelpBox("Choose one direction at least", MessageType.Error);
                GUILayout.EndHorizontal();
            }

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
            GUILayout.Label("Respawn Timer", GUILayout.Width(120));
            script.respawnTimer = EditorGUILayout.FloatField(script.respawnTimer);
            GUILayout.EndHorizontal();
        }

    }
}