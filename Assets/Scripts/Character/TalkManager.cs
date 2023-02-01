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
    public GameOverDisplay gameOverDisplay;
    public GameObject questionDisplay;
    public TMP_Text questionText;
    public UIReturnToGuardianArrow arrow;

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
            if (!ConstellationManager.Instance.isSpawned)
            {
                // Spawn constelaciones
                ConstellationManager.Instance.SpawnConstellations();
            }
            // Paso Fase GENERAL->FINAL, al completar constelaciones
            if (ConstellationManager.Instance.isConstellationsComplete())
            {
                // Se anima flecha indicando volver al guardián
                arrow.Play(5, .5f);
                gameManager.currentPhase = GameManager.Phase.FINAL;
            }
        }
        // Fase FINAL (todavía NO termina)
        else if (gameManager.currentPhase == GameManager.Phase.FINAL && !gameManager.isGameFinished)
        {
        }
        // FIN de JUEGO: Despliega cuadro GameOverDisplay
        else if (gameManager.isGameFinished)
        {
            if (!gameOverDisplay.isActiveAndEnabled)
            {
                gameOverDisplay.Show();
            }
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
                messageDisplay.ShowMessagesAndSetGameFinishedOnClose(finalMessages);
            }
        }        
    }
}
