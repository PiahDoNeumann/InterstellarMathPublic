using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class CacaAsCordenadas : MonoBehaviour
{
    private float tempoDecorrido = 0f;
    public float tempoTotal = 90f;
    public TextMeshPro textTimer, cordenadas1, cordenadas2, cordenadas3, cordenadas4;

    // Referência para o SpriteRenderer e o array de sprites
    public SpriteRenderer cursosP1, cursosP2, cursosP3, cursosP4, div1, div2, div3, div4;
    public Sprite[] sprites;

    private int indice;
    public float moveAmount; // Quantidade de movimento em unidades do mundo
    public int xMax = 0, yMax = 0;

    public int[] CordenadasP1 = {0, 0}, CordenadasP2 = {0, -8}, CordenadasP3 = {0, 0}, CordenadasP4 = {0, 0};
    public int[] CordenaP1Ans = {-1, -2}, CordenaP2Ans = {-1, 2}, CordenaP3Ans = {1, -2}, CordenaP4Ans = {1, 2};
    private int pontosP1 = 0, pontosP2 = 0, pontosP3 = 0, pontosP4 = 0;

    public string[] cordenadasGeral = new string[]
    {
        "-4, -4", "-4, -3", "-4, -2", "-4, -1", "-4, 0", "-4, 1", "-4, 2", "-4, 3", "-4, 4",
        "-3, -4", "-3, -3", "-3, -2", "-3, -1", "-3, 0", "-3, 1", "-3, 2", "-3, 3", "-3, 4",
        "-2, -4", "-2, -3", "-2, -2", "-2, -1", "-2, 0", "-2, 1", "-2, 2", "-2, 3", "-2, 4",
        "-1, -4", "-1, -3", "-1, -2", "-1, -1", "-1, 0", "-1, 1", "-1, 2", "-1, 3", "-1, 4",
        "0, -4", "0, -3", "0, -2", "0, -1", "0, 0", "0, 1", "0, 2", "0, 3", "0, 4",
        "1, -4", "1, -3", "1, -2", "1, -1", "1, 0", "1, 1", "1, 2", "1, 3", "1, 4",
        "2, -4", "2, -3", "2, -2", "2, -1", "2, 0", "2, 1", "2, 2", "2, 3", "2, 4",
        "3, -4", "3, -3", "3, -2", "3, -1", "3, 0", "3, 1", "3, 2", "3, 3", "3, 4",
        "4, -4", "4, -3", "4, -2", "4, -1", "4, 0", "4, 1", "4, 2", "4, 3", "4, 4",
    };

    public IEnumerator AnimateTextTransition(TMPro.TextMeshPro pointText, string newValue, float scaleMid) {
    
        Vector3 originalScale = pointText.transform.localScale;
        Vector3 smallScale = originalScale * scaleMid;
        float duration = 0.7f;

        // Shrink
        float time = 0;
        while (time < duration / 2) {
            pointText.transform.localScale = Vector3.Lerp(originalScale, smallScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }
        pointText.transform.localScale = smallScale;

        // Update the text
        pointText.text = newValue;

        // Grow back to original size
        time = 0;
        while (time < duration / 2) {
            pointText.transform.localScale = Vector3.Lerp(smallScale, originalScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        pointText.transform.localScale = originalScale; 
    }

    public IEnumerator AnimateSpriteTransition(SpriteRenderer spriteRenderer, Sprite newSprite, float scaleMid) {
    
        Vector3 originalScale = spriteRenderer.transform.localScale;
        Vector3 smallScale = originalScale * scaleMid;
        float duration = 0.5f;

        // Shrink
        float time = 0;
        while (time < duration / 2) {
            spriteRenderer.transform.localScale = Vector3.Lerp(originalScale, smallScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.transform.localScale = smallScale;

        // Update the sprite
        spriteRenderer.sprite = newSprite;

        // Grow back to original size
        time = 0;
        while (time < duration / 2) {
            spriteRenderer.transform.localScale = Vector3.Lerp(smallScale, originalScale, time / (duration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.transform.localScale = originalScale; 
    }

    void AtualizarTextoTempo()
    {
        float tempoRestante = Mathf.Max(tempoTotal - tempoDecorrido, 0f);
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private void SalvarPontos()
    {
        // Divide as pontuações por 3 e arredonda para inteiro
        pontosP1 = Mathf.RoundToInt(pontosP1 / 3f);
        pontosP2 = Mathf.RoundToInt(pontosP2 / 3f);
        pontosP3 = Mathf.RoundToInt(pontosP3 / 3f);
        pontosP4 = Mathf.RoundToInt(pontosP4 / 3f);

        // Recupera os pontos já armazenados nos PlayerPrefs
        int pontosAcumulados1 = PlayerPrefs.GetInt("Pontos_1", 0);
        int pontosAcumulados2 = PlayerPrefs.GetInt("Pontos_2", 0);
        int pontosAcumulados3 = PlayerPrefs.GetInt("Pontos_3", 0);
        int pontosAcumulados4 = PlayerPrefs.GetInt("Pontos_4", 0);

        // Adiciona os novos pontos aos acumulados
        pontosAcumulados1 += pontosP1;
        pontosAcumulados2 += pontosP2;
        pontosAcumulados3 += pontosP3;
        pontosAcumulados4 += pontosP4;

        // Salva os novos valores nos PlayerPrefs
        PlayerPrefs.SetInt("Pontos_1", pontosAcumulados1);
        PlayerPrefs.SetInt("Pontos_2", pontosAcumulados2);
        PlayerPrefs.SetInt("Pontos_3", pontosAcumulados3);
        PlayerPrefs.SetInt("Pontos_4", pontosAcumulados4);

        // Garante que as mudanças sejam persistidas
        PlayerPrefs.Save();

        Debug.Log($"Pontuações salvas: P1 = {pontosAcumulados1}, P2 = {pontosAcumulados2}, P3 = {pontosAcumulados3}, P4 = {pontosAcumulados4}");
    }

    private void UpdatePositionPlayer(int direcao){
        switch(direcao){
            case 1:
                CordenadasP1[0]--;
                MoverSprite(-1, 0, 1);
            break;
            case 2:
                CordenadasP1[0]++;
                MoverSprite(1, 0, 1);
            break;
            case 3:
                CordenadasP1[1]++;
                MoverSprite(0, 1, 1);
            break;
            case 4:
                CordenadasP1[1]--;
                MoverSprite(0, -1, 1);
            break;
        }
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            indice = Random.Range(0, cordenadasGeral.Length);
            AlternarSprite(1, Random.Range(0, sprites.Length), indice);
        }

        // Mover Cursor1 para a esquerda ou direita 
        if (Input.GetKeyDown(KeyCode.A) && CordenadasP1[0] > 0)
        {
            UpdatePositionPlayer(1);
        }
        else if (Input.GetKeyDown(KeyCode.D) && CordenadasP1[0] < 8)
        {
            UpdatePositionPlayer(2);
        }

        // Mover Cursor1 para cima ou para baixo
        if (Input.GetKeyDown(KeyCode.W) && CordenadasP1[1] < 0)
        {
            UpdatePositionPlayer(3);
        }
        else if (Input.GetKeyDown(KeyCode.S) && CordenadasP1[1] > -8)
        {
            UpdatePositionPlayer(4);
        }

        tempoDecorrido += Time.deltaTime;
        AtualizarTextoTempo();

        if (tempoDecorrido >= tempoTotal)
        {
            SalvarPontos();
            SceneManager.LoadScene("Fase1");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SalvarPontos();
            SceneManager.LoadScene("Fase1");
        }

        Debug.Log("Aqui " + pontosP1 + pontosP2 + pontosP3 + pontosP4);

        //Ficava aqui as funções de movimento do cursor #lembrete pro futuro


    }

    private void AtualizarCordenadas(string coordenada, int[] cordenadasAns)
    {
        string[] coordenadasSplit = coordenada.Split(',');
        cordenadasAns[0] = int.Parse(coordenadasSplit[0].Trim());
        cordenadasAns[1] = int.Parse(coordenadasSplit[1].Trim());
    }

    private bool VerificarAcerto(int[] cordenadasAns, int[] cordenadasPlayer)
    {
        // Verifica se as coordenadas alvo são iguais às coordenadas do jogador
        bool acertou = cordenadasAns[0] == (cordenadasPlayer[0] - 4) && cordenadasAns[1] == ((cordenadasPlayer[1] + 4)* -1);

        // Debug.Log(acertou);

        return acertou;
    }

    public void AlternarSprite(int player, int index, int indexCordenadas)
    {
        if (sprites.Length == 0)
            return;

        Sprite newSprite = sprites[index];

        switch(player)
        {
            case 1:
                if(VerificarAcerto(CordenaP1Ans, CordenadasP1))
                    pontosP1++;

                StartCoroutine(AnimateSpriteTransition(cursosP1, newSprite, 0.8f));
                StartCoroutine(AnimateSpriteTransition(div1, newSprite, 0.8f));

                StartCoroutine(AnimateTextTransition(cordenadas1, "(" + cordenadasGeral[indexCordenadas] + ")", 0.6f));

                // Atualizar as coordenadas para o Player 1
                AtualizarCordenadas(cordenadasGeral[indexCordenadas], CordenaP1Ans);

                // Certificar que div está visível e na mesma camada de renderização
                div1.sortingLayerID = cursosP1.sortingLayerID;
                div1.sortingOrder = cursosP1.sortingOrder;
            break;
            case 2:
                if(VerificarAcerto(CordenaP2Ans, CordenadasP2))
                    Debug.Log("P2 recebeu pontos");
                    pontosP2++;

                StartCoroutine(AnimateSpriteTransition(cursosP2, newSprite, 0.8f));
                StartCoroutine(AnimateSpriteTransition(div2, newSprite, 0.8f));

                StartCoroutine(AnimateTextTransition(cordenadas2, "(" + cordenadasGeral[indexCordenadas] + ")", 0.6f));

                // Certificar que div está visível e na mesma camada de renderização
                div2.sortingLayerID = cursosP2.sortingLayerID;
                div2.sortingOrder = cursosP2.sortingOrder;
            break;
            case 3:
                if(VerificarAcerto(CordenaP3Ans, CordenadasP3))
                    pontosP3++;

                StartCoroutine(AnimateSpriteTransition(cursosP3, newSprite, 0.8f));
                StartCoroutine(AnimateSpriteTransition(div3, newSprite, 0.8f));

                StartCoroutine(AnimateTextTransition(cordenadas3, "(" + cordenadasGeral[indexCordenadas] + ")", 0.6f));

                // Certificar que div está visível e na mesma camada de renderização
                div3.sortingLayerID = cursosP3.sortingLayerID;
                div3.sortingOrder = cursosP3.sortingOrder;
            break;
            case 4:
                if(VerificarAcerto(CordenaP4Ans, CordenadasP4))
                    pontosP4++;

                StartCoroutine(AnimateSpriteTransition(cursosP4, newSprite, 0.8f));
                StartCoroutine(AnimateSpriteTransition(div4, newSprite, 0.8f));

                StartCoroutine(AnimateTextTransition(cordenadas4, "(" + cordenadasGeral[indexCordenadas] + ")", 0.6f));

                // Certificar que div está visível e na mesma camada de renderização
                div4.sortingLayerID = cursosP4.sortingLayerID;
                div4.sortingOrder = cursosP4.sortingOrder;
            break;
        }
    }

    public void MoverSprite(int xDirection, int yDirection, int player)
    {
        Vector3 newPosition = cursosP1.transform.position;

        if(player == 1)
            newPosition = cursosP1.transform.position;
        if(player == 2)
            newPosition = cursosP2.transform.position;
        if(player == 3)
            newPosition = cursosP3.transform.position;
        if(player == 4)
            newPosition = cursosP4.transform.position;
            
        newPosition.x += moveAmount * xDirection;
        newPosition.y += moveAmount * yDirection;
        

        if(player == 1)
            cursosP1.transform.position = newPosition;
        if(player == 2)
            cursosP2.transform.position = newPosition;
        if(player == 3)
            cursosP3.transform.position = newPosition;
        if(player == 4)
            cursosP4.transform.position = newPosition;

    }
}