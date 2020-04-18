using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private LaunchLevel managerLevel;
    private Player playerScript;
    private LevelManager levelManager;
    private bool check = true;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        managerLevel = FindObjectOfType<LaunchLevel>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (check)
        {
            foreach (GameObject collect in GameObject.FindGameObjectsWithTag("Collectible"))
            {
                levelManager.posCollectibles.Add(collect.transform.position);
            }

            levelManager.timeInLightSave = playerScript.timerInLight;

            if (managerLevel.checkpoint != gameObject.transform.position)
            {
                managerLevel.checkpoint = gameObject.transform.position;
                playerScript.CheckPoint = gameObject.transform.position;
            }
            check = false;
        }
    }

}
