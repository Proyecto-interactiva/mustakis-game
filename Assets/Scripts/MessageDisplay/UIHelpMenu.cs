using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelpMenu : MonoBehaviour
{
    public UIReturnToGuardianArrow arrow;

    private bool isFirstTimeShown;

    public void Awake()
    {
        isFirstTimeShown = true;
    }

    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        if (isFirstTimeShown) { arrow.Play(5, .5f); isFirstTimeShown = false; };
        gameObject.SetActive(false);
    }    
}
