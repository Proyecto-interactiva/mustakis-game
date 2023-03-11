using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MessagesDisplay : MonoBehaviour
{
    private GameManager gameManager;
    public TMP_Text messageDisplay;
    public Image image; // Personaje lado izquierdo
    public GameOverDisplay gameOverDisplay;
    public Sprite defaultSprite;
    List<string> messages;
    // Fase GameManager
    private GameManager.Phase pendingPhase; // Fase pendiente por aplicar a GameManager
    private bool isPhasePending; // Determina si se aplica pendingPhase o no
    // Fase Constelación
    private ConstellationManager.ConstellationPhase pendingConstellationPhase;
    private bool isConstellationPhasePending;
    private ConstellationNPC pendingConstellationTarget;
    // CIERRE de JUEGO
    private bool isGameOverPending;

    int currentMessageIndex = -1;

    [NonSerialized]
    public bool isFinished; // Indica si se terminaron los mensajes por mostrar

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        isPhasePending = false;
        isConstellationPhasePending = false;
        isGameOverPending = false;
        isFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMessageIndex != -1)
        {
            messageDisplay.text = messages[currentMessageIndex];
        }
        else
        {
            messageDisplay.text = "";
        }
    }

    public void ShowMessages(List<string> messages, Sprite characterSprite = null)
    {
        // Cambio de sprite lateral izquierdo por el sprite del personaje (y si no, predeterminado)
        if (characterSprite != null) { ChangeSprite(characterSprite); }
        else if (characterSprite != defaultSprite) { ChangeSprite(defaultSprite); }

        isFinished = false;
        this.gameObject.SetActive(true);
        this.messages = messages;
        Debug.Log("Message count: " + this.messages.Count);
        NextMessage();
    }

    // Cambio de fase en gameManager al cerrar los mensajes
    public void ShowMessagesAndChangePhaseOnClose(List<string> messages, GameManager.Phase phase)
    {
        ShowMessages(messages);
        isPhasePending = true;
        pendingPhase = phase;
    }

    // Cambio de fase local de una constelación al cerrar los mensajes
    public void ShowMessagesAndChangeConstellationPhaseOnClose(List<string> messages, ConstellationNPC constellation, ConstellationManager.ConstellationPhase phase)
    {
        ShowMessages(messages, constellation.GetComponent<SpriteRenderer>().sprite);
        isConstellationPhasePending = true;
        pendingConstellationTarget = constellation;
        pendingConstellationPhase = phase;
    }

    // Para usarse con el discurso final del NPC. Cierra la partida.
    public void ShowMessagesAndSetGameFinishedOnClose(List<string> messages)
    {
        ShowMessages(messages);
        isGameOverPending = true;
    }

    public void NextMessage()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        if (currentMessageIndex + 1 < messages.Count)
        {
            currentMessageIndex++;
        }
        else
        {
            HideDisplay();
        }
    }

    private void HideDisplay()
    {
        isFinished = true;
        currentMessageIndex = -1;
        messages = new List<string>();
        this.gameObject.SetActive(false);

        // Aplicación de nueva fase, si se encuentra pendiente
        if (isPhasePending)
        {
            gameManager.currentPhase = pendingPhase;
            isPhasePending = false;
            Debug.Log("MessagesDisplay: Fase cambiada a " + pendingPhase);
        }
        // Aplicación de nueva fase local a constelacion, de estar pendiente
        if (isConstellationPhasePending)
        {
            pendingConstellationTarget.currentConstellationPhase = pendingConstellationPhase;
            isConstellationPhasePending = false;
            pendingConstellationTarget = null;
            Debug.Log("MessagesDisplay: Fase constelación local cambiada a " + pendingConstellationPhase);
        }
        if (isGameOverPending)
        {
            FindObjectOfType<GameManager>().isGameFinished = true;
            Debug.Log("MessageDisplay: Seteando el juego como TERMINADO");
        }
    }

    private void ChangeSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
    }


}
