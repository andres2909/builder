using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int StudentNumber;
    public int Happiness;

    public int Social;

    public GameData(Game game)
    {
        StudentNumber = game.StudentNumber;
        Happiness = game.Happiness;

        Social = game.Social;
    }
}
