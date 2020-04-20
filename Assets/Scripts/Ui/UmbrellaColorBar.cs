using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmbrellaColorBar : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Player player;

    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void RefreshBar()
    {
        text.text = player.Courage.ToString() + " / " + player.maxCourage.ToString();
    }
}
