using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class jsonDataClass
{
    public List<Market> market { get; set; }
    public string closeButton { get; set; }
    public string moveButton { get; set; }
}
[System.Serializable]
public class Market
{
    public int id { get; set; }
    public string itemName { get; set; }
    public string itemImage { get; set; }
    public int itemQuantity { get; set; }
    public int itemPrice { get; set; }
    public string playerName { get; set; }
    public int playerLevel { get; set; }
    public string playerAvatar { get; set; }
}
