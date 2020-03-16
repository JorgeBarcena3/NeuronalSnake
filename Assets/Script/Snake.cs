using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Direccione
/// </summary>
public enum DIRECTION
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

/// <summary>
/// Snake
/// </summary>
public class Snake : MonoBehaviour
{
    /// <summary>
    /// Partes del cuerpo
    /// </summary>
    private List<BodyParts> body = new List<BodyParts>();

    /// <summary>
    /// Referencia a la cabeza de la snake
    /// </summary>
    private BodyParts head;

    /// <summary>
    /// Prefab del cuerpo
    /// </summary>
    public GameObject bodyPrefab;

    /// <summary>
    /// Direccion elegida
    /// </summary>
    public DIRECTION currentDirection;

    /// <summary>
    /// Inicializa la serpiente
    /// </summary>
    public void init()
    {
        head = Instantiate(bodyPrefab, this.transform).GetComponent<BodyParts>().init(BODY_TYPE.HEAD);
        head.changeColor(Color.red);

        body.Add(head);
        body.Add(Instantiate(bodyPrefab, body.Last().transform).GetComponent<BodyParts>().init(BODY_TYPE.BODY, body.Last()));
        body.Add(Instantiate(bodyPrefab, body.Last().transform).GetComponent<BodyParts>().init(BODY_TYPE.BODY, body.Last()));

        currentDirection = DIRECTION.UP;

        GameManager.OnTickUpdate += updateBody;

    }

    /// <summary>
    /// Establecemos la nueva posicion del cuerpo
    /// </summary>
    private void updateBody()
    {
        switch (currentDirection)
        {
            case DIRECTION.UP:
                head.transform.localPosition = new Vector3(head.transform.localPosition.x, head.transform.localPosition.y + 1, head.transform.localPosition.z);
                break;
            case DIRECTION.DOWN:
                head.transform.localPosition = new Vector3(head.transform.localPosition.x, head.transform.localPosition.y - 1, head.transform.localPosition.z);
                break;
            case DIRECTION.LEFT:
                head.transform.localPosition = new Vector3(head.transform.localPosition.x - 1, head.transform.localPosition.y, head.transform.localPosition.z);
                break;
            case DIRECTION.RIGHT:
                head.transform.localPosition = new Vector3(head.transform.localPosition.x + 1, head.transform.localPosition.y, head.transform.localPosition.z);
                break;
        }

        for (int i = body.Count - 1; i >= 0; i--)
        {
            body[i].updatePosition();
        }

    }

    /// <summary>
    /// Funcion de update de la serpiente
    /// </summary>
    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.W) )
        {
            currentDirection = DIRECTION.UP;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = DIRECTION.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentDirection = DIRECTION.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentDirection = DIRECTION.RIGHT;
        }
    }



}
