using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using MLAgents;
using MLAgents.Sensors;

/// <summary>
/// Informacion a la hora de debuguear
/// </summary>
public class DebugInfo
{
    /// <summary>
    /// Determinanos si se debe pintar o no
    /// </summary>
    public bool eneabled = false;

    /// <summary>
    /// Color de la linea
    /// </summary>
    public Color color;

    /// <summary>
    /// Posicion inicial de la linea
    /// </summary>
    public Vector2 pos_i;

    /// <summary>
    /// Posicion final de la linea
    /// </summary>
    public Vector2 pos_f;

    /// <summary>
    /// Pintamos la linea
    /// </summary>
    public void print()
    {
        if (eneabled)
            Debug.DrawLine(pos_i, pos_f, color);
    }
}

/// <summary>
/// Maneja la serpiente
/// </summary>
public class AISnake : Agent
{
    [Header("Manager")]
    /// <summary>
    /// Manager del room
    /// </summary>
    public AIRoomManager roomManager;

    [Header("Prefab de la cola")]
    /// <summary>
    /// Prefab de la cola de la serpiente
    /// </summary>
    public GameObject tailPrefab;

    [Header("Velocidad de movimiento")]
    /// <summary>
    /// Cada cuanto tiempo se movera la serpiente
    /// </summary>
    public float time_to_move = 0.3f;

    [Header("Modo debug")]
    /// <summary>
    /// Cada cuanto tiempo se movera la serpiente
    /// </summary>
    public bool debugging = false;

    /// <summary>
    /// Tiempo actual
    /// </summary>
    private float current_time = 0;

    /// <summary>
    /// Variables para la depuracion de la vision de la serpiente
    /// </summary>
    private List<DebugInfo> wall_debug = new List<DebugInfo>(8);
    private List<DebugInfo> food_debug = new List<DebugInfo>(8);
    private List<DebugInfo> tail_debug = new List<DebugInfo>(8);

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
    /// Ultima direccion tomada
    /// </summary>
    private Vector2 last_direction = Vector2.right;

    /// <summary>
    /// Lista de posiciones de la cola
    /// </summary>
    public List<Transform> tail = new List<Transform>();

    /// <summary>
    /// Posicion inicial de spawneo
    /// </summary>
    private Vector2 start_pos = Vector2.zero;

    /// <summary>
    /// Determina si quien juega es la IA
    /// </summary>
    public bool isIA = true;

    /// <summary>
    /// Delegado que se lanza cuando la serpiente muere
    /// </summary>
    public event Action<AIRoomManager> deadDelegate = delegate { };

    /// <summary>
    /// Inicializa la serpiente
    /// </summary>
    public void Start()
    {

        start_pos = this.transform.localPosition;

        movement  = this.transform.localScale.x;

        wall_debug = new List<DebugInfo>(8);
        food_debug = new List<DebugInfo>(8);
        tail_debug = new List<DebugInfo>(8);

        // Por cada direccion
        for (int i = 0; i < 8; i++)
        {
            wall_debug.Add(new DebugInfo());
            food_debug.Add(new DebugInfo());
            tail_debug.Add(new DebugInfo());
        }

    }

    /// <summary>
    /// Update
    /// </summary>
    private void FixedUpdate()
    {
        current_time += Time.deltaTime;

        if (current_time > time_to_move && isIA)
        {
            DoMovement();
            RequestDecision();
            current_time = 0;
        }
        else if( !isIA )
        {
            if(current_time > time_to_move)
            {
                current_time = 0;
                DoMovement();
            }
            RequestDecision();
        }

        if (collideLimits(this.transform.position))
        {
            SetReward(-1.0f);
            EndEpisode();
        }


        if (debugging)
        {
            wall_debug.ForEach(m => m.print());
            food_debug.ForEach(m => m.print());
            tail_debug.ForEach(m => m.print());
        }
    }

    /// <summary>
    /// Inicializa la snake
    /// </summary>
    public void init()
    {

        deadDelegate(roomManager);
        food_debug.ForEach(m => m.color = Color.blue);
        tail_debug.ForEach(m => m.color = Color.red);
        wall_debug.ForEach(m => m.color = Color.green);

        restart();
        CancelInvoke();

    }

    /// <summary>
    /// Cuando comienza un episodio
    /// </summary>
    public override void OnEpisodeBegin()
    {
        init();
    }

    /// <summary>
    /// Recogemos la informacion
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {

        float[] temp = lookInDirection(new Vector2(-1, 0), food_debug[0], tail_debug[0], wall_debug[0]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(-1, -1), food_debug[1], tail_debug[1], wall_debug[1]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(0, -1), food_debug[2], tail_debug[2], wall_debug[2]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(1, -1), food_debug[3], tail_debug[3], wall_debug[3]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(1, 0), food_debug[4], tail_debug[4], wall_debug[4]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(1, 1), food_debug[5], tail_debug[5], wall_debug[5]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(0, 1), food_debug[6], tail_debug[6], wall_debug[6]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);

        temp = lookInDirection(new Vector2(-1, 1), food_debug[7], tail_debug[7], wall_debug[7]);
        sensor.AddObservation(temp[0]);
        sensor.AddObservation(temp[1]);
        sensor.AddObservation(temp[2]);


    }

