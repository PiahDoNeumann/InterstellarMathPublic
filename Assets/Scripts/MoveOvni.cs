using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOvni : MonoBehaviour
{
    public float velocidade = 5.0f;
    public GameObject controller;
    public GameObject Player1, Player2, Player3, Player4; // Referência ao Player1

    private ControllerRodadas controllerScript;

    void Start()
    {
        controllerScript = controller.GetComponent<ControllerRodadas>();
    }

    void Update()
    {
        if (controllerScript.idPlayerCurrent == 0)
        {
            // Obtenha a posição do Player1
            Vector3 playerPos = Player1.transform.position;

            // Defina a posição do OVNI acima da cabeça do Player1
            Vector3 ovniPos = playerPos + Vector3.up * 2.0f; // Ajuste 2.0f para a altura desejada

            // Interpole suavemente a posição do OVNI para seguir o Player1
            transform.position = Vector3.Lerp(transform.position, ovniPos, Time.deltaTime * velocidade);
        }

        if (controllerScript.idPlayerCurrent == 1)
        {
            // Obtenha a posição do Player2
            Vector3 playerPos = Player2.transform.position;

            // Defina a posição do OVNI acima da cabeça do Player1
            Vector3 ovniPos = playerPos + Vector3.up * 2.0f; // Ajuste 2.0f para a altura desejada

            // Interpole suavemente a posição do OVNI para seguir o Player1
            transform.position = Vector3.Lerp(transform.position, ovniPos, Time.deltaTime * velocidade);
        }

        if (controllerScript.idPlayerCurrent == 2)
        {
            // Obtenha a posição do Player3
            Vector3 playerPos = Player3.transform.position;

            // Defina a posição do OVNI acima da cabeça do Player1
            Vector3 ovniPos = playerPos + Vector3.up * 2.0f; // Ajuste 2.0f para a altura desejada

            // Interpole suavemente a posição do OVNI para seguir o Player1
            transform.position = Vector3.Lerp(transform.position, ovniPos, Time.deltaTime * velocidade);
        }

        if (controllerScript.idPlayerCurrent == 3)
        {
            // Obtenha a posição do Player4
            Vector3 playerPos = Player4.transform.position;

            // Defina a posição do OVNI acima da cabeça do Player1
            Vector3 ovniPos = playerPos + Vector3.up * 2.0f; // Ajuste 2.0f para a altura desejada

            // Interpole suavemente a posição do OVNI para seguir o Player1
            transform.position = Vector3.Lerp(transform.position, ovniPos, Time.deltaTime * velocidade);
        }
    }
}
         