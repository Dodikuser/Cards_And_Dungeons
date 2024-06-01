using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;

[Serializable]
public class Enemy : Card
{
    protected List<string> MyList { get; set; }

    public Enemy(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
    }
    protected void InitMyList(string nameOfList)
    {
        MyList = PixelSpawner.EnemyLists[nameOfList];
        int randomCard = PixelSpawner.random.Next(0, (int)MyList.LongCount());
        filePath = MyList[randomCard];
    }
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(300f / 360f, 1f, 0.5f), limit);
    }

    public override void Interactions()
    {       
    }
    public void KillCard(Card card)
    {
        PixelSpawner.selectedCard = null;
        card.ClaerHashSet(card.ImagePixlels);
        card.ClaerHashSet(card.BorderPixels);
        card.ClaerHashSet(card.FramePixels);
        card.ClaerHashSet(card.ValuePixels);
        card.actionsForCard = actions.Deleted;
        card.select = false;

        string key = (card.Xmain + 2).ToString() + "; " + (card.Ymain + 4).ToString();
        PixelSpawner.Cards.Remove(key);
        PixelSpawner.OnlyCards.Remove(card);

        PixelManager.SelectNear();
        PixelSpawner.countKillcard++;
    }
    public void OpenCard()
    {
        if (PixelSpawner.animationEnable) MyAnimator.StartCoroutine(MyAnimator.ShowAnimation());
        else
        {
            actionsForCard = actions.Visible;
            DrawCard();
        }
        if (PixelSpawner.nowLocation != PixelSpawner.location.Chest) AudioManager.PlaySound("openLine", 1f);
    }
    public override void Select(Color selectedColor, bool drawSeleck)
    {
        if (drawSeleck)
        {
            DrawBorder(selectedColor);
            DrawFrame(Color.HSVToRGB(0f, 1f, 0.5f), (actionsForCard == actions.InInventory) ? false : true);
        }

        select = true;
        PixelSpawner.selectedCard = this;
        if (Enabled) PixelManager.SelectEnable();
    }

    public static void Substitution(Card firstCard, Card secondCard)
    {
        AudioManager.PlaySound("mag", 1f);
        int index = PixelSpawner.OnlyCards.IndexOf(firstCard);
        PixelSpawner.OnlyCards[index] = secondCard;
        string key = (firstCard.Xmain + 2).ToString() + "; " + (firstCard.Ymain + 4).ToString();
        PixelSpawner.Cards[key] = secondCard;
        PixelSpawner.selectedCard = secondCard;

        firstCard.ClearCard();
        secondCard.actionsForCard = actions.Visible;
        secondCard.Enabled = true;
        secondCard.DrawCard();

        secondCard.PixelManager.SelectNear();
    }

    protected void DefaultFight(Card lastCard, int thisValueEnemy)
    {
        if (PixelSpawner.Inventory.ActivSlot.Count != 0)
        {
            int lastValueCard = Convert.ToInt32(lastCard.Value);

            if (lastValueCard <= thisValueEnemy)
            {
                PixelSpawner.Inventory.RemoveCard(PixelSpawner.Inventory.FindListIndex(PixelSpawner.Inventory.ActivSlot), lastCard);
                AudioManager.PlaySound("crash", 1f);
            }
            else AudioManager.PlaySound("damege2", 1f);
            actionsForCard = actions.Deleted;
            DrawBorder(PixelSpawner.selectColor);
            PixelManager.SelectNear();
            PixelSpawner.stats.MyStatistics["CountOfKills"]++;

            PixelSpawner.countKillcard++;
        }
    }
    protected void HpFight()
    {
        if (PixelSpawner.Inventory.CardInventory[0].Count != 0)
        {
            PixelSpawner.Inventory.RemoveCard(0, PixelSpawner.Inventory.CardInventory[0][PixelSpawner.Inventory.CardInventory[0].Count - 1]);
            actionsForCard = actions.Deleted;
            DrawBorder(PixelSpawner.selectColor);

            PixelManager.SelectNear();
            AudioManager.PlaySound("damege1", 1f);
            PixelSpawner.stats.MyStatistics["CountOfKills"]++;

            PixelSpawner.countKillcard++;
        }
        else
        {
            AudioManager.StopBackgroundMusic();
            AudioManager.PlaySound("GameOver", 1f);
            PixelSpawner.nowLocation = PixelSpawner.location.GameOver;
            PixelManager.NextLevl();
        }
    }
    protected void DefaultDelateAction()
    {
        PixelSpawner.Inventory.AddCard(2, this);
        if (PixelSpawner.controlModNow == PixelSpawner.controlMod.Keyboard) PixelSpawner.FindCard(PixelSpawner.axes.Up, true, false, true);
        AudioManager.PlaySound("take", 1f);
        PixelSpawner.stats.MyStatistics["CountOfPickUp"]++;
    }
}
public class Slime : Enemy
{
    public Slime(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        InitMyList("slimes");
    }
    public override void Interactions()
    {
        switch (actionsForCard)
        {
            case actions.Hide:
                OpenCard();
                break;
            case actions.Visible:
                int thisValueEnemy = Convert.ToInt32(Value);
                Card lastCard;
                if (PixelSpawner.Inventory.ActivSlot.Count > 0)
                {
                    lastCard = PixelSpawner.Inventory.ActivSlot[PixelSpawner.Inventory.ActivSlot.Count - 1];
                    int lastValueCard = Convert.ToInt32(lastCard.Value);

                    if (lastCard is Slime)
                    {
                        AudioManager.PlaySound("slime", 1f);
                        ClaerHashSet(ValuePixels);
                        Value = (Convert.ToInt32(Value) + Convert.ToInt32(lastCard.Value)).ToString();
                        DrowValue(Color.green, 0);

                        PixelSpawner.Inventory.RemoveCard(PixelSpawner.Inventory.FindListIndex(PixelSpawner.Inventory.ActivSlot), lastCard);
                    }
                    else if (lastCard is Mag && lastValueCard > thisValueEnemy)
                    {
                        Substitution(this, new HP(PixelManager, Xmain, Ymain, Value));
                    }
                    else if (lastCard is Piphiy)
                    {
                        Piphiy piphiy = (Piphiy)lastCard;
                        piphiy.InteractWhisAtherCard(lastCard, this);
                    }
                    else if (lastCard is Goblin)
                    {
                        Goblin goblin = (Goblin)lastCard;
                        bool grafted = goblin.GraftAtherCard(this, 30);
                        if (!grafted) DefaultFight(lastCard, thisValueEnemy);
                    }
                    else DefaultFight(lastCard, thisValueEnemy);
                }
                else HpFight();
                break;
            case actions.Deleted:
                DefaultDelateAction();
                break;
        }
    }

