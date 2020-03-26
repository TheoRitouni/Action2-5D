using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerChoose : MonoBehaviour
{
    public bool leftRight = false;
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private GameObject uiButton;

    private int buttonsIndex = 0;
    private float smooth = 0.25f;

    private void Start()
    {
        uiButton.GetComponent<RectTransform>().sizeDelta = buttons[buttonsIndex].GetComponent<RectTransform>().sizeDelta + new Vector2(20f, 20f);
        uiButton.transform.position = buttons[buttonsIndex].transform.position;
    }

    void Update()
    {
        if (smooth > 0f)
            smooth -= Time.deltaTime;
        else if (leftRight)
        {
            if (Input.GetButtonDown("Jump"))
                ButtonClick();

            float Horizontal = Input.GetAxis("Horizontal");
            if (Horizontal >= 0.75f)
            {
                if (buttonsIndex + 1 == buttons.Count)
                    buttonsIndex = 0;
                else
                    buttonsIndex++;

                uiButton.GetComponent<RectTransform>().sizeDelta = buttons[buttonsIndex].GetComponent<RectTransform>().sizeDelta + new Vector2(20f, 20f);
                uiButton.transform.position = buttons[buttonsIndex].transform.position;
                smooth = 0.25f;
            }
            else if (Horizontal <= -0.75f)
            {
                if (buttonsIndex - 1 == -1)
                    buttonsIndex = buttons.Count - 1;
                else
                    buttonsIndex--;

                uiButton.GetComponent<RectTransform>().sizeDelta = buttons[buttonsIndex].GetComponent<RectTransform>().sizeDelta + new Vector2(20f, 20f);
                uiButton.transform.position = buttons[buttonsIndex].transform.position;
                smooth = 0.25f;
            }
        }
        else if (!leftRight)
        {
            if (Input.GetButtonDown("Jump"))
                ButtonClick();

            float Horizontal = -Input.GetAxis("Vertical");
            if (Horizontal >= 0.75f)
            {
                if (buttonsIndex + 1 == buttons.Count)
                    buttonsIndex = 0;
                else
                    buttonsIndex++;

                uiButton.GetComponent<RectTransform>().sizeDelta = buttons[buttonsIndex].GetComponent<RectTransform>().sizeDelta + new Vector2(20f, 20f);
                uiButton.transform.position = buttons[buttonsIndex].transform.position;
                smooth = 0.25f;
            }
            else if (Horizontal <= -0.75f)
            {
                if (buttonsIndex - 1 == -1)
                    buttonsIndex = buttons.Count - 1;
                else
                    buttonsIndex--;

                uiButton.GetComponent<RectTransform>().sizeDelta = buttons[buttonsIndex].GetComponent<RectTransform>().sizeDelta + new Vector2(20f, 20f);
                uiButton.transform.position = buttons[buttonsIndex].transform.position;
                smooth = 0.25f;
            }
        }
    }

    public void Reset()
    {
        uiButton.transform.position = Vector3.zero;
    }

    private void ButtonClick()
    {
        buttons[buttonsIndex].GetComponent<Button>().onClick.Invoke();
    }
}
