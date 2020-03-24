using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent navAgent = null;
    private float naturalSpeed;
    private int indexMovement = 0;
    private float errorMovementMargin = 0f;
    private bool playerInFov = false;
    private Player playerScript;
    [SerializeField] private bool showDebug = false;
    [SerializeField] private Transform player;


    [Header("Basics info")]
    [SerializeField] [Range(0f, 10f)] private float speed = 0f;
    [Tooltip("If color of player if higher than this or equal, enemy go on him (speed * colorPlayer)")]
    [SerializeField] [Range(0f, 1f)] private float seeColorPlayer = 0f;
    [Tooltip("Movement point in order, it close the path alone, dont make last point in the first")]
    [SerializeField] private Transform[] movementPoint = null;

    [Header("View")]
    [Tooltip("Sphere radius")]
    [SerializeField] [Range(5f, 50f)] private float maxRadius = 0f;
    [Tooltip("Between the purple vector and one of blue vector, so the ° between the 2 blue vectors is (Max Radius x2)")]
    [SerializeField] [Range(10f, 85f)] private float maxAngle = 0f;
    

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        navAgent.speed = speed;
        naturalSpeed = speed;
        errorMovementMargin = 0.01f;
    }

    void Update()
    {
        playerInFov = InFov();

        if (navAgent.speed != speed)
            navAgent.speed = speed;

        if (playerInFov && playerScript.colorPlayer >= seeColorPlayer)
        {
            speed = naturalSpeed * playerScript.colorPlayer;
            navAgent.SetDestination(player.position);
        }
        else if (movementPoint.Length > 1)
        {
            if (speed != naturalSpeed)
            {
                speed = naturalSpeed;
                navAgent.speed = speed;
            }

            if (IsOnMovementPoint())
            {
                indexMovement++;
                if (indexMovement == movementPoint.Length)
                    indexMovement = 0;
            }
            else if (navAgent.velocity.x == 0f)
            {
                navAgent.SetDestination(movementPoint[indexMovement].position);
            }
        }
    }

    private bool IsOnMovementPoint()
    {
        if (transform.position.x >= movementPoint[indexMovement].position.x - errorMovementMargin && transform.position.x <= movementPoint[indexMovement].position.x + errorMovementMargin &&
            transform.position.z >= movementPoint[indexMovement].position.z - errorMovementMargin && transform.position.z <= movementPoint[indexMovement].position.z + errorMovementMargin)
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.black;

            for (int i = 0; i < movementPoint.Length; i++)
            {
                if (i + 1 != movementPoint.Length)
                    Gizmos.DrawRay(movementPoint[i].position, movementPoint[i + 1].position - movementPoint[i].position);
                else
                    Gizmos.DrawRay(movementPoint[i].position, movementPoint[0].position - movementPoint[i].position);
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
}
