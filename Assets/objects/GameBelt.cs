using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.InputSystem;
public class GameBelt : MonoBehaviour
{

    [SerializeField] public GameObject prefab;

    [SerializeField] public GameObject belt;

    [SerializeField] public Vector2 spawn;
    [SerializeField] public Vector2 scale = new Vector2(0.5f, 0.5f);

    private GameObject[] gameInstances = new GameObject[3];
    private int current = 0;
  

    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
    }

    public void spawnPrefab()
    {

        for (int i = 0; i < 1; i++)
        {
            GameObject obj = Instantiate(prefab, spawn, Quaternion.identity);
            RectTransform rt = (RectTransform)obj.transform;
            rt.SetParent(belt.transform, true);
            rt.localScale = scale;
            gameInstances[i] = obj;
        }

    }

    void onEnable()
    {
    
    }
    void onDisable()
    {
    
    }


    public void Start()
    {
        spawnPrefab();
    }

    public void Update()
    {

    }

    private void FetchGames()
    {
        string dirr = @"C:\Program Files\GameFiles";
        string[] dirs = Directory.GetFiles(dirr, "*", SearchOption.TopDirectoryOnly);

        foreach (string dir in dirs)
        {
            Debug.Log(dir);
        }
        Debug.Log(dirs);
    }
}
