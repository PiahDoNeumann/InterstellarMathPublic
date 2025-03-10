using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{

    [SerializeField] string videoFileName;

    void Start()
    {
        PlayVideo();
    }

    void Update()
    {
        // Verifica se a tecla 'Esc' foi pressionada para pular o vídeo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadNextScene("TelaSalas");
        }
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if(videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;

            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play();
        }
    }

    // Método chamado quando o vídeo termina
    private void OnVideoEnd(VideoPlayer vp)
    {
        LoadNextScene("TelaSalas");
    }

    // Carrega a próxima cena
    private void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
