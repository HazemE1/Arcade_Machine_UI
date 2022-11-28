using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Rendering;

public class GameObjectBehaviour : MonoBehaviour
{
    public GameInstance gameInstance;
    [SerializeField]
    private GameObject gameTitleObject;
    [SerializeField]
    private GameObject videoObject;
    [SerializeField]
    private GameObject rawImageObject;
    [SerializeField]
    private GameObject overlayObject;

    private bool isSplashScreen = true;
    private RenderTexture videoTexture;
    int pos = 0;

    public void SetGameInstance(GameInstance val, int _pos)
    {
        gameInstance = val;
        this.pos = _pos;

        gameTitleObject.GetComponent<TextMeshProUGUI>().text = gameInstance.gameTitle;


        switch (gameInstance.isVideo)
        {
            case true:
                if (pos == 1)
                {
                    PrepareVideoTexture();
                }
                SetSplashScreen();
                break;

            case false:
                SetImageTexture();
                break;

        }

        print(val.gamePath);
    }


    private void SetImageTexture()
    {
        Texture2D texture = new(2, 2);
        byte[] imageByes = File.ReadAllBytes(gameInstance.mediaPath);
        texture.LoadImage(imageByes);
        rawImageObject.GetComponent<RawImage>().texture = texture;


    }

    private void SetSplashScreen()
    {
        Texture2D texture = new(2, 2);
        byte[] imageByes = File.ReadAllBytes(gameInstance.splashScreen);
        texture.LoadImage(imageByes);
        rawImageObject.GetComponent<RawImage>().texture = texture;
        isSplashScreen = true;
    }


    private void PrepareVideoTexture()
    {
        videoTexture = new(500, 500, 32)
        {
            name = "test"
        };
        videoObject.GetComponent<VideoPlayer>().targetTexture = videoTexture;
        videoObject.GetComponent<VideoPlayer>().url = gameInstance.mediaPath;
        videoObject.GetComponent<VideoPlayer>().Prepare();

    }

    public void Update()
    {
        if (isSplashScreen && videoObject.GetComponent<VideoPlayer>().isPrepared)
        {
            rawImageObject.GetComponent<RawImage>().texture = videoTexture;
            videoObject.GetComponent<VideoPlayer>().Play();
            isSplashScreen = false;
        }


    }



}
