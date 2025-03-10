using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class playerNetObj{
    private int id;
    private bool vezPlay;
        
    public playerNetObj(){
        id = 0;
        vezPlay = false;
    }
    public void idChange(int id){
        this.id = id;
        return;
    }
    public int idGet(){
        return id;
    }
    public void setVezPlay(bool vezPlay){
        this.vezPlay = vezPlay;
        return;
    }
    public bool getVezPlay(){
        return vezPlay;
    }
}

public class ControllerRodadas : MonoBehaviour
{
    public int[] currentPlayerIndex;
    public int idPlayerCurrent = 0;
    public int CurrentRole;
    public TMPro.TextMeshPro dado, ponto_1, ponto_2, ponto_3, ponto_4;
    private int PontosP1, PontosP2, PontosP3, PontosP4;

    public int casaAdd;                               //Variaveis referentes a casa boa
    public int[] dadoAdd;

    public GameObject audioController; 

    // public NetworkVariable<string> dadoTexto;
    public bool abduzir = false;

    public bool perguntaRespondida, moveClient;
    public bool travarEspaco, espacoApertado, respondeuCorreto;
    public int respostaDoPlayer;

    public int ClientId;

    public playerNetObj playerNet1;
    public playerNetObj playerNet2;
    public playerNetObj playerNet3;
    public playerNetObj playerNet4; 

    public string sceneName;

    private string caminhoArquivo;

    void Awake(){
        // dadoTexto= new NetworkVariable<string>("");
        dadoAdd = new int[4];
        playerNet1 = new playerNetObj();
        playerNet2 = new playerNetObj();
        playerNet3 = new playerNetObj();
        playerNet4 = new playerNetObj();
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

    void Start()
    {

        caminhoArquivo = Path.Combine(Application.persistentDataPath, "Pontos.txt");
        AtualizarPontos();

        moveClient = false;
        ClientId = -1;
        currentPlayerIndex = new int[4];    
        currentPlayerIndex[0] = 3;
        currentPlayerIndex[1] = 2;
        currentPlayerIndex[2] = 1;
        currentPlayerIndex[3] = 0;
        perguntaRespondida = false;
        travarEspaco = false;
        espacoApertado = false;
    }

    private void AtualizarPontos()
    {
        PontosP1 = PlayerPrefs.GetInt("Pontos_1", 0); // Obtém o valor armazenado ou 0 se não existir
        PontosP2 = PlayerPrefs.GetInt("Pontos_2", 0);
        PontosP3 = PlayerPrefs.GetInt("Pontos_3", 0);
        PontosP4 = PlayerPrefs.GetInt("Pontos_4", 0);

        StartCoroutine(AnimateTextTransition(ponto_1, PontosP1, 0.5f));
        StartCoroutine(AnimateTextTransition(ponto_2, PontosP2, 0.5f));
        StartCoroutine(AnimateTextTransition(ponto_3, PontosP3, 0.5f));
        StartCoroutine(AnimateTextTransition(ponto_4, PontosP4, 0.5f));
    }

    public void SalvarPontos(int pontos1, int pontos2, int pontos3, int pontos4)
    {
        PlayerPrefs.SetInt("Pontos_1", pontos1); // Salva os pontos no PlayerPrefs
        PlayerPrefs.SetInt("Pontos_2", pontos2);
        PlayerPrefs.SetInt("Pontos_3", pontos3);
        PlayerPrefs.SetInt("Pontos_4", pontos4);

        PlayerPrefs.Save(); // Garante que os dados sejam salvos
    }

    public void playSound(string name){
        soundController audioCtrl = audioController.GetComponent<soundController>();
        if (audioCtrl != null) {
            audioCtrl.playBotao(name);
        } else {
            Debug.LogError("soundController component not found on audioController GameObject.");
        }
    }

    public void spacePressionado(int role){

        moveClient = true;

        CurrentRole = role + dadoAdd[idPlayerCurrent]; // Dado de 1, 2, 3, 4

        if(dadoAdd[idPlayerCurrent] == 1)
            dadoAdd[idPlayerCurrent] = 0;

        StartCoroutine(AnimateTextTransition(dado, CurrentRole, 0.5f));
        abduzir = true;
        
        playSound("AbdPegar");

        // Verificar se idPlayerCurrent está dentro dos limites do array
        if (idPlayerCurrent >= 0 && idPlayerCurrent < currentPlayerIndex.Length)
        {
            currentPlayerIndex[idPlayerCurrent] += CurrentRole;
        }
        else
        { 
            Debug.LogError("Índice de jogador fora dos limites.");
        }

        idPlayerCurrent = 0;

    }

    TMPro.TextMeshPro GetPlayerPointText(int playerId) {
        switch(playerId) {
            case 0: return ponto_1;
            case 1: return ponto_2;
            case 2: return ponto_3;
            case 3: return ponto_4;
            default: return null;
        }
    }

    public void trocarDeTurno()
    {
        int role = Random.Range(1, 5);
        if (idPlayerCurrent == ClientId)
        {
            spacePressionado(role);
        }
    }

    public void RespostasVerificar(int respPlayer, bool pergResp)
    {
        respostaDoPlayer = respPlayer;
        perguntaRespondida = pergResp;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)){
            // espacoApertado = true;
            ClientId = 0;

            Debug.Log("aaaaaaaaaaaaaaaaaaaa");
            SalvarPontos(int.Parse(ponto_1.text), int.Parse(ponto_2.text), int.Parse(ponto_3.text), int.Parse(ponto_4.text));
            AtualizarPontos();
            trocarDeTurno();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            // RespostasServerRpc(1, true);
            RespostasVerificar(1, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            // RespostasServerRpc(2, true);
            RespostasVerificar(2, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            // RespostasServerRpc(3, true);
            RespostasVerificar(3, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            // RespostasServerRpc(4, true);
            RespostasVerificar(4, true);
        }
        return;

        if(Input.GetKeyDown(KeyCode.B)){
            SceneManager.LoadScene("ReceitaRacionais");
        }

        sceneName = SceneManager.GetActiveScene().name;

    }

    void OnDestroy()
    {
        sceneName = "minigame";
    }
    
}
