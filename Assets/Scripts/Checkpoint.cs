using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private LaunchLevel managerLevel;

    void Start()
    {
        managerLevel = FindObjectOfType<LaunchLevel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (managerLevel.checkpoint != gameObject.transform.position)
            managerLevel.checkpoint = gameObject.transform.position;
    }

}
