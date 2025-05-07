using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewModel : MonoBehaviour
{
    string path;
    public RawImage rawImage;

    public void OpenFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all images (.png)", "", "png");
        StartCoroutine(GetTexture());

    }


    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest(); 

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = myTexture;

        }

    }
}
