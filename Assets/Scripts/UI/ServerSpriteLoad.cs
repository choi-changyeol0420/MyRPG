using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerSpriteLoad : MonoBehaviour
{
    #region Variables
    public Image img;
    public string imgUrl;
    UnityWebRequest www;
    #endregion
    private void Start()
    {
        StartCoroutine(TextureLoad());
    }
    IEnumerator TextureLoad()
    {
        string url = "https://image-server-choi-changyeol0420s-projects.vercel.app/Image/" + imgUrl;
        www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            img.sprite = MakeSprite(img.GetComponent<RectTransform>().pivot);
        }
    }
    Sprite MakeSprite(Vector2 pivot)
    {
        Texture2D texture = new Texture2D(100,100);
        texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),pivot);
        sprite.name = imgUrl;
        return sprite;
    }
}
