using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal class Shop : Card
{
    public Shop(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        filePath = "Old_Sptites/mazaziz.txt";
    }
    
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(120f / 360f, 1f, 0.5f));
    }

    public override void Interactions()
    {

        if (actionsForCard == actions.Hide)
        {
            actionsForCard = actions.Visible;
            DrawCard();
            PixelManager.SelectNear();
        }
        else if (actionsForCard == actions.Visible)
        {           
            enabletToSelect = false;
            PixelSpawner.nowLocation = PixelSpawner.location.Shop;
            UnSelect(PixelSpawner.unSelectColor);
            AudioManager.StopBackgroundMusic();
            AudioManager.PlaySound("next", 1f);
            PixelManager.NextLevl();

        }
    }    
}

[Serializable]
public class Coin
{
    private int _value;

    public Coin(PixelSpawner pixelSpawner, int x, int y, int value = 0)
    {
        X = x;
        Y = y;
        _value = value;
        PixelManager = pixelSpawner;
    }
    [JsonIgnore]
    public PixelSpawner PixelManager { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    [JsonIgnore]
    public HashSet<Pixel> moneyText = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> moneyImage = new HashSet<Pixel>();

    public int Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                DrowValue();
                AudioManager.PlaySound("coins", 0.5f);
            }
        }
    }

    private int lastValue = 0;

    public void DrowCoin()
    {
        for (int i = 0; i < 3; i++)
        {
            PixelManager.CreatePixel(X + i, Y, Color.HSVToRGB(60f / 360f, 1f, 0.5f), moneyImage, -2);
            PixelManager.CreatePixel(X + i, Y + 5, Color.HSVToRGB(60f / 360f, 1f, 0.5f), moneyImage, -2);

            for(int j = 1; j < 5; j++)
            {
                PixelManager.CreatePixel(X + i, Y + j, Color.yellow, moneyImage, -2);

                if (i < 1 && j<5)
                {
                    PixelManager.CreatePixel(X - 1, Y + j, Color.HSVToRGB(60f / 360f, 1f, 0.5f), moneyImage, -2);
                    PixelManager.CreatePixel(X + 3, Y + j, Color.HSVToRGB(60f / 360f, 1f, 0.5f), moneyImage, -2);
                }
            }
                        
        }
    }
    public void DrowValue()
    {
        ClaerHashSet(moneyText);
        PixelSpawner.text.DrawText(PixelManager, Value.ToString(), X + 5, Y, Color.yellow, 1, moneyText, -2);               
        lastValue = Value;
    }
    public void ClaerHashSet(HashSet<Pixel> hashSet)
    {
        foreach (Pixel pixel in hashSet)
        {
            PixelSpawner.DestroyObject(pixel.gameObject);
        }
        hashSet.Clear();
    }
}

internal class Sell : Card
{
    private string _value;

    public Sell(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        filePath = "Old_Sptites/sell.txt";
        actionsForCard = actions.Visible;
        if (Xmain > 0 & Ymain > 0) DrawCard();
        Value = ValueCards();
        _value = ValueCards();
    }
    [JsonIgnore]
    public HashSet<Pixel> sellText = new HashSet<Pixel>();
    public override string Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                string temp = (_value != null) ? _value : "0";
                _value = value;
                if(PixelManager != null) DrowValue(Color.green, 0);
            }
        }

    }

    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(300f / 360f, 1f, 0.5f));       
    }   
    public override void Interactions()
    {
        if (PixelSpawner.Inventory.CardInventory[2] != null && PixelSpawner.Inventory.CardInventory[2].Count > 0)
        {
            PixelSpawner.coin.Value += Convert.ToInt32(ValueCards());
            PixelSpawner.Inventory.CleenAll(2);
            PixelSpawner.Inventory.CardInventory[2].Clear();          

            foreach (Card iCard in PixelSpawner.OnlyCards)
            {
                if (iCard is SellLast && PixelSpawner.Inventory.CardInventory[2].Count == 0) iCard.Value = "0";
            }

            Value = ValueCards();
        }
    }
    public string ValueCards()
    {
        int value = 0;
        foreach (Card iCard in PixelSpawner.Inventory.CardInventory[2])
        {
            value += Convert.ToInt32(iCard.Value) / 2;
        }
        return value.ToString();
    }
    public override void DrowValue(Color color, int offsetX)
    {
        ClaerHashSet(sellText);
        PixelSpawner.text.DrawText(PixelManager, Value, Xmain + 8 - (Value.Length - 1) - offsetX, Ymain + Hight - 3, color, 1, sellText, -2, CenterPoint);
        if(sellText.Count !=0 ) PixelManager.AllText.AddRange(sellText);
    }
}

internal class SellLast : Sell
{
    private Card lastElement;

    public SellLast(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        if (PixelSpawner.Inventory.CardInventory[2].Count > 0) Value = (Convert.ToInt32(lastElement.Value) / 2).ToString();
        else Value = "0";
        lastElement = (PixelSpawner.Inventory.CardInventory[2].Count != 0) ? PixelSpawner.Inventory.CardInventory[2][PixelSpawner.Inventory.CardInventory[2].Count - 1] : new Card(PixelManager, 0, 0, "0");
    }

    public override void Interactions()
    {
        if (PixelSpawner.Inventory.CardInventory[2] != null && PixelSpawner.Inventory.CardInventory[2].Count > 0)
        {           
            PixelSpawner.coin.Value += Convert.ToInt32(Value);

            lastElement = PixelSpawner.Inventory.CardInventory[2][PixelSpawner.Inventory.CardInventory[2].Count - 1];
            PixelSpawner.Inventory.RemoveCard(2, lastElement);
            lastElement = (PixelSpawner.Inventory.CardInventory[2].Count != 0) ? PixelSpawner.Inventory.CardInventory[2][PixelSpawner.Inventory.CardInventory[2].Count - 1] : new Card(PixelManager, 0, 0, "0");
            Value = (Convert.ToInt32(lastElement.Value) / 2).ToString();

            foreach (Card iCard in PixelSpawner.OnlyCards)
            {
                if (iCard.GetType() == typeof(Sell)) iCard.Value = ValueCards();
            }

            if (PixelSpawner.Inventory.CardInventory[2].Count == 0)
            {
                Value = "0";
            }
        }
    }
}