    public void Merger(Card mergerCard)
    {
        AudioManager.PlaySound("slime", 1f);
        Value = (Convert.ToInt32(Value) + Convert.ToInt32(mergerCard.Value)).ToString();
        PixelSpawner.Inventory.RemoveCard(2, mergerCard);
    }
}
public class Undead : Enemy
{
    public Undead(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        InitMyList("undeads");
    }
    public override void Interactions()
    {
        switch (actionsForCard)
        {
            case actions.Hide:
                OpenCard();
                break;
            case actions.Visible:
                int thisValueEnemy = Convert.ToInt32(Value);
                Card lastCard;

                if (PixelSpawner.Inventory.ActivSlot.Count > 0)
                {
                    lastCard = PixelSpawner.Inventory.ActivSlot[PixelSpawner.Inventory.ActivSlot.Count - 1];
                    int lastValueCard = Convert.ToInt32(lastCard.Value);

                    if (lastCard is Mag && lastValueCard > thisValueEnemy)
                    {
                        Substitution(this, new Arrmy(PixelManager, Xmain, Ymain, Value));
                    }
                    else if (lastCard is Piphiy)
                    {
                        Piphiy piphiy = (Piphiy)lastCard;
                        piphiy.InteractWhisAtherCard(lastCard, this);
                    }
                    else if (lastCard is Goblin)
                    {
                        Goblin goblin = (Goblin)lastCard;
                        bool grafted = goblin.GraftAtherCard(this, 30);
                        if (!grafted) DefaultFight(lastCard, thisValueEnemy);
                    }
                    else if(lastCard is Slime)
                    {
                        AudioManager.PlaySound("undead", 1f);
                        Slime slime = (Slime)lastCard;
                        slime.ClaerHashSet(slime.ValuePixels);
                        slime.Value = (Convert.ToInt32(slime.Value) / 2).ToString();
                        slime.DrowValue(Color.green, 0);
                        KillCard(this);
                    }
                    else DefaultFight(lastCard, thisValueEnemy);
                }
                else HpFight();
                break;
            case actions.Deleted:
                DefaultDelateAction();
                break;
        }
    }

