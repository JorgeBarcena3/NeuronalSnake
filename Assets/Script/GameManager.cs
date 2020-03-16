using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instancia que maneja la partida
/// </summary>
public class GameManager : Singelton<GameManager>
{

    /// <summary>
    /// Tiempo para realizar el update
    /// </summary>
    [Header("Update time")]
    public float updateTime;

    /// <summary>
    /// Tiempo que ha pasado hasta el siguiente update
    /// </summary>
    private float currentTime = 0;

    /// <summary>
    /// Delegado que se lanza cuando se cumple el tiempo de espera
    /// </summary>
    public static event Action OnTickUpdate = delegate { };

    [Header("Objetos a administrar")]
    /// <summary>
    /// Campo donde se va a ejecutar el juego
    /// </summary>
    public Field field;

    /// <summary>
    /// Snake
    /// </summary>
    public Snake snake;

    // Start is called before the first frame update
    void Start()
    {
        field.init();
        snake.init();
       
    }

    // Update is called once per frame
    void Update()
    {

        currentTime += Time.deltaTime;

        if(currentTime >= updateTime)
        {
            OnTickUpdate();
            currentTime = 0;
        }
    }
}
