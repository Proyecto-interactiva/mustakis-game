using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelpMenu : MonoBehaviour
{
    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        gameObject.SetActive(false);
    }    
}
