using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LaunchLevel launchManager;
    private Player playerScript;
    private GameObject player;

    public List<Vector3> posCollectibles = new List<Vector3>();
    public List<Vector3> localposCollectibles = new List<Vector3>();
    public List<Vector3> scaleCollectibles = new List<Vector3>();
    public List<Quaternion> rotCollectibles = new List<Quaternion>();
    public List<Transform> parentCollectibles = new List<Transform>();
    public float timeInLightSave = 0f;
    public float courageSave = 0f;

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
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player");

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

    public void Checkpoint()
    {
        if (playerScript.CheckPoint != new Vector3(0,0,0))
        {

            foreach(GameObject collectibles in GameObject.FindGameObjectsWithTag("Collectible"))
            {
                Destroy(collectibles);
            }
            for(int i = 0; i < posCollectibles.Count; i++)
            {
                GameObject tempObject = Instantiate(Resources.Load("Prefabs/Pref_Collectible"),posCollectibles[i],rotCollectibles[i]) as GameObject;
                tempObject.tag = "Collectible";
                tempObject.transform.localScale = scaleCollectibles[i];
                tempObject.transform.parent = parentCollectibles[i];
                if(tempObject.transform.parent != null)
                {
                    tempObject.transform.localPosition = localposCollectibles[i]; 
                }
            }

            playerScript.timerInLight = timeInLightSave;
            playerScript.colorPlayer = 0;
            player.transform.position = playerScript.CheckPoint;
            playerScript.Courage = courageSave;
            dead = false;
            deadScreen.SetActive(false);
        }
        else
        {
            launchManager.RestartLevel();
        }

    }
}
