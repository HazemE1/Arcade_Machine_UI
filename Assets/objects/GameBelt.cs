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

    [SerializeField] public Vector3 spawn;
    [SerializeField] public Vector2 scale = new Vector3(0.5f, 0.5f);

    private GameObject[] gameInstances = new GameObject[3];
    private int current = 0;


    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
    }

    public void spawnPrefab()
    {
       

        float x = UnityEngine.Random.value * 1000;
        float y = UnityEngine.Random.value * 1000;
        GameObject obj = Instantiate(prefab);
        Transform rt = obj.transform;
        rt.position = new Vector3(x, y, 0);



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
        spawnPrefab();
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
