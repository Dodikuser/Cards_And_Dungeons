using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal class Case : Card
{
    public static System.Random random = new System.Random();

    public Case(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        actionsForCard = actions.Visible;
    }
    //public int countOfCardContains { get; set; }

    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.magenta);
    }
    public override void Interactions()
    {
        for (int i = 0; i < PixelSpawner.OnlyCards.Count; i++)
        {
            if (PixelSpawner.OnlyCards[i].InCace)
            {
                PixelSpawner.KillCard(PixelSpawner.OnlyCards[i]);
                i--;
            }
        }
        PixelSpawner.coin.Value -= Convert.ToInt32(Value);

    }
    public static int[] GenerateArrayForCase(int countCard)
    {
        int[] array = new int[4];
        int cardsOnlayers = 5;
        if (countCard < 7) cardsOnlayers = 3;
        int Layers = countCard / cardsOnlayers;
        int addCards = countCard % cardsOnlayers;

        for (int i = 0; i < Layers; i++)
        {
            array[i] = cardsOnlayers;
        }
        array[Layers] = addCards;
        return array;
    }

    public int[] RemoveElementFromArray(int[] array, int indexToRemove)
    {

        int[] newArray = new int[array.Length - 1];
        int newIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (i != indexToRemove)
            {
                newArray[newIndex] = array[i];
                newIndex++;
            }
        }

        return newArray;
    }
    public int[] NewRender(int[] numOfCard = null, string mod = "Normal", int? Xsize = null, int? Ysize = null, int offsetX = 0, int offsetY = 0)
    {

        int countOfLins = 0;
        foreach (int i in numOfCard)
        {
            if (i > 0) countOfLins++;
            else numOfCard = RemoveElementFromArray(numOfCard, Array.IndexOf(numOfCard, 0));
        }


        int lastY = (int)((Ysize - countOfLins * 10) % (countOfLins + 1));
        int spaceY = (int)((Ysize - countOfLins * 10) / (countOfLins + 1));

        for (int i = (int)Ysize - spaceY - (int)(lastY / 2) - 7, numOfLine = 0; i >= 2; i -= 10 + spaceY, numOfLine++)
        {

            int lastX = (int)((Xsize - numOfCard[numOfLine] * 12) % (numOfCard[numOfLine] + 1));
            int spaceX = (int)((Xsize - numOfCard[numOfLine] * 12) / (numOfCard[numOfLine] + 1));

            for (int j = spaceX + (int)(lastX / 2) + 5; j <= Xsize - 8; j += 12 + spaceX)
            {

                int randomNumber = random.Next(1, 4);

                Card card = new Card(PixelManager, 0, 0);

                if (randomNumber == 1) card = new CaseDefault(PixelManager, j + offsetX, i + offsetY);

                if (randomNumber == 2) card = new CaseHp(PixelManager, j + offsetX, i + offsetY);

                if (randomNumber == 3) card = new CaseArrmy(PixelManager, j + offsetX, i + offsetY);
              
                //Thread.Sleep(1);
                card.actionsForCard = actions.Visible;
                card.Enabled = true;
                card.DrawCard();
                card.VisibleRender();

                string vector = (card.Xmain + 2).ToString() + "; " + (card.Ymain + 4).ToString();
                PixelSpawner.Cards.Add(vector, card);
                PixelSpawner.OnlyCards.Add(card);
            }

        }
        return numOfCard;
    }
}

internal class CaseDefault : Case
{

    public CaseDefault(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        Value = random.Next(6, 7 + PixelSpawner.dungenLevel).ToString();
        filePath = "Old_Sptites/default_case.txt";
    }


    public override void Interactions()
    {
        if (PixelSpawner.coin.Value >= Convert.ToInt32(Value))
        {
            base.Interactions();
            int countCards = Convert.ToInt32(Value) / 2;
            if (countCards > 15) countCards = 15;

            int[] myCards = GenerateArrayForCase(countCards);
            PixelManager.NewRender(myCards, "Shop_defaulCase", 120, 70, 80, 40);
            UnSelect(PixelSpawner.unSelectColor);
            PixelSpawner.selectedCard = null;
            PixelSpawner.KillCard(this);
        }
    }

}

internal class CaseHp : Case
{
    public CaseHp(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        Value = random.Next(6, 7 + PixelSpawner.dungenLevel).ToString();
        filePath = "Old_Sptites/caseHP.txt";
    }
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(0f, 1f, 0.5f));
    }

    public override void Interactions()
    {
        if (PixelSpawner.coin.Value >= Convert.ToInt32(Value))
        {
            base.Interactions();

            int countCards = Convert.ToInt32(Value) / 2;
            if (countCards > 15) countCards = 15;

            int[] myCards = GenerateArrayForCase(countCards);
            PixelManager.NewRender(myCards, "Shop_HpCase", 120, 70, 80, 40);
            UnSelect(PixelSpawner.unSelectColor);
            PixelSpawner.selectedCard = null;
            PixelSpawner.KillCard(this);
        }
    }

}

internal class CaseArrmy : Case
{
    public CaseArrmy(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        Value = random.Next(6, 7 + PixelSpawner.dungenLevel).ToString();
        filePath = "Old_Sptites/caseArmy.txt";
    }
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(60f / 360f, 1f, 0.5f));
    }

    public override void Interactions()
    {
        if (PixelSpawner.coin.Value >= Convert.ToInt32(Value))
        {
            base.Interactions();
            int countCards = Convert.ToInt32(Value) / 2;
            if (countCards > 15) countCards = 15;

            int[] myCards = GenerateArrayForCase(countCards);
            PixelManager.NewRender(myCards, "Shop_ArrmyCase", 120, 70, 80, 40);
            UnSelect(PixelSpawner.unSelectColor);
            PixelSpawner.selectedCard = null;
            PixelSpawner.KillCard(this);
        }
    }
}
