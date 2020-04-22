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
            levelManager.posCollectibles.Clear();
            levelManager.rotCollectibles.Clear();
            levelManager.scaleCollectibles.Clear();
            levelManager.parentCollectibles.Clear();
            levelManager.localposCollectibles.Clear();

            foreach (GameObject collect in GameObject.FindGameObjectsWithTag("Collectible"))
            {
                levelManager.posCollectibles.Add(collect.transform.position);
                levelManager.rotCollectibles.Add(collect.transform.rotation);
                levelManager.scaleCollectibles.Add(collect.transform.localScale);
                levelManager.parentCollectibles.Add(collect.transform.parent);
                levelManager.localposCollectibles.Add(collect.transform.localPosition);
            }

            levelManager.timeInLightSave = playerScript.timerInLight;
            levelManager.courageSave = playerScript.Courage;

            if (managerLevel.checkpoint != gameObject.transform.position)
            {
                managerLevel.checkpoint = gameObject.transform.position;
                playerScript.CheckPoint = gameObject.transform.position;
            }
            check = false;
        }
    }

}
