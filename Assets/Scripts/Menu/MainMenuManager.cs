using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void LogIn()
    {
        SceneManager.LoadScene("LogIn");
        FindObjectOfType<AudioManager>().Play("Text");
    }

    public void Register()
    {
        SceneManager.LoadScene("Register");
        FindObjectOfType<AudioManager>().Play("Text");
    }

    public void OpenTrailer()
    {
        gameManager.lastSceneBeforeTrailer = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Trailer");
        FindObjectOfType<AudioManager>().Play("Text");
    }
}
