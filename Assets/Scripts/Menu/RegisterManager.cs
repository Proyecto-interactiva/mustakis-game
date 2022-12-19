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

    // Revelar/Ocultar contraseña
    public void TogglePassword()
    {
        Debug.Log("Toggled password");
        passwordInputField.contentType = (passwordInputField.contentType == TMP_InputField.ContentType.Password) ?
            TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate(); // Actualiza texto para visualizar el cambio
    }

    // Revelar/Ocultar repetición de contraseña
    public void ToggleRepeatPassword()
    {
        Debug.Log("Toggled Repeat password");
        repeatPasswordInputField.contentType = (repeatPasswordInputField.contentType == TMP_InputField.ContentType.Password) ?
            TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        repeatPasswordInputField.ForceLabelUpdate(); // Actualiza texto para visualizar el cambio
    }

    // Nuevo registro y accedo a menú del código ("Play Menu")
    public void RegisterAndPlay()
    {
        if (ArePasswordsEqual()) // Chequeo de contraseñas iguales
        {
            WWWForm form = Register();
            StartCoroutine(gameManager.PostForm(uri, form, SuccessRegisterFallBackPLAY, ErrorRegisterFallBack));
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Text");
            FindObjectOfType<AudioManager>().Play("Close");
            errorLabel.SetText(unequalPassWarningMessage); // Advierte sobre contraseñas desiguales
        }
    }

    // Nuevo registro y vuelvo a menú inicial (Menu)
    public void RegisterAndExit()
    {
        if (ArePasswordsEqual()) // Chequeo de contraseñas iguales
        {
            WWWForm form = Register();
            StartCoroutine(gameManager.PostForm(uri, form, SuccessRegisterFallBackEXIT, ErrorRegisterFallBack));
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Text");
            FindObjectOfType<AudioManager>().Play("Close");
            errorLabel.SetText(unequalPassWarningMessage); // Advierte sobre contraseñas desiguales
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

    // Chequeo de igualdad de contraseñas
    private bool ArePasswordsEqual()
    {
        return passwordInputField.text == repeatPasswordInputField.text;
    }
}
