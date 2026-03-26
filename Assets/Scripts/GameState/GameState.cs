using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daadab
{
    public enum GameState
    {
        StateLess = -1,
        None = 0,
        Gameplay = 1,
        Pause = 2,
        GameOver = 3,
        MainMenu = 4,
        Conversation = 5,
        MissionComplete = 6,
        Tutorial = 7,
    }
}
