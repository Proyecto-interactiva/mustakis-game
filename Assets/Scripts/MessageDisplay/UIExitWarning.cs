using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIExitWarning : MonoBehaviour
{
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();  
    }

    public void ExitToTitle()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        // Reinicio GLOBAL
        gameManager.RestartEverything();
        // Vuelta a menú
        SceneManager.LoadScene("Menu");
    }

    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        gameObject.SetActive(false);
    }
}
