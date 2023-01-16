using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ConstellationNPC> itemList;
    public delegate void UpdateInventory();
    public event UpdateInventory Updated;

    public Inventory()
    {
        itemList = new List<ConstellationNPC>();
        Debug.Log("Inventario de Constelaciones");
    }

    public void AddConstellation(ConstellationNPC constellation)
    {
        Debug.Log("Constellation added");
        itemList.Add(constellation);
        Updated?.Invoke();
    }

    public List<ConstellationNPC> GetConstellationList()
    {
        return itemList;
    }

    public void Clear()
    {
        itemList.Clear();
        Updated?.Invoke();
    }

    public void Update()
    {
        Updated?.Invoke();
    }
}
