using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject warning;

    public void OpenCredits()
    {
        FindObjectOfType<AudioManager>().Play("Open");
        credits.SetActive(true);
    }

    public void ExitGameWithWarning()
    {
        FindObjectOfType<AudioManager>().Play("Open");
        warning.SetActive(true);
    }

    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        gameObject.SetActive(false);
    }
}
