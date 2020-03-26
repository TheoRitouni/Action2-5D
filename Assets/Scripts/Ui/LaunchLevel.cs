using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchLevel : MonoBehaviour
{
    public int actualLevel = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int level)
    {
        actualLevel = level;
        SceneManager.LoadSceneAsync(actualLevel);
    }

    public void LoadnextLevel()
    {
        actualLevel = actualLevel + 1;
        SceneManager.LoadSceneAsync(actualLevel);
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
        actualLevel = 0;
        
        SceneManager.LoadSceneAsync(0);
        Destroy(gameObject);
    }
}
