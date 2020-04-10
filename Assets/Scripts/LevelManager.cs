using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LaunchLevel launchManager;

    public bool dead = false;
    public bool pause = false;
    public bool win = false;

    [SerializeField] private GameObject deadScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
        launchManager = FindObjectOfType<LaunchLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (win && !winScreen.activeSelf && !pauseScreen.activeSelf)
            winScreen.SetActive(true);

        if (dead && !deadScreen.activeSelf && !pauseScreen.activeSelf)
            deadScreen.SetActive(true);
        
        if (Input.GetButtonDown("PauseButton") && !pauseScreen.activeSelf && !deadScreen.activeSelf)
        {
            pause = true;
            pauseScreen.SetActive(true);
        }
        else if (Input.GetButtonDown("PauseButton") && pauseScreen.activeSelf && !deadScreen.activeSelf)
        {
            pause = false;
            pauseScreen.SetActive(false);
        }
    }

    public void GoMainMenu()
    {
        launchManager.LoadMainMenu();
    }

    public void RestartLevel()
    {
        launchManager.RestartLevel();
    }

    public void NextLevel()
    {
        launchManager.LoadnextLevel();
    }
}
