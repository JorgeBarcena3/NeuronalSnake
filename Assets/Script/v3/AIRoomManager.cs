using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja toda la ejecucion del juego
/// </summary>
public class AIRoomManager : MonoBehaviour
{
  

    /// <summary>
    /// Maneja el estado del campo
    /// </summary>
    public AIFieldManager fieldManager;

    /// <summary>
    /// Seripiente que se encuentra en juego
    /// </summary>
    public AISnake snake;

    /// <summary>
    /// Delegado que se lanza cuando se cumple el tiempo de espera
    /// </summary>
    public event Action SpawnFood = delegate { };

   

    // Start is called before the first frame update
    void Awake()
    {
        fieldManager.init();
        snake.init();
    }

    /// <summary>
    /// Llama al delegado para spawnear una comida
    /// </summary>
    public void spawnFoodOnField()
    {
        SpawnFood();
    }
}
