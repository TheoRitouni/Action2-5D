using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    [SerializeField] private float limitOfUmbrella = 0.8f;
    [SerializeField] private float speedOfUmbrella = 0.01f;
    private Player player;
    private Rigidbody rig;

    private float distX = 0f;
    private float distZ = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rig = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        rig.velocity = rig.velocity / 2f;

        if (Input.GetKey(KeyCode.M))
        {
            if (distX < limitOfUmbrella)
                distX += speedOfUmbrella;
        }
        if (Input.GetKey(KeyCode.K))
        {
            if (distX > -limitOfUmbrella)
                distX -= speedOfUmbrella;
        }
        else
        { 
            if( distX < 0)
                distX += speedOfUmbrella;
            if (distX > 0)
                distX -= speedOfUmbrella;
        }

        if (Input.GetKey(KeyCode.O))
        {
            if (distZ < limitOfUmbrella)
                distZ += speedOfUmbrella;
        }
        if (Input.GetKey(KeyCode.L))
        {
            if (distZ > -limitOfUmbrella)
                distZ -= speedOfUmbrella;
        }
        else
        {
            if (distZ < 0)
                distZ += speedOfUmbrella;
            if (distZ > 0)
                distZ -= speedOfUmbrella;
        }


        gameObject.transform.position = new Vector3(player.transform.position.x + distX, player.transform.position.y + 0.9f, player.transform.position.z + distZ);
    }
}
