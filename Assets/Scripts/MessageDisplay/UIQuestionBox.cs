using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuestionBox : MonoBehaviour
{
    [SerializeField]
    private TMP_Text questionField;
    [SerializeField]
    private GameObject answerContainer;
    [SerializeField]
    private GameObject answerTemplate;

    // Paso de preguntas
    public static bool isAnswerConfirmedAndSent = false; // Se modifica en UIAnswer tras POST de resp. exitoso!
    public static bool isAnswerInProgress = false; // Usado en UIAnswer para prevenir solicitud rápida múltiple (posible prevención BUG de memory leak?)
    private ConstellationNPC currConstellationNPC;
    private int currQuestionIndex;
    // Fase constelacion pendiente
    private ConstellationManager.ConstellationPhase pendingConstellationPhase;
    private bool isConstellationPhasePending;

    private void Awake()
    {
        isConstellationPhasePending = false;
    }

    private void Update()
    {
        // Cambio de fase al terminar una pregunta + CIERRE
        if (isConstellationPhasePending && isAnswerConfirmedAndSent)
        {
            currConstellationNPC.currentConstellationPhase = pendingConstellationPhase;
            isConstellationPhasePending = false;
            isAnswerConfirmedAndSent = false;
            gameObject.SetActive(false);
            Debug.Log("QuestionBox: Pasando a fase constelacion " + pendingConstellationPhase);

            // Actualiza inventario de jugador si se está pasando a OUTRO, para colorear la nueva constelacion completa
            if (pendingConstellationPhase == ConstellationManager.ConstellationPhase.OUTRO)
            {
                FindObjectOfType<PlayerLogic>().inventory.Update();
            }
        }
    }

    // Agregar respuestas
    void AddAnswers(int questionPackId, int questionId,  List<Constellation.Question.Answer> answers)
    {
        int answerId = 0;
        foreach (Constellation.Question.Answer answer in answers)
        {
            AddAnswer(answer, questionPackId, questionId, answerId);
            answerId++;
        }
    }

    // Agrega una respuesta
    void AddAnswer(Constellation.Question.Answer answer, int questionPacksId, int questionId, int answerId)
    {
        GameObject newAnswer = Instantiate(answerTemplate, answerContainer.transform);
        UIAnswer aux = newAnswer.GetComponent<UIAnswer>();
        aux.GetComponentInChildren<TextMeshProUGUI>().text = answer.content;
        aux.data = answer;
        aux.questionPacksId = questionPacksId;
        aux.questionId = questionId;
        aux.answerId = answerId;
        newAnswer.SetActive(true); // Necesario ya que el template está desactivado
    }

    public void ShowQuestion(ConstellationNPC constellationNPC, int questionIndex)
    {
        // Limpiar respuestas pasadas
        foreach (Transform childAnswer in answerContainer.transform)
        {
            // Template es excepción
            if (childAnswer.gameObject != answerTemplate)
            {
                Destroy(childAnswer.gameObject);
            }
        }
        // Colocar última pregunta según índice recibido e instanciar respuestas
        currQuestionIndex = questionIndex;
        currConstellationNPC = constellationNPC;
        Constellation.Question currQuestion = constellationNPC.constellation.questionPacks[currQuestionIndex];
        questionField.text = currQuestion.question;
        AddAnswers(constellationNPC.constellation.round, currQuestionIndex, currQuestion.answers);
    }

    public void ShowQuestionAndChangeConstellationPhaseOnClose(ConstellationNPC constellationNPC, int startingQuestionIndex, ConstellationManager.ConstellationPhase phase)
    {
        ShowQuestion(constellationNPC, startingQuestionIndex);
        pendingConstellationPhase = phase;
        isConstellationPhasePending = true;
    }

    public static void RestartStatic()
    {
        isAnswerConfirmedAndSent = false;
        isAnswerInProgress = false;
    }
}
