using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEditor.PackageManager;

public class GameBelt : MonoBehaviour
{

    private List<String> videoFormat = new() { "mp4" };
    private List<String> imageFormat = new() { "jpg", "jpeg" };
    private List<String> gamePathFormat = new() { "exe", "msi" };

    public GameObject videoPrefab;

    public GameObject center;
    public GameObject left;
    public GameObject right;
    public GameObject belt;


    private readonly List<GameInstance> gameInstances = new();
    private int current = 0;
    private float movement;


    TcpClient gameClient;

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
        gameInstances.ForEach(var =>
        {

            center.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[current], 1);
            if (current - 1 < 0)
            {
                left.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[gameInstances.Count - 1], 0);
            }
            else
            {
                left.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[current - 1], 0);

            }
            if (current + 1 >= gameInstances.Count)
            {
                right.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[0], 0);
            }
            else
            {
                right.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[current + 1], 0);
            }
        });
    }



    public void Start()
    {
        FetchGames();
        ClientConnect();

    }

    public void testGame()
    {
        print("TEST");
    }
    public void SelectGame(InputAction.CallbackContext context)
    {

        GameInstance instance = gameInstances[current];
        SharedData data = new(instance.gameTitle, instance.gamePath);
        NetworkStream stream = gameClient.GetStream();
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
        stream.Flush();
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
            string splashPath = "";

            string[] directoryParts = dir.Split("\\");

            gameTitle = directoryParts[^1];


            foreach (string file in files)
            {


                string[] parts = Path.GetFileName(file).Split(".");
                string ext = parts[1];

                if (videoFormat.Contains(ext))
                {
                    isVideo = true;
                    mediaPath = file;
                }

                if (imageFormat.Contains(ext))
                {

                    if (parts[0].ToLower().Equals("splash_screen"))
                    {
                        splashPath = file;


                    }
                    else
                    {
                        mediaPath = file;
                        isVideo = false;

                    }
                }

                if (gamePathFormat.Contains(ext))
                {
                    gamePath = file;
                }

            }

            if (gamePath.Equals("") || mediaPath.Equals("") || (isVideo && splashPath.Equals("")))
            {
                print("MISSING GAME FILES FOR DIRECTORY " + dir);
            }
            else
            {
                GameInstance gameInstance = new(gameTitle, gamePath, mediaPath, isVideo);
                gameInstance.splashScreen = splashPath;
                gameInstances.Add(gameInstance);
            }


        }


        center.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[0], 1);
        left.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[gameInstances.Count - 1], 0);
        right.GetComponent<GameObjectBehaviour>().SetGameInstance(gameInstances[1], 0);


        /* for (int x = 0; x < 10; x++)
         {

             gameInstances.Add(new GameInstance(x + "", "", "", false));
         }
         */



    }



    private void ClientConnect()
    {
        try
        {
            gameClient = new TcpClient("localhost", 11111);
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }
}





public class GameInstance
{
    public string gameTitle;
    public string gamePath;
    public string mediaPath;
    public bool isVideo;
    public string splashScreen;
    public GameInstance(string gameTitle, string gamePath, string mediaPath, bool isVideo)
    {
        this.gameTitle = gameTitle;
        this.gamePath = gamePath;
        this.mediaPath = mediaPath;
        this.isVideo = isVideo;
    }


}


class SharedData
{
    private String key { get; } // title of game represented as String 
    private String value { get; } // path of game's .exe  represented as String

    public SharedData(String key, String value)
    {
        this.key = key;
        this.value = value;
    }
}