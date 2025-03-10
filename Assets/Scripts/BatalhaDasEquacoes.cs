using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class BatalhaDasEquacoes : MonoBehaviour
{
    private float tempoDecorrido = 0f;
    public float tempoTotal = 90f;
    public TextMeshPro textTimer;
    public TextMeshPro Div1, Div1Fim, Div2, Div2Fim, Div3, Div3Fim, Div4, Div4Fim;

    public TextMeshPro boxPontos1, boxWrite1, boxPontos2, boxWrite2, boxPontos3, boxWrite3, boxPontos4, boxWrite4;

    int points1, points2, points3, points4;
    int indice1, indice2, indice3, indice4;

    public TextMeshPro textoPontuacao1, textoPontuacao2, textoPontuacao3, textoPontuacao4;    

    // Armazena a posição inicial de Div1
    private Vector3 posicaoInicialDiv1, posicaoInicialDiv2, posicaoInicialDiv3, posicaoInicialDiv4;
    private string inputString1 = "", inputString2 = "", inputString3 = "", inputString4 = ""; // Para armazenar a entrada do jogador

    string[] questoes = new string[] {"x + 5 = 8", "x + 9 = -1", "4 = x - 10", "3x = 15", "3x - 13 = 8", "2x = 14", "x - 4 = 3", 
                                      "x + 6 = 5", "x - 7 = -7", "x - 39 = -79", "10 = x + 8", "15 = x + 20", "7 = x + 8", 
                                      "x - 1 = 5", "2x + 4 = 16", "2x = 10", "3x = -9", "7x = 21"};

    int[] respostas = new int[] {3, -10, 14, 5, 7, 7, 7, -1, 0, -40, 2, -5, -1, 6, 6, 5, -3, 3};

    public bool LugarOcupado1, LugarOcupado2, LugarOcupado3, LugarOcupado4;
    public Coroutine movimentoCoroutine1, movimentoCoroutine2, movimentoCoroutine3, movimentoCoroutine4;

    public void ArmazenarPontos()
    {
        // Divide as pontuações por 3 e arredonda para inteiro
        points1 = Mathf.RoundToInt(points1 / 3f);
        points2 = Mathf.RoundToInt(points2 / 3f);
        points3 = Mathf.RoundToInt(points3 / 3f);
        points4 = Mathf.RoundToInt(points4 / 3f);

        // Recupera os pontos já armazenados nos PlayerPrefs
        int pontosAcumulados1 = PlayerPrefs.GetInt("Pontos_1", 0);
        int pontosAcumulados2 = PlayerPrefs.GetInt("Pontos_2", 0);
        int pontosAcumulados3 = PlayerPrefs.GetInt("Pontos_3", 0);
        int pontosAcumulados4 = PlayerPrefs.GetInt("Pontos_4", 0);

        // Adiciona os novos pontos aos acumulados
        pontosAcumulados1 += points1;
        pontosAcumulados2 += points2;
        pontosAcumulados3 += points3;
        pontosAcumulados4 += points4;

        // Salva os novos valores nos PlayerPrefs
        PlayerPrefs.SetInt("Pontos_1", pontosAcumulados1);
        PlayerPrefs.SetInt("Pontos_2", pontosAcumulados2);
        PlayerPrefs.SetInt("Pontos_3", pontosAcumulados3);
        PlayerPrefs.SetInt("Pontos_4", pontosAcumulados4);

        // Garante que as mudanças sejam persistidas
        PlayerPrefs.Save();

        Debug.Log($"Pontuações salvas: P1 = {pontosAcumulados1}, P2 = {pontosAcumulados2}, P3 = {pontosAcumulados3}, P4 = {pontosAcumulados4}");
    }

    void Awake()
    {
        LugarOcupado1 = false;
        LugarOcupado2 = false;
        LugarOcupado3 = false;
        LugarOcupado4 = false;

        // Armazena a posição inicial de Div1
        posicaoInicialDiv1 = Div1.transform.position;
        posicaoInicialDiv2 = Div2.transform.position;
        posicaoInicialDiv3 = Div3.transform.position;
        posicaoInicialDiv4 = Div4.transform.position;
    }

    public int getIndice(int player){
        int indice = Random.Range(0, questoes.Length);
        if(player == 1)
            indice1 = indice;
        if(player == 2)
            indice2 = indice;
        if(player == 3)
            indice3 = indice;
        if(player == 4)
            indice4 = indice;
        
        return indice;
    } 

    public void MudarDiv(TextMeshPro Div, int indice)
    {
        Div.text = questoes[indice];

        Debug.Log(questoes[indice]);
        Debug.Log(respostas[indice]);
    }

    void Start()
    {
        MudarDiv(Div1, getIndice(1));
        MudarDiv(Div2, getIndice(2));
        MudarDiv(Div3, getIndice(3));
        MudarDiv(Div4, getIndice(4));
        points1 = 0;
        points2 = 0;
        points3 = 0;
        points4 = 0;
    }

    public IEnumerator AnimateTextTransition(TMPro.TextMeshPro pointText, int newValue, float scaleMid) {
    
        Vector3 originalScale = pointText.transform.localScale;
        Vector3 smallScale = originalScale * scaleMid;
        float duration = 0.8f;

        // Shrink
        float time = 0;
        while (time < duration / 2) {
            pointText.transform.localScale = Vector3.Lerp(originalScale, smallScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }
        pointText.transform.localScale = smallScale;

        // Update the text
        pointText.text = newValue.ToString();

        // Grow back to original size
        time = 0;
        while (time < duration / 2) {
            pointText.transform.localScale = Vector3.Lerp(smallScale, originalScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        pointText.transform.localScale = originalScale; 
    }

    public void AtualizarTextoTempo()
    {
        float tempoRestante = Mathf.Max(tempoTotal - tempoDecorrido, 0f);
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void LerEntradaJogador(int player)
    {
        if(player == 1){
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) || (c == '-' && inputString1.Length == 0)) // Permite apenas dígitos e '-' como o primeiro caractere
                {
                    inputString1 += c;
                }
                else if (c == '\b' && inputString1.Length > 0) // Backspace para remover o último caractere
                {
                    inputString1 = inputString1.Substring(0, inputString1.Length - 1);
                }
            }

            boxWrite1.text = inputString1;
        }

        if(player == 2){
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) || (c == '-' && inputString2.Length == 0)) // Permite apenas dígitos e '-' como o primeiro caractere
                {
                    inputString2 += c;
                }
                else if (c == '\b' && inputString2.Length > 0) // Backspace para remover o último caractere
                {
                    inputString2 = inputString2.Substring(0, inputString2.Length - 1);
                }
            }

            boxWrite2.text = inputString2;
        }

        if(player == 3){
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) || (c == '-' && inputString3.Length == 0)) // Permite apenas dígitos e '-' como o primeiro caractere
                {
                    inputString3 += c;
                }
                else if (c == '\b' && inputString3.Length > 0) // Backspace para remover o último caractere
                {
                    inputString3 = inputString3.Substring(0, inputString3.Length - 1);
                }
            }

            boxWrite3.text = inputString3;
        }

        if(player == 4){
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c) || (c == '-' && inputString4.Length == 0)) // Permite apenas dígitos e '-' como o primeiro caractere
                {
                    inputString4 += c;
                }
                else if (c == '\b' && inputString4.Length > 0) // Backspace para remover o último caractere
                {
                    inputString4 = inputString4.Substring(0, inputString4.Length - 1);
                }
            }

            boxWrite4.text = inputString4;
        }
    }

    public void ProcessarResposta(int player)
    {
        int respostaInt;
        
        if (player == 1 && int.TryParse(inputString1, out respostaInt))
        {
            if (respostas[indice1] == respostaInt)
            {
                points1++;
                StartCoroutine(AnimateTextTransition(boxPontos1, points1, 0.5f));
            }
            inputString1 = "";
            ResetarDiv(1);
        }
        if (player == 2 && int.TryParse(inputString2, out respostaInt))
        {
            if (respostas[indice2] == respostaInt)
            {
                points2++;
                StartCoroutine(AnimateTextTransition(boxPontos2, points2, 0.5f));
            }
            inputString2 = "";
            ResetarDiv(2);
        }
        if (player == 3 && int.TryParse(inputString3, out respostaInt))
        {
            if (respostas[indice3] == respostaInt)
            {
                points3++;
                StartCoroutine(AnimateTextTransition(boxPontos3, points3, 0.5f));
            }
            inputString3 = "";
            ResetarDiv(3);
        }
        if (player == 4 && int.TryParse(inputString4, out respostaInt))
        {
            if (respostas[indice4] == respostaInt)
            {
                points4++;
                StartCoroutine(AnimateTextTransition(boxPontos4, points4, 0.5f));
            }
            inputString4 = "";
            ResetarDiv(4);
        }
    }

    public void ResetarDiv(int player)
    {
        switch (player)
        {
            case 1:
                if (movimentoCoroutine1 != null) StopCoroutine(movimentoCoroutine1);
                Div1.transform.position = posicaoInicialDiv1;
                MudarDiv(Div1, getIndice(1));
                LugarOcupado1 = false;
                break;
            case 2:
                if (movimentoCoroutine2 != null) StopCoroutine(movimentoCoroutine2);
                Div2.transform.position = posicaoInicialDiv2;
                MudarDiv(Div2, getIndice(2));
                LugarOcupado2 = false;
                break;
            case 3:
                if (movimentoCoroutine3 != null) StopCoroutine(movimentoCoroutine3);
                Div3.transform.position = posicaoInicialDiv3;
                MudarDiv(Div3, getIndice(3));
                LugarOcupado3 = false;
                break;
            case 4:
                if (movimentoCoroutine4 != null) StopCoroutine(movimentoCoroutine4);
                Div4.transform.position = posicaoInicialDiv4;
                MudarDiv(Div4, getIndice(4));
                LugarOcupado4 = false;
                break;
        }
    }

    public bool GetLugarOcp(int player){
        bool LugarOcupado = false;

        switch(player){
            case 1:
                LugarOcupado = LugarOcupado1;
            break;
            case 2:
                LugarOcupado = LugarOcupado2;
            break;
            case 3:
                LugarOcupado = LugarOcupado3;
            break;
            case 4:
                LugarOcupado = LugarOcupado4;
            break;
        }

        return LugarOcupado;
    }

    public void MovementarDivs(int player){
        
        switch(player){
            case 1:
                movimentoCoroutine1 = StartCoroutine(MoveDiv(Div1, Div1Fim, 1));
            break;
            case 2:
                movimentoCoroutine2 = StartCoroutine(MoveDiv(Div2, Div2Fim, 2));
            break;
            case 3:
                movimentoCoroutine3 = StartCoroutine(MoveDiv(Div3, Div3Fim, 3));
            break;
            case 4:
                movimentoCoroutine4 = StartCoroutine(MoveDiv(Div4, Div4Fim, 4));
            break;
        }
        return;   
    }

    public void Update()
    {
        tempoDecorrido += Time.deltaTime;
        AtualizarTextoTempo();

        if (tempoDecorrido >= tempoTotal)
        {
            ArmazenarPontos();
            SceneManager.LoadScene("Fase1");
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            ArmazenarPontos();
            SceneManager.LoadScene("Fase1");
        }

        if(!(LugarOcupado1) && movimentoCoroutine1 == null){
            MovementarDivs(1);
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ProcessarResposta(1);
        }else{
            LerEntradaJogador(1);
        }

    }

    // Rotina para mover Div1 até Div1Fim em 3 segundos
    IEnumerator MoveDiv(TextMeshPro Div, TextMeshPro DivFim, int player)
    {
        float duration = 3f;
        Vector3 startPosition = Div.transform.position;
        Vector3 endPosition = DivFim.transform.position;

        float time = 0f;
        while (time < duration)
        {
            Div.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Garante que a posição final seja exatamente a posição de destino
        Div.transform.position = endPosition;

        // Define LugarOcupado como verdadeiro quando Div1 alcançar Div1Fim

        switch(player)
        {
            case 1:
                LugarOcupado1 = true;
                movimentoCoroutine1 = null; 
            break;
            case 2:
                LugarOcupado2 = true; 
                movimentoCoroutine2 = null;
            break;
            case 3:
                LugarOcupado3 = true;
                movimentoCoroutine3 = null; 
            break;
            case 4:
                LugarOcupado4 = true;
                movimentoCoroutine4 = null; 
            break;
        }

    }
}
