using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstellationNPC : MonoBehaviour
{
    private GameManager gameManager;

    // Sprites
    private GameObject info;
    private SpriteRenderer spriteRenderer;

    [NonSerialized]
    public ConstellationManager.ConstellationType constellationType;
    [NonSerialized]
    public string content;

    public TMP_Text infoBoxText;

    // Data (API)
    [NonSerialized]
    public Constellation constellation;
    [NonSerialized]
    public MustakisSaveData.ConstellationSave constellationSave;

    // Fase local de la constelación
    [NonSerialized]
    public ConstellationManager.ConstellationPhase currentConstellationPhase;

    // UITextMessages
    private MessagesDisplay messagesDisplay;

    // UIQuestionBox
    private UIQuestionBox questionBox;
    private int lastUnansweredQuestionIndex;

    // Completación
    [NonSerialized]
    public bool isComplete;
    private bool isTurnedOn;


    private void Start()
    {
        isComplete = false;
        gameManager = FindObjectOfType<GameManager>();
        currentConstellationPhase = ConstellationManager.ConstellationPhase.INTRO; // Fase inicial
        messagesDisplay = ConstellationManager.Instance.messagesDisplay;
        questionBox = ConstellationManager.Instance.questionBox;
        lastUnansweredQuestionIndex = ParseInitialLastUnansweredQuestionIndex();
        
        infoBoxText.text = content;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite =  ConstellationAssets.Instance.GetSprite(constellationType, false); // Sprite apagado

    }

    private void Update()
    {
        // Si NO está completo...
        if (!isComplete)
        {
        // Si ya se completó hasta la última pregunta se establece isComplete=true
            if (lastUnansweredQuestionIndex == -1 && !isComplete)
            {
                isComplete = true;
                // Se pasa a OUTRO y se actualiza inventario. Para caso de libros que estuviesen listos desde antes.
                if (ParseInitialLastUnansweredQuestionIndex() == -1)
                {
                    currentConstellationPhase = ConstellationManager.ConstellationPhase.OUTRO;
                    FindObjectOfType<PlayerLogic>().inventory.Update();
                }
            }
            // Mensajes previos de pregunta actual
            else if (currentConstellationPhase == ConstellationManager.ConstellationPhase.QMESSAGES)
            {
                if (!messagesDisplay.isActiveAndEnabled)
                {
                    messagesDisplay.gameObject.SetActive(true);
                    messagesDisplay.ShowMessagesAndChangeConstellationPhaseOnClose(constellation.questionPacks[lastUnansweredQuestionIndex].messages,
                        this, ConstellationManager.ConstellationPhase.QUESTIONS);
                }
            }
            // Pregunta actual
            else if (currentConstellationPhase == ConstellationManager.ConstellationPhase.QUESTIONS)
            {
                if (!questionBox.isActiveAndEnabled)
                {
                    questionBox.gameObject.SetActive(true);
                    // Si es última pregunta, pasa a OUTRO al cerrar.
                    if (lastUnansweredQuestionIndex+1 >= constellation.questionPacks.Count)
                    {
                        questionBox.ShowQuestionAndChangeConstellationPhaseOnClose(this, lastUnansweredQuestionIndex, ConstellationManager.ConstellationPhase.OUTRO);
                    }
                    // Si NO es última pregunta, pasa a QMESSAGES al cerrar.
                    else
                    {
                        questionBox.ShowQuestionAndChangeConstellationPhaseOnClose(this, lastUnansweredQuestionIndex, ConstellationManager.ConstellationPhase.QMESSAGES);
                    }
                    // +1 para pasar a la siguiente pregunta!!! -1 si era la última.
                    lastUnansweredQuestionIndex = lastUnansweredQuestionIndex + 1 < constellation.questionPacks.Count ? lastUnansweredQuestionIndex + 1 : -1;
                }
            }
        }
        // Si está completo, enciende constelación
        else
        {
            if (!isTurnedOn)
            {
                // Se obtiene sprite encendido
                spriteRenderer.sprite = ConstellationAssets.Instance.GetSprite(constellationType, true);
                isTurnedOn = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        info = this.gameObject.transform.Find("Info").gameObject;
        info.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        info = this.gameObject.transform.Find("Info").gameObject;
        info.SetActive(false);
    }

    // Gabo - Iniciar interacción. Con Botón del prompt "Info".
    public void Interact()
    {
        if (!messagesDisplay.isActiveAndEnabled && currentConstellationPhase == ConstellationManager.ConstellationPhase.INTRO)
        {
            messagesDisplay.gameObject.SetActive(true);
            messagesDisplay.ShowMessagesAndChangeConstellationPhaseOnClose(constellation.intro, this, ConstellationManager.ConstellationPhase.QMESSAGES);
        }
        else if (!messagesDisplay.isActiveAndEnabled && currentConstellationPhase == ConstellationManager.ConstellationPhase.OUTRO)
        {
            messagesDisplay.gameObject.SetActive(true);
            messagesDisplay.ShowMessages(constellation.outro);
        }
    }

    // Se obtiene índice de la última pregunta sin contestar
    private int ParseInitialLastUnansweredQuestionIndex()
    {
        //return 0; // ***DEBUGGING***
        int questionIndex = 0;
        foreach (MustakisSaveData.ConstellationSave.QuestionSave questionSave in constellationSave.questions)
        {
            if (questionSave.unlocked)
            {
                questionIndex++;
            }
            else
            {
                return questionIndex;
            }
        }
        // Si ya se completó, retorna -1.
        return -1;
    }
}
