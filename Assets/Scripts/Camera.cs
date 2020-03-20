using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x , player.transform.position.y + 4, player.transform.position.z - 13);
        transform.LookAt(player.transform);
    }
}
