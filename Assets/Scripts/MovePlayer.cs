using UnityEngine;
using UnityEngine.SceneManagement;

public static class CharacterControllerKeys
{
    public const string abduzirState = "Abduzir";
    public const string StateEmpty = "StateEmpty";
    public const string Chegada = "Chegada";
}

public class MovePlayer : MonoBehaviour
{
    public float velocidade = 5.0f;     //Velocidade de movimento
    public Transform trilha;            // O objeto vazio que contém os pontos da trilha
    public int posicaoAtual = 0;        // Posição inicial na trilha
    public GameObject controller;
    int currentRole, idPlayer;  


    private Animator animator;         //Componente de animação
    private bool isInStateEmpty = false, vezPlayerCurrent, teveAdd;
    private float timeEnteredStateEmpty = 0f;

    private ControllerRodadas controllerScript;
    private Transform[] pontosDaTrilha;

    void Awake() {
        animator = GetComponent<Animator>();
        teveAdd = false;
    }

    void Start()
    {
        string key = "PlayerPosition_" + idPlayer;
        if (PlayerPrefs.HasKey(key))
        {
            posicaoAtual = PlayerPrefs.GetInt(key);
            Debug.Log($"Posição do jogador {idPlayer} restaurada: {posicaoAtual}");
        }
        else
        {
            Debug.Log($"Nenhuma posição salva encontrada para o jogador {idPlayer}. Iniciando na posição padrão.");
        }
        
        // Coleta todos os pontos da trilha
        if(gameObject.name == "GFX1")
            idPlayer = 0;
        if(gameObject.name == "GFX2")
            idPlayer = 1;
        if(gameObject.name == "GFX3")
            idPlayer = 2;
        if(gameObject.name == "GFX4")
            idPlayer = 3;

        int numPontos = trilha.childCount;
        pontosDaTrilha = new Transform[numPontos];

        for (int i = 0; i < numPontos; i++)
        {
            pontosDaTrilha[i] = trilha.GetChild(i);
        }

        // Move o jogador diretamente para a posição salva
        if (posicaoAtual >= 0 && posicaoAtual < pontosDaTrilha.Length)
        {
            transform.position = pontosDaTrilha[posicaoAtual].position;
        }
        else
        {
            Debug.LogWarning($"Posição inválida ({posicaoAtual}) para o jogador {idPlayer}. Verifique os dados salvos.");
        }
        
        controllerScript = controller.GetComponent<ControllerRodadas>();
    }

    void OnDestroy()
    {
        string key = "PlayerPosition_" + idPlayer;          // Chave única para cada jogador
        PlayerPrefs.SetInt(key, (posicaoAtual + 1));              // Salva a posição atual
        PlayerPrefs.Save();                                 // Garante que os dados sejam escritos
        Debug.Log($"Posição do jogador {idPlayer} salva: {posicaoAtual}");       
    }

    void Update()
    {       
        Debug.Log("Id player do move: " + idPlayer);
        Debug.Log("Id player do Controller: " + controllerScript.idPlayerCurrent);

        currentRole = controllerScript.CurrentRole;
        
        // if(controllerScript.idPlayerCurrent + 1 == 1){
        //     if(controllerScript.playerNet1.getVezPlay() == true){
        //         vezPlayerCurrent = true;
        //     }
        // }

        // if(controllerScript.idPlayerCurrent + 1 == 2){
        //     if(controllerScript.playerNet2.getVezPlay() == true){
        //         vezPlayerCurrent = true;
        //     }
        // }

        // if(controllerScript.idPlayerCurrent + 1 == 3){
        //     if(controllerScript.playerNet3.getVezPlay() == true){
        //         vezPlayerCurrent = true;
        //     }
        // }

        // if(controllerScript.idPlayerCurrent + 1 == 4){
        //     if(controllerScript.playerNet4.getVezPlay() == true){
        //         vezPlayerCurrent = true;
        //     }
        // }

        if(idPlayer == controllerScript.idPlayerCurrent)
        {
            if (controllerScript.moveClient == true && controllerScript.travarEspaco == false)
            {   
                if(posicaoAtual < 4){
                    posicaoAtual = 3;
                }

                posicaoAtual = Mathf.Clamp(posicaoAtual + currentRole, 0, pontosDaTrilha.Length - 1);
            }
        
        Debug.Log("OO: " + posicaoAtual);

        controllerScript.moveClient = false;

        if (controllerScript.abduzir)
        {
            animator.SetBool(CharacterControllerKeys.abduzirState, true);
        }else{
            animator.SetBool(CharacterControllerKeys.Chegada, false);  
        }
        
        }
        // Move o jogador em direção ao ponto atual da trilha
        Vector3 destino = pontosDaTrilha[posicaoAtual].position;
        
        if(posicaoAtual >= 43){
            SceneManager.LoadScene("TelaInicial");
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName(CharacterControllerKeys.StateEmpty)){
            if (!isInStateEmpty)
            {
                isInStateEmpty = true;                                              // Entrou no estado "StateEmpty", marque o tempo atual
                timeEnteredStateEmpty = Time.time;
            } else if (Time.time - timeEnteredStateEmpty >= 2.2f)
            {
                controllerScript.playSound("AbdSoltar");
                animator.SetBool(CharacterControllerKeys.Chegada, true);            // Passaram-se 3 segundos, saia do estado "StateEmpty"
                animator.SetBool(CharacterControllerKeys.abduzirState, false);      // Passaram-se 3 segundos, saia do estado "StateEmpty"
                controllerScript.abduzir = false;                      
            }

            transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);

        }else isInStateEmpty = false;

        
    }
}