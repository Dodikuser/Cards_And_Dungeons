using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleText
{
    private Dictionary<char, bool[,]> _charPatterns;

    public ConsoleText()
    {
        InitializeCharPatterns();
    }

    private void InitializeCharPatterns()
    {
        _charPatterns = new Dictionary<char, bool[,]>
            {
    { 'A', new[,] {
        { false, true, true, true, false },
        { true, false, false, false, true },
        { true, true, true, true, true },
        { true, false, false, false, true },
        { true, false, false, false, true }
    }},
    { 'B', new[,] {
        { true, true, true, false },
        { true, false, false, true },
        { true, true, true, false },
        { true, false, false, true },
        { true, true, true, false }
    }},
    { 'C', new[,] {
        { false, true, true, true },
        { true, false, false, false },
        { true, false, false, false },
        { true, false, false, false },
        { false, true, true, true }
    }},
    { 'D', new[,] {
        { true, true, true, false },
        { true, false, false, true },
        { true, false, false, true },
        { true, false, false, true },
        { true, true, true, false }
    }},
    { 'E', new[,] {
        { true, true, true, true },
        { true, false, false, false },
        { true, true, true, false },
        { true, false, false, false },
        { true, true, true, true }
    }},
    { 'F', new[,] {
        { true, true, true, true },
        { true, false, false, false },
        { true, true, true, false },
        { true, false, false, false },
        { true, false, false, false }
    }},
    { 'G', new[,] {
        { false, true, true, true },
        { true, false, false, false },
        { true, false, true, true },
        { true, false, false, true },
        { false, true, true, true }
    }},
    { 'H', new[,] {
        { true, false, false, false, true },
        { true, false, false, false, true },
        { true, true, true, true, true },
        { true, false, false, false, true },
        { true, false, false, false, true }
    }},
    { 'S', new[,] {
        { false, true, true, true },
        { true, false, false, false },
        { false, true, true, false },
        { false, false, false, true },
        { true, true, true, false }
    }},
    { 'T', new[,] {
        { true, true, true, true, true },
        { false, false, true, false, false },
        { false, false, true, false, false },
        { false, false, true, false, false },
        { false, false, true, false, false }
    }},
    { 'U', new[,] {
        { true, false, false, false, true },
        { true, false, false, false, true },
        { true, false, false, false, true },
        { true, false, false, false, true },
        { false, true, true, true, false }
    }},
    { 'O', new[,] {
        { false, true, true, false },
        { true, false, false, true },
        { true, false, false, true },
        { true, false, false, true },
        { false, true, true, false }
    }},
    { 'N', new[,] {
        { true, false, false, false, true },
        { true, true, false, false, true },
        { true, false, true, false, true },
        { true, false, false, true, true },
        { true, false, false, false, true }
    }},
    { 'P', new[,] {
        { true, true, true, false },
        { true, false, false, true },
        { true, true, true, false },
        { true, false, false, false },
        { true, false, false, false }
    }},
   { 'R', new[,] {
    { true, true, true, false },
    { true, false, false, true },
    { true, true, true, false },
    { true, false, true, false },
    { true, false, false, true }
}},
   { 'X', new[,] {
        { true, false, false, false, true },
        { false, true, false, true, false },
        { false, false, true, false, false },
        { false, true, false, true, false },
        { true, false, false, false, true }
    }},
    { 'I', new[,] {
        { true, true, true, true, true },
        { false, false, true, false, false },
        { false, false, true, false, false },
        { false, false, true, false, false },
        { true, true, true, true, true }
    }},
    { 'M', new[,] {
        { true, false, false, false, true },
        { true, true, false, true, true },
        { true, false, true, false, true },
        { true, false, false, false, true },
        { true, false, false, false, true }
    }},
    { 'V', new[,] {
    { true, false, false, false, true },
    { true, false, false, false, true },
    { true, false, false, false, true },
    { false, true, false, true, false },
    { false, false, true, false, false }
}},

{ 'L', new[,] {
    { true, false, false, false },
    { true, false, false, false },
    { true, false, false, false },
    { true, false, false, false },
    { true, true, true, true }
}},
{ 'K', new[,] {
    { true, false, false, true},
    { true, false, true, false},
    { true, true, false, false},
    { true, false, true, false},
    { true, false, false, true}
}},
{ 'Y', new[,] {
    { true, false, false, false, true },
    { false, true, false, true, false },
    { false, false, true, false, false },
    { false, false, true, false, false },
    { false, false, true, false, false }
}},
{ '↑', new[,] {
    { false, false, true, false, false },
    { false, true, true, true, false },
    { true, false, true, false, true },
    { false, false, true, false, false },
    { false, false, true, false, false }
}},
{ '↓', new[,] {
    { false, false, true, false, false },
    { false, false, true, false, false },
    { true, false, true, false, true },
    { false, true, true, true, false },
    { false, false, true, false, false }
}},
{ '⤴', new[,] {
    { false, false, true, false, false },
    { false, true, false, false, false },
    { true, true, true, true, false },
    { false, true, false, false, true },
    { false, false, true, false, true }
}},
{ '⤵', new[,] {
    { false, false, true, false, false },
    { false, false, false, true, false },
    { false, true, true, true, true },
    { true, false, false, true, false },
    { true, false, true, false, false }
}},
{ '✎', new[,] {
    { false, false, false, false, true },
    { false, false, true, true, true },
    { false, true, true, true, false },
    { false, true, true, false, false },
    { true, false, false, false, false }
}},
{ '♜', new[,] {
    { true, false, true, false, true },
    { true, true, false, true, true },
    { true, false, false, false, true },
    { true, false, false, false, true },
    { false, true, true, true, false }
}},
{ '✑', new[,] {
    { false, false, true, false, false },
    { true, true, true, true, true },
    { false, true, true, true, false },
    { false, true, true, true, false },
    { false, false, true, false, false }
}},
{ '_', new[,] {
    { false, false, false, false},
    { false, false, false, false},
    { true, true, true, true },
    { false, false, false, false },
    { false, false, false, false}
}},
{ '+', new[,] {
    { false, false, true, false, false },
    { false, false, true, false , false},
    { true, true, true, true, true },
    { false, false, true, false, false },
    { false, false, true, false, false }
}},
{ '-', new[,] {
    { false, false, false, false, false },
    { false, false, false, false, false },
    { true, true, true, true, true },
    { false, false, false, false, false },
    { false, false, false, false, false }
}},
{ ':', new[,] {
    { false, true, false,  },
    { false, false, false,  },
    { false, false, false, },
    { false, false, false, },
    { false, true, false,  }
}},
    { '0', new[,] {
        { false, true, true, false },
        { true, false, false, true },
        { true, false, false, true },
        { true, false, false, true },
        { false, true, true, false }
    }},
    { '1', new[,] {
        { false, true, false },
        { true, true, false },
        { false, true, false },
        { false, true, false },
        { true, true, true }
    }},
    { '2', new[,] {
        { true, true, true, false },
        { false, false, false, true },
        { false, true, true, false },
        { true, false, false, false },
        { true, true, true, true }
    }},
    { '3', new[,] {
        { true, true, true, false },
        { false, false, false, true },
        { false, true, true, false },
        { false, false, false, true },
        { true, true, true, false }
    }},
    { '4', new[,] {
        { false, false, true, false },
        { false, true, true, false },
        { true, false, true, false },
        { true, true, true, true },
        { false, false, true, false }
    }},
    { '5', new[,] {
        { true, true, true, true },
        { true, false, false, false },
        { true, true, true, false },
        { false, false, false, true },
        { true, true, true, false }
    }},
    { '6', new[,] {
        { false, true, true, false },
        { true, false, false, false },
        { true, true, true, false },
        { true, false, false, true },
        { false, true, true, false }
    }},
    { '7', new[,] {
        { true, true, true, true },
        { false, false, false, true },
        { false, false, true, false },
        { false, true, false, false },
        { true, false, false, false }
    }},
    { '8', new[,] {
        { false, true, true, false },
        { true, false, false, true },
        { false, true, true, false },
        { true, false, false, true },
        { false, true, true, false }
    }},
    { '9', new[,] {
        { false, true, true, false },
        { true, false, false, true },
        { false, true, true, true },
        { false, false, false, true },
        { false, true, true, false }
    }},
    { ' ', new[,] {
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false },
        { false, false, false, false }
    }}
            };
    }

    public void DrawText(PixelSpawner PixelManager, string text, int startX, int startY, Color color, int scale, 
                         HashSet<Pixel> hashSet, int z = - 1, GameObject colliderCard = null)
    {
        int x = startX;       
        int y = startY;

        foreach (char c in text.ToUpper())
        {
            if (_charPatterns.TryGetValue(c, out bool[,] pattern) && pattern != null)
            {
                int charHeight = pattern.GetLength(0);

                for (int i = 0; i < charHeight; i++)
                {
                    for (int j = 0; j < pattern.GetLength(1); j++)
                    {
                        if (pattern[i, j])
                        {
                            for (int dx = 0; dx < scale; dx++)
                            {
                                for (int dy = 0; dy < scale; dy++)
                                {                                   
                                    int invertedY = startY + (charHeight * scale) - (i * scale) - dy;
                                    Vector3 newPosition = new Vector3 (x + j * scale + dx, invertedY, z);                                   
                                    if (colliderCard == null && hashSet != null) 
                                        PixelManager.MovePixelToPosition(PixelManager.CreatePixel(PixelSpawner.random.Next(0, 200), PixelSpawner.random.Next(0, 120), color, hashSet, 0, colliderCard), newPosition);
                                    else PixelManager.CreatePixel(x + j * scale + dx, invertedY, color, hashSet, z, colliderCard);                                                                        
                                }
                            }
                        }
                    }
                }
                x += (pattern.GetLength(1) + 1) * scale;
            }
            else Debug.Log($"Pattern for '{c}' not found.");
        }
    }

}
