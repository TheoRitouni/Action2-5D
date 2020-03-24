using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool showDebug = false;

    [SerializeField] private Color onColor = Color.green;
    [SerializeField] private Color offColor = Color.red;

    [SerializeField] private List<Platform> platforms = new List<Platform>();
    [SerializeField] private List<OneTimeTrigger> oneTimeTriggers = new List<OneTimeTrigger>();
    [SerializeField] private List<Enemy> ennemies = new List<Enemy>();

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
                    Gizmos.DrawWireSphere(platforms[i].transform.position, 1f);
                }
                else
                {
                    Gizmos.color = offColor;
                    Gizmos.DrawWireSphere(platforms[i].transform.position, 1f);
                }
            }

            for (int i = 0; i < oneTimeTriggers.Count; i++)
            {
                if (oneTimeTriggers[i].isActive)
                {
                    Gizmos.color = onColor;
                    Gizmos.DrawWireSphere(oneTimeTriggers[i].transform.position, 1f);
                }
                else
                {
                    Gizmos.color = offColor;
                    Gizmos.DrawWireSphere(oneTimeTriggers[i].transform.position, 1f);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("CircleButton") && other.CompareTag("Player"))
        {
            ChangeState();
        }
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
