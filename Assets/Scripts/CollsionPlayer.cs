using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class GameState
{
    public static int player1PositionIndex = -1; // -1 significa que a posição ainda não foi salva
}

public class CollsionPlayer : MonoBehaviour
{
    public Transform[] housePositions;
    public TMP_Text enunciadoTexto, especiaisTexto;
    public TMP_Text respostaA;
    public TMP_Text respostaB;
    public TMP_Text respostaC;
    public TMP_Text respostaD;

    private bool retirouDaPilha;
    private string[] miniGames;

    public GameObject janelaPerguntas;
    public GameObject janelaBomRuim;
    private Animator animator;         //Componente de animação

    private bool stateResposta;

    Stack<string> stackDePerguntas = new Stack<string>();

    private int respostaCorreta;

    public static string Read(string filename) {

        TextAsset theTextFile = Resources.Load<TextAsset>(filename);

        if(theTextFile != null)
            return theTextFile.text;

        return string.Empty;
    }

    private void stackPerguntas(string[] perguntas){
        foreach (string pergunta in perguntas){
            stackDePerguntas.Push(pergunta.Trim());
        }
    }

    private void DefinirStateResposta()
    {
        stateResposta = true;
    }

    private void EmbaralharPerguntas(string[] perguntas)
    {
        System.Random rng = new System.Random();
        int n = perguntas.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = perguntas[k];
            perguntas[k] = perguntas[n];
            perguntas[n] = value;
        }
    }

    void cleanController(ControllerRodadas controllerRodadas){
        controllerRodadas.casaAdd = 0;
    }

    int playerID(){
        if(gameObject.name == "GFX1"){
            return 1;
        }

        if(gameObject.name == "GFX2"){
            return 2;
        }

        if(gameObject.name == "GFX3"){
            return 3;
        }

        if(gameObject.name == "GFX4"){
            return 4;
        }        
        return 1;
    }

    void condPositivas(int tipo, ControllerRodadas controllerRodadas){
        switch(tipo){
            case 1: 
                adicionarPont(controllerRodadas, true);
            break;
            case 2:
                controllerRodadas.casaAdd = 2;
            break;
            case 3:
                controllerRodadas.dadoAdd[playerID() - 1] = 1;
            break;
            case 4:
                controllerRodadas.casaAdd = 2;
            break;
            case 5:
                controllerRodadas.casaAdd = 2;
            break;
        }
    }

    void condNegativas(int tipo, ControllerRodadas controllerRodadas){
        switch(tipo){
            case 1: 
                adicionarPont(controllerRodadas, false);
            break;
            case 2:
                controllerRodadas.casaAdd = 2;
            break;
            case 3:
                controllerRodadas.dadoAdd[playerID() - 1] = 1;
            break;
            case 4:
                controllerRodadas.casaAdd = 2;
            break;
            case 5:
                controllerRodadas.casaAdd = 2;
            break;
        }
    }

    void adicionarPont(ControllerRodadas controllerRodadas, bool positivo){
        controllerRodadas.playSound("Acerto");

        int pontosAtuais = 0;
        TMPro.TextMeshPro pontoTexto = null;

        if(gameObject.name == "GFX1" && positivo){
            pontosAtuais = int.Parse(controllerRodadas.ponto_1.text);
            pontosAtuais = positivo ? pontosAtuais + 1 : pontosAtuais - 1;
            pontoTexto = controllerRodadas.ponto_1;

            PlayerPrefs.SetInt("Pontos_1", pontosAtuais);

            controllerRodadas.ponto_1.text = somarPonto(int.Parse(controllerRodadas.ponto_1.text));
            StartCoroutine(controllerRodadas.AnimateTextTransition(pontoTexto, pontosAtuais, 0.5f));
        }

        if(gameObject.name == "GFX2"  && positivo){
            pontosAtuais = int.Parse(controllerRodadas.ponto_2.text);
            pontosAtuais = positivo ? pontosAtuais + 1 : pontosAtuais - 1;
            pontoTexto = controllerRodadas.ponto_2;

            controllerRodadas.ponto_2.text = somarPonto(int.Parse(controllerRodadas.ponto_2.text));
        }

        if(gameObject.name == "GFX3" && positivo){
            pontosAtuais = int.Parse(controllerRodadas.ponto_3.text);
            pontosAtuais = positivo ? pontosAtuais + 1 : pontosAtuais - 1;
            pontoTexto = controllerRodadas.ponto_3;

            controllerRodadas.ponto_3.text = somarPonto(int.Parse(controllerRodadas.ponto_3.text));
        }

        if(gameObject.name == "GFX4" && positivo){
            pontosAtuais = int.Parse(controllerRodadas.ponto_4.text);
            pontosAtuais = positivo ? pontosAtuais + 1 : pontosAtuais - 1;
            pontoTexto = controllerRodadas.ponto_4;

            controllerRodadas.ponto_4.text = somarPonto(int.Parse(controllerRodadas.ponto_4.text));
        }


        if (pontoTexto != null) {
            StartCoroutine(controllerRodadas.AnimateTextTransition(pontoTexto, pontosAtuais, 0.5f));
        }
    }

    void Awake() {
        animator = GetComponent<Animator>();
        miniGames = new string[3];
        miniGames[0] = "BatalhaDasEquacoes";    //BatalhaDasEquacoes
        miniGames[1] = "CacaAsCordenadas";      //CacaAsCordenadas
        miniGames[2] = "CacaTesouros";          //CacaTesouros
    }

    private void Start() {
        janelaPerguntas.SetActive(false);
        janelaBomRuim.SetActive(false);
        stateResposta = false;

        string arqLer = Read("Perguntas");
        
        retirouDaPilha = false;
        
        string[] bancoSeparado = arqLer.Split("\n");

        EmbaralharPerguntas(bancoSeparado);

        stackPerguntas(bancoSeparado);
    }

    void DesativarTelaDePerguntas()
    {
        janelaPerguntas.SetActive(false);
    }

    void DesativarTelaDeCondicao()
    {
        ControllerRodadas controllerRodadas = FindObjectOfType<ControllerRodadas>();
        cleanController(controllerRodadas);
        janelaBomRuim.SetActive(false);
    }

    string somarPonto(int temp){
        return (temp + 1).ToString();
    }

    void sceneLoader(){
        if (gameObject.name == "GFX1")
        {
            // Salvar a posição atual como índice na trilha
            for (int i = 0; i < housePositions.Length; i++)
            {
                if (transform.position == housePositions[i].position)
                {
                    GameState.player1PositionIndex = i;
                    break;
                }
            }
        }

        int rand = Random.Range(0, 3);
        SceneManager.LoadScene(miniGames[rand]);
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "CasaDePerguntas" && !(animator.GetCurrentAnimatorStateInfo(0).IsName("StateEmpty")) && stateResposta == false){
            
            janelaPerguntas.SetActive(true);
            stateResposta = false;
            ControllerRodadas controllerRodadas = FindObjectOfType<ControllerRodadas>();

            controllerRodadas.travarEspaco = true;

            if (stackDePerguntas.Count > 0 && retirouDaPilha == false) {
                string perguntaX = stackDePerguntas.Pop();


                retirouDaPilha = true;
                
                string[] questaoSeparada = perguntaX.Split(new string[] { "-E", "-R1", "-R2", "-R3", "-R4", "-RV"}, System.StringSplitOptions.RemoveEmptyEntries);

                enunciadoTexto.text = questaoSeparada[0].Trim();
                respostaA.text = questaoSeparada[1].Trim();
                respostaB.text = questaoSeparada[2].Trim();
                respostaC.text = questaoSeparada[3].Trim();
                respostaD.text = questaoSeparada[4].Trim();
                respostaCorreta = int.Parse(questaoSeparada[5].Trim());
            }
            


            // Debug.Log("a resposta é: " + respostaCorreta);
            // Debug.Log("O player: " + controllerRodadas.respostaDoPlayer);

            controllerRodadas.respondeuCorreto = false;

            if(controllerRodadas.perguntaRespondida == true)
            {
                stateResposta = true;
                if(respostaCorreta == controllerRodadas.respostaDoPlayer)
                {
                    enunciadoTexto.text = "Acertou!!!";
                    controllerRodadas.respostaDoPlayer = -1;
                    controllerRodadas.respondeuCorreto = true;
                    controllerRodadas.perguntaRespondida = false;
                }else
                {
                    controllerRodadas.playSound("Erro");
                    enunciadoTexto.text = "Errou!!!";
                    controllerRodadas.respostaDoPlayer = -1;
                    controllerRodadas.respondeuCorreto = false;
                    controllerRodadas.perguntaRespondida = false;
                }
                controllerRodadas.travarEspaco = false;
                Invoke("DesativarTelaDePerguntas", 1.0f);
            } 

            if(controllerRodadas.respondeuCorreto == true){
                adicionarPont(controllerRodadas, true);
            }

        }

        if(other.gameObject.tag == "CasaBoa" && !(animator.GetCurrentAnimatorStateInfo(0).IsName("StateEmpty")) && stateResposta == false){
            
            ControllerRodadas controllerRodadas = FindObjectOfType<ControllerRodadas>();
            
            string arqLer = Read("CasaBom");
            
            string[] bancoSeparado = arqLer.Split("\n");

            int bancoMax = bancoSeparado.Length;
            int indRand = Random.Range(0, bancoMax);

            string[] partes = bancoSeparado[indRand].Split("-C");
            
            string texto = partes[0].Trim();                // Parte do texto antes de "-C"
            int valor = int.Parse(partes[1].Trim());        // Valor após "-C"
            
            condPositivas(valor, controllerRodadas);

            especiaisTexto.text = "--Recompensa--\n" + texto;
            janelaPerguntas.SetActive(false);
            janelaBomRuim.SetActive(true);
            stateResposta = true;



            Invoke("DesativarTelaDeCondicao", 5.0f);
        }

        if(other.gameObject.tag == "CasaRuim" && !(animator.GetCurrentAnimatorStateInfo(0).IsName("StateEmpty")) && stateResposta == false){

            ControllerRodadas controllerRodadas = FindObjectOfType<ControllerRodadas>();

            string arqLer = Read("CasaRuim");
            
            string[] bancoSeparado = arqLer.Split("\n");

            int bancoMax = bancoSeparado.Length;
            int indRand = Random.Range(0, bancoMax);

            string[] partes = bancoSeparado[indRand].Split("-C");
            
            string texto = partes[0].Trim();                // Parte do texto antes de "-C"
            int valor = int.Parse(partes[1].Trim());        // Valor após "-C"
            
            condNegativas(valor, controllerRodadas);

            especiaisTexto.text = "--Penalidade--\n" + texto;
            janelaPerguntas.SetActive(false);
            janelaBomRuim.SetActive(true);
            stateResposta = true;
            Invoke("DesativarTelaDeCondicao", 5.0f);
        }

        if(other.gameObject.tag == "CasaDeDesafio" && !(animator.GetCurrentAnimatorStateInfo(0).IsName("StateEmpty"))){
            Invoke("sceneLoader", 2.5f);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "CasaDePerguntas"){
            janelaPerguntas.SetActive(false);
            janelaBomRuim.SetActive(false);
            stateResposta = false;
            retirouDaPilha = false;
        }
    }

    void Update(){

        if(Input.GetKeyDown(KeyCode.N))
            Invoke("sceneLoader", 2.5f);            //Retirar isso depois 
        
    }
}
	