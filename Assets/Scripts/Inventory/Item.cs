using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;


public class Item : MonoBehaviour
{
    private GameObject info;
    private PlayerLogic picker;
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

    public TMP_Text messageDisplay;


    private void Start()
    {
        messageDisplay.text = content;
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

    private void OnTriggerEnter2D(Collider2D player)
    {
        info = this.gameObject.transform.Find("Info").gameObject;
        picker = player.gameObject.GetComponent<PlayerLogic>();
        Debug.Log(picker);
        info.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        info = this.gameObject.transform.Find("Info").gameObject;
        info.SetActive(false);
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
