using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagesDisplay : MonoBehaviour
{
    public TMP_Text messageDisplay;
    public GameOverDisplay gameOverDisplay;
    List<string> messages;
    int currentMessageIndex = -1;

    [NonSerialized]
    public bool isFinished;

    void Start()
    {
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

    public void nextMessage()
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

    void HideDisplay()
    {
        isFinished = true;
        currentMessageIndex = -1;
        messages.Clear();
        this.gameObject.SetActive(false);
    }


}
