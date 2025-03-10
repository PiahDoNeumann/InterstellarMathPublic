using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class CorridaEquacoes : MonoBehaviour
{
    private float tempoDecorrido = 0f;
    public float tempoTotal = 90f;
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textQuestoes1;

    public TMP_InputField inputRespostaPlayer1;

    // Array de questões e respostas predefinidas
    string[] questoes = new string[] {
        "2x + 3 = 7",        // Resposta: 2
        "5x - 2 = 13",       // Resposta: 3
        "-3x + 4 = 1",       // Resposta: 1
        "4x - 8 = 0",        // Resposta: 2
        "6x + 5 = 11",       // Resposta: 1
        "3x + 4 = 10",       // Resposta: 2
        "7x - 3 = 18",       // Resposta: 3
        "-5x + 2 = -8",      // Resposta: 2
        "4x + 7 = 19",       // Resposta: 3
        "8x - 5 = 19",       // Resposta: 3
        "9x + 4 = 22",       // Resposta: 2
        "2x - 7 = 5",        // Resposta: 6
        "-4x + 3 = -5",      // Resposta: 2
        "5x + 9 = 24",       // Resposta: 3
        "6x - 8 = 10",       // Resposta: 3
        "-7x + 5 = -9",      // Resposta: 2
        "3x + 2 = 11",       // Resposta: 3
        "4x - 5 = 7",        // Resposta: 3
        "6x + 4 = 22",       // Resposta: 3
        "5x - 6 = 14",       // Resposta: 4
        "7x + 3 = 24",       // Resposta: 3
        "-3x + 6 = 3",       // Resposta: 1
        "8x - 9 = 15",       // Resposta: 3
        "10x + 5 = 35",      // Resposta: 3
        "9x - 6 = 12"        // Resposta: 2
    };

    // Array de respostas correspondentes
    int[] respostas = new int[] {
        2, 3, 1, 2, 1, 2, 3, 2, 3, 3, 2, 6, 2, 3, 3, 2, 3, 3, 3, 4, 3, 1, 3, 3, 2
    };

    string[] questoesEspeciais = new string[] { 
        "3x - 7 = 2x + 5",  // Resposta: 12
        "4x + 3 = 2x - 5",  // Resposta: -4
    };
    int[] respostasEspeciais = new int[] { 12, -4};

    private float tempoQuestaoEspecial = 15f;

    private int indiceQuestaoAtual = 0;
    private int resultadoCorreto;

    private bool isEspecial;

    // Referência ao jogador
    public GameObject Player1;

    public GameObject BalaoEspecial, divInicial, divFinal;

    // Movimentação do jogador
    public Transform[] housePositionsPlayer1;

    private int currentHouseIndexPlayer1 = 0;
    public int IndicePlayerAdd1;

    private float velocidadeAnimacao = 40.0f, escalaMinima = 491.632f, escalaMaxima = 560f;

    void Start()
    {
        isEspecial = false;
        IndicePlayerAdd1 = -7;

        // Inicializa a posição dos jogadores
        Player1.transform.position = housePositionsPlayer1[currentHouseIndexPlayer1].position;

        StartCoroutine(MoverBalao(BalaoEspecial.transform, divFinal.transform.position));

        // Exibe a primeira questão
        ExibirQuestao();
    }

    IEnumerator MoverBalao(Transform objeto, Vector3 destino)
    {
        Vector3 posicaoInicial = objeto.position;
        float tempoDecorrido = 0f, duracaoMovimento = 1.0f;

        while (tempoDecorrido < duracaoMovimento)
        {
            // Interpola a posição com base no tempo
            objeto.position = Vector3.Lerp(posicaoInicial, destino, tempoDecorrido / duracaoMovimento);

            tempoDecorrido += Time.deltaTime;
            yield return null; // Espera até o próximo frame
        }

        // Garante que a posição final seja exatamente a esperada
        objeto.position = destino;
    }

    void AtualizarTextoTempo()
    {
        float tempoRestante = Mathf.Max(tempoTotal - tempoDecorrido, 0f);
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void Update()
    {
        tempoDecorrido += Time.deltaTime;
        AtualizarTextoTempo();
        Debug.Log(resultadoCorreto);

        float escala = Mathf.PingPong(Time.time * velocidadeAnimacao, escalaMaxima - escalaMinima) + escalaMinima;
        BalaoEspecial.transform.localScale = new Vector3(escala, escala, 1f);

        if (tempoDecorrido >= tempoTotal)
        {
            // ArmazenarPontos();
            SceneManager.LoadScene("Fase2");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ArmazenarPontos();
            SceneManager.LoadScene("Fase2");
        }
        
        // Gatilho para verificar a resposta ao pressionar Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            VerificarResposta();
        }
    }

    void ExibirQuestao()
    {
        // Verifica se uma questão especial deve ser exibida (20% de chance)
        bool questaoEspecial = Random.value > 0.8f;

        // Exibe a próxima questão
        if (questaoEspecial)
        {
            isEspecial = true;

            // Escolhe uma questão especial aleatória
            int indiceEspecial = Random.Range(0, questoesEspeciais.Length);
            textQuestoes1.text = "Questão Especial: " + questoesEspeciais[indiceEspecial];
            resultadoCorreto = respostasEspeciais[indiceEspecial];

            StartCoroutine(MoverBalao(BalaoEspecial.transform, divInicial.transform.position));

        }
        else
        {
            // Se atingiu o fim das questões, volta ao início (opcional)
            if (indiceQuestaoAtual >= questoes.Length)
            {
                indiceQuestaoAtual = 0; // Reinicia o ciclo de questões (opcional)
            }

            textQuestoes1.text = questoes[indiceQuestaoAtual];
            resultadoCorreto = respostas[indiceQuestaoAtual];
        }
    }

    void VerificarResposta()
    {
        int respostaJogador;

        // Verifica se a entrada do jogador é um número válido
        if (int.TryParse(inputRespostaPlayer1.text, out respostaJogador))
        {
            Debug.Log("Resposta do jogador: " + respostaJogador);
            Debug.Log("Resposta correta: " + resultadoCorreto);

            if (respostaJogador == resultadoCorreto)
            {
                if(isEspecial){
                    //Aqui caso especial
                    MovimentarPlayer(1);            // Move o jogador para frente se a resposta estiver correta
                    inputRespostaPlayer1.text = ""; // Limpa o campo de resposta
                    indiceQuestaoAtual++;           // Avança para a próxima questão
                    ExibirQuestao();
                    StartCoroutine(MoverBalao(BalaoEspecial.transform, divFinal.transform.position));

                }else
                {
                    MovimentarPlayer(1);            // Move o jogador para frente se a resposta estiver correta
                    inputRespostaPlayer1.text = ""; // Limpa o campo de resposta
                    indiceQuestaoAtual++;           // Avança para a próxima questão
                    ExibirQuestao();                // Exibe a próxima questão

                }
            }
            else
            {
                if(!isEspecial){
                    MovimentarPlayer(-1);               // Move o jogador para trás se a resposta estiver errada
                    inputRespostaPlayer1.text = "";     // Limpa o campo de resposta
                }else{
                    MovimentarPlayer(-1);               // Move o jogador para trás se a resposta estiver errada
                    inputRespostaPlayer1.text = "";     // Limpa o campo de resposta
                    indiceQuestaoAtual++;
                    isEspecial = false;
                    ExibirQuestao();  
                    StartCoroutine(MoverBalao(BalaoEspecial.transform, divFinal.transform.position));
       
                }
            }
        }
        else
        {
            Debug.Log("Entrada inválida. Digite um número!");
        }
    }

    public void MovimentarPlayer(int direcao)
    {
        if (direcao == 1)
        {
            currentHouseIndexPlayer1 += 1; // Movimenta para frente
        }
        else if (direcao == -1)
        {
            currentHouseIndexPlayer1 -= 1; // Movimenta para trás
        }

        IndicePlayerAdd1 += direcao;
        currentHouseIndexPlayer1 = Mathf.Clamp(currentHouseIndexPlayer1, 0, housePositionsPlayer1.Length - 1);

        Player1.transform.position = housePositionsPlayer1[currentHouseIndexPlayer1].position;

        // Verifica se o jogador alcançou o final
        if (currentHouseIndexPlayer1 == housePositionsPlayer1.Length - 1)
        {
            SceneManager.LoadScene("Fase2");
        }
    }
}
