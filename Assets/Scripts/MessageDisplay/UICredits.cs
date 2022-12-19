using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICredits : MonoBehaviour
{
    public string link;

    public void OpenLink()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        Application.OpenURL(link);
    }
    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        gameObject.SetActive(false);
    }
}
