using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// Gabo - Clase nueva para el manejo del diálogo con Mustakis
public class TalkManager : MonoBehaviour
{
    public MessagesDisplay messageDisplay;
    public GameObject questionDisplay;
    //public Button submitButton;
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

    private void Update()
    {
        // Fase GENERAL
        if (gameManager.currentPhase == GameManager.Phase.GENERAL)
        {
            // TERMINARRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR
            // TODO:
            // ---- SPAWNEO CONSTELACIONES
            // - PERMITIR INTERACCION
            // - CAMBIO DE FASES
            if (!ConstellationManager.Instance.isSpawned)
            {
                // Spawn constelaciones
                ConstellationManager.Instance.SpawnConstellations();
            }
        }
        // FIN de JUEGO
        else if (gameManager.isGameFinished && gameManager.currentPhase == GameManager.Phase.FINAL)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // Gabo - Para usarse con botón de interacción NPC
    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("Text");

        // DEBUGGING
        //// --- Prueba API: PostAnswer() ---
        //WWWForm form = new WWWForm();
        //form.AddField("questionPacksId", 0);
        //form.AddField("questionId", 0);
        //form.AddField("answerId", 2);
        //form.AddField("answer", gameManager.mustakisGameData.scenes[0] // ***Será mejor eliminar este parametro???***
        //    .questionPacks[0].answers[2].content);
        //Debug.Log("DEBUG: Probando API");
        //StartCoroutine(gameManager.PostAnswer(form, (FeedbackResponse response)=>{}, () =>{}));
        // FIN DEBUGGING

        ///// Diálogo
        // Fase INTRO
        if (gameManager.currentPhase == GameManager.Phase.INTRO)
        {
            // Activar dialogo
            if (!messageDisplay.isActiveAndEnabled)
            {
                List<string> introMessages = gameManager.mustakisGameData.dialogues.introDialogues;
                messageDisplay.ShowMessagesAndChangePhaseOnClose(introMessages, GameManager.Phase.GENERAL);
            }
        }
        // Fase GENERAL
        else if (gameManager.currentPhase == GameManager.Phase.GENERAL)
        {
            // Activar dialogo
            if (!messageDisplay.isActiveAndEnabled)
            {
                List<string> generalMessages = gameManager.mustakisGameData.dialogues.generalDialogues;
                messageDisplay.ShowMessages(generalMessages);
            }
        }
        // Fase FINAL
        else if (gameManager.currentPhase == GameManager.Phase.FINAL)
        {
            // Activar dialogo
            if (!messageDisplay.isActiveAndEnabled)
            {
                List<string> finalMessages = gameManager.mustakisGameData.dialogues.finalDialogues;
                messageDisplay.ShowMessages(finalMessages);
            }
        }


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

    //public void HideSubmit()
    //{
    //    submitButton.gameObject.SetActive(false);
    //}

    //public void ShowSubmit()
    //{
    //    submitButton.gameObject.SetActive(true);
    //}
}
