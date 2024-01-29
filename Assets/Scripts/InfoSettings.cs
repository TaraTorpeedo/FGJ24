using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoSettings : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    public void Start()
    {
        toggle.isOn = IsMicApproved();

        toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(toggle);
        });

        Debug.Log(IsMicApproved());
    }

    public bool IsMicApproved()
    {
        if (PlayerPrefs.HasKey("UseMic"))
        {
            Debug.Log(PlayerPrefs.GetInt("UseMic"));
            if (PlayerPrefs.GetInt("UseMic") == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void ToggleValueChanged(Toggle change)
    {
        toggle.isOn = change.isOn;

        if (change.isOn == true)
        {
            PlayerPrefs.SetInt("UseMic", 1);
        }
        else
        {
            PlayerPrefs.SetInt("UseMic", 0);
        }
    }
}
