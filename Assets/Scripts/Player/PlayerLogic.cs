using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private UiInventory uiInventory;

    private void Start()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }
}
