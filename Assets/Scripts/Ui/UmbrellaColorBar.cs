using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmbrellaColorBar : MonoBehaviour
{
    private LevelManager levelManager;

    [SerializeField] private Image colorImageBar;

    [SerializeField] private RectTransform rectWhiteBar;
    [SerializeField] private Player player;
    public float part;

    private float timer = 0.75f;

    void Awake()
    {
        part = rectWhiteBar.sizeDelta.y / player.maxCourage;
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (!levelManager.pause)
        {
            if (timer > 0f)
                timer -= Time.deltaTime;
            else
            {
                timer = 0.75f;
                colorImageBar.color = Random.ColorHSV(0, 1, 1, 1, 0.75f, 1, 1, 1);
            }
        }
    }

    public void RefreshBar()
    {
        float calcul = part * player.Courage;

        rectWhiteBar.sizeDelta = new Vector2(rectWhiteBar.sizeDelta.x, calcul);
    }
}
