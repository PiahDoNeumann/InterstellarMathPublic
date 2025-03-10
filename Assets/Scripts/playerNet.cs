using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FishNet.Object;
using FishNet.Connection;
using FishNet;

public class playerNet : NetworkBehaviour{

    private ControllerRodadas controllerScript;
    private CacaAsCordenadas CacaAsCordenadasScript;
    private CacaTesourosMain CacaTesourosScript;
    private BatalhaDasEquacoes BatalhaDasEquacoesScript;
    private ReceitasRacionais ReceitasRacionaisScript;

    private int playerIdNet, indice;

    private string currentSceneName;

    private static int nextPlayerId = 1;
    private bool atribuido, atribuidoCaca;

    private void AssignPlayerId()
    {
        // Atribui o ID do jogador no servidor
        playerIdNet = nextPlayerId++;
        // Envia o ID de volta ao cliente correspondente
        TargetAssignPlayerId(Owner, playerIdNet);
    }

    [TargetRpc]
    private void TargetAssignPlayerId(NetworkConnection conn, int assignedPlayerId)
    {
        // Sincroniza o ID no cliente
        playerIdNet = assignedPlayerId;
        Debug.Log("ID do jogador atribuído: " + playerIdNet);
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;      
        //O Diretorio .SceneManagement não funciona decente, tem que especificar assim!
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        currentSceneName = scene.name;

        if(scene.name == "CacaAsCordenadas")
        {
            GameObject controllerObject = GameObject.Find("CacaAsCordenadas");

            if(controllerObject != null)
            {
                CacaAsCordenadasScript = controllerObject.GetComponent<CacaAsCordenadas>();   
            }        
            else
            {
                Debug.LogError("GameObject com o script ControllerRodadas não encontrado!");
            }
        }
 
        else if(scene.name == "CacaTesouros")
        {
            GameObject controllerObject = GameObject.Find("CacaTesourosMain");

            if(controllerObject != null)
            {
                CacaTesourosScript = controllerObject.GetComponent<CacaTesourosMain>();
            }        
            else
            {
                Debug.LogError("GameObject com o script ControllerRodadas não encontrado!");
            }
        }

        else if(scene.name == "BatalhaDasEquacoes")
        {
            GameObject controllerObject = GameObject.Find("BatalhaDasEquacoes");

            if(controllerObject != null)
            {
                BatalhaDasEquacoesScript = controllerObject.GetComponent<BatalhaDasEquacoes>();
            }        
            else
            {
                Debug.LogError("GameObject com o script ControllerRodadas não encontrado!");
            }
        }

        else if(scene.name == "ReceitaRacionais")
        {
            GameObject controllerObject = GameObject.Find("ReceitasRacionais");

            if(controllerObject != null)
            {
                ReceitasRacionaisScript = controllerObject.GetComponent<ReceitasRacionais>();
            }        
            else
            {
                Debug.LogError("GameObject com o script ReceitasRacionais não encontrado!");
            }
        }
    }

    void Start()
    {
        // if(SceneManager.GetActiveScene().name != "CacaTesouros"){
        atribuido = false;
        atribuidoCaca = false;

        if (IsServer)
        {
            AssignPlayerId();
        }

        // Ajuste o nome do GameObject contendo o script ControllerRodadas
        GameObject controllerObject = GameObject.Find("ControllerRodadas");

        // Verifica se encontrou o GameObject com o script ControllerRodadas
        if (controllerObject != null)
        {
            // Obtém o componente ControllerRodadas do GameObject encontrado
            controllerScript = controllerObject.GetComponent<ControllerRodadas>();
        }
        else
        {
            Debug.LogError("GameObject com o script ControllerRodadas não encontrado!");
        }
    }

    void updateTabuleiro()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            controllerScript.espacoApertado = true;

