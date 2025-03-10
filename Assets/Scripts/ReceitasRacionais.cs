using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class ReceitasRacionais : MonoBehaviour
{

    private float tempoDecorrido = 0f;
    public float tempoTotal = 90f;
    public TextMeshPro textTimer;

    private bool secondPart;

    public GameObject spw1, spw2, spw3, spw4, spw5;
    public Sprite[] sprites;

    public TextMeshProUGUI Numerador, Denominador, p1Div, p2Div, p3Div, p4Div;

    public int[] numeradores;
    public int[] denominadores;
    public int[] spwOpcional;

    private int pontoP1, pontoP2, pontoP3, pontoP4;

    private float resultado;

    private float resposta;

    public void Awake(){
        pontoP1 = 0;
        pontoP1 = 0;
        pontoP1 = 0;
        pontoP1 = 0;
        secondPart = false;
    }

    public void Update()
    {
        tempoDecorrido += Time.deltaTime;
        AtualizarTextoTempo();

        if(tempoDecorrido >= 5f)
        {
            secondPart = true;
        }

        if (tempoDecorrido >= tempoTotal)
        {
            SceneManager.LoadScene("Fase2");
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("Fase2");
        }

    }

    public void AtualizarTextoTempo()
    {
        float tempoRestante = Mathf.Max(tempoTotal - tempoDecorrido, 0f);
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public float valorEscala(int valor)
    {
        float valorFracionado;

        valorFracionado = (valor + 1)/10f;

        return valorFracionado;
    }

    public void mudarReceita(){
        
        int randomIndex = Random.Range(0, denominadores.Length);

        resultado = (float)numeradores[randomIndex] / (float)denominadores[randomIndex];

        Debug.Log("Resultado: " + resultado);

        Numerador.text = numeradores[randomIndex].ToString();
        Denominador.text = denominadores[randomIndex].ToString();

        if(secondPart){
            SpriteRenderer spriteRenderer1 = spw1.GetComponent<SpriteRenderer>();
            spriteRenderer1.sprite = sprites[spwOpcional[randomIndex]];

            resposta = resposta + valorEscala(spwOpcional[randomIndex]);

        }

    }

    private void mudarPlacar(){
        p1Div.text = pontoP1.ToString();
        p2Div.text = pontoP2.ToString();
        p3Div.text = pontoP3.ToString();
        p4Div.text = pontoP4.ToString();
    }

    private GameObject spwVazio(){
        SpriteRenderer spriteRenderer1 = spw1.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer2 = spw2.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer3 = spw3.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer4 = spw4.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer5 = spw5.GetComponent<SpriteRenderer>();

        if(spriteRenderer1.sprite == null)
            return spw1;
        else if(spriteRenderer2.sprite == null)
            return spw2;
        else if(spriteRenderer3.sprite == null)
            return spw3;
        else if(spriteRenderer4.sprite == null)
            return spw4;
        else if(spriteRenderer5.sprite == null)
            return spw5;
        else
            return null;

        return null;
    } 

    public void verificarResposta(){
        SpriteRenderer spriteRenderer1 = spw1.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer2 = spw2.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer3 = spw3.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer4 = spw4.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer5 = spw5.GetComponent<SpriteRenderer>();

        spriteRenderer1.sprite = null;
        spriteRenderer2.sprite = null;
        spriteRenderer3.sprite = null;
        spriteRenderer4.sprite = null;
        spriteRenderer5.sprite = null;

        if(resposta == resultado){
            pontoP1++;
        }

        resposta = 0f;
        mudarPlacar();
        mudarReceita();
    }

    public void spawnEscalas(int scaleN){

        GameObject spw = spwVazio();
    
        if(spw != null)
            switch(scaleN){
                case 1:
                    resposta = resposta + 0.1f;
                    SpriteRenderer sprite1 = spw.GetComponent<SpriteRenderer>();
                    sprite1.sprite = sprites[0];
                break;
                case 2:
                    resposta = resposta + 0.2f;
                    SpriteRenderer sprite2 = spw.GetComponent<SpriteRenderer>();
                    sprite2.sprite = sprites[1];
                break;
                case 3:
                    resposta = resposta + 0.3f;
                    SpriteRenderer sprite3 = spw.GetComponent<SpriteRenderer>();
                    sprite3.sprite = sprites[2];
                break;
                case 4:
                    resposta = resposta + 0.4f;
                    SpriteRenderer sprite4 = spw.GetComponent<SpriteRenderer>();
                    sprite4.sprite = sprites[3];
                break;
                case 5:
                    resposta = resposta + 0.5f;
                    SpriteRenderer sprite5 = spw.GetComponent<SpriteRenderer>();
                    sprite5.sprite = sprites[4];
                break;
                default:
                break;
            }
        }

    public void Start(){
        mudarReceita();
    }
}
