using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

public class jsonController : MonoBehaviour
{
    [SerializeField]
    string jsonURL;
    Texture2D texture;
    List<Market> marketList = new List<Market>();
    public GameObject marketPrefab;
    int iNext;
    static int currentPage = 0;
    List<List<Market>> screenList = new List<List<Market>>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendRequest());
    }

    IEnumerator SendRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(this.jsonURL);
        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
        JsonParse(request.downloadHandler.text);
    }

    void JsonParse(string jsonString) 
    {

        var marketCollection = JsonConvert.DeserializeObject<Market[]>(jsonString);
        
        // Распредление по листам
        int count = 0, bruh = 0;
        foreach (var child in marketCollection)
        {
            bruh++;
            if (count < 6 && bruh<marketCollection.Length)
            {
                marketList.Add(child);
                //Debug.Log(child.playerName);
                count++;
            }
            else
            {
                if (bruh < marketCollection.Length)
                {
                    screenList.Add(new List<Market>(marketList));
                    //Debug.Log("тут новый массив" + count);
                    //Debug.Log(child.playerName);
                    marketList.Clear();
                    marketList.Add(child);
                    count = 1;
                }
                else
                {
                    marketList.Add(child);
                    //Debug.Log(child.playerName);
                    screenList.Add(new List<Market>(marketList));
                    marketList.Clear();
                }
            }
            
        }
        /*Debug.Log(bruh);
        Debug.Log("в 1 массиве "+screenList.Count);
        Debug.Log("в 1 массиве " + screenList[16].Count);*/
        //Загрузка кнопок
        Image exitImg = GameObject.Find("Close").GetComponent<Image>();
        Image forwardImg = GameObject.Find("NextButton").GetComponent<Image>();
        Image backImg = GameObject.Find("BackButton").GetComponent<Image>();
        /*string closeLink = "http://javteq.ru/tst/" + screenList[currentPage][iNext].playerAvatar;
        string moveLink = "http://javteq.ru/tst/" + screenList[currentPage][iNext].itemImage;
        StartCoroutine(DownloadAvatar(avatarLink, avatarImg));
        StartCoroutine(DownloadItemImage(itemImageLink, itemImg));*/
        //Первая генерация
        for (iNext=0; iNext < screenList[currentPage].Count; iNext++)
        {
            try
            {
                DownloadInfo();
            }
            catch { Debug.Log("Bruh"); }
        }
    }

    IEnumerator DownloadAvatar(string lnk, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(lnk);
        yield return www.SendWebRequest();
        Texture2D txtr = ((DownloadHandlerTexture)www.downloadHandler).texture;
        image.sprite = Sprite.Create(txtr, new Rect(0, 0, txtr.width, txtr.height), new Vector2(0f, 0f),100.0f);
    }

    IEnumerator DownloadItemImage(string lnk, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(lnk);
        DownloadHandlerTexture textDl = new DownloadHandlerTexture(true);
        www.downloadHandler = textDl;
        yield return www.SendWebRequest();
        Texture2D txtr = textDl.texture;
        image.sprite = Sprite.Create(txtr, new Rect(0, 0, txtr.width, txtr.height), new Vector2(0f, 0f), 100.0f);
        
    }
    //Загрузка кнопок
    IEnumerator DownloadExitButton(string lnk, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(lnk);
        yield return www.SendWebRequest();
        Texture2D txtr = ((DownloadHandlerTexture)www.downloadHandler).texture;
        image.sprite = Sprite.Create(txtr, new Rect(0, 0, txtr.width, txtr.height), new Vector2(0f, 0f), 100.0f);
    }

    IEnumerator DownloadMoveButton(string lnk, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(lnk);
        DownloadHandlerTexture textDl = new DownloadHandlerTexture(true);
        www.downloadHandler = textDl;
        yield return www.SendWebRequest();
        Texture2D txtr = textDl.texture;
        image.sprite = Sprite.Create(txtr, new Rect(0, 0, txtr.width, txtr.height), new Vector2(0f, 0f), 100.0f);

    }

    void DownloadInfo()
    {
  
            marketPrefab = Instantiate(marketPrefab, GameObject.Find("MarketPanel").transform) as GameObject;

            Image avatarImg = marketPrefab.transform.Find("PlayerAvatar").GetComponent<Image>();
            Image itemImg = marketPrefab.transform.Find("ItemImage").GetComponent<Image>();
            string avatarLink = "http://javteq.ru/tst/" + screenList[currentPage][iNext].playerAvatar;
            string itemImageLink = "http://javteq.ru/tst/" + screenList[currentPage][iNext].itemImage;
            TextMeshProUGUI itemName = marketPrefab.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemQuantity = marketPrefab.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemPrice = marketPrefab.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI playerName = marketPrefab.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI playerLevel = marketPrefab.transform.Find("PlayerAvatar").Find("Levelcon").Find("Level").GetComponent<TextMeshProUGUI>();
            itemName.text = screenList[currentPage][iNext].itemName;
            itemQuantity.text = "x" + screenList[currentPage][iNext].itemQuantity;
            itemPrice.text = screenList[currentPage][iNext].itemPrice + " <sprite index=0>";
            playerName.text = screenList[currentPage][iNext].playerName;
            playerLevel.text = screenList[currentPage][iNext].playerLevel.ToString();
            StartCoroutine(DownloadAvatar(avatarLink, avatarImg));
            StartCoroutine(DownloadItemImage(itemImageLink, itemImg));
    }
    public void NextButton()
    {
        if (currentPage < screenList.Count-1)
        {
            DestroyAllChildren();
            currentPage++;
            for (iNext = 0; iNext < screenList[currentPage].Count; iNext++)
            {
                try
                {
                    DownloadInfo();
                }
                catch
                {
                    Debug.Log("Bruh");
                }
            }
            Debug.Log(currentPage);
        }
    }

    public void BackButton()
    {
        if (currentPage > 0)
        {
            DestroyAllChildren();
            currentPage--;
            for (iNext = 0; iNext < screenList[currentPage].Count; iNext++)
            {
                try
                {
                    DownloadInfo();
                }
                catch
                {
                    Debug.Log("Bruh");
                }
            }
            Debug.Log(currentPage);
        }

    }
    void DestroyAllChildren()
    {
        GameObject[] marketSlots = GameObject.FindGameObjectsWithTag("MarketSlot");
        foreach (GameObject slot in marketSlots)
        {
            Destroy(slot,0.01f);
        }
    }
}
