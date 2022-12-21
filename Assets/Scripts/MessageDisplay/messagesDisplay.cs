using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagesDisplay : MonoBehaviour
{
    bool lastMessage = false;
    public TMP_Text messageDisplay;
    public GameOverDisplay gameOverDisplay;
    List<string> messages;
    int currentMessageIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentMessageIndex != -1)
        //{
        //    messageDisplay.text = messages[currentMessageIndex];
        //}
        //else
        //{
        //    messageDisplay.text = "";
        //}
        
    }

    public void ShowMessages(List<string> messages)
    {
        this.gameObject.SetActive(true);
        this.messages = messages;
        Debug.Log(this.messages);
        Debug.Log(this.messages.Count);
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
            if (lastMessage) gameOverDisplay.Show();
        }
    }

    void HideDisplay()
    {
        currentMessageIndex = -1;
        messages.Clear();
        this.gameObject.SetActive(false);
    }

    public void ShowLastMessage(List<string> messages)
    {
        lastMessage = true;
        this.gameObject.SetActive(true);
        this.messages = messages;
        Debug.Log(this.messages);
        Debug.Log(this.messages.Count);
        nextMessage();
    }


}
