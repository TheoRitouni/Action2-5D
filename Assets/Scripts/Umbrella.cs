using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    private LevelManager levelManager;

    [Header("Umbrella Settings")]
    [SerializeField] [Range(0f, 90f)] private float limitOfUmbrella = 45;
    [SerializeField] [Range(0f, 20f)] private float speedOfUmbrella = 2f;

    private Player player;
    private Vector3 savePosUmbrella;
    private Quaternion saveRotUmbrella;
    private Quaternion rotation;
    private float saveRotPlayerH = 0f;
    private float saveRotPlayerV = 0f;

    private void Awake()
    {
        //rotation = transform.rotation;
        levelManager = FindObjectOfType<LevelManager>();
    }

    void LateUpdate()
    {
        //transform.rotation = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelManager.pause && !levelManager.dead && !levelManager.win)
        {
            float horizontalLeft = Input.GetAxis("JoystickRightHorizontal") * speedOfUmbrella;
            float verticalLeft = Input.GetAxis("JoystickRightVertical") * speedOfUmbrella;

            float rotPlayerY = player.transform.rotation.eulerAngles.y;

            if (Input.GetKey(KeyCode.M))
                horizontalLeft = -1;
            if (Input.GetKey(KeyCode.K))
                horizontalLeft = 1;
            if (Input.GetKey(KeyCode.L))
                verticalLeft = -1;
            if (Input.GetKey(KeyCode.O))
                verticalLeft = 1;

            savePosUmbrella = gameObject.transform.position;
            saveRotUmbrella = gameObject.transform.rotation;

            if (horizontalLeft != 0)
            {
                gameObject.transform.RotateAround(gameObject.transform.parent.position, Vector3.forward, horizontalLeft);
            }
            if (verticalLeft != 0)
            {
                gameObject.transform.RotateAround(gameObject.transform.parent.position, Vector3.right, verticalLeft);
            }

            if (gameObject.transform.rotation.eulerAngles.z > limitOfUmbrella && gameObject.transform.rotation.eulerAngles.z < 180)
            {
                gameObject.transform.position = savePosUmbrella;
                gameObject.transform.rotation = Quaternion.Euler(saveRotUmbrella.eulerAngles.x, saveRotUmbrella.eulerAngles.y, limitOfUmbrella);

            }
            if (gameObject.transform.rotation.eulerAngles.z < 360 - limitOfUmbrella && gameObject.transform.rotation.eulerAngles.z > 180)
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
}
