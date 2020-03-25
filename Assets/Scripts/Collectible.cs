using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float addCourage = 1f;
    [Header("Color")]
    [SerializeField] private bool changeOfColor = false;
    [SerializeField] [Range(1f,10f)] private float speedChangeColor = 3f;
    private Player player;
    private MeshRenderer meshRend;
    private float maxColor = 255 ;
    private float r , g , b ;


    [Header("Movement")]
    [SerializeField] private bool movement = false;
    [SerializeField] [Range(0f, 1f)] private float dist = 0.2f;
    [SerializeField] [Range(0f, 1f)] private float speedCollectible = 0.2f;
    private Vector3 initialPos ;
    private bool direction = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        meshRend = gameObject.GetComponent<MeshRenderer>();
        g = 0;
        b = 1;
        r = maxColor;
        initialPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
        Movement();      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.courage += addCourage;
            Destroy(gameObject);
        }
    }

    private void ChangeColor()
    {
        if (changeOfColor)
        {
            // change value of R
            if (b == 0 && g == maxColor) // green to yellow
            {
                r += speedChangeColor;
            }
            if (b == maxColor && g == 0) // purple to blue
            {
                r -= speedChangeColor;
            }
            if (r > maxColor)
            {
                r = maxColor;
            }
            if (r < 0)
            {
                r = 0;
            }
            // change value of G
            if (b == 0 && r == maxColor) // yellow to red 
            {
                g -= speedChangeColor;
            }
            if (r == 0 && b == maxColor) // blue to cyan 
            {
                g += speedChangeColor;
            }
            if (g > maxColor)
            {
                g = maxColor;
            }
            if (g < 0)
            {
                g = 0;
            }

            // change value of B
            if (r == 0 && g == maxColor) // cyan to green
            {
                b -= speedChangeColor;
            }
            if (r == maxColor && g == 0) // red to purple
            {
                b += speedChangeColor;
            }
            if (b > maxColor)
            {
                b = maxColor;
            }
            if (b < 0)
            {
                b = 0;
            }

            // apply color
            meshRend.material.color = new Color(r / 255, g / 255, b / 255, 255);
        }
    }

    private void Movement()
    {
        if(movement)
        {
            if (gameObject.transform.position.y > initialPos.y + dist)
                direction = true;
            if (gameObject.transform.position.y < initialPos.y - dist)
                direction = false;

            if (direction == false)
                gameObject.transform.Translate(new Vector3(0, speedCollectible * Time.deltaTime, 0));
            if (direction == true)
                gameObject.transform.Translate(new Vector3(0, -speedCollectible * Time.deltaTime, 0));
        }
    }
}
