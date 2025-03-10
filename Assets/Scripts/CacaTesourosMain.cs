using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class CacaTesourosMain : MonoBehaviour
{
    private float tempoDecorrido = 0f;
    public float tempoTotal = 90f;
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textQuestoes1, textQuestoes2, textQuestoes3, textQuestoes4;
    public TextMeshProUGUI textPontos1, textPontos2, textPontos3, textPontos4;

    string[] questoes = new string[] { "0 - 2", " 4 + 3 - 1", "-2 + 2", "(5 x 5) - 20", "2 x 2 + 1", "2 x 2 - 8", "-3 x 2", "4 + 4 + 4 - 6", "-3 x 2 + 6", "7 + 3 - 10" };
    int[] respostas = new int[] { -2, 6, 0, 5, 5, -4, -6, 6, 0, 0 };

    public int indPlayer;
    private int pontuationPlayer1, pontuationPlayer2, pontuationPlayer3, pontuationPlayer4;

    // Referência aos jogadores
    public GameObject Player1, Player2, Player3, Player4;

    private string perguntaAleatoria1, perguntaAleatoria2, perguntaAleatoria3, perguntaAleatoria4;
    int respostaCor1, respostaCor2, respostaCor3, respostaCor4;

    // Movimentação dos jogadores - cada um com sua própria trilha
    public Transform[] housePositionsPlayer1;
    public Transform[] housePositionsPlayer2;
    public Transform[] housePositionsPlayer3;
    public Transform[] housePositionsPlayer4;

    private int currentHouseIndexPlayer1 = 0;
    private int currentHouseIndexPlayer2 = 0;
    private int currentHouseIndexPlayer3 = 0;
    private int currentHouseIndexPlayer4 = 0;

    public int IndicePlayerAdd1;
    public int IndicePlayerAdd2;
    public int IndicePlayerAdd3;
    public int IndicePlayerAdd4;

    public void ArmazenarPontos()
    {
        // Divide as pontuações por 3 e arredonda para inteiro
        pontuationPlayer1 = Mathf.RoundToInt(pontuationPlayer1 / 3f);
        pontuationPlayer2 = Mathf.RoundToInt(pontuationPlayer2 / 3f);
        pontuationPlayer3 = Mathf.RoundToInt(pontuationPlayer3 / 3f);
        pontuationPlayer4 = Mathf.RoundToInt(pontuationPlayer4 / 3f);

        // Recupera os pontos já armazenados nos PlayerPrefs
        int pontosAcumulados1 = PlayerPrefs.GetInt("Pontos_1", 0);
        int pontosAcumulados2 = PlayerPrefs.GetInt("Pontos_2", 0);
        int pontosAcumulados3 = PlayerPrefs.GetInt("Pontos_3", 0);
        int pontosAcumulados4 = PlayerPrefs.GetInt("Pontos_4", 0);

        // Adiciona os novos pontos aos acumulados
        pontosAcumulados1 += pontuationPlayer1;
        pontosAcumulados2 += pontuationPlayer2;
        pontosAcumulados3 += pontuationPlayer3;
        pontosAcumulados4 += pontuationPlayer4;

        // Salva os novos valores nos PlayerPrefs
        PlayerPrefs.SetInt("Pontos_1", pontosAcumulados1);
        PlayerPrefs.SetInt("Pontos_2", pontosAcumulados2);
        PlayerPrefs.SetInt("Pontos_3", pontosAcumulados3);
        PlayerPrefs.SetInt("Pontos_4", pontosAcumulados4);

        // Garante que as mudanças sejam persistidas
        PlayerPrefs.Save();

        Debug.Log($"Pontuações salvas: P1 = {pontosAcumulados1}, P2 = {pontosAcumulados2}, P3 = {pontosAcumulados3}, P4 = {pontosAcumulados4}");
    }


    void Start()
    {
        pontuationPlayer1 = 0;
        pontuationPlayer2 = 0;
        pontuationPlayer3 = 0;
        pontuationPlayer4 = 0;

        IndicePlayerAdd1 = -7;
        IndicePlayerAdd2 = -7;
        IndicePlayerAdd3 = -7;
        IndicePlayerAdd4 = -7;

        // Inicializa a posição dos jogadores
        Player1.transform.position = housePositionsPlayer1[currentHouseIndexPlayer1].position;
        Player2.transform.position = housePositionsPlayer2[currentHouseIndexPlayer2].position;
        Player3.transform.position = housePositionsPlayer3[currentHouseIndexPlayer3].position;
        Player4.transform.position = housePositionsPlayer4[currentHouseIndexPlayer4].position;
    }

    void AtualizarTextoTempo()
    {
        float tempoRestante = Mathf.Max(tempoTotal - tempoDecorrido, 0f);
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public IEnumerator AnimateTextTransition(TextMeshProUGUI pointText, string newValue, float scaleMid)
    {
        Vector3 originalScale = pointText.transform.localScale;
        Vector3 smallScale = originalScale * scaleMid;
        float duration = 0.8f;

        float time = 0;
        while (time < duration / 2)
        {
            pointText.transform.localScale = Vector3.Lerp(originalScale, smallScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        pointText.transform.localScale = smallScale;
        pointText.text = newValue;

        time = 0;
        while (time < duration / 2)
        {
            pointText.transform.localScale = Vector3.Lerp(smallScale, originalScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        pointText.transform.localScale = originalScale;
    }

    void Update()
    {
        tempoDecorrido += Time.deltaTime;
        AtualizarTextoTempo();

        if (tempoDecorrido >= tempoTotal)
        {
            ArmazenarPontos();
            SceneManager.LoadScene("Fase1");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ArmazenarPontos();
            SceneManager.LoadScene("Fase1");
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            MovimentarPlayer1(1);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            MovimentarPlayer1(0);  
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            VerificarRespostaPlayer(1, GetIndice());
        }

    }

    public int GetIndice(){
        return Random.Range(0, questoes.Length);
    }

    public void VerificarRespostaPlayer(int player, int indiceAleatorio)
    {
        if(player == 1){
            
            if (IndicePlayerAdd1 == respostaCor1)
            {
                pontuationPlayer1 += 1;
                StartCoroutine(AnimateTextTransition(textPontos1, pontuationPlayer1.ToString(), 0.6f));
            }
            
            perguntaAleatoria1 = questoes[indiceAleatorio];
            respostaCor1 = respostas[indiceAleatorio];

            StartCoroutine(AnimateTextTransition(textQuestoes1, perguntaAleatoria1, 0.6f));
            Debug.Log("resposta: " + respostaCor1);
        }
        if(player == 2){
            if (IndicePlayerAdd2 == respostaCor2)
            {
                pontuationPlayer2 += 1;
                StartCoroutine(AnimateTextTransition(textPontos2, pontuationPlayer2.ToString(), 0.6f));
            }            

            perguntaAleatoria2 = questoes[indiceAleatorio];
            respostaCor2 = respostas[indiceAleatorio];

            StartCoroutine(AnimateTextTransition(textQuestoes2, perguntaAleatoria2, 0.6f));
            Debug.Log("resposta: " + respostaCor2);
        }
        if(player == 3){
            if (IndicePlayerAdd3 == respostaCor3)
            {
                pontuationPlayer3 += 1;
                StartCoroutine(AnimateTextTransition(textPontos3, pontuationPlayer3.ToString(), 0.6f));
            }     

            perguntaAleatoria3 = questoes[indiceAleatorio];
            respostaCor3 = respostas[indiceAleatorio];

            StartCoroutine(AnimateTextTransition(textQuestoes3, perguntaAleatoria3, 0.6f));
            Debug.Log("resposta: " + respostaCor3);       
        }
        if(player == 4){
            if (IndicePlayerAdd4 == respostaCor4)
            {
                pontuationPlayer4 += 1;
                StartCoroutine(AnimateTextTransition(textPontos4, pontuationPlayer4.ToString(), 0.6f));
            }    

            perguntaAleatoria4 = questoes[indiceAleatorio];
            respostaCor4 = respostas[indiceAleatorio];

            StartCoroutine(AnimateTextTransition(textQuestoes4, perguntaAleatoria4, 0.6f));
            Debug.Log("resposta: " + respostaCor4);        
        }
    }

    public void MovimentarPlayer1(int direcao)
    {
        if (direcao == 1)
        {
            currentHouseIndexPlayer1 += 1;
            IndicePlayerAdd1 += 1;
            currentHouseIndexPlayer1 = Mathf.Clamp(currentHouseIndexPlayer1, 0, housePositionsPlayer1.Length - 1);

            Player1.transform.position = housePositionsPlayer1[currentHouseIndexPlayer1].position;
        }
        else if (direcao == 0)
        {
            currentHouseIndexPlayer1 -= 1;
            IndicePlayerAdd1 -= 1;
            currentHouseIndexPlayer1 = Mathf.Clamp(currentHouseIndexPlayer1, 0, housePositionsPlayer1.Length - 1);

            Player1.transform.position = housePositionsPlayer1[currentHouseIndexPlayer1].position;
        }
    }

    public void MovimentarPlayer2(int direcao)
    {
        if (direcao == 1)
        {
            currentHouseIndexPlayer2 += 1;
            IndicePlayerAdd2 += 1;
            currentHouseIndexPlayer2 = Mathf.Clamp(currentHouseIndexPlayer2, 0, housePositionsPlayer2.Length - 1);

            Player2.transform.position = housePositionsPlayer2[currentHouseIndexPlayer2].position;
        }
        else if (direcao == 0)
        {
            currentHouseIndexPlayer2 -= 1;
            IndicePlayerAdd2 -= 1;
            currentHouseIndexPlayer2 = Mathf.Clamp(currentHouseIndexPlayer2, 0, housePositionsPlayer2.Length - 1);

            Player2.transform.position = housePositionsPlayer2[currentHouseIndexPlayer2].position;
        }
    }

    public void MovimentarPlayer3(int direcao)
    {
        if (direcao == 1)
        {
            currentHouseIndexPlayer3 += 1;
            IndicePlayerAdd3 += 1;
            currentHouseIndexPlayer3 = Mathf.Clamp(currentHouseIndexPlayer3, 0, housePositionsPlayer3.Length - 1);

            Player3.transform.position = housePositionsPlayer3[currentHouseIndexPlayer3].position;
        }
        else if (direcao == 0)
        {
            currentHouseIndexPlayer3 -= 1;
            IndicePlayerAdd3 -= 1;
            currentHouseIndexPlayer3 = Mathf.Clamp(currentHouseIndexPlayer3, 0, housePositionsPlayer3.Length - 1);

            Player3.transform.position = housePositionsPlayer3[currentHouseIndexPlayer3].position;
        }
    }

    public void MovimentarPlayer4(int direcao)
    {
        if (direcao == 1)
        {
            currentHouseIndexPlayer4 += 1;
            IndicePlayerAdd4 += 1;
            currentHouseIndexPlayer4 = Mathf.Clamp(currentHouseIndexPlayer4, 0, housePositionsPlayer4.Length - 1);

            Player4.transform.position = housePositionsPlayer4[currentHouseIndexPlayer4].position;
        }
        else if (direcao == 0)
        {
            currentHouseIndexPlayer4 -= 1;
            IndicePlayerAdd4 -= 1;
            currentHouseIndexPlayer4 = Mathf.Clamp(currentHouseIndexPlayer4, 0, housePositionsPlayer4.Length - 1);

            Player4.transform.position = housePositionsPlayer4[currentHouseIndexPlayer4].position;
        }
    }
}
