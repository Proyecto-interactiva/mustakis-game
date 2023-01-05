using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class confirmationBox : MonoBehaviour
{
    GameManager gameManager;
    Inventory inventory;
    public MessagesDisplay messagesDisplay;
    public TalkManager talkManager;
    int characterId;
    WWWForm form;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        inventory = FindObjectOfType<PlayerLogic>().inventory;
        talkManager = FindObjectOfType<TalkManager>(); // Gabo
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Gabo - Modificación TEMPORAL
    public void Show()//int chid)
    {
        //string answers = "";
        //characterId = chid;
        gameObject.SetActive(true);
        //Debug.Log(inventory.GetItemList());
        //foreach (Item item in inventory.GetItemList())
        //{
        //    answers += item.content + ";";
        //}
        //Debug.Log(answers);
        //if (answers.Length > 0) answers = answers.Remove(answers.Length - 1);
        //form = new WWWForm();
        //form.AddField("answers", answers);
    }

    // GABO - Modificacion TEMPORAL
    public void Accept()
    {
        FindObjectOfType<AudioManager>().Play("Text");
        Success();//StartCoroutine(gameManager.PostAnswer(form, characterId, Success, Close));
    }

    // Gabo - Modificación TEMPORAL
    public void Close()
    {
        FindObjectOfType<AudioManager>().Play("Close");
        gameObject.SetActive(false);
        //form = null;
    }

    // Gabo - Modificación TEMPORAL
    public void Success()
    {
        gameObject.SetActive(false);
        List<string> messages = new List<string>();
        talkManager.HideSubmit();
        inventory.Clear();
        GameObject[] items;
        items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            Destroy(item);
        }
        Close();
    }

    
}

[Serializable]
public class SerializableList<T>
{
    public List<T> list;
}
