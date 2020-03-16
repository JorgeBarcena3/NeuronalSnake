using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Posibles partes del cuerpo
/// </summary>
public enum BODY_TYPE
{
    HEAD = 0,
    BODY = 1,
    TAIL = 2
}

/// <summary>
/// Partes del cuerpo de la serpiente
/// </summary>
public class BodyParts : MonoBehaviour
{

    [HideInInspector]
    /// <summary>
    /// Tipo del cuerpo
    /// </summary>
    public BODY_TYPE type;

    /// <summary>
    /// Padre donde se instanciará la parte
    /// </summary>
    private BodyParts parent;

    /// <summary>
    /// Sprite Render
    /// </summary>
    private SpriteRenderer _SpriteRendererComponent;

    /// <summary>
    /// Inicializa la parte del cuerpo
    /// </summary>
    public BodyParts init(BODY_TYPE t, BodyParts p = null)
    {
        this.type = t;
        this.parent = p;

        this._SpriteRendererComponent = this.gameObject.AddComponent<SpriteRenderer>();
        this._SpriteRendererComponent.sprite = Resources.Load<Sprite>(t.ToString());

        if (p != null)
        {
            this.transform.localPosition = new Vector3(
                this.transform.localPosition.x,
                this.transform.localPosition.y - 1,
                this.transform.localPosition.z);
        }

        return this;
    }

    /// <summary>
    /// Cambiamos el
    /// </summary>
    /// <param name="color"></param>
    public void changeColor(Color color)
    {
        _SpriteRendererComponent.color = color;
    }

    /// <summary>
    /// Hacemos un update de la posicion
    /// </summary>
    public void updatePosition()
    {
        if (parent != null)
        {
            this.transform.position = new Vector3(
                parent.transform.position.x,
                parent.transform.position.y,
                parent.transform.position.z
                );

            print(this.transform.position);
        }
    }
}
