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
    GameManager gameManager;

    //VARIABLES TEMPORALES
    //string jsonPathTEMPORAL = Application.streamingAssetsPath + "/testJSON.txt"; //***WebGL necesita usar UnityWebRequest!!!***
    //string jsonTextTEMPORAL;
    QuestionPack questionPackTEMPORAL;
    List<string> dialoguesTEMPORAL;
    //Fases
    bool isDialoguePhase = true;
    bool isSpawnPhase = false;
    bool isQuestionsPhase = false;

    // Mensajes Di�logo
    int currentDialogueIndex = 0;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

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
        questionPackTEMPORAL.questions.Add(b);
        Debug.Log(JsonUtility.ToJson(questionPackTEMPORAL));

        //=========================

        dialoguesTEMPORAL = new List<string>
        {
            "�Bienvenido! A continuaci�n, se te realizar�n algunas preguntas.",
            "�Lo m�s importante es contestar con sinceridad!",
            "�nimos y d�mosle."
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
        else if (isSpawnPhase)
        {
            // SPAWNEAR LIBROS!!!!
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
}