            TestServerRpc();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            RespostasServerRpc(1, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            RespostasServerRpc(2, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            RespostasServerRpc(3, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            RespostasServerRpc(4, true);
        }
        return;
    }

    void updateCacaCordenadas() //Salsicho do bem KKKKKKKKKKK
    {
        Debug.Log(playerIdNet);
        if(!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.Return))
        {
            indice = Random.Range(0, CacaAsCordenadasScript.cordenadasGeral.Length);
            UpdateCordenadasServer(indice, playerIdNet, Random.Range(0, CacaAsCordenadasScript.sprites.Length));
        }

        if(playerIdNet == 1){
            // Mover Cursor1 para a esquerda ou direita 
            if (Input.GetKeyDown(KeyCode.A) && CacaAsCordenadasScript.CordenadasP1[0] > 0)
            {
                UpdateP1Server(1);
            }
            else if (Input.GetKeyDown(KeyCode.D) && CacaAsCordenadasScript.CordenadasP1[0] < 8)
            {
                UpdateP1Server(2);
            }

            // Mover Cursor1 para cima ou para baixo
            if (Input.GetKeyDown(KeyCode.W) && CacaAsCordenadasScript.CordenadasP1[1] < 0)
            {
                UpdateP1Server(3);
            }
            else if (Input.GetKeyDown(KeyCode.S) && CacaAsCordenadasScript.CordenadasP1[1] > -8)
            {
                UpdateP1Server(4);
            }
        }

        if(playerIdNet == 2){
            // Mover Cursor2 para a esquerda ou direita
            if (Input.GetKeyDown(KeyCode.A) && CacaAsCordenadasScript.CordenadasP2[0] > 0)
            {
                UpdateP2Server(1);
            }
            else if (Input.GetKeyDown(KeyCode.D) && CacaAsCordenadasScript.CordenadasP2[0] < 8)
            {
                UpdateP2Server(2);
            }

            // Mover Cursor2 para cima ou para baixo
            if (Input.GetKeyDown(KeyCode.W) && CacaAsCordenadasScript.CordenadasP2[1] < 0)
            {
                UpdateP2Server(3);
            }
            else if (Input.GetKeyDown(KeyCode.S) && CacaAsCordenadasScript.CordenadasP2[1] > -8)
            {
                UpdateP2Server(4);
            }
        }        

        if(playerIdNet == 3){
            // Mover Cursor3 para a esquerda ou direita
            if (Input.GetKeyDown(KeyCode.A) && CacaAsCordenadasScript.CordenadasP3[0] > 0)
            {
                UpdateP3Server(1);
            }
            else if (Input.GetKeyDown(KeyCode.D) && CacaAsCordenadasScript.CordenadasP3[0] < 8)
            {
                UpdateP3Server(2);
            }

            // Mover Cursor3 para cima ou para baixo
            if (Input.GetKeyDown(KeyCode.W) && CacaAsCordenadasScript.CordenadasP3[1] < 0)
            {
                UpdateP3Server(3);
            }
            else if (Input.GetKeyDown(KeyCode.S) && CacaAsCordenadasScript.CordenadasP3[1] > -8)
            {
                UpdateP3Server(4);
            }
        }

        if(playerIdNet == 4){
            // Mover Cursor4 para a esquerda ou direita
            if (Input.GetKeyDown(KeyCode.A) && CacaAsCordenadasScript.CordenadasP4[0] > 0)
            {
                UpdateP4Server(1);
            }
            else if (Input.GetKeyDown(KeyCode.D) && CacaAsCordenadasScript.CordenadasP4[0] < 8)
            {
                UpdateP4Server(2);
            }

            // Mover Cursor4 para cima ou para baixo
            if (Input.GetKeyDown(KeyCode.W) && CacaAsCordenadasScript.CordenadasP4[1] < 0)
            {
                UpdateP4Server(3);
            }
            else if (Input.GetKeyDown(KeyCode.S) && CacaAsCordenadasScript.CordenadasP4[1] > -8)
            {
                UpdateP4Server(4);
            }
        }

        return;
    }

    void updateCacaTesouros(){
        Debug.Log(playerIdNet);
        if(!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.D))
        {
            movimentarCacaTesourosSrv(1, playerIdNet);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            movimentarCacaTesourosSrv(0, playerIdNet);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mudarTextosSrv(playerIdNet, CacaTesourosScript.GetIndice());    
        }


