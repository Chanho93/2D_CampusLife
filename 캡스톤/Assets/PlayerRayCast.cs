using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx.Triggers;
using UniRx;
public class PlayerRayCast : MonoBehaviour
{
    //? 이 스크립트는 플레이어에 광선을 쏘아서 무슨 물체가 있는지 식별해준다.

    Transform _Player;

    RaycastHit2D hit;
    float MaxDistance = 2f; //? Ray 길이

    private void Start()
    {
        _Player = GetComponent<Transform>();
        
        var clickStream = this.FixedUpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
    }

    
}
