using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    private GameObject player;

    public List<Vector3> pos;

    public Transform directional = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < pos.Count; i++)
        {

            pos[i] = new Vector3(pos[i].x * transform.localScale.x, pos[i].y * transform.localScale.y, pos[i].z * transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckShadows();
    }

    private void CheckShadows()
    {
        RaycastHit hit;

        for (int i = 0; i < pos.Count; i++)
        {
            if (Physics.Raycast(transform.position + pos[i], directional.forward, out hit))
            {
                Debug.DrawRay(transform.position + pos[i], hit.point - (transform.position + pos[i]), Color.green);
            }
        }
        for (int i = 0; i < pos.Count; i++)
        {
            if (Physics.Raycast(transform.position + pos[i], directional.forward, out hit))
            {
                Debug.DrawRay(transform.position + pos[i], hit.point - (transform.position + pos[i]), Color.green);
            }
        }
    }
}
