using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTrigger : MonoBehaviour
{
    public bool isActive = false;

    [SerializeField] private bool dirX = false;
    [SerializeField] private bool dirY = false;

    [SerializeField] private float move = 1f;
    private Vector3 minPos = Vector3.zero;
    private Vector3 maxPos = Vector3.zero;

    private void Start()
    {
        minPos = transform.position;
        if (dirX)
            maxPos = transform.position + new Vector3(move, 0f);
        else if (dirY)
            maxPos = transform.position + new Vector3(0f, move * Time.deltaTime);
    }

    private void Update()
    {
        if (isActive)
        {
            if (dirX && transform.position.x < maxPos.x)
                transform.Translate(new Vector3(move * Time.deltaTime, 0f));
            else if (dirY && transform.position.y < maxPos.y)
                transform.Translate(new Vector3(0f, move * Time.deltaTime));
        }
        else if (!isActive)
        {
            if (dirX && transform.position.x > minPos.x)
                transform.Translate(new Vector3(-move * Time.deltaTime, 0f));
            else if (dirY && transform.position.x > minPos.x)
                transform.Translate(new Vector3(0f, -move * Time.deltaTime));
        }
    }
}
