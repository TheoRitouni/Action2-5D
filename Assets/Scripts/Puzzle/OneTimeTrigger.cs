using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTrigger : MonoBehaviour
{
    public bool isActive = false;
    private bool max = false;

    [SerializeField] private bool dirX = false;
    [SerializeField] private bool dirY = false;

    [SerializeField] private float move = 1f;
    [SerializeField] private float time = 1f;
    private float theTime = 0f;

    private void Start()
    {
        theTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && !max)
        {

        }
        else if (isActive && max)
        {

        }
    }
}
