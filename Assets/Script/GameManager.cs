using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja toda la ejecucion del juego
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Instancia del gamemanager
    /// </summary>
    private static GameManager instance;

    /// <summary>
    /// Devuelve la estancia activa del gamemanager
    /// </summary>
    /// <returns></returns>
    public static GameManager getInstance()
    {
        return instance;
    }

    /// <summary>
    /// Maneja el estado del campo
    /// </summary>
    public FieldManager FieldManager;

    /// <summary>
    /// Seripiente que se encuentra en juego
    /// </summary>
    public Snake snake;

    /// <summary>
    /// Delegado que se lanza cuando se cumple el tiempo de espera
    /// </summary>
    public static event Action SpawnFood = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        FieldManager.init();
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
