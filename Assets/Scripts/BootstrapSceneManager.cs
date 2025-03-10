using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishNet;
using FishNet.Managing.Scened;

public class BootstrapSceneManager : MonoBehaviour
{
    private void Awake()
    {
        // DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(!InstanceFinder.IsServer)    //para não rodar nos clientes
            return;
        
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            LoadScene("CacaAsCordenadas");
        }

    }

    void LoadScene(string nameScene)
    {
        if(!InstanceFinder.IsServer)    //para não rodar nos clientes
            return;

        SceneLoadData sld = new SceneLoadData(nameScene);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

}
