using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ITEM_csh 
{
    public string item_name;
    //아이템 명:

    public PlayerState.STATUSENUM stat;
    //아이템에 의해 증가할 스탯

    public uint value;

    //생성자

    public ITEM_csh(string name = "", PlayerState.STATUSENUM stat = 0, uint value = 0)
    {
        this.item_name = name;
        this.stat = stat;
        this.value = value;
    }
    
    
   
}
