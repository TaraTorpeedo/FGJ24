using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject CreditsMenu;
    [SerializeField] private GameObject InfoMenu;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameManager manager;

    private List<string> list = new List<string>();

    public void Start()
    {
        if (dropdown)
        {
            DropdownValues();
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        manager.StartGame();
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
        StartMenu.SetActive(false);
        InfoMenu.SetActive(false);
    }

    public void OpenInfo()
    {
        InfoMenu.SetActive(true);
        StartMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void PressBackButton()
    {
        StartMenu.SetActive(true);
        gameObject.gameObject.SetActive(false);
    }

    public void DropdownValues()
    {
        dropdown.ClearOptions();

        for (int i = 0; i< Microphone.devices.Length; i++)
        {
            list.Add(Microphone.devices[i]);
        }

        dropdown.AddOptions(list);
        dropdown.RefreshShownValue();
    }

    public void LoadLevelAgain() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void QuitGame() => Application.Quit();
}
