using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Maneja el juego de la IA contra el usuario
/// </summary>
public class GameplayManager : MonoBehaviour
{
    /// <summary>
    /// Manager de la IA
    /// </summary>
    public AIRoomManager IA;

    /// <summary>
    /// Manager del usuario
    /// </summary>
    public AIRoomManager user;

    /// <summary>
    /// Texto de la IA
    /// </summary>
    public Text IA_text;

    /// <summary>
    /// Texto del usuario
    /// </summary>
    public Text user_text;

    /// <summary>
    /// Texto del usuario
    /// </summary>
    public Text winner_text;

    /// <summary>
    /// Puntos de la IA
    /// </summary>
    private int IAPoints;

    /// <summary>
    /// Puntos del usuario
    /// </summary>
    private int userPoints;


    // Start is called before the first frame update
    void Start()
    {
        IA.SpawnFood += IAPoint;
        user.SpawnFood += UserPoint;
        IA.snake.deadDelegate += restart;
        user.snake.deadDelegate += restart;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// La IA obtiene un punto 
    /// </summary>
    void IAPoint()
    {
        IAPoints += 100 + (IA.snake.tail.Count * 10);
        IA_text.GetComponent<Text>().text = "Puntuacion: " + IAPoints;
    }

    /// <summary>
    /// El usuario obtiene un punto
    /// </summary>
    void UserPoint()
    {
        userPoints += 100 + (user.snake.tail.Count * 10);
        user_text.GetComponent<Text>().text = "Puntuacion: " + userPoints;
    }

    /// <summary>
    /// Reiniciamos la escena
    /// </summary>
    private void restart(AIRoomManager room)
    {
        IA.snake.restart();
        user.snake.restart();

        if(room == IA)
        {
            IAPoints -= 1000;
        }
        else
        {
            userPoints -= 1000;
        }

        winner_text.GetComponent<Text>().text = "Ultimo ganador\n" + ( (userPoints > IAPoints) ? "Usuario" : "IA" );

        userPoints = 0;
        user_text.GetComponent<Text>().text = "Puntuacion: " + userPoints;

        IAPoints = 0;
        IA_text.GetComponent<Text>().text = "Puntuacion: " + IAPoints;

    }
}
