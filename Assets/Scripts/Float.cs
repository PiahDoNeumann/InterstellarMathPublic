using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float floatStrength = 0.2f; // Força da flutuação
    public float speed = 0.5f; // Velocidade da flutuação
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }


    void Update()
    {

        float newY = initialPosition.y + (Mathf.Sin(Time.time * speed)/2) * floatStrength;      //Seno
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
