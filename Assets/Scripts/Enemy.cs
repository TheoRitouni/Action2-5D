using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private LevelManager levelManager;

    private NavMeshAgent navAgent = null;
    private int indexMovementA = 0;
    private int indexMovementB = 0;
    private float errorMovementMargin = 0f;
    private bool playerInFov = false;
    private Player playerScript;
    public bool pathA = true;
    [SerializeField] private bool showDebug = false;
    [SerializeField] private Transform player;


    [Header("Basics info")]
    [SerializeField] [Range(0f, 10f)] private float speed = 0f;
	[SerializeField] [Range(0f, 10f)] private float speedFollowPlayer = 0f;
    [Tooltip("(Path A) Movement point in order, it close the path alone, dont make last point in the first")]
    [SerializeField] private Transform[] movementPointA = null;
    [Tooltip("(Path B) Movement point in order, it close the path alone, dont make last point in the first")]
    [SerializeField] private Transform[] movementPointB = null;

    [Header("View")]
    [Tooltip("Sphere radius")]
    [SerializeField] [Range(5f, 50f)] private float maxRadius = 0f;
    [Tooltip("Between the purple vector and one of blue vector, so the ° between the 2 blue vectors is (Max Radius x2)")]
    [SerializeField] [Range(1f, 85f)] private float maxAngle = 0f;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        navAgent.speed = speed;
        errorMovementMargin = 0.01f;
    }

    void Update()
    {
        if (!levelManager.pause && !levelManager.win)
        {
            playerInFov = InFov();

            if (navAgent.speed != speed && !playerInFov)
                navAgent.speed = speed;

            if (playerInFov && !playerScript.inShadow && !levelManager.dead)
            {
				if (navAgent.speed != speedFollowPlayer)
					navAgent.speed = speedFollowPlayer;
				
                navAgent.SetDestination(player.position);
            }
            else if (pathA && movementPointA.Length > 1)
            {
                if (IsOnMovementPoint())
                {
                    indexMovementA++;
                    if (indexMovementA == movementPointA.Length)
                        indexMovementA = 0;
                }
                else if (navAgent.velocity.x == 0f)
                {
                    navAgent.SetDestination(movementPointA[indexMovementA].position);
                }
            }
            else if (!pathA && movementPointB.Length > 1)
            {
                if (IsOnMovementPoint())
                {
                    indexMovementB++;
                    if (indexMovementB == movementPointB.Length)
                        indexMovementB = 0;
                }
                else if (navAgent.velocity.x == 0f)
                {
                    navAgent.SetDestination(movementPointB[indexMovementB].position);
                }
            }
        }
        else if (navAgent.speed != 0)
            navAgent.speed = 0;
        
    }

    private bool IsOnMovementPoint()
    {
        if (pathA)
        {
            if (transform.position.x >= movementPointA[indexMovementA].position.x - errorMovementMargin && transform.position.x <= movementPointA[indexMovementA].position.x + errorMovementMargin &&
                transform.position.z >= movementPointA[indexMovementA].position.z - errorMovementMargin && transform.position.z <= movementPointA[indexMovementA].position.z + errorMovementMargin)
                return true;
        }
        else
        {
            if (transform.position.x >= movementPointB[indexMovementB].position.x - errorMovementMargin && transform.position.x <= movementPointB[indexMovementB].position.x + errorMovementMargin &&
                transform.position.z >= movementPointB[indexMovementB].position.z - errorMovementMargin && transform.position.z <= movementPointB[indexMovementB].position.z + errorMovementMargin)
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            if (pathA)
            {
                Gizmos.color = Color.black;

                for (int i = 0; i < movementPointA.Length; i++)
                {
                    if (i + 1 != movementPointA.Length)
                        Gizmos.DrawRay(movementPointA[i].position, movementPointA[i + 1].position - movementPointA[i].position);
                    else
                        Gizmos.DrawRay(movementPointA[i].position, movementPointA[0].position - movementPointA[i].position);
                }
            }
            else
            {
                Gizmos.color = Color.white;

                for (int i = 0; i < movementPointB.Length; i++)
                {
                    if (i + 1 != movementPointB.Length)
                        Gizmos.DrawRay(movementPointB[i].position, movementPointB[i + 1].position - movementPointB[i].position);
                    else
                        Gizmos.DrawRay(movementPointB[i].position, movementPointB[0].position - movementPointB[i].position);
                }
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxRadius);

            Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;
            Vector3 fovLine3 = Quaternion.AngleAxis(-maxAngle + 90, transform.right) * transform.up * maxRadius;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);
            Gizmos.DrawRay(transform.position, fovLine3);

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, transform.forward * maxRadius);


            if (player != null)
            {
                if (!playerInFov)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.green;

                Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);
            }
        }

    }

    private bool InFov()
    {
        /*Collider[] overlaps = Physics.OverlapSphere(transform.position, maxRadius);

        for (int i = 0; i < overlaps.Length; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == player)
                {*/
                    Vector3 directionBetween = (player.position - transform.position).normalized;

                    float angle = Vector3.Angle(transform.forward, directionBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(transform.position, player.position - transform.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == player)
                                return true;
                        }
                    }

               /* }
            }
        }*/

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (levelManager.dead)
            return;

        if (collision.gameObject.CompareTag("Player"))
            levelManager.dead = true;
    }
}
