using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.UI;
namespace HJ.STATE
{
    public enum STATE 
    {
        DAILOGUE, FIRSTMESSAGE
    }

    public enum NPCMoveRATE
    {
        NONE = 0,VERYSLOW = 1, SLOW = 2, NORMAL = 3, FAST = 4, VERYFAST = 5
    }

    public enum NPCMovePetton
    {
        UP, DOWN, LEFT, RIGHT, STOP
    }
}