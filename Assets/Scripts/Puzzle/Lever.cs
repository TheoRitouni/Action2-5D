using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool showDebug = false;

    [SerializeField] private Color onColor = Color.green;
    [SerializeField] private Color offColor = Color.red;

    [Space]
    [Tooltip("Lever = true, Pressure PLate = false")]
    [SerializeField] private bool isLever = true;

    [SerializeField] private List<Platform> platforms = new List<Platform>();
    [SerializeField] private List<OneTimeTrigger> oneTimeTriggers = new List<OneTimeTrigger>();
    [SerializeField] private List<Enemy> ennemies = new List<Enemy>();

    [SerializeField] private List<GameObject> objectsOnPlate = new List<GameObject>();

    private bool isOn = false;

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].isActive)
                {
                    Gizmos.color = onColor;
                    Gizmos.DrawWireSphere(platforms[i].transform.position, 2f);
                }
                else
                {
                    Gizmos.color = offColor;
                    Gizmos.DrawWireSphere(platforms[i].transform.position, 2f);
                }
            }

            for (int i = 0; i < oneTimeTriggers.Count; i++)
            {
                if (oneTimeTriggers[i].isActive)
                {
                    Gizmos.color = onColor;
                    Gizmos.DrawWireSphere(oneTimeTriggers[i].transform.position, 2f);
                }
                else
                {
                    Gizmos.color = offColor;
                    Gizmos.DrawWireSphere(oneTimeTriggers[i].transform.position, 2f);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isLever)
        {
            if (Input.GetButtonDown("CircleButton") && other.CompareTag("Player"))
            {
                
                ChangeState();
            }
        }
        else
        {
            if (!objectsOnPlate.Contains(other.gameObject))
                objectsOnPlate.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsOnPlate.Contains(other.gameObject))
            objectsOnPlate.Remove(other.gameObject);

        if (!isLever && objectsOnPlate.Count == 0)
            ChangeState();
}

    private void Update()
    {
        if (!isLever && !isOn && objectsOnPlate.Count > 0)    
            ChangeState();

    }

    private void ChangeState()
    {
        isOn = !isOn;
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].isActive = !platforms[i].isActive;
        }

        for (int i = 0; i < oneTimeTriggers.Count; i++)
        {
            oneTimeTriggers[i].isActive = !oneTimeTriggers[i].isActive;
        }

        for (int i = 0; i < ennemies.Count; i++)
        {
            ennemies[i].pathA = !ennemies[i].pathA;
        }
    }
}
