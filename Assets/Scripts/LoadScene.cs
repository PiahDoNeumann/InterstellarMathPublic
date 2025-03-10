using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.IO;

[System.Serializable]
public class User
{
    public int id;
    public string username;
    public string password;
}

public static class JsonHelper
{
    public static T[] FromJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

// [System.Serializable]
// public class UserRaiz
// {
//     public User[] usuariosCadastrado;
// }

public class LoadScene : MonoBehaviour
{
    public int level;
    public Animator transition;

    public float transitionTime = 1f;

    private bool playerAutorizado;

    public bool stat;

    private Vector3 originalPosition;   // Para armazenar a posição original do loginHud
    public float moveSpeed = 500f;      // Velocidade de movimento, você pode ajustar conforme necessário

    public TMP_Text avisoTexto;
    public GameObject buttonOpcoes, buttonOpcoesVoltar, buttonJogar, buttonCreditos, telaOpcoes, telaCreditos, loginHud;

    public TMP_InputField UsuarioField, SenhaField;

    private string usuarioStr, senhaStr;

    private StreamReader leitor;
    [SerializeField]private string json;
    [SerializeField] private User[] usuarios;

    public void Awake(){
        playerAutorizado = true;
    }

    private void Start()
    {
        // Armazena a posição original do loginHud
        if (loginHud != null)
        {
            originalPosition = loginHud.transform.localPosition;
        }
    }

    public void LoadScenes(string cenaName)
    {
        // StartCoroutine(ShowLoginWarning("Acesso permitido"));
        StartCoroutine(LoadScenesCoroutine(cenaName));
    }

    private void avisoLogin(){
        // StartCoroutine(ShowLoginWarning("Você precisa fazer login para jogar!"));
    }

    IEnumerator LoginRoutine()
    {
        usuarioStr = UsuarioField.text;
        senhaStr = SenhaField.text;

        string url = "http://localhost:3000/users";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // Debug.LogError("Erro de conexão ou protocolo: " + request.error);
            playerAutorizado = true;
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log("Resposta do servidor: " + response);

            // Usar o JsonHelper para desserializar o array de usuários
            usuarios = JsonHelper.FromJsonArray<User>(response);

            // Verifica se o usuário existe e a senha está correta
            playerAutorizado = true; // Inicializa como falso
            foreach (User user in usuarios)
            {
                if (user.username == usuarioStr && user.password == senhaStr)
                {
                    Debug.Log("Login bem-sucedido: " + user.username);
                    playerAutorizado = true; // Login autorizado
                    break; // Sai do loop se o login for bem-sucedido
                }
            }

            if (!playerAutorizado)
            {
                Debug.Log("Nome de usuário ou senha inválidos");
            }
        }
    }

    // IEnumerator ShowLoginWarning(string texto)
    // {
    //     EventSystem.current.SetSelectedGameObject(null);
    //     avisoTexto.text = texto;
        
    //     // Efeito de piscar
    //     for (int i = 0; i < 6; i++)                     // Pisca 6 vezes (3 on e 3 off)
    //     {
    //         avisoTexto.enabled = !avisoTexto.enabled;   // Alterna entre visível e invisível
    //         yield return new WaitForSeconds(0.3f);      // Tempo de cada "piscada" (0.3 segundos)
    //     }

    //     avisoTexto.enabled = true;                      // Garante que o texto fique visível no final
    //     avisoTexto.text = "LOGIN";                      // Volta ao texto original
    // }

    public void jogarBotton(string cenaName)
    {
        // Inicia o login e aguarda o resultado antes de verificar
        StartCoroutine(VerificaLoginCoroutine(cenaName));
    }

    IEnumerator VerificaLoginCoroutine(string cenaName)
    {
        // Inicia o processo de login e aguarda o término
        yield return StartCoroutine(LoginRoutine());

        // Verifica se o player foi autorizado após o login
        if (playerAutorizado)
        {
            // Se o login foi bem-sucedido, carrega a cena
            StartCoroutine(LoadScenesCoroutine(cenaName));
        }
        else
        {
            // Caso contrário, mostra o aviso de login
            avisoLogin();
        }
    }

    public void moveLogin(bool active)
    {
        StopAllCoroutines();    // Para evitar conflitos ao iniciar uma nova movimentação
        StartCoroutine(MoveLoginCoroutine(active));
    }

    private IEnumerator MoveLoginCoroutine(bool active)
    {
        Vector3 startPosition = loginHud.transform.localPosition;
        Vector3 targetPosition = active ? originalPosition : originalPosition + new Vector3(700f, 0, 0); // Ajuste a distância conforme necessário
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float journeyTime = 0f;
        float duration = 1.5f;  // Duração do movimento; ajuste conforme necessário

        while (journeyTime < duration)
        {
            journeyTime += Time.deltaTime;

            // Calculo quadrático para ajustar a velocidade
            float t = journeyTime / duration;
            float quadraticT = t * t; // Aplicando uma função quadrática

            // Interpolação da posição
            loginHud.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, quadraticT);

            yield return null;
        }

        // Garante que ele termine exatamente na posição desejada
        loginHud.transform.localPosition = targetPosition;
    }

    public void showOption(){
        buttonOpcoesVoltar.SetActive(true);
        telaOpcoes.SetActive(true);
        buttonOpcoes.SetActive(false);
        buttonJogar.SetActive(false);
        buttonCreditos.SetActive(false);
    }

    public void showVoltar(){
        buttonOpcoesVoltar.SetActive(false);
        telaCreditos.SetActive(false);
        telaOpcoes.SetActive(false);
        buttonOpcoes.SetActive(true);
        buttonJogar.SetActive(true);
        buttonCreditos.SetActive(true);
        moveLogin(true);
    }

    public void showCreditos(){
        buttonOpcoesVoltar.SetActive(true);
        telaCreditos.SetActive(true);
        buttonOpcoes.SetActive(false);
        buttonJogar.SetActive(false);
        buttonCreditos.SetActive(false);
        moveLogin(false);
    }

    IEnumerator LoadScenesCoroutine(string cenaName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(cenaName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
