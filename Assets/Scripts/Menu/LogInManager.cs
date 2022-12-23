using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LogInManager : MonoBehaviour
{
    public GameObject email;
    public GameObject password;
    [Header("Error Message Settings")]
    public TMP_Text errorLabel;
    public string errorMessage = "";
    public string awaitingConnectionMessage = "";

    private TMP_InputField emailInputField;
    private TMP_InputField passwordInputField;
    private string specificUri = "/sign/in-user";

    GameManager gameManager;

    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        emailInputField = email.GetComponent<TMP_InputField>();
        passwordInputField = password.GetComponent<TMP_InputField>();

        errorLabel.SetText(""); // Empty error field at start
    }

    public void Back()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        SceneManager.LoadScene("Menu");

    }

    public void LogIn()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        // do the login
        WWWForm form = new WWWForm();
        form.AddField("email", emailInputField.text);
        form.AddField("password", passwordInputField.text);
        Debug.Log("Executing login post");
        errorLabel.SetText(awaitingConnectionMessage);
        StartCoroutine(gameManager.PostForm(specificUri, form, SuccessLogInFallBack, ErrorLogInFallBack));
        

    }

    private void SuccessLogInFallBack()
    {
        FindObjectOfType<AudioManager>().Play("Open");
        SceneManager.LoadScene("Play Menu");
    }

    private void ErrorLogInFallBack()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        emailInputField.text = "";
        passwordInputField.text = "";

        // Error message
        errorLabel.SetText(errorMessage);
    }
}