    /// <summary>
    /// Miramos en una direccion
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public float[] lookInDirection(Vector2 direction, DebugInfo food_d, DebugInfo tail_d, DebugInfo wall_d)
    {
        float[] values = new float[3];

        Vector2 pos = this.transform.position;
        pos += (direction * movement);

        float distance = 0;

        food_d.eneabled = false;
        food_d.pos_i = this.transform.position;

        tail_d.eneabled = false;
        tail_d.pos_i = this.transform.position;


        wall_d.eneabled = true;
        wall_d.pos_i = this.transform.position;

        distance += movement;

        while (!collideLimits(pos))
        {
            if (!food_d.eneabled && collideFood(pos))
            {
                food_d.eneabled = true;
                values[0] = 1 / distance;
                food_d.pos_f = pos;
            }
            if (!tail_d.eneabled && collideBody(pos))
            {
                tail_d.eneabled = true;
                values[1] = 1 / distance;
                tail_d.pos_f = pos;
            }

            wall_d.pos_f = pos;

            pos += (direction * movement);
            distance += movement;
        }

        values[2] = 1 / distance;
        return values;
    }



    /// <summary>
    /// Hacemos colision con los limites
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool collideLimits(Vector2 point)
    {
        if (point.x + 2 > roomManager.fieldManager.borderRight.position.x ||
               point.x - 2 < roomManager.fieldManager.borderLeft.position.x ||
               point.y + 2 > roomManager.fieldManager.borderTop.position.y ||
               point.y - 2 < roomManager.fieldManager.borderBottom.position.y)
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
        return Vector2.Distance(roomManager.fieldManager.food.transform.position, point) < 4;
    }

    /// <summary>
    /// Seteamos unos u otros valores de manera manual
    /// </summary>
    /// <returns></returns>
    public override float[] Heuristic()
    {
        var action = new float[1];

        action[0] = last_direction == Vector2.right ? 0 : (last_direction == -Vector2.right) ? 2: (last_direction == Vector2.up) ? 3 : 1 ;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            action[0] = 0f;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            action[0] = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            action[0] = 2;
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            action[0] = 3f;

        return action;
    }

    /// <summary>
    /// Cuando recibimos la informacion, selecionamos una direccion
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {

        //print("Accion elegida: " + (int)vectorAction[0] + " || " + vectorAction[0]);

        switch ((int)vectorAction[0])
        {
            case 0:
                current_direction = last_direction != Vector2.left ? Vector2.right : current_direction;
                break;
            case 1:
                current_direction = last_direction != Vector2.up ? Vector2.down : current_direction;
                break;
            case 2:
                current_direction = last_direction != Vector2.right ? Vector2.left : current_direction;
                break;
            case 3:
                current_direction = last_direction != Vector2.down ? Vector2.up : current_direction;
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// Realizamos un movimiento de la serpiente
    /// </summary>
    void DoMovement()
    {

        float init_distance_to_fruit = Vector2.Distance(this.transform.position, roomManager.fieldManager.food.transform.position);
        // Guardamos la posicion en la que estamos (Para añadir a la cola)
        Vector2 old_pos = transform.position;

        last_direction = current_direction;
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

            if (collideBody(this.transform.position))
            {
                SetReward(-1.0f);
                EndEpisode();
            }
            else
            {
                //Cambiamos las posiciones de la ultima cola
                tail.Last().position = old_pos;

                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }


        }

        float end_distance_to_fruit = Vector2.Distance(this.transform.position, roomManager.fieldManager.food.transform.position);
        if (end_distance_to_fruit < init_distance_to_fruit)
        {
            AddReward(+0.1f);
        }


        // Lo añadimos cuanto mas rapido mejor
        if (maxStep > 0) AddReward(-1f / maxStep);
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
        transform.localPosition = start_pos;


        Destroy(roomManager.fieldManager.food);
        roomManager.fieldManager.food = null;
        roomManager.spawnFoodOnField();



    }


    /// <summary>
    /// Cuando colisionamos con algun elemento
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerEnter2D(Collider2D coll)
    {

        // Si es una comida
        if (coll.tag == "food")
        {
            // Get longer in next Move call
            lleno = true;

            roomManager.fieldManager.food = null;
            Destroy(coll.gameObject);
            roomManager.spawnFoodOnField();

            // Remove the Food
            AddReward(.5f);

        }
        if (coll.tag == "tail" || coll.tag == "border") // ||
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
}
