using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;

// Gabo - Clase nueva para el manejo del diálogo con Mustakis
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

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inventory = FindObjectOfType<PlayerLogic>().inventory;
    }

    // Gabo - Mecanismo de interacción TEMPORAL
    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        //// Diálogos 
        //if (isDialoguePhase)
        //{
        //    // Activar dialogo con primer mensaje
        //    if (!messageDisplay.isActiveAndEnabled)
        //    {
        //        messageDisplay.gameObject.SetActive(true);
        //        TextMeshProUGUI auxText = messageDisplay.GetComponentInChildren<TextMeshProUGUI>();
        //        auxText.SetText(dialoguesTEMPORAL[0]);
        //        currentDialogueIndex++;
        //    }
        //}
        //// Spawneo de libros
        //else if (isSpawnPhase)
        //{
        //    List<string> bookTitles = new() { "Trabajo en Equipo", "Equipos de Trabajo", "Trabajo grupal"};
        //    gameManager.SpawnBooks(bookTitles);
        //    ShowSubmit();
        //    isSpawnPhase = false;
        //}
        //// Preguntas tras recibir libro
        //else if (isQuestionsPhase)
        //{
        //    QuestionPack.Question currQuestion = questionPackTEMPORAL.questions[0];
        //    /// TODO:
        //    // - MOSTRAR PREGUNTA EN EL WIDGET
        //    // - CONTESTAR PREGUNTA
        //    // - PASAR A PREGUNTA SIGUIENTE
        //    // - TRAS CONTESTAR A ULTIMA PREGUNTA, CERRAR WIDGET.

        //    // Setear primera pregunta
        //    questionText.text = currQuestion.question;
        //    answerText1.text = currQuestion.answers[0].content;
        //    answerText2.text = currQuestion.answers[1].content;
        //    answerText3.text = currQuestion.answers[2].content;
        //    answerText4.text = currQuestion.answers[3].content;
        //    // Abrir widget
        //    questionDisplay.SetActive(true);
        //}
    }

    // Siguiente pregunta. Para ser usado con los botones de respuestas.
    public void NextQuestion()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        //    if (isQuestionsPhase)
        //    {
        //        FindObjectOfType<AudioManager>().Play("Text");
        //        currentQuestionIndex++;
        //        if (currentQuestionIndex < questionPackTEMPORAL.questions.Count)
        //        {
        //            // Pregunta
        //            QuestionPack.Question currQuestion = questionPackTEMPORAL.questions[currentQuestionIndex];
        //            // Setear pregunta
        //            questionText.text = currQuestion.question;
        //            answerText1.text = currQuestion.answers[0].content;
        //            answerText2.text = currQuestion.answers[1].content;
        //            answerText3.text = currQuestion.answers[2].content;
        //            answerText4.text = currQuestion.answers[3].content;
        //        }
        //        else
        //        {
        //            questionDisplay.SetActive(false);
        //        }
        //    }
    }

    // Siguiente mensaje. Para ser usado con el botón de la caja de diálogo.
    public void NextMessage()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        //// Solo actúa si es fase de diálogo
        //if (isDialoguePhase)
        //{
        //    if (currentDialogueIndex < dialoguesTEMPORAL.Count)
        //    {
        //        // Mostrar mensaje y preparar para el siguiente
        //        TextMeshProUGUI auxText = messageDisplay.GetComponentInChildren<TextMeshProUGUI>();
        //        auxText.SetText(dialoguesTEMPORAL[currentDialogueIndex]);//auxText.text = dialoguesTEMPORAL[currentDialogueIndex];
        //        //===
        //        //Debug.Log(auxText.text);
        //        //===
        //        currentDialogueIndex++;
        //    }
        //    else
        //    {
        //        isDialoguePhase = false;
        //        isSpawnPhase = true;
        //        // Desactivar caja de mensajes si es el último y dar paso a fase de preguntas
        //        messageDisplay.gameObject.SetActive(false);
        //    }
        //}
        //else { Debug.LogWarning("Se intentó usar NextMessage fuera de fase de diálogo"); }      
    }

    public void HideSubmit()
    {
        submitButton.gameObject.SetActive(false);
    }

    public void ShowSubmit()
    {
        submitButton.gameObject.SetActive(true);
    }
}