    public int FightInInventory(Card interactCard)
    {
        int interactCardValue = Convert.ToInt32(interactCard.Value);
        if (interactCard is Goblin) return 0;

        if (Convert.ToInt32(Value) >= interactCardValue)
        {
            AudioManager.PlaySound("undead", 1f);
            PixelSpawner.Inventory.RemoveCard(2, interactCard);
            return 1;
        }
        else
        {
            PixelSpawner.Inventory.RemoveCard(2, this);
            if (interactCard is Piphiy)
            {
                Piphiy piphiy = (Piphiy)interactCard;
                piphiy.SwapPilarity();
                return 2;
            }           
            else
            {
                AudioManager.PlaySound("undead", 1f);
                PixelSpawner.Inventory.RemoveCard(2, interactCard);
                return 1;
            }
            
        }           
    }
}
public class Mag : Enemy
{
    public Mag(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        InitMyList("mags");
    }
    public override void Interactions()
    {
        switch (actionsForCard)
        {
            case actions.Hide:
                OpenCard();
                break;
            case actions.Visible:
                int thisValueEnemy = Convert.ToInt32(Value);
                Card lastCard;
                if (PixelSpawner.Inventory.ActivSlot.Count > 0)
                {
                    lastCard = PixelSpawner.Inventory.ActivSlot[PixelSpawner.Inventory.ActivSlot.Count - 1];

                    if (lastCard is Slime)
                    {
                        AudioManager.PlaySound("error", 1f);
                    }
                    else if (lastCard is Piphiy)
                    {
                        Piphiy piphiy = (Piphiy)lastCard;
                        piphiy.InteractWhisAtherCard(lastCard, this);
                    }
                    else if (lastCard is Goblin)
                    {
                        Goblin goblin = (Goblin)lastCard;
                        bool grafted = goblin.GraftAtherCard(this, 30);
                        if (!grafted) DefaultFight(lastCard, thisValueEnemy);
                    }
                    else DefaultFight(lastCard, thisValueEnemy);
                }
                else HpFight();
                break;
            case actions.Deleted:
                DefaultDelateAction();
                break;
        }
    }
}
public class Goblin : Enemy
{
    public Goblin(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        InitMyList("goblins");
    }
    public override void Interactions()
    {
        switch (actionsForCard)
        {
            case actions.Hide:
                OpenCard();
                break;
            case actions.Visible:
                int thisValueEnemy = Convert.ToInt32(Value);
                Card lastCard;
                if (PixelSpawner.Inventory.ActivSlot.Count > 0)
                {
                    lastCard = PixelSpawner.Inventory.ActivSlot[PixelSpawner.Inventory.ActivSlot.Count - 1];
                    if (lastCard is Undead) 
                    {
                        AudioManager.PlaySound("error", 1f);
                    }
                    else if (lastCard is Piphiy)
                    {
                        Piphiy piphiy = (Piphiy)lastCard;
                        piphiy.InteractWhisAtherCard(lastCard, this);
                    }
                    else
                    {
                        bool grafted = Graft(lastCard, (lastCard is Goblin) ? 20 : 30);
                        if (!grafted)
                        {
                            int lastValueCard = Convert.ToInt32(lastCard.Value);
                            if (lastCard is Mag && lastValueCard > thisValueEnemy) PixelSpawner.coin.Value += Convert.ToInt32(Value);
                            else if (lastCard is Slime)
                            {
                                Slime slime = (Slime)lastCard;
                                slime.ClaerHashSet(slime.ValuePixels);
                                slime.Value = (Convert.ToInt32(slime.Value) + (Convert.ToInt32(Value) / 10) + 1).ToString();
                                slime.DrowValue(Color.green, 0);
                            }
                            DefaultFight(lastCard, thisValueEnemy);
                        }
                    }
                }
                else HpFight();
                break;
            case actions.Deleted:
                DefaultDelateAction();
                break;
        }
    }
    public bool Graft(Card card, int chance)
    {
        System.Random random = new System.Random();
        int randGraft = random.Next(0, 100);
        if (randGraft < chance)
        {
            AudioManager.PlaySound("goblin", 1f);
            ClaerHashSet(ValuePixels);
            Value = (Convert.ToInt32(Value) + Convert.ToInt32(card.Value)).ToString();
            DrowValue(Color.green, 0);

            PixelSpawner.Inventory.RemoveCard(PixelSpawner.Inventory.FindListIndex(PixelSpawner.Inventory.ActivSlot), card);
            return true;
        }
        else return false;
    }
    public bool GraftAtherCard(Card card, int chance)
    {
        System.Random random = new System.Random();
        int randGraft = random.Next(0, 100);
        if (randGraft < chance)
        {
            AudioManager.PlaySound("goblin", 1f);
            KillCard(card);           

            ClaerHashSet(ValuePixels);
            Value = (Convert.ToInt32(Value) + Convert.ToInt32(card.Value)).ToString();
            DrowValue(Color.green, 0);

            PixelSpawner.stats.MyStatistics["CountOfKills"]++;           
            
            return true;
        }
        else return false;
    }
    public void TryGrab(int chance)
    {
        int randGraft = PixelSpawner.random.Next(0, 100);
        if (randGraft < chance)
        {
            AudioManager.PlaySound("goblin", 1f);
            PixelSpawner.coin.Value -= Convert.ToInt32(Value);
            if (PixelSpawner.coin.Value < 0 )
                if (PixelSpawner.Inventory.CardInventory[0].Count != 0)
                {
                    PixelSpawner.Inventory.RemoveCard(0, PixelSpawner.Inventory.CardInventory[0][PixelSpawner.Inventory.CardInventory[0].Count - 1]);                    
                }
                else
                {
                    AudioManager.StopBackgroundMusic();
                    AudioManager.PlaySound("GameOver", 1f);
                    PixelSpawner.nowLocation = PixelSpawner.location.GameOver;
                    PixelManager.NextLevl();
                }
        }      
    }
}
public class Piphiy : Enemy
{
    public bool polarity;
    public Piphiy(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        InitMyList("piphiis");
        polarity = PixelSpawner.random.Next(0, 2) == 1;
    }
    public override void Interactions()
    {
        switch (actionsForCard)
        {
            case actions.Hide:
                OpenCard();
                break;
            case actions.Visible:
                int thisValueEnemy = Convert.ToInt32(Value);
                Card lastCard;

                if (PixelSpawner.Inventory.ActivSlot.Count > 0)
                {
                    lastCard = PixelSpawner.Inventory.ActivSlot[PixelSpawner.Inventory.ActivSlot.Count - 1];
                    int lastValueCard = Convert.ToInt32(lastCard.Value);

                    InteractPiphii(this, lastCard);
                }
                else HpFight();
                break;
            case actions.Deleted:
                DefaultDelateAction();
                break;
        }
    }
    public void SwapPilarity()
    {
        AudioManager.PlaySound("piphiy", 1f);
        polarity = !polarity;
        ClaerHashSet(ValuePixels);
        DrowValue(Color.green, 0);
    }

