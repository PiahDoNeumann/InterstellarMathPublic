using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Translacao : MonoBehaviour
{
    public float raio = 5f; // Raio do movimento circular
    public float velocidadeRotacao = 2f; // Velocidade de rotação
    public float xIni = 0f; // Posição inicial x
    public float yIni = 0f; // Posição inicial y

    public GameObject objetoAlvo;
 
    void Awake(){
        ResetPlayerPositions();
        LimparArquivoDePontos();
    }

    void ResetPlayerPositions()
    {
        for (int i = 0; i < 4; i++) // Considerando que há 4 jogadores
        {
            string key = "PlayerPosition_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key); // Remove a posição salva de cada jogador
                Debug.Log($"Posição do jogador {i} foi resetada.");
            }
        }
        PlayerPrefs.Save(); // Garante que as mudanças sejam salvas
        Debug.Log("Posições dos jogadores foram resetadas no início do jogo.");
    }


    public void LimparArquivoDePontos()
    {
        // Define os pontos de todos os jogadores para 0
        PlayerPrefs.SetInt("Pontos_1", 0);
        PlayerPrefs.SetInt("Pontos_2", 0);
        PlayerPrefs.SetInt("Pontos_3", 0);
        PlayerPrefs.SetInt("Pontos_4", 0);

        // Salva os dados no PlayerPrefs
        PlayerPrefs.Save();
    }

    void Update()
    {
        // Obtém o tempo atual
        float tempoAtual = Time.time;

        // Calcula as coordenadas circulares em torno de (xIni, yIni) usando seno e cosseno
        float x = xIni + raio * Mathf.Cos(tempoAtual * velocidadeRotacao);
        float y = yIni + raio * Mathf.Sin(tempoAtual * velocidadeRotacao);

        // Atualiza a posição do objeto
        objetoAlvo.transform.position = new Vector3(x, y, -0.54f);
    }
}
