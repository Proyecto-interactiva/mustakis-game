using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        // Reinicio GLOBAL
        gameManager.RestartEverything();
        // Salida a menú
        SceneManager.LoadScene("Menu");
    }
}
