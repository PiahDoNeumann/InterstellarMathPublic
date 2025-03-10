using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class limparResorces : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LimparPontuacoes();
    }

    // Função para limpar o arquivo de pontuações
    private void LimparPontuacoes()
    {
        // Caminho do arquivo
        string caminhoArquivo = Path.Combine(Application.persistentDataPath, "Pontos.txt");

        // String com as pontuações zeradas
        string resultado = "0 0 0 0";

        // Escrever a string no arquivo
        File.WriteAllText(caminhoArquivo, resultado);

    }
}
