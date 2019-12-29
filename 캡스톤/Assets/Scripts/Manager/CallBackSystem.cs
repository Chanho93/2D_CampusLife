using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CallBackSystem : MonoBehaviour
{
    public delegate void OnMessageRecevied();
    public event OnMessageRecevied onComplete;
    void Start(){
        onComplete += WriteMesssage;

        onComplete();
    }

    void WriteMesssage(){
    Debug.Log("WriteMessage CallBack");
    }
}
