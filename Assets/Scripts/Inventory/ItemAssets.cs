using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite book1;
    public Sprite book2;
    public Sprite book3;
    public Sprite book4;
    public Sprite book5;

    public static void RestartStatic()
    {
        Instance = null;
    }
}
