using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Campo donde se jugara
/// </summary>
public class Field : MonoBehaviour
{
    [Header("Limites del campo")]
    /// <summary>
    /// Campo jugable por el jugador
    /// </summary>
    public Rect size;

    [Header("Arte")]
    /// <summary>
    /// Background que se aplicara al campo
    /// </summary>
    public Sprite background;

    /// <summary>
    /// Componente del sprite renderer
    /// </summary>
    private SpriteRenderer _SpriteRendererComponent;

    /// <summary>
    /// Inicial el gameobject
    /// </summary>
    public void init()
    {
        _SpriteRendererComponent = this.gameObject.AddComponent<SpriteRenderer>();
        _SpriteRendererComponent.sprite = background;
        _SpriteRendererComponent.drawMode = SpriteDrawMode.Sliced;
        _SpriteRendererComponent.size = size.size;
        _SpriteRendererComponent.color = Color.black;
    }
}
