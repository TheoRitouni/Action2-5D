using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorBar : MonoBehaviour
{
    [SerializeField] private RectTransform whiteBar;
    private Player player;
    private float maxHeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxHeight = whiteBar.sizeDelta.y;
    }

    public void RefreshBar()
    {
        float calcul = maxHeight * player.colorPlayer;

        whiteBar.sizeDelta = new Vector2(whiteBar.sizeDelta.x, calcul);
    }
}
