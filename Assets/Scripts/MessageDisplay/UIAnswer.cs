using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase básica para contener info de una respuesta en pantalla (ej.: Al apretarla)
public class UIAnswer : MonoBehaviour
{
    // Recibe la info del objeto Answer correspondiente.
    [NonSerialized]
    public Constellation.Question.Answer data;

    [NonSerialized]
    public int questionPacksId;
    [NonSerialized]
    public int questionId;
    [NonSerialized]
    public int answerId;

    // Envío de respuesta. Usado con botones de respuesta.
    public void SendAnswer()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        GameManager gameManager = FindObjectOfType<GameManager>();

        WWWForm form = new WWWForm();
        form.AddField("questionPacksId", questionPacksId);
        form.AddField("questionId", questionId);
        form.AddField("answerId", answerId);
        form.AddField("answer", data.content);

        UIQuestionBox.isAnswerConfirmedAndSent = false;
        StartCoroutine(gameManager.PostAnswer(form, SendSuccess, SendError));
    }
    private void SendSuccess(FeedbackResponse feedbackResponse)
    {
        FindObjectOfType<AudioManager>().Play("Open");
        Debug.Log("UIAnswer: PostAnswer exitoso! answerId=" + answerId + " questionId=" + questionId);
        UIQuestionBox.isAnswerConfirmedAndSent = true;

    }
    private void SendError()
    {
        Debug.LogError("UIAnswer: PostAnswer FALLÓ. Revisar conexión!");
    }
}
