using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrucaoDeEquacoes : MonoBehaviour
{
    public GameObject Mira;
    public float velocidade = 5f;
    public float amplitudeOscilacao = 0.007f; // controla a intensidade da oscilação
    public float frequenciaOscilacao = 2f;    // controla a velocidade da oscilação
    private bool podeMover = true;            // controla se a mira pode se mover

    void Update()
    {
        if (Mira != null && podeMover)
        {
            // Captura o input do jogador
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Calcula o vetor de movimento com base na entrada do jogador e na velocidade
            Vector3 movimento = new Vector3(horizontal, vertical, 0) * velocidade * Time.deltaTime;

            // Aplica uma oscilação à posição da mira
            float oscilacaoX = Mathf.Sin(Time.time * frequenciaOscilacao) * amplitudeOscilacao;
            float oscilacaoY = Mathf.Cos(Time.time * frequenciaOscilacao) * amplitudeOscilacao;

            // Adiciona a oscilação ao movimento
            movimento.x += oscilacaoX;
            movimento.y += oscilacaoY;

            // Move o objeto para a nova posição com oscilação
            Mira.transform.Translate(movimento);
        }

        // Verifica se a tecla espaço foi pressionada para pausar o movimento
        if (Input.GetKeyDown(KeyCode.Space) && podeMover)
        {
            StartCoroutine(PausarMovimento());
        }
    }

    // Corrotina para pausar o movimento da mira
    IEnumerator PausarMovimento()
    {
        podeMover = false;       // Desativa o movimento
        yield return new WaitForSeconds(3f);  // Espera por 3 segundos
        podeMover = true;        // Reativa o movimento
    }
}
