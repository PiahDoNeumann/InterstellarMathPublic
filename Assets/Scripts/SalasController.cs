using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importar o SceneManagement
using TMPro;

public class SalasController : MonoBehaviour
{

    public TMP_InputField CriarSala, EntrarSala;
    string[] salas = {"sala um", "sala dois", "exemplo", "123"};

    public void criarSala()
    {
        // Obtém o nome da sala do campo de entrada
        string novaSala = CriarSala.text;

        if (!string.IsNullOrEmpty(novaSala))
        {
            // Cria um novo array com um espaço adicional
            string[] novasSalas = new string[salas.Length + 1];

            // Copia os elementos existentes para o novo array
            for (int i = 0; i < salas.Length; i++)
            {
                novasSalas[i] = salas[i];
            }

            // Adiciona a nova sala ao final do array
            novasSalas[salas.Length] = novaSala;

            // Substitui o array antigo pelo novo
            salas = novasSalas;

            // Limpa o campo de entrada
            CriarSala.text = "";

            SceneManager.LoadScene("TelaFases");
        }
    }

    public void entrarSala()
    {
        string nomeSala = EntrarSala.text;

        if (!string.IsNullOrEmpty(nomeSala))
        {
            // Verifica se a sala existe no array
            bool salaEncontrada = false;
            for (int i = 0; i < salas.Length; i++)
            {
                if (salas[i] == nomeSala)
                {
                    salaEncontrada = true;
                    break;
                }
            }

            if (salaEncontrada)
            {
                // Troca para a cena correspondente (substitua "NomeDaCena" pelo nome real da sua cena)
                SceneManager.LoadScene("TelaFases");
            }
            else
            {
                Debug.Log("Sala não encontrada: " + nomeSala);
            }
        }
    }
}
