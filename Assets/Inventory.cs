using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Card;

public class Inventory
{
    public Inventory(PixelSpawner pixelSpawner)
    {
        PixelManager = pixelSpawner;
    }
    [JsonIgnore]
    public PixelSpawner PixelManager { get; set; }

    private int lastX = 0;
    public List<Card>[] CardInventory = new List<Card>[3];
    public List<Card> ActivSlot;

    public void RenderPartOfInventory(List<Card> part, int startX, int numInventoryPart, int lastCount)
    {
        int startY = 3;
        int length = 64;
        int CardLength = 12;

        if (part.Count <= (length / CardLength))
        {
            if (lastCount == 6)
            {
                for (int i = 0; i < part.Count; i++)
                {
                    part[i].Ymain = startY;
                    part[i].Xmain = startX + CardLength * i;

                    part[i].DrawCard(false);
                    part[i].VisibleRender();
                }
            }
            else
            {
                Card currentCard = part[part.Count - 1];
                int lastCard;
                if (part.Count > 1) lastCard = part[part.Count - 2].Xmain + 12;
                else lastCard = numInventoryPart * 64 + 3 + numInventoryPart;

                currentCard.Ymain = startY;
                currentCard.Xmain = lastCard;
                if (PixelSpawner.animationEnable)
                {
                    currentCard.MyAnimator.StartCoroutine(currentCard.MyAnimator.AddToInventoryAnimation(new Vector3(currentCard.Xmain + currentCard.Weight / 2, currentCard.Ymain + currentCard.Hight / 2, -1)));
                }
                else
                {
                    //currentCard.CenterPoint.transform.position = new Vector3(currentCard.Xmain + currentCard.Weight / 2, currentCard.Ymain + currentCard.Hight / 2, -1);
                    currentCard.ClearCard();
                    currentCard.DrawCard(false);
                    currentCard.VisibleRender();
                }

            }

        }
        else
        {
            double totalSpace = length - CardLength * part.Count;
            double spacePerCard = totalSpace / (part.Count - 1);

            int? different = (int)(CardLength + spacePerCard);

            Cleen(FindListIndex(part));
            for (int i = 0; i < part.Count; i++)
            {
                part[i].Ymain = startY;
                part[i].Xmain = startX + (int)(CardLength + spacePerCard) * i;

                if (part.Count - i == 1)
                {
                    different = null;
                    lastX = part[i].Xmain + part[i].Weight + 1;
                }


                part[i].DrawCard(false, different - 1);
                part[i].VisibleRender(different);
            }


        }

    }

    public void AddCard(int numInventoryPart, Card card)
    {
        if (!(card is HP) && ActivSlot.Count > 0) ActivSlot[ActivSlot.Count - 1].VisibleRender();

        PixelSpawner.DestroyObject(card.Collider.gameObject);
        //card.locked = false;       
        card.enabletToSelect = false;


        int lastCount = CardInventory[numInventoryPart].Count;
        CardInventory[numInventoryPart].Add(card);
        if (PixelSpawner.nowLocation == PixelSpawner.location.Chest) PixelManager.UpdateFrame();
        card.ClaerHashSet(card.BorderPixels);
        card.actionsForCard = actions.InInventory;
        card.ClaerHashSet(card.FramePixels);
        card.VisibleRender();
        //card.ClaerHashSet(card.ImagePixlels);
        //card.ClaerHashSet(card.BorderPixels);

        card.actionsForCard = actions.InInventory;
        card.select = false;


        string key = (card.Xmain + 2).ToString() + "; " + (card.Ymain + 4).ToString();
        PixelSpawner.Cards.Remove(key);
        PixelSpawner.OnlyCards.Remove(card);

        int startX = numInventoryPart * 64 + 3 + numInventoryPart;
        RenderPartOfInventory(CardInventory[numInventoryPart], startX, numInventoryPart, lastCount);

        if (!(card is HP))
        {
            if (ActivSlot.Count > 0) ActivSlot[ActivSlot.Count - 1].DrawBorder(Color.red);
            else SwapActivSlot();
        }
    }
    public void RemoveCard(int numInventoryPart, Card card)
    {
        int lastCount = CardInventory[numInventoryPart].Count;
        CardInventory[numInventoryPart].Remove(card);
        card.ClearCard();
        if (CardInventory[numInventoryPart].Count == 5) Cleen(numInventoryPart);
        card.actionsForCard = actions.Deleted;
        PixelSpawner.DestroyObject(card.CenterPoint);

        int startX = numInventoryPart * 64 + 3 + numInventoryPart;

        if (CardInventory[numInventoryPart].Count > 0)
            RenderPartOfInventory(CardInventory[numInventoryPart], startX, numInventoryPart, lastCount);

        if (!(card is HP))
        {
            if (ActivSlot.Count > 0)
            {
                ActivSlot[ActivSlot.Count - 1].DrawBorder(Color.red);
            }
            else SwapActivSlot();
        }

    }

    public void Cleen(int numInventoryPart)
    {
        foreach (Card card in CardInventory[numInventoryPart])
        {
            card.ClearCard();
        }
    }
    public void CleenAll()
    {
        Cleen(0);
        Cleen(1);
        Cleen(2);
    }
    public void CleenAll(int numOfinventoryPart)
    {
        Cleen(numOfinventoryPart);
    }

    public int FindListIndex(List<Card> targetList)
    {
        for (int i = 0; i < CardInventory.Length; i++)
        {
            if (CardInventory[i] == targetList)
                return i;
        }
        return -1;
    }

    public void InventoryChecker()
    {        
        for (int i = 0; i < CardInventory[2].Count; i++)
        {
            if (CardInventory[2][i] is Slime)
            {
                Slime slime = (Slime)CardInventory[2][i];
                if (i + 1 < CardInventory[2].Count && CardInventory[2][i + 1] is Slime)
                {
                    slime.Merger(CardInventory[2][i + 1]);                    
                }
            }
            else if (CardInventory[2][i] is Goblin)
            {
                Goblin goblin = (Goblin)CardInventory[2][i];
                goblin.TryGrab(10);
            }
            else if (CardInventory[2][i] is Undead)
            {
                Undead undead = (Undead)CardInventory[2][i];
                if (i + 1 < CardInventory[2].Count)
                {
                    undead.FightInInventory(CardInventory[2][i + 1]);                    
                }
                if (i > 0 && undead.actionsForCard != actions.Deleted)
                {
                    int c = undead.FightInInventory(CardInventory[2][i - 1]);
                    //if (CardInventory[2].Count != 0) 
                    i -= c;
                }
            }            
        }
    }
    public void SwapActivSlot()
    {
        if (ActivSlot.Count > 0) ActivSlot[ActivSlot.Count - 1].VisibleRender();
        ActivSlot = (ActivSlot == CardInventory[1]) ? CardInventory[2] : CardInventory[1];
        if (ActivSlot.Count > 0) ActivSlot[ActivSlot.Count - 1].DrawBorder(Color.red);
    }
}
