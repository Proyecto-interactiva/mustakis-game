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

    public Sprite constellation1;
    public Sprite constellation2;
    public Sprite constellation3;

    public static void RestartStatic()
    {
        Instance = null;
    }
}
