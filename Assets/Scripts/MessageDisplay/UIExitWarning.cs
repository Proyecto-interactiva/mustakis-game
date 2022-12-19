using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIExitWarning : MonoBehaviour
{
    public void ExitToTitle()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        SceneManager.LoadScene("Menu");
    }

    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        gameObject.SetActive(false);
    }
}