    public void InteractWhisAtherCard(Card lastCard, Card interactCard)
    {
        Piphiy piphiy = (Piphiy)lastCard;
        int thisValueEnemy = Convert.ToInt32(interactCard.Value);
        int lastValueCard = Convert.ToInt32(lastCard.Value);

        if (lastValueCard <= thisValueEnemy)
        {
            AudioManager.PlaySound("piphiy", 1f);
            interactCard.ClaerHashSet(interactCard.ValuePixels);
            interactCard.Value = (Convert.ToInt32(interactCard.Value) + lastValueCard * ((piphiy.polarity) ? 1 : -1)).ToString();
            interactCard.DrowValue(Color.green, 0);

            PixelSpawner.Inventory.RemoveCard(PixelSpawner.Inventory.FindListIndex(PixelSpawner.Inventory.ActivSlot), this);
        }
        else
        {
            piphiy.SwapPilarity();
            KillCard(interactCard);
            PixelSpawner.stats.MyStatistics["CountOfKills"]++;            
        }
    }
    public void InteractPiphii(Card piphiyCard, Card interactCard)
    {
        Piphiy piphiy = (Piphiy)piphiyCard;
        int interactCardValue = Convert.ToInt32(interactCard.Value);
        int piphiyValue = Convert.ToInt32(piphiyCard.Value);

        if (piphiyValue <= interactCardValue)
        {
            HpFight();
        }
        else
        {
            piphiy.SwapPilarity();
            PixelSpawner.Inventory.RemoveCard(PixelSpawner.Inventory.FindListIndex(PixelSpawner.Inventory.ActivSlot), interactCard);
        }
    }

    public override void DrowValue(Color color, int offsetX)
    {
        offsetX += 5;
        PixelSpawner.text.DrawText(PixelManager, Value + ((polarity) ? "↑" : "↓"), Xmain + 8 - (Value.Length - 1) - offsetX, Ymain + Hight - 3, (polarity) ? color : Color.red, 1, ValuePixels, Zmain - 1, CenterPoint);
        MyAnimator.SetStartInfo();
    }
}