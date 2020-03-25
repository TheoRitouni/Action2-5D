using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    [SerializeField] private float limitOfUmbrella = 15;
    [SerializeField] private float speedOfUmbrella = 2f;
    private Player player;

    private float distX = 0f;
    private float distZ = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // limiter la rotation
        if (Input.GetKey(KeyCode.M))
        {
            if (distX < limitOfUmbrella)
                distX = -speedOfUmbrella;
            gameObject.transform.RotateAround(player.transform.position, Vector3.forward, distX);
            
        }
        if (Input.GetKey(KeyCode.K))
        {
            if (distX > -limitOfUmbrella)
                distX = speedOfUmbrella;
            gameObject.transform.RotateAround(player.transform.position, Vector3.forward, distX);
            
        }
        

        if (Input.GetKey(KeyCode.O))
        {
            if (distZ < limitOfUmbrella)
                distZ = speedOfUmbrella;
            gameObject.transform.RotateAround(player.transform.position, Vector3.right, distZ);
            
        }
        if (Input.GetKey(KeyCode.L))
        {
            if (distZ > -limitOfUmbrella)
                distZ = -speedOfUmbrella;
            gameObject.transform.RotateAround(player.transform.position, Vector3.right, distZ);
            
        }
        
        
    }
}
