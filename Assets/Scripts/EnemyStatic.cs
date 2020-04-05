using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : MonoBehaviour
{
    private LevelManager levelManager;
    private Light spotlight;
    private bool playerInFov = false;
    private Transform meshChild; 

    [SerializeField] private bool showDebug = false;
    private Transform player;

    [Header("View")]
    [Tooltip("Sphere radius")]
    [SerializeField] [Range(5f, 50f)] private float maxRadius = 0f;
    [Tooltip("Between the purple vector and one of blue vector, so the ° between the 2 blue vectors is (Max Radius x2)")]
    [SerializeField] [Range(1f, 85f)] private float maxAngle = 0f;

    [Header("Light")]
    [SerializeField] private bool light = true;

    [Header("Movement")]
    [SerializeField] private bool dirX;
    [SerializeField] private bool MovementOnX = false;
    [SerializeField] private bool dirY;
    [SerializeField] private float speedMove = 1f;
    [SerializeField] [Range(0f, 55f)] private float maxRotation;
    [SerializeField] private bool positive, negative;

    private float rotationValue = 0f;
    private bool direction = false;
    private float initialYValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spotlight = gameObject.GetComponentInChildren<Light>();
        spotlight.range = maxRadius;
        spotlight.innerSpotAngle = maxAngle * 2;
        if(!light)
        {
            spotlight.enabled = light; 
        }
        meshChild = gameObject.transform.GetChild(0);
        initialYValue = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (!levelManager.pause)
        {
            if(dirX)
                dirY = false;
            if(dirY)
                dirX = false;

            playerInFov = InFov();
            MovementCalc();
            AddRotationOnAxis();

            if (playerInFov)
            {
                if (levelManager.dead)
                    return;

                levelManager.dead = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
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

        return false;
    }

    private void MovementCalc()
    {
        if(positive && !negative)
        {
            if (rotationValue > maxRotation)
                direction = true;
            if (rotationValue <  0)
                direction = false;

            if(direction)
                rotationValue -= speedMove * Time.deltaTime;
            else
                rotationValue += speedMove * Time.deltaTime;
        }
        if (!positive && negative)
        {
            if (rotationValue < -maxRotation)
                direction = true;
            if (rotationValue > 0)
                direction = false;

            if (direction)
                rotationValue += speedMove * Time.deltaTime;
            else
                rotationValue -= speedMove * Time.deltaTime;
        }
        if (positive && negative)
        {
            if (rotationValue > maxRotation)
                direction = true;
            if (rotationValue < -maxRotation)
                direction = false;

            if (direction)
                rotationValue -= speedMove * Time.deltaTime;
            else
                rotationValue += speedMove * Time.deltaTime;
        }
    }

    private void AddRotationOnAxis()
    {
        if(dirX)
        {
            gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x - rotationValue, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            
            if(!MovementOnX)
                meshChild.rotation = Quaternion.Euler(meshChild.rotation.x, 0, 0);

        }
        if (dirY)
        {
            gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, (transform.rotation.y + rotationValue) + initialYValue, transform.rotation.eulerAngles.z);
            
        }
    }

}
