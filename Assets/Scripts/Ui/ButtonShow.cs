using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShow : MonoBehaviour
{
    [SerializeField] private GameObject firstMenu;
    [SerializeField] private GameObject menuShow;

    [SerializeField] private List<Button> buttonsSelection;

    private void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for (int i = 0; i < buttonsSelection.Count; i++)
        {
            if (i + 1 > levelAt)
                buttonsSelection[i].interactable = false;
        }
    }

    public void ShowMenu()
    {
        firstMenu.SetActive(!firstMenu.activeSelf);
        menuShow.SetActive(!menuShow.activeSelf);
    }

    public void ResetLevel()
    {
        if (PlayerPrefs.GetInt("levelAt", 1) != 1)
        {
            PlayerPrefs.SetInt("levelAt", 1);
            for (int i = 0; i < buttonsSelection.Count; i++)
            {
                if (i + 1 > 1)
                    buttonsSelection[i].interactable = false;
            }
        }
    }
}
