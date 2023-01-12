using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manejo del orden general de las constelaciones en el mapa
public class ConstellationManager : MonoBehaviour
{
    public enum ConstellationPhase { INTRO, QUESTION, OUTRO };

    // Variables
    public MessagesDisplay messagesDisplay;
    [SerializeField]
    private GameObject constellationPrefab;
    [NonSerialized]
    public bool isSpawned; // Indica si ya se realizó el spawneo
    private GameManager gameManager;
    private List<Constellation> constellations;

    // Singleton
    public static ConstellationManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        constellations = gameManager.mustakisGameData.scenes;
    }

    // Spawnear constelaciones (al azar)
    public void SpawnConstellations()
    {

        List<(int x, int y)> outerCoords = new List<(int, int)> {
            (-8, 9), (-6, 9), (-4, 9), (-2, 9), (-0, 9), (2, 9), (4, 9), (6, 9), (8, 9),
            (11, 9), (11, 11), (11, 13), (11, 15), (11, 17), (11, 19), (11, 21), (11, 23), (11, 25),
            (11, 27), (-6, 27), (-4, 27), (-2, 27), (-0, 27), (2, 27), (4, 27), (6, 27), (8, 27),
            (-8, 27), (-8, 11), (-8, 13), (-8, 15), (-8, 17), (-8, 19), (-8, 21), (-8, 23), (-8, 25),
        };

        int type = 1;
        foreach (Constellation constellation in constellations)
        {
            int n_coords = outerCoords.Count;
            int indexChoosen = UnityEngine.Random.Range(0, n_coords - 1);
            SpawnConstellation(constellation.name, constellation, outerCoords[indexChoosen].x, outerCoords[indexChoosen].y, type);
            outerCoords.RemoveAt(indexChoosen);
            type++;
            if (type > 5) type = 1;
        }
        isSpawned = true;
    }

    // Spawnear una constelación
    private void SpawnConstellation(string info, Constellation constellation, float x, float y, int type)
    {
        GameObject newConstellation = Instantiate(constellationPrefab, new Vector3(x, y, 0), Quaternion.identity);
        ConstellationNPC currConstellationNPC = newConstellation.GetComponent<ConstellationNPC>();
        switch (type)
        {
            case 1:
                currConstellationNPC.itemType = ConstellationNPC.ItemType.Book1;
                break;
            case 2:
                currConstellationNPC.itemType = ConstellationNPC.ItemType.Book2;
                break;
            case 3:
                currConstellationNPC.itemType = ConstellationNPC.ItemType.Book3;
                break;
            case 4:
                currConstellationNPC.itemType = ConstellationNPC.ItemType.Book4;
                break;
            case 5:
                currConstellationNPC.itemType = ConstellationNPC.ItemType.Book5;
                break;
            default:
                currConstellationNPC.itemType = ConstellationNPC.ItemType.Book1;
                break;
        }
        currConstellationNPC.content = info;
        currConstellationNPC.constellation = constellation;
        currConstellationNPC.gameObject.SetActive(true); // Necesario pues template "constellationPrefab" está desactivado
    }
}
