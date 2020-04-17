﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using MLAgents;
using MLAgents.Sensors;

/// <summary>
/// Maneja la serpiente
/// </summary>
public class Snake : Agent
{


    /// <summary>
    /// Prefab de la cola de la serpiente
    /// </summary>
    public GameObject tailPrefab;

    /// <summary>
    /// Cada cuanto tiempo se movera la serpiente
    /// </summary>
    public float time_to_move = 0f;

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
    /// posicion de la siguiente fruta
    /// </summary>
    public Vector2 position_fruit;
    /// <summary>
    /// Maneja el estado del campo
    /// </summary>
    public FieldManager FieldManager;
    /// <summary>
    /// deltatime del juego
    /// </summary>
    private float my_delta_time;

    private bool [] sensor_cola = new bool[8];
    private bool [] sensor_fruta = new bool[8];


    /// <summary>
    /// Inicializa la serpiente
    /// </summary>
    /// 
    public void Start()
    {
        FieldManager.init();
        my_delta_time = 0;
        movement = this.transform.localScale.x;
    }
    public override void OnEpisodeBegin()
    {
        restart();
        CancelInvoke();
        position_fruit = FieldManager.position_fruit;
    }
    private void FixedUpdate()
    {
        my_delta_time += Time.deltaTime;
        if (my_delta_time > time_to_move)
        {
            DoMovement();
            RequestAction();
            my_delta_time = 0;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Mathf.Abs(transform.position.x - FieldManager.borderRight.position.x));      //1
        sensor.AddObservation(Mathf.Abs(transform.position.x - FieldManager.borderLeft.position.x));        //1
        sensor.AddObservation(Mathf.Abs(transform.position.y - FieldManager.borderTop.position.y));         //1
        sensor.AddObservation(Mathf.Abs(transform.position.y - FieldManager.borderBottom.position.y));      //1
        sensor.AddObservation(Vector2.Distance(transform.position, position_fruit));                        //1
        sensor.AddObservation(current_direction);                                                   //2
        sensor.AddObservation(tail.Count());                                                        //1
       
    }
    public override float[] Heuristic()
    {
        var action = new float[2];
      
        action[0] = Input.GetAxis("Horizontal") > 0 ? 1 : Input.GetAxis("Horizontal") < 0 ? -1 : 0;
        action[1] = Input.GetAxis("Vertical") > 0 ? 1 : Input.GetAxis("Vertical") < 0 ? - 1 : 0;
        
        return action;
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        current_direction += new Vector2(vectorAction[0], vectorAction[1]);
        current_direction.Normalize();
        
        
        //if (current_direction == Vector2.left)
        //{
        //    current_direction = vectorAction[1] > vectorAction[0] && vectorAction[1] > vectorAction[2] ? Vector2.left : vectorAction[2] > vectorAction[0] ? Vector2.up : Vector2.down;
        //}
        //else if (current_direction == Vector2.up)
        //{
        //    current_direction = vectorAction[1] > vectorAction[0] && vectorAction[1] > vectorAction[2] ? Vector2.up : vectorAction[2] > vectorAction[0] ? Vector2.right :Vector2.left;
        //}
        //else if (current_direction == Vector2.right)
        //{
        //    current_direction = vectorAction[1] > vectorAction[0] && vectorAction[1] > vectorAction[2] ? Vector2.right : vectorAction[2] > vectorAction[0] ? Vector2.down : Vector2.up;
        //}
        //else if (current_direction == Vector2.down)
        //{
        //    current_direction = vectorAction[1] > vectorAction[0] && vectorAction[1] > vectorAction[2] ? Vector2.down : vectorAction[2] > vectorAction[0] ? Vector2.left : Vector2.right;
        //}
        
    }
    /// <summary>
    /// Realizamos un movimiento de la serpieten
    /// </summary>
    void DoMovement()
    {
        float init_distance_to_fruit = Vector2.Distance(this.transform.position, position_fruit);
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
        if (this.transform.position.x > FieldManager.borderRight.position.x ||
                this.transform.position.x < FieldManager.borderLeft.position.x ||
                this.transform.position.y > FieldManager.borderTop.position.y ||
                this.transform.position.y < FieldManager.borderBottom.position.y)
        {
            AddReward(-2f);
            EndEpisode();
        }

        float end_distance_to_fruit = Vector2.Distance(this.transform.position, position_fruit);
        if (end_distance_to_fruit < init_distance_to_fruit)
        {
            AddReward(0.1f);
        }
        else if(end_distance_to_fruit > init_distance_to_fruit)
        {
            AddReward(-0.2f);
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
        transform.position = FieldManager.transform.position;
        Destroy(FieldManager.reference);
        FieldManager.SpawnFood();

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Si es una comida
        if (coll.tag == "food")
        {
            // Get longer in next Move call
            lleno = true;

            FieldManager.SpawnFood();
            if (tail.Count > 25)
            {
                AddReward(25f);
                EndEpisode();
            }
            else
            {
                // Remove the Food
                Destroy(coll.gameObject);
                AddReward(5f);
                position_fruit = FieldManager.position_fruit;
            }
           
        }
        if(coll.tag == "tail" || coll.tag == "border")
        {
            AddReward(-2f);
            EndEpisode();
        }
    }
}
