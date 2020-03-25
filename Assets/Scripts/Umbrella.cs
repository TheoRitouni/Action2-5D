using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    [Header("Umbrella Settings")]
    [SerializeField] [Range(0f, 90f)] private float limitOfUmbrella = 45;
    [SerializeField] [Range(0f, 20f)] private float speedOfUmbrella = 2f;
    private Player player;
    private Vector3 savePosUmbrella;
    private Quaternion saveRotUmbrella;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalLeft = Input.GetAxis("JoystickRightHorizontal") * speedOfUmbrella;
        float verticalLeft = Input.GetAxis("JoystickRightVertical") * speedOfUmbrella;

        savePosUmbrella = gameObject.transform.position;
        saveRotUmbrella = gameObject.transform.rotation;

        if ( horizontalLeft != 0)
        {
            gameObject.transform.RotateAround(player.transform.position, Vector3.forward, horizontalLeft);  
        }
        if (verticalLeft != 0)
        {
            gameObject.transform.RotateAround(player.transform.position, Vector3.right, verticalLeft);
        }


        if (gameObject.transform.rotation.eulerAngles.z > limitOfUmbrella && gameObject.transform.rotation.eulerAngles.z < 180)
        {
            gameObject.transform.position = savePosUmbrella;
            gameObject.transform.rotation = Quaternion.Euler(saveRotUmbrella.eulerAngles.x, saveRotUmbrella.eulerAngles.y, limitOfUmbrella);
        
        }
        if (gameObject.transform.rotation.eulerAngles.z <  360 - limitOfUmbrella && gameObject.transform.rotation.eulerAngles.z > 180 )
        {
            gameObject.transform.position = savePosUmbrella;
            gameObject.transform.rotation = Quaternion.Euler(saveRotUmbrella.eulerAngles.x, saveRotUmbrella.eulerAngles.y, -limitOfUmbrella);
        }

        if (gameObject.transform.rotation.eulerAngles.x > limitOfUmbrella && gameObject.transform.rotation.eulerAngles.x < 180)
        {
            gameObject.transform.position = savePosUmbrella;
            gameObject.transform.rotation = Quaternion.Euler(limitOfUmbrella, saveRotUmbrella.eulerAngles.y, saveRotUmbrella.eulerAngles.z);
        }
        if (gameObject.transform.rotation.eulerAngles.x < 360 - limitOfUmbrella && gameObject.transform.rotation.eulerAngles.x > 180)
        {
            gameObject.transform.position = savePosUmbrella;
            gameObject.transform.rotation = Quaternion.Euler(-limitOfUmbrella, saveRotUmbrella.eulerAngles.y, saveRotUmbrella.eulerAngles.z);
        }



    }
}
