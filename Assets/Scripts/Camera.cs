using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Player player;
    private List<Vector3> listPosPlayer;
    private float initialTimeFollow;
    [SerializeField] private float timeFollow = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        initialTimeFollow = timeFollow;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        listPosPlayer = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        timeFollow -= Time.deltaTime;
        if(timeFollow < 0)
        {
            timeFollow = initialTimeFollow;
        }
        transform.position = new Vector3(player.transform.position.x , player.transform.position.y + 4.5f, player.transform.position.z - 9.5f);
        transform.LookAt(player.transform);
    }
}
