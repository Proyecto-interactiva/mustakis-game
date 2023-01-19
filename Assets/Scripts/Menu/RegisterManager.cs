using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RegisterManager : MonoBehaviour
{
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject repeatPassword;
    [Header("Error Message Settings")]
    public TMP_Text errorLabel;
    public string errorMessage = "";
    public string unequalPassWarningMessage = "";
    public string awaitingConnectionMessage = "";
    private TMP_InputField usernameInputField;
    private TMP_InputField emailInputField;
    private TMP_InputField passwordInputField;
    private TMP_InputField repeatPasswordInputField;
    private string uri = "/sign/up";

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        usernameInputField = username.GetComponent<TMP_InputField>();
        emailInputField = email.GetComponent<TMP_InputField>();
        passwordInputField = password.GetComponent<TMP_InputField>();
        repeatPasswordInputField = repeatPassword.GetComponent<TMP_InputField>();

        errorLabel.SetText(""); // Empty error field at start
    }

    public void Back()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        SceneManager.LoadScene("Menu");
    }

    // Revelar/Ocultar contrase�a
    public void TogglePassword()
    {
        Debug.Log("Toggled password");
        passwordInputField.contentType = (passwordInputField.contentType == TMP_InputField.ContentType.Password) ?
            TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate(); // Actualiza texto para visualizar el cambio
    }

    // Revelar/Ocultar repetici�n de contrase�a
    public void ToggleRepeatPassword()
    {
        Debug.Log("Toggled Repeat password");
        repeatPasswordInputField.contentType = (repeatPasswordInputField.contentType == TMP_InputField.ContentType.Password) ?
            TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        repeatPasswordInputField.ForceLabelUpdate(); // Actualiza texto para visualizar el cambio
    }

    // Nuevo registro y accedo a men� del c�digo ("Play Menu")
    public void RegisterAndPlay()
    {
        if (ArePasswordsEqual()) // Chequeo de contrase�as iguales
        {
            WWWForm form = Register();
            errorLabel.SetText(awaitingConnectionMessage);
            StartCoroutine(gameManager.PostForm(uri, form, SuccessRegisterFallBackPLAY, ErrorRegisterFallBack));
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Text");
            FindObjectOfType<AudioManager>().Play("Close");
            errorLabel.SetText(unequalPassWarningMessage); // Advierte sobre contrase�as desiguales
        }
    }

    // Nuevo registro y vuelvo a men� inicial (Menu)
    public void RegisterAndExit()
    {
        if (ArePasswordsEqual()) // Chequeo de contrase�as iguales
        {
            WWWForm form = Register();
            errorLabel.SetText(awaitingConnectionMessage);
            StartCoroutine(gameManager.PostForm(uri, form, SuccessRegisterFallBackEXIT, ErrorRegisterFallBack));
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Text");
            FindObjectOfType<AudioManager>().Play("Close");
            errorLabel.SetText(unequalPassWarningMessage); // Advierte sobre contrase�as desiguales
        }
    }

    private WWWForm Register()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        // do the registration
        WWWForm form = new WWWForm();
        form.AddField("name", usernameInputField.text);
        form.AddField("email", emailInputField.text);
        form.AddField("password", passwordInputField.text);
        Debug.Log("Executing Register post");
        return form;
    }

    private void SuccessRegisterFallBackPLAY()
    {
        FindObjectOfType<AudioManager>().Play("Open");

        gameManager.lastSceneBeforeTrailer = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Trailer"); // Primer ingreso, se inicia trailer
    }
    private void SuccessRegisterFallBackEXIT()
    {
        FindObjectOfType<AudioManager>().Play("Open");
        SceneManager.LoadScene("Menu");
    }

    private void ErrorRegisterFallBack()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        usernameInputField.text = "";
        emailInputField.text = "";
        passwordInputField.text = "";
        repeatPasswordInputField.text = "";

        // Error message
        errorLabel.SetText(errorMessage);
    }

    // Chequeo de igualdad de contrase�as
    private bool ArePasswordsEqual()
    {
        return passwordInputField.text == repeatPasswordInputField.text;
    }
}
