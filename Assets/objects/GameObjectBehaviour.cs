using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class GameObjectBehaviour : MonoBehaviour
{
    public GameInstance gameInstance;
    [SerializeField]
    private GameObject gameTitleObject;
    [SerializeField]
    private GameObject recourceObject;
    [SerializeField]
    private GameObject overlayObject;




    public void SetGameInstance(GameInstance val)
    {
        gameInstance = val;

        gameTitleObject.GetComponent<TextMeshProUGUI>().text = val.gameTitle;


        switch (val.isVideo)
        {
            case true:
                RenderTexture centerText = new(500, 500, 32);
                centerText.name = val.gameTitle.ToString(); 
                this.transform.GetChild(0).GetComponent<RawImage>().texture = centerText;
                this.transform.GetChild(1).GetComponent<VideoPlayer>().targetTexture = centerText;
                this.transform.GetChild(1).GetComponent<VideoPlayer>().url = val.mediaPath;
                break;
            case false:
                Texture2D texture = new(2, 2);
                byte[] imageByes = File.ReadAllBytes(val.mediaPath);
                texture.LoadImage(imageByes);
                this.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
                break;
        }



    }



}
