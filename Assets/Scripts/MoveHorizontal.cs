using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{
    public float velocidade = 2.0f;
    public float limiteDireita = 10.0f; // Posição onde a nuvem volta ao início

    private void Update()
    {
        // Move a nuvem para a direita
        transform.Translate(Vector3.right * velocidade * Time.deltaTime);

        // Verifica se a nuvem passou do limite da direita
        if (transform.position.x >= limiteDireita)
        {
            // Reposiciona a nuvem para a esquerda
            Vector3 novaPosicao = transform.position;
            novaPosicao.x = -limiteDireita;
            transform.position = novaPosicao;
        }
    }
}