        return;
    }

    [ServerRpc]
    private void mudarTextosSrv(int player, int indice){
        mudarTextosObs(player, indice);
    }

    [ObserversRpc]
    private void mudarTextosObs(int player, int indice){
        CacaTesourosScript.VerificarRespostaPlayer(player, indice);
    }

    [ServerRpc]
    private void movimentarCacaTesourosSrv(int direcao, int player){
        movimentarCacaTesourosObs(direcao, player);
    }

    [ObserversRpc]
    private void movimentarCacaTesourosObs(int direcao, int player){
        if(player == 1){
            if(direcao == 1)
                CacaTesourosScript.MovimentarPlayer1(1);
            else
                CacaTesourosScript.MovimentarPlayer1(0);   
        }
        if(player == 2){
            if(direcao == 1)
                CacaTesourosScript.MovimentarPlayer2(1);
            else
                CacaTesourosScript.MovimentarPlayer2(0);   
        }
        if(player == 3){
            if(direcao == 1)
                CacaTesourosScript.MovimentarPlayer3(1);
            else
                CacaTesourosScript.MovimentarPlayer3(0);   
        }
        if(player == 4){
            if(direcao == 1)
                CacaTesourosScript.MovimentarPlayer4(1);
            else
                CacaTesourosScript.MovimentarPlayer4(0);   
        }
    }

    [ServerRpc]
    private void UpdateP1Server(int direcao){
        UpdateP1Observer(direcao);
    }

    [ServerRpc]
    private void UpdateP2Server(int direcao){
        UpdateP2Observer(direcao);
    }

    [ServerRpc]
    private void UpdateP3Server(int direcao){
        UpdateP3Observer(direcao);

    }

    [ServerRpc]
    private void UpdateP4Server(int direcao){
        UpdateP4Observer(direcao);
    }

    [ServerRpc]
    private void UpdateCordenadasServer(int indiceX, int player, int spriteIndex){
        UpdateCordenadasObserver(indiceX, player, spriteIndex);
    }

    [ObserversRpc]
    private void UpdateCordenadasObserver(int indiceX, int player, int spriteIndex){
        CacaAsCordenadasScript.AlternarSprite(player, spriteIndex, indiceX);
    }

    [ObserversRpc]
    private void UpdateP1Observer(int direcao){
        switch(direcao){
            case 1:
                CacaAsCordenadasScript.CordenadasP1[0]--;
                CacaAsCordenadasScript.MoverSprite(-1, 0, 1);
            break;
            case 2:
                CacaAsCordenadasScript.CordenadasP1[0]++;
                CacaAsCordenadasScript.MoverSprite(1, 0, 1);
            break;
            case 3:
                CacaAsCordenadasScript.CordenadasP1[1]++;
                CacaAsCordenadasScript.MoverSprite(0, 1, 1);
            break;
            case 4:
                CacaAsCordenadasScript.CordenadasP1[1]--;
                CacaAsCordenadasScript.MoverSprite(0, -1, 1);
            break;
        }
    }

    [ObserversRpc]
    private void UpdateP2Observer(int direcao){
        switch(direcao){
            case 1:
                CacaAsCordenadasScript.CordenadasP2[0]--;
                CacaAsCordenadasScript.MoverSprite(-1, 0, 2);
            break;
            case 2:
                CacaAsCordenadasScript.CordenadasP2[0]++;
                CacaAsCordenadasScript.MoverSprite(1, 0, 2);
            break;
            case 3:
                CacaAsCordenadasScript.CordenadasP2[1]++;
                CacaAsCordenadasScript.MoverSprite(0, 1, 2);
            break;
            case 4:
                CacaAsCordenadasScript.CordenadasP2[1]--;
                CacaAsCordenadasScript.MoverSprite(0, -1, 2);
            break;
        }
    }

    [ObserversRpc]
    private void UpdateP3Observer(int direcao){
        switch(direcao){
            case 1:
                CacaAsCordenadasScript.CordenadasP3[0]--;
                CacaAsCordenadasScript.MoverSprite(-1, 0, 3);
            break;
            case 2:
                CacaAsCordenadasScript.CordenadasP3[0]++;
                CacaAsCordenadasScript.MoverSprite(1, 0, 3);
            break;
            case 3:
                CacaAsCordenadasScript.CordenadasP3[1]++;
                CacaAsCordenadasScript.MoverSprite(0, 1, 3);
            break;
            case 4:
                CacaAsCordenadasScript.CordenadasP3[1]--;
                CacaAsCordenadasScript.MoverSprite(0, -1, 3);
            break;
        }
    }

    [ObserversRpc]
    private void UpdateP4Observer(int direcao){
        switch(direcao){
            case 1:
                CacaAsCordenadasScript.CordenadasP4[0]--;
                CacaAsCordenadasScript.MoverSprite(-1, 0, 4);
            break;
            case 2:
                CacaAsCordenadasScript.CordenadasP4[0]++;
                CacaAsCordenadasScript.MoverSprite(1, 0, 4);
            break;
            case 3:
                CacaAsCordenadasScript.CordenadasP4[1]++;
                CacaAsCordenadasScript.MoverSprite(0, 1, 4);
            break;
            case 4:
                CacaAsCordenadasScript.CordenadasP4[1]--;
                CacaAsCordenadasScript.MoverSprite(0, -1, 4);
            break;
        }
    }


    void updateBatalhaEquacoes()
    {

        if(!IsOwner)
            return;

        if(playerIdNet == 1){
            if(!(BatalhaDasEquacoesScript.LugarOcupado1) && BatalhaDasEquacoesScript.movimentoCoroutine1 == null){
                MovDivsSrv(1);
            }

            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ProcessarRespostaSrv(1);
            }else{
                BatalhaDasEquacoesScript.LerEntradaJogador(1);
            }
        }

        if(playerIdNet == 2){
            if(!(BatalhaDasEquacoesScript.LugarOcupado2) && BatalhaDasEquacoesScript.movimentoCoroutine2 == null){
                MovDivsSrv(2);
            }

            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ProcessarRespostaSrv(2);
            }else{
                BatalhaDasEquacoesScript.LerEntradaJogador(2);
            }
        }

        if(playerIdNet == 3){
            if(!(BatalhaDasEquacoesScript.LugarOcupado3) && BatalhaDasEquacoesScript.movimentoCoroutine3 == null){
                MovDivsSrv(3);
            }

            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ProcessarRespostaSrv(3);
            }else{
                BatalhaDasEquacoesScript.LerEntradaJogador(3);
            }
        }

        if(playerIdNet == 4){
            if(!(BatalhaDasEquacoesScript.LugarOcupado4) && BatalhaDasEquacoesScript.movimentoCoroutine4 == null){
                MovDivsSrv(4);
            }

            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                ProcessarRespostaSrv(4);
            }else{
                BatalhaDasEquacoesScript.LerEntradaJogador(4);
            }        
        }

        return;
    }

    [ServerRpc]
    private void MovDivsSrv(int player){
        movDivObs(player);
    }

    [ObserversRpc]
    private void movDivObs(int player){
        BatalhaDasEquacoesScript.MovementarDivs(player);
    }

    [ServerRpc]
    private void ProcessarRespostaSrv(int player){
        ProcessarRespostaObs(player);
    }

    [ObserversRpc]
    private void ProcessarRespostaObs(int player){
        BatalhaDasEquacoesScript.ProcessarResposta(player);
    }

    [ServerRpc]
    private void LerEntradaJogadorSrv(int player){
        LerEntradaJogadorObs(player);
    }

    [ObserversRpc]
    private void LerEntradaJogadorObs(int player){
        BatalhaDasEquacoesScript.LerEntradaJogador(player);
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;

        switch(currentSceneName){
            case "CacaAsCordenadas":
                updateCacaCordenadas();
            break;
            case "CacaTesouros":
                updateCacaTesouros();
            break;
            case "BatalhaDasEquacoes":
                updateBatalhaEquacoes();
            break;
            case "ReceitaRacionais":
                Debug.Log("Receitas racionais minigame");
            break;
            case "Fase2":
                Debug.Log("Fase2");
            break;
            default:
                updateTabuleiro();
            break;
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if(Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if(Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if(Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }


    [ServerRpc]
    private void TestServerRpc()
    {
        int serverRole = Random.Range(1, 5);
        TestObserversRpc(serverRole);
    }

    [ServerRpc]
    private void RespostasServerRpc(int respPlayer, bool pergResp)
    {
        RespostasObserversRpc(respPlayer, pergResp);
    }

    [ObserversRpc]
    private void RespostasObserversRpc(int respPlayer, bool pergResp)
    {
        controllerScript.respostaDoPlayer = respPlayer;
        controllerScript.perguntaRespondida = pergResp;
    }

    [ObserversRpc]
    private void TestObserversRpc(int role)
    {
        controllerScript.ClientId = (int)Owner.ClientId;
        if (controllerScript.idPlayerCurrent == controllerScript.ClientId)
        {
            controllerScript.spacePressionado(role);
        }
    }
}
