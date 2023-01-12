using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagesDisplay : MonoBehaviour
{
    private GameManager gameManager;
    public TMP_Text messageDisplay;
    public GameOverDisplay gameOverDisplay;
    List<string> messages;
    // Fase
    private GameManager.Phase pendingPhase; // Fase pendiente por aplicar a GameManager
    private bool isPhasePending; // Determina si se aplica pendingPhase o no

    int currentMessageIndex = -1;

    [NonSerialized]
    public bool isFinished; // Indica si se terminaron los mensajes por mostrar

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        isPhasePending = false;
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

    public void ShowMessages(List<string> messages)
    {
        isFinished = false;
        this.gameObject.SetActive(true);
        this.messages = messages;
        Debug.Log("Message count: " + this.messages.Count);
        nextMessage();
    }

    // Cambio de fase en gameManager al cerrar los mensajes
    public void ShowMessagesAndChangePhaseOnClose(List<string> messages, GameManager.Phase phase)
    {
        ShowMessages(messages);
        isPhasePending = true;
        pendingPhase = phase;
    }

    private void nextMessage()
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
        if (isPhasePending && isFinished)
        {
            gameManager.currentPhase = pendingPhase;
            isPhasePending = false;
            Debug.Log("Fase cambiada a " + pendingPhase);
        }
    }


}
