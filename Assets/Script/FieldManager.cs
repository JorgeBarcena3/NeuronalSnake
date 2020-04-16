﻿using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class FieldManager : MonoBehaviour
{
    // Food Prefab
    public GameObject foodPrefab;

    private float limit_spacing;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    public GameObject position_fruit;


    // Spawn one piece of food
    void SpawnFood()
    {

        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x + limit_spacing,
                                  borderRight.position.x - limit_spacing);

        // y position between top & bottom border
        int y = (int)Random.Range(borderBottom.position.y + limit_spacing,
                                  borderTop.position.y - limit_spacing);

        // Instantiate the food at (x, y)
        position_fruit = Instantiate(foodPrefab,
                    new Vector2(x, y),
                    Quaternion.identity);
    }

    /// <summary>
    /// Inicializa el campo
    /// </summary>
    public void init()
    {
        GameManager.SpawnFood += SpawnFood;
        limit_spacing = foodPrefab.transform.localScale.x * 2;

        SpawnFood();    

    }
}