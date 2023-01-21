using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationAssets : MonoBehaviour
{
    public static ConstellationAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite constellation1OFF;
    public Sprite constellation2OFF;
    public Sprite constellation3OFF;

    public Sprite constellation1ON;
    public Sprite constellation2ON;
    public Sprite constellation3ON;

    public static void RestartStatic()
    {
        Instance = null;
    }
}
