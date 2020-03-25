using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmbrellaColorBar : MonoBehaviour
{
    [SerializeField] private Image colorImageBar;

    [SerializeField] private RectTransform rectWhiteBar;
    private Player player;
    private float part;

    private float timer = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        part = rectWhiteBar.sizeDelta.y / player.maxCourage;
    }
    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
        else
        {
            timer = 0.75f;
            colorImageBar.color = Random.ColorHSV(0, 1, 1, 1, 0.75f, 1, 1, 1);
        }
    }

    public void RefreshBar()
    {
        float calcul = part * player.Courage;

        rectWhiteBar.sizeDelta = new Vector2(rectWhiteBar.sizeDelta.x, calcul);
    }
}
