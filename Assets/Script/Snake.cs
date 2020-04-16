using UnityEngine;
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
    /// Accion que estamos realizando ahora
    /// </summary>
    private float currentAction = 1;

    /// <summary>
    /// Lista de posiciones de la cola
    /// </summary>
    private List<Transform> tail = new List<Transform>();
    /// <summary>
    /// posicion de la siguiente fruta
    /// </summary>
    public Vector2 position_fruit;
    /// <summary>
    /// Inicializa la serpiente
    /// </summary>
    public void Start()
    {
        movement = this.transform.localScale.x;
    }

    public void init()
    {
        restart();
        CancelInvoke();
        InvokeRepeating("DoMovement", time_to_move, time_to_move);
        position_fruit = GameManager.getInstance().FieldManager.position_fruit.transform.position;
    }

    public override void OnEpisodeBegin()
    {
       
       
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        float[] temp = lookInDirection(new Vector2(-1, 0));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(-1, -1));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(0, -1));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(1, -1));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(1, 0));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(1, 1));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(0, 1));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(-1, 1));
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);


    }

    /// <summary>
    /// HAcemos colision con los limites
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool collideLimits(Vector2 point)
    {
        if (point.x > GameManager.getInstance().FieldManager.borderRight.position.x ||
               point.x < GameManager.getInstance().FieldManager.borderLeft.position.x ||
               point.y > GameManager.getInstance().FieldManager.borderTop.position.y ||
               point.y < GameManager.getInstance().FieldManager.borderBottom.position.y)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Colisionamos con un cuerpo
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool collideBody(Vector2 point)
    {
        foreach (var item in tail)
        {
            if (Vector2.Distance(item.transform.position, point) < 2)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Colisionamos con una comida
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool collideFood(Vector2 point)
    {
        return Vector2.Distance(GameManager.getInstance().FieldManager.position_fruit.transform.position, point) < 4;
    }

    /// <summary>
    /// MIramos en una direccion
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public float[] lookInDirection(Vector2 direction)
    {
        float[] values = new float[3];

        Vector2 pos = this.transform.position;
        pos += (direction * movement);

        float distance = 0;
        bool foodFound = false;
        bool bodyFound = false;
        distance += movement;

        while (!collideLimits(pos))
        {
            if (!foodFound && collideFood(pos))
            {
                foodFound = true;
                values[0] = 1;
            }
            //if (!bodyFound && collideBody(pos))
            //{
            //    bodyFound = true;
            //    values[1] = 1;
            //}


            if (foodFound)
            {
                Debug.DrawLine(this.transform.position, GameManager.getInstance().FieldManager.position_fruit.transform.position, Color.blue);
            }
            else
            {
                Debug.DrawLine(this.transform.position, pos, Color.red);
            }

            pos += (direction * movement);
            distance += movement;
        }

        values[2] = 1 / distance;
        return values;
    }


    /// <summary>
    /// Seteamos unos u otros valores de manera manual
    /// </summary>
    /// <returns></returns>
    public override float[] Heuristic()
    {
        var action = new float[1];
        action[0] = currentAction;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            action[0] = currentAction = -0.7f;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            action[0] = currentAction = -0.2f;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            action[0] = currentAction = 0.2f;
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            action[0] = currentAction = 0.7f;

        return action;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (vectorAction[0] > 0.5f)
            vectorAction[0] = 4;
        else if (vectorAction[0] > 0.0f)
            vectorAction[0] = 3;
        else if (vectorAction[0] > -0.5f)
            vectorAction[0] = 2;
        else
            vectorAction[0] = 1;


        switch ((int)vectorAction[0])
        {
            case 1:
                current_direction = current_direction != Vector2.left ? Vector2.right : current_direction;
                break;
            case 2:
                current_direction = current_direction != Vector2.up ? Vector2.down : current_direction;
                break;
            case 3:
                current_direction = current_direction != Vector2.right ? Vector2.left : current_direction;
                break;
            case 4:
                current_direction = current_direction != Vector2.down ? Vector2.up : current_direction;
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// Realizamos un movimiento de la serpieten
    /// </summary>
    void DoMovement()
    {
        RequestDecision();

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
        if (this.transform.position.x > GameManager.getInstance().FieldManager.borderRight.position.x ||
                this.transform.position.x < GameManager.getInstance().FieldManager.borderLeft.position.x ||
                this.transform.position.y > GameManager.getInstance().FieldManager.borderTop.position.y ||
                this.transform.position.y < GameManager.getInstance().FieldManager.borderBottom.position.y)
        {
            AddReward(-2f);
            EndEpisode();
        }

        //float end_distance_to_fruit = Vector2.Distance(this.transform.position, position_fruit);
        //if (end_distance_to_fruit < init_distance_to_fruit)
        //{
        //    AddReward(-Vector2.Distance(this.transform.position, position_fruit));
        //}
        //else
        //{
        //    AddReward(-0.5f);
        //}

        AddReward(-Vector2.Distance(this.transform.position, position_fruit));

        print("Reward: " + GetCumulativeReward());

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

            GameManager.getInstance().spawnFoodOnField();
            if (tail.Count > 25)
            {
                AddReward(5f);
                EndEpisode();
            }
            else
            {
                // Remove the Food
                AddReward(5f);
                Destroy(coll.gameObject);
                EndEpisode();
                position_fruit = GameManager.getInstance().FieldManager.position_fruit.transform.position;
            }

        }
        if ( coll.tag == "border") //coll.tag == "tail" ||
        {
            AddReward(-5f);
            init();
        }
    }
}
