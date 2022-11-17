using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class GameBelt : MonoBehaviour
{

    private List<String> videoFormat = new List<string> { "mp4" };
    private List<String> imageFormat = new List<string> { "jpg", "jpeg" };
    private List<String> gamePathFormat = new List<string> { "exe", "msi" };

    public GameObject videoPrefab;
    public GameObject picturePrefab;

    public GameObject center;
    public GameObject left;
    public GameObject right;


    public GameObject belt;

    public Vector3 spawn;
    public Vector3 scale = new Vector3(1f, 1f, 1f);

    private List<GameInstance> gameInstances = new List<GameInstance>();
    private int current = 0;
    private int size = 500;


    private bool isEnabled = false;

    private float movement;


    public void Move(InputAction.CallbackContext context)
    {

        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }
        movement = context.ReadValue<float>();
        current += (int)movement;

        if (current > gameInstances.Count - 1) { current = 0; }
        if (current < 0) { current = gameInstances.Count - 1; }
        print(current);
        gameInstances.ForEach(var =>
        {

            center.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[current]);
            if (current - 1 < 0)
            {
                left.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[gameInstances.Count - 1]);
            }
            else
            {
                left.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[current - 1]);

            }
            if (current + 1 >= gameInstances.Count)
            {
                right.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[0]);
            }
            else
            {
                right.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[current + 1]);
            }
        });
    }





    public void toggle()
    {

    }

    public void Start()
    {
        FetchGames();


    }

    private void Update()
    {
    }

    public void FixedUpdate()
    {
    }
    private void FetchGames()
    {
        string dirr = @"C:\Program Files\GameFiles";
        string[] dirs = Directory.GetDirectories(dirr, "*", SearchOption.TopDirectoryOnly);

        foreach (string dir in dirs)
        {
            string[] files = Directory.GetFiles(dir);
            string gameTitle = "";
            string gamePath = "";
            bool isVideo = false;
            string mediaPath = "";

            string[] directoryParts = dir.Split("\\");

            gameTitle = directoryParts[directoryParts.Length - 1];


            foreach (string file in files)
            {
                string[] parts = file.Split(".");
                string ext = parts[1];

                if (videoFormat.Contains(ext))
                {
                    isVideo = true;
                    mediaPath = file;
                }

                if (imageFormat.Contains(ext))
                {
                    isVideo = false;
                    mediaPath = file;
                }

                if (gamePathFormat.Contains(ext))
                {
                    gamePath = file;
                }

            }

            if (gamePath.Equals("") || mediaPath.Equals("") || isVideo.Equals(""))
            {
                print("MISSING GAME FILES FOR DIRECTORY " + dir);
            }
            else
            {
                GameInstance gameInstance = new(gameTitle, gamePath, mediaPath, isVideo);
                gameInstances.Add(gameInstance);
            }


        }


        center.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[0]);
        left.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[gameInstances.Count - 1]);
        right.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[1]);


        /* for (int x = 0; x < 10; x++)
         {

             gameInstances.Add(new GameInstance(x + "", "", "", false));
         }
         */



    }
}





public class GameInstance
{
    public string gameTitle;
    public string gamePath;
    public string mediaPath;
    public bool isVideo;
    public GameInstance(string gameTitle, string gamePath, string mediaPath, bool isVideo)
    {
        this.gameTitle = gameTitle;
        this.gamePath = gamePath;
        this.mediaPath = mediaPath;
        this.isVideo = isVideo;
    }
}