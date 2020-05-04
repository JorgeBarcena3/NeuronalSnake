using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{

    /// <summary>
    /// Texto de titulo
    /// </summary>
    Text texto;

    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        texto.color =  new Color(Mathf.Sin(Time.time), Mathf.Sin(Time.time / 2), Mathf.Sin(Time.time));
        print(Mathf.Sin(Time.time * 0.1f));
    }
}
