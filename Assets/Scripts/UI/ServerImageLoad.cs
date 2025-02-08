using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerImageLoad : MonoBehaviour
{
    #region Variables
    public RawImage img;
    public string imageUrl;
    #endregion
    private void Start()
    {
        StartCoroutine(TextureLoad());
    }
    IEnumerator TextureLoad()
    {
        string url = "https://image-server-choi-changyeol0420s-projects.vercel.app/Image/" + imageUrl;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
