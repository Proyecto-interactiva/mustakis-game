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
    public Sprite book1Sprite;
    public Sprite book2Sprite;
    public Sprite book3Sprite;
    public Sprite book4Sprite;
    public Sprite book5Sprite;
    public enum ItemType
    {
        Book1,
        Book2,
        Book3,
        Book4,
        Book5
    }

    public ItemType itemType;
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


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentConstellationPhase = ConstellationManager.ConstellationPhase.INTRO; // Fase inicial
        messagesDisplay = ConstellationManager.Instance.messagesDisplay;
        questionBox = ConstellationManager.Instance.questionBox;
        lastUnansweredQuestionIndex = ParseInitialLastUnansweredQuestionIndex();
        
        infoBoxText.text = content;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (itemType)
        {
            case ItemType.Book1:
                spriteRenderer.sprite = book1Sprite;
                break;
            case ItemType.Book2:
                spriteRenderer.sprite = book2Sprite;
                break;
            case ItemType.Book3:
                spriteRenderer.sprite = book3Sprite;
                break;
            case ItemType.Book4:
                spriteRenderer.sprite = book4Sprite;
                break;
            case ItemType.Book5:
                spriteRenderer.sprite = book5Sprite;
                break;
            default:
                spriteRenderer.sprite = book1Sprite;
                break;
        }
    }

    private void Update()
    {
        // Si ya se completó hasta la última pregunta se pasa a fase OUTRO
        if (lastUnansweredQuestionIndex == -1)
        {
            currentConstellationPhase = ConstellationManager.ConstellationPhase.OUTRO;
        }
        // Mensajes de pregunta actual
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
                questionBox.ShowQuestionAndChangeConstellationPhaseOnClose(this, lastUnansweredQuestionIndex, ConstellationManager.ConstellationPhase.QMESSAGES);

                // +1 para pasar a la siguiente pregunta!!! -1 si era la última.
                lastUnansweredQuestionIndex = lastUnansweredQuestionIndex+1 < constellation.questionPacks.Count ? lastUnansweredQuestionIndex+1 : -1;
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
        //return 0; // TODO: BORRRArrrrrrrrrrrrrrrrrrrrrrrrr ------DEBUGGING
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

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Book1: return ItemAssets.Instance.book1;
            case ItemType.Book2: return ItemAssets.Instance.book2;
            case ItemType.Book3: return ItemAssets.Instance.book3;
            case ItemType.Book4: return ItemAssets.Instance.book4;
            case ItemType.Book5: return ItemAssets.Instance.book5;
        }
    }
}
