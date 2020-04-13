using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Maneja la serpiente
/// </summary>
public class Snake : MonoBehaviour
{


    /// <summary>
    /// Prefab de la cola de la serpiente
    /// </summary>
    public GameObject tailPrefab;

    /// <summary>
    /// Cada cuanto tiempo se movera la serpiente
    /// </summary>
    public float time_to_move = 0.3f;

    /// <summary>
    /// Determina si la serpiente ha comido o no
    /// </summary>
    private bool lleno = false;

    /// <summary>
    /// Movimiento de avance
    /// </summary>
    private float movement;

    /// <summary>
    /// Direccion actual de la serpiete
    /// </summary>
    private Vector2 current_direction = Vector2.right;

    /// <summary>
    /// Lista de posiciones de la cola
    /// </summary>
    private List<Transform> tail = new List<Transform>();

    /// <summary>
    /// Inicializa la serpiente
    /// </summary>
    public void init()
    {
        InvokeRepeating("DoMovement", time_to_move, time_to_move);
        movement = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            current_direction = current_direction == Vector2.left ? current_direction : Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            current_direction = current_direction == Vector2.up? current_direction : Vector2.down;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            current_direction = current_direction == Vector2.right ? current_direction : Vector2.left;
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            current_direction = current_direction == Vector2.down ? current_direction : Vector2.up;

    }

    /// <summary>
    /// Realizamos un movimiento de la serpieten
    /// </summary>
    void DoMovement()
    {
        // Guardamos la posicion en la que estamos (Para añadir a la cola)
        Vector2 old_pos = transform.position;

        // Movemos la serpiesnte
        transform.Translate(current_direction * movement);

        // Si hemos comido algo, lo añadimos
        if (lleno)
        {
            // Instanciamos el objeto en la posicion anterior
            GameObject obj = Instantiate(tailPrefab,
                              old_pos,
                              Quaternion.identity);

            // Lo guardamos en nuestra lista
            tail.Insert(0, obj.transform);

            // Ya ha comido, reseteamos el flag
            lleno = false;
        }
        else if (tail.Count > 0)
        {   
            //Cambiamos las posiciones de la ultima cola
            tail.Last().position = old_pos;
            
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }

    }

    /// <summary>
    /// Reinicia la serpiente
    /// </summary>
    public void restart()
    {

        foreach (var item in tail)
        {
            Destroy(item.gameObject);
        }

        //clear the tail
        tail.Clear();

        //reset to origin
        transform.position = new Vector3(0, 0, 0);

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Si es una comida
        if (coll.tag == "food")
        {
            // Get longer in next Move call
            lleno = true;

            Gamemanager.getInstance().spawnFoodOnField();

            // Remove the Food
            Destroy(coll.gameObject);
        }
        if(coll.tag == "tail" || coll.tag == "border")
        {
            Gamemanager.getInstance().restart();
        }
    }
}
