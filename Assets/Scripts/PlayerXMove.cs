using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXMove : MonoBehaviour
{
    public Transform[] housePositions; // Array de posições das casas.
    public float speed = 5f; // Velocidade de movimento do personagem.
    public int currentHouseIndex = 0; // Índice da casa atual.
    public int IndicePlayerAdd;

    private void Start()
    {
        // Define a posição inicial do personagem para a primeira casa.
        transform.position = housePositions[currentHouseIndex].position;
        IndicePlayerAdd = -7;
    }

    private void Update()
    {
        // Verifica se a tecla de espaço foi pressionada.
        if (Input.GetKeyDown(KeyCode.D))
        {

            // Adiciona o número aleatório ao índice da casa.
            currentHouseIndex += 1;
            IndicePlayerAdd += 1;

            // Garante que o índice não ultrapasse os limites do array.
            currentHouseIndex = Mathf.Clamp(currentHouseIndex, 0, housePositions.Length - 1);

            // Move o personagem para a casa correspondente.
            transform.position = housePositions[currentHouseIndex].position;

        }else if(Input.GetKeyDown(KeyCode.A))
        {
            //Remove o número 1 no índice da casa.
            currentHouseIndex -= 1;
            IndicePlayerAdd -= 1;

            // Garante que o índice não ultrapasse os limites do array.
            currentHouseIndex = Mathf.Clamp(currentHouseIndex, 0, housePositions.Length - 1);

            // Move o personagem para a casa correspondente.
            transform.position = housePositions[currentHouseIndex].position;

        }
    }
}
