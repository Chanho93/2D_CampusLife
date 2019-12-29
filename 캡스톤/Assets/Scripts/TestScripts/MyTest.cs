using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
public class MyTest : MonoBehaviour
{
    public Text _MyText;


    void Start(){
        //! 매 프레임 마우스 클릭 이벤트를 관찰하는 스트림을 정의한다.
        var clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(300)))
        .Where(x => x.Count >= 2)   // 마우스 클릭 이벤트가 2회 이상 발생한 경우 필터링
        .SubscribeToText(_MyText, x =>
        string.Format("Double Click Count {0}",x.Count));
    }
}
