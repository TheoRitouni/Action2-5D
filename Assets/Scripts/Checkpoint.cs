using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private LaunchLevel managerLevel;
    private Player playerScript;
    private LevelManager levelManager;
    private bool check = true;
    private MeshRenderer flag;

    private AudioSource asCheckpoint;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        managerLevel = FindObjectOfType<LaunchLevel>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        flag = gameObject.transform.GetChild(0).transform.GetChild(2).GetComponent<MeshRenderer>();
        asCheckpoint = gameObject.AddComponent<AudioSource>();
        asCheckpoint.volume = 0.1f;
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


            flag.material.color = new Color(0, 255, 0);
            
            asCheckpoint.PlayOneShot(Resources.Load("Sounds/Checkpoint") as AudioClip);

            check = false;
        }
    }

   

}
