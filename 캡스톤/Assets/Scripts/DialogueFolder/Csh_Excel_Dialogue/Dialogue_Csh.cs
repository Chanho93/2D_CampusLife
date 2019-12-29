using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue_Csh 
{
    [Tooltip("캐릭터 명")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;

    public class DialogueEvent
    {
        public string name;

        public Vector2 line_csh;
        public Dialogue_Csh[] dialogue_csh
            ;
    }
}
