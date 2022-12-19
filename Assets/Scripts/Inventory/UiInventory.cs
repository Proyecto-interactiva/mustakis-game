using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    [Header("Menus")]
    public UIPauseMenu pauseMenu;
    public UIHelpMenu helpMenu;
    public UICredits credits;
    public UIExitWarning exitWarning;

    [Header("")]
    private Inventory inventory;
    private Transform itemSlotContainer; // Grid Layout Group que contiene los libros
    private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
        Debug.Log(itemSlotContainer);
        Debug.Log(itemSlotTemplate);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.Updated += RefreshInventoryItems;
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        Debug.Log("Updating UI");
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            //itemSlotRectTransform.anchoredPosition = new Vector2(0, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            Button button = itemSlotRectTransform.Find("DropButton").GetComponent<Button>();
            button.onClick.AddListener(delegate { inventory.RemoveItem(item); });
        }
    }

    // Abre y cierra el UIPauseMenu
    public void TogglePauseMenu()
    {
        if (pauseMenu.gameObject.activeInHierarchy)
        {
            // Desactivar
            FindObjectOfType<AudioManager>().Play("Close");
            pauseMenu.gameObject.SetActive(false);
        }
        else 
        {
            // Desactivar otros menús
            helpMenu.gameObject.SetActive(false);
            exitWarning.gameObject.SetActive(false);
            credits.gameObject.SetActive(false);

            // Activar
            FindObjectOfType<AudioManager>().Play("Open");
            pauseMenu.gameObject.SetActive(true);
        }
    }

    public void ToggleHelpMenu()
    {
        if (helpMenu.gameObject.activeInHierarchy)
        {
            FindObjectOfType<AudioManager>().Play("Close");
            helpMenu.gameObject.SetActive(false);
        }
        else
        {
            // Desactivar otros menús
            pauseMenu.gameObject.SetActive(false);
            exitWarning.gameObject.SetActive(false);
            credits.gameObject.SetActive(false);

            FindObjectOfType<AudioManager>().Play("Open");
            helpMenu.gameObject.SetActive(true);
        }
    }

}
