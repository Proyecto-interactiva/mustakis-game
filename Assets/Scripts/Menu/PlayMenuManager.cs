using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayMenuManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject code;
    private TMP_InputField codeInputField;
    [Header("Error Message Settings")]
    public TMP_Text errorLabel;
    public string errorMessage = "";

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        codeInputField = code.GetComponent<TMP_InputField>();

        errorLabel.SetText(""); // Empty error field at start
    }
    public void Play()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        StartCoroutine(gameManager.CheckGame(codeInputField.text, gameCheckSuccess, gameCheckError));
    }

    public void ExitToMenu()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        SceneManager.LoadScene("Menu");
    }


        public void gameCheckSuccess()
    {
        StartCoroutine(gameManager.getSave(saveLoadSuccess, saveLoadError));
    }

    public void gameCheckError()
    {
        // something to aler the player that the code doesn't exist.
        FindObjectOfType<AudioManager>().Play("Close"); // Error audio cue
        errorLabel.SetText(errorMessage); // Error message
        Debug.Log("Game not found");
    }

    public void saveLoadSuccess(Save save)
    {
        // Do something with save. Probably use it to store what's being made and stuff.
        FindObjectOfType<AudioManager>().Play("Open"); // Success audio cue
        SceneManager.LoadScene("MainScene");
    }

    public void saveLoadError()
    {
        StartCoroutine(gameManager.newSave(saveLoadSuccess, newSaveError));
    }

    public void newSaveError()
    {
        // Something to do if the save is not found example, try again.
    }
}
