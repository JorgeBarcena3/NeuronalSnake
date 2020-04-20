using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class AIFieldManager : MonoBehaviour
{

    /// <summary>
    /// Manager de la habitacion
    /// </summary>
    [Header("Manager")]
    public AIRoomManager gameManager;

    /// <summary>
    /// Prefab de la comida
    /// </summary>
    [Header("Prefab de la comida a spawnear")]
    public GameObject foodPrefab;

    /// <summary>
    /// Posiciones de los limites
    /// </summary>
    [Header("Posicion de los bordes")]
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    /// <summary>
    /// Comida activa
    /// </summary>
    [HideInInspector]
    public GameObject food;

    /// <summary>
    /// Espacio de margen al spawnear la comida
    /// </summary>
    private float limit_spacing;

    // Spawn one piece of food
    void SpawnFood()
    {

        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x + limit_spacing,
                                  borderRight.position.x - limit_spacing);

        // y position between top & bottom border
        int y = (int)Random.Range(borderBottom.position.y + limit_spacing,
                                  borderTop.position.y - limit_spacing);

        if (food == null)
        {
            // Instantiate the food at (x, y)
            food = Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
        }
        else
        {
            food.transform.position = new Vector2(x, y);
        }
            
    }

    /// <summary>
    /// Inicializa el campo
    /// </summary>
    public void init()
    {
        gameManager.SpawnFood += SpawnFood;
        limit_spacing = foodPrefab.transform.localScale.x * 4;

        SpawnFood();

    }
}