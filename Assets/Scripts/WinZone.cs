using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private LevelManager levelManager;
    private AudioSource asWin;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        asWin = gameObject.AddComponent<AudioSource>();
        asWin.volume = 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !levelManager.win)
        {
            levelManager.win = true;
            asWin.PlayOneShot(Resources.Load("Sounds/Win") as AudioClip);
        }
    }
}
