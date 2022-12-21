using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

// Gabo - Clase nueva para el manejo del diálogo con Mustakis
public class TalkManager : MonoBehaviour
{
    public MessagesDisplay messageDisplay;
    GameManager gameManager;

    //VARIABLES TEMPORALES
    string jsonPathTEMPORAL = Application.streamingAssetsPath + "/testJSON.txt";
    string jsonTextTEMPORAL;
    QuestionPack questionPackTEMPORAL;
    List<string> dialoguesTEMPORAL;
    //Fases
    bool isDialoguePhase = true;
    bool isSpawnPhase = false;
    bool isQuestionsPhase = false;

    // Mensajes Diálogo
    int currentDialogueIndex = 0;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // TEMPORALES
        jsonTextTEMPORAL = File.ReadAllText(jsonPathTEMPORAL);
        //Debug.Log(jsonTextTEMPORAL);
        questionPackTEMPORAL = /*new QuestionPack();*/JsonUtility.FromJson<QuestionPack>(jsonTextTEMPORAL);

        //=========================
        //QuestionPack.Question a = new()
        //{
        //    id = 0,
        //    text = "banana",
        //    answers = new List<string> { "a1", "a2", "a3", "a4" }
        //};
        //questionPackTEMPORAL.questions.Add(a);

        //QuestionPack.Question b = new()
        //{
        //    id = 1,
        //    text = "apple",
        //    answers = new List<string> { "b1", "b2", "b3", "b4" }
        //};
        //questionPackTEMPORAL.questions.Add(b);
        //Debug.Log(JsonUtility.ToJson(questionPackTEMPORAL));

        //=========================

        dialoguesTEMPORAL = new List<string>
        {
            "placeholder 1",
            "placeholder 2",
            "placeholder 3"
        };
        //Debug.Log("PREG1: " + questionPackTEMPORAL.questions[0].text);
        //Debug.Log("PREG2: " + questionPackTEMPORAL.questions[1].text);
        //Debug.Log("PREG3: " + questionPackTEMPORAL.questions[2].text);
    }

    // Gabo - Mecanismo de interacción TEMPORAL
    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        // Diálogos 
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

    // Siguiente mensaje. Para ser usado con el botón de la caja de diálogo.
    public void NextMessage()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        // Solo actúa si es fase de diálogo
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
                // Desactivar caja de mensajes si es el último y dar paso a fase de preguntas
                messageDisplay.gameObject.SetActive(false);
            }
        }
        else { Debug.LogWarning("Se intentó usar NextMessage fuera de fase de diálogo"); }      
    }
}
