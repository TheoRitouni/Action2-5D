using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchLevel : MonoBehaviour
{
    public int actualLevel = 0;
    [HideInInspector] public Vector3 checkpoint;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int level)
    {
        checkpoint = Vector3.zero;
        actualLevel = level;
        SceneManager.LoadSceneAsync(actualLevel);
    }

    public void LoadnextLevel()
    {
        checkpoint = Vector3.zero;
        actualLevel = actualLevel + 1;

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            LoadMainMenu();
        }
        else
        {
            if (actualLevel > PlayerPrefs.GetInt("levelAt"))
                PlayerPrefs.SetInt("levelAt", actualLevel);
            SceneManager.LoadSceneAsync(actualLevel);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(actualLevel);
    }

    public void LoadMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            if (actualLevel > PlayerPrefs.GetInt("levelAt"))
                PlayerPrefs.SetInt("levelAt", actualLevel);
            actualLevel = 0;
            checkpoint = Vector3.zero;
        }
        
        SceneManager.LoadSceneAsync(0);
        Destroy(gameObject);
    }
}
