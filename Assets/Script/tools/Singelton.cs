using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que obliga a tener solamente un objeto de este tipo
/// </summary>
public class Singelton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

        }
        else
        {
            if (this != instance)
                Destroy(this.gameObject);
        }
    }
}