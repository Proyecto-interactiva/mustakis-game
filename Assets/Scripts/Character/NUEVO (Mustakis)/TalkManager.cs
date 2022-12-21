using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;

// Gabo - Clase nueva para el manejo del di�logo con Mustakis
public class TalkManager : MonoBehaviour
{
    public MessagesDisplay messageDisplay;
    public GameObject questionDisplay;
    public Button submitButton;
    public TMP_Text questionText;
    public TMP_Text answerText1;
    public TMP_Text answerText2;
    public TMP_Text answerText3;
    public TMP_Text answerText4;

    GameManager gameManager;
    Inventory inventory;

    //VARIABLES TEMPORALES
    //string jsonPathTEMPORAL = Application.streamingAssetsPath + "/testJSON.txt"; //***WebGL necesita usar UnityWebRequest!!!***
    //string jsonTextTEMPORAL;
    QuestionPack questionPackTEMPORAL;
    List<string> dialoguesTEMPORAL;
    //Fases
    public bool isDialoguePhase = true;
    public bool isSpawnPhase = false;
    public bool isQuestionsPhase = false;
    //Preguntas
    public int currentQuestionIndex = 0;

    // Mensajes Di�logo
    int currentDialogueIndex = 0;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inventory = FindObjectOfType<PlayerLogic>().inventory;

        // TEMPORALES
        //jsonTextTEMPORAL = File.ReadAllText(jsonPathTEMPORAL);
        questionPackTEMPORAL = new QuestionPack();//JsonUtility.FromJson<QuestionPack>(jsonTextTEMPORAL);

        //=========================
        QuestionPack.Question a = new()
        {
            id = 0,
            text = "�Te sientes apoyado por tus compa�eros?",
            answers = new List<string> { "Nada", "Algo", "Bastante", "Mucho" }
        };
        questionPackTEMPORAL.questions.Add(a);

        QuestionPack.Question b = new()
        {
            id = 1,
            text = "�Te gusta el trabajo en equipo?",
            answers = new List<string> { "Nada", "Algo", "Bastante", "Mucho" }
        };
        questionPackTEMPORAL.questions.Add(b);

        QuestionPack.Question c = new()
        {
            id = 2,
            text = "�Te interesar�a realizar actividades de fortalecimiento grupal?",
            answers = new List<string> { "S�", "No", "Quiz�s", "No lo s�" }
        };
        questionPackTEMPORAL.questions.Add(c);
        Debug.Log(JsonUtility.ToJson(questionPackTEMPORAL));

        //=========================

        dialoguesTEMPORAL = new List<string>
        {
            "�Bienvenido! A continuaci�n, aparecer�n libros con preguntas.",
            "�Lo m�s importante es contestar con sinceridad!",
            "Tr�eme el que prefieras."
        };
        //Debug.Log("PREG1: " + questionPackTEMPORAL.questions[0].text);
        //Debug.Log("PREG2: " + questionPackTEMPORAL.questions[1].text);
        //Debug.Log("PREG3: " + questionPackTEMPORAL.questions[2].text);
    }

    // Gabo - Mecanismo de interacci�n TEMPORAL
    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        // Di�logos 
        if (isDialoguePhase)
        {
            // Activar dialogo con primer mensaje
            if (!messageDisplay.isActiveAndEnabled)
            {
                messageDisplay.gameObject.SetActive(true);
                TextMeshProUGUI auxText = messageDisplay.GetComponentInChildren<TextMeshProUGUI>();
                auxText.SetText(dialoguesTEMPORAL[0]);
                currentDialogueIndex++;
            }
        }
        // Spawneo de libros
        else if (isSpawnPhase)
        {
            List<string> bookTitles = new() { "Trabajo en Equipo", "Equipos de Trabajo", "Trabajo grupal"};
            gameManager.SpawnBooks(bookTitles);
            ShowSubmit();
            isSpawnPhase = false;
        }
        // Preguntas tras recibir libro
        else if (isQuestionsPhase)
        {
            QuestionPack.Question currQuestion = questionPackTEMPORAL.questions[0];
            /// TODO:
            // - MOSTRAR PREGUNTA EN EL WIDGET
            // - CONTESTAR PREGUNTA
            // - PASAR A PREGUNTA SIGUIENTE
            // - TRAS CONTESTAR A ULTIMA PREGUNTA, CERRAR WIDGET.

            // Setear primera pregunta
            questionText.text = currQuestion.text;
            answerText1.text = currQuestion.answers[0];
            answerText2.text = currQuestion.answers[1];
            answerText3.text = currQuestion.answers[2];
            answerText4.text = currQuestion.answers[3];
            // Abrir widget
            questionDisplay.SetActive(true);
        }
    }

    // Siguiente pregunta. Para ser usado con los botones de respuestas.
    public void NextQuestion()
    {
        if (isQuestionsPhase)
        {
            FindObjectOfType<AudioManager>().Play("Text");
            currentQuestionIndex++;
            if (currentQuestionIndex < questionPackTEMPORAL.questions.Count)
            {
                // Pregunta
                QuestionPack.Question currQuestion = questionPackTEMPORAL.questions[currentQuestionIndex];
                // Setear pregunta
                questionText.text = currQuestion.text;
                answerText1.text = currQuestion.answers[0];
                answerText2.text = currQuestion.answers[1];
                answerText3.text = currQuestion.answers[2];
                answerText4.text = currQuestion.answers[3];
            }
            else
            {
                questionDisplay.SetActive(false);
            }
        }
    }

    // Siguiente mensaje. Para ser usado con el bot�n de la caja de di�logo.
    public void NextMessage()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        // Solo act�a si es fase de di�logo
        if (isDialoguePhase)
        {
            if (currentDialogueIndex < dialoguesTEMPORAL.Count)
            {
                // Mostrar mensaje y preparar para el siguiente
                TextMeshProUGUI auxText = messageDisplay.GetComponentInChildren<TextMeshProUGUI>();
                auxText.SetText(dialoguesTEMPORAL[currentDialogueIndex]);//auxText.text = dialoguesTEMPORAL[currentDialogueIndex];
                //===
                //Debug.Log(auxText.text);
                //===
                currentDialogueIndex++;
            }
            else
            {
                isDialoguePhase = false;
                isSpawnPhase = true;
                // Desactivar caja de mensajes si es el �ltimo y dar paso a fase de preguntas
                messageDisplay.gameObject.SetActive(false);
            }
        }
        else { Debug.LogWarning("Se intent� usar NextMessage fuera de fase de di�logo"); }      
    }
    internal void HideSubmit()
    {
        submitButton.gameObject.SetActive(false);
    }

    internal void ShowSubmit()
    {
        submitButton.gameObject.gameObject.SetActive(true);
    }
}
