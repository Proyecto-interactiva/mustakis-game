using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{

    GameManager gameManager;
    public GameObject messageDisplay;
    public GameObject confirmatioBox;
    // Start is called before the first frame update
    public int characterId;
    public bool questMode = false;
    void Start()
    {
        
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        //Get next messages
        StartCoroutine(gameManager.NextMessage(characterId, ManageMessages, ManageError));
        FindObjectOfType<AudioManager>().Play("Text");
    }

    public void Submit()
    {
        confirmatioBox.GetComponent<confirmationBox>().Show(characterId);
        FindObjectOfType<AudioManager>().Play("Text");

    }

    public void ManageMessages(MessagesResponse response)
    {
        //show message in message display
        if (!response.lastScene)
        {
            messageDisplay.GetComponent<MessagesDisplay>().ShowMessages(response.dialogs);
        }
        else
        {
            // Display last message and then game over.
            messageDisplay.GetComponent<MessagesDisplay>().ShowLastMessage(response.dialogs);
        }
        
        // spawn items in the world
        if (response.quest && !questMode && !response.lastScene)
        {
            gameManager.SpawnBooks(response.answers);
            questMode = true;
            ShowSubmit();
        }
    }

    public void ManageError()
    {
        Debug.Log("Messages failed to load");
        StartCoroutine(gameManager.NextMessage(characterId, ManageMessages, ManageError));
    }

    internal void HideSubmit()
    {
        gameObject.transform.Find("InteractionCanvas").Find("SubmitButton").gameObject.SetActive(false);
    }

    internal void ShowSubmit()
    {
        gameObject.transform.Find("InteractionCanvas").Find("SubmitButton").gameObject.SetActive(true);
    }
}
