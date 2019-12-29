using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
public class OperatorTest : MonoBehaviour
{
   //! UniRx에서 Observable을 통해 관찰 대상이 결정되면 스트림 값들을 흘려보내게 된다.
   //! Observable ==> Stream [Operator 데이터 필터링, 스킵, 변환 등] ==> Subscrobe 구독자
   //! 데이터를 조작하게 해주는 명력어를 Operator라 한다.

    //! Where 사용하기
    //? 흐르는 이벤트 들 중 필요없는 이벤트 걸러내는 필터링 오퍼레이터이다. if 보다 훨씬 더 간결한 가독성 코드를 만들 수 있다.
   void WhereStart(){
       Observable.EveryUpdate()
       .Where (_ => Input.GetMouseButton(0))
       .Subscribe(_ => Debug.Log("Click"));
   }

   //! Select 사용하기
   //? 스트림을 변경하기 위해 사용한다. int타입 데이터 흘려보내는 스트림에서 select 구문을 사용하여 다른 타입의 데이터를 전달하게 된다.
   void SelectStart(){
       Observable.EveryUpdate()
       .Where (_ =>Input.GetMouseButtonDown(0))
       .Select(_ => Input.mousePosition)    // 이 스트림은 Select 이후 Vector3 타입의 mousePosition 통지
       .Subscribe(x => Debug.Log(x));       // Subscribe에 들어가는 메세지는 Vector3
   }

   //! SelectMany 사용하기
   //? Select가 기존 스트림 값을 바꾸는 처리를 수행하는 비해 SelectMany는 새로운 스트림을 생성하여 본래 스트림을 대체한다.
   void SelectManyStart(){
       Button btn = GetComponent<Button>();
       
       btn.OnPointerDownAsObservable()
       .SelectMany(_ => btn.UpdateAsObservable()) // 매 프레임 관찰하는 스트림을 새로 생성하여 대체
       .TakeUntil(btn.OnPointerUpAsObservable())    // 매 프레임 관찰하는 스트림 중에 button PointerUp이 오면 중단
       .Subscribe(_ => {
           Debug.Log("Pressing...");
       });
   }

   //! 펙토리 메소드
    void FactoryMethodStart(){
        Subject<int> N = null;
        //Observable.Create<int>(observer => (a){
            
        //});     // 직접 Observable 객체를 생성
        Observable.Return("Return T Value");       // 1개의 메세지만 전달한다.
        Observable.Repeat("Loop");                 // 메세지의 전달을 반복한다.
        Observable.Range(2,4);                    // 지정된 수치의 메세지를 전달한다.
        Observable.Timer(TimeSpan.FromMilliseconds(10));                     // 실행후 몇 초 이후로 발생
        Observable.Interval(TimeSpan.FromMilliseconds(400));                // 실행 후 몇 초 마다 발생
        Observable.Empty<int>();                                            // OnCompleted 즉시 전달
        // Observable.FromEvent(Func Delegate)                              // 유니티 이벤트를 Observable로 변환


    }

    //! 메서드 필터
    void MethodFilterStart(){
        //Observable.EveryUpdate()
        //?.Where 조건이 True 인 것만 통과 시킴
        //? .Distinct 중복된 메세지들은 제거한다.
        //?.DistinctUntilChanged 값이 변화했을 경우 통과 
        //?.Throttle 지정 간격 내에 들어온 메세지 중 마지막 메세지만 통과
        //?.First 가장 먼저 발생한 메세지를 전달하고 Observable 완료시킨다.
        //?.Take 지정된 개수 만큼 메세지 전달
        //?.TakeUntil 인자로 지정한 메세지가 올 때 계속 전달
        //?.TakeWhile 조건이 True인 동안 메세지 전달
        //?.Skip 지정된 갯수 만큼 메세지 스킵
        //?.SkipUtil 인자로 지정한 스트림에 지정한 메세지가 올 때 까지 계속 스킵
        //?.SkipWhile 조건이 true 인 동안 계속 메세지 스킵
    }
    
    
    //! 코루틴 변환
    void Start(){
        Observable.FromCoroutine(MyCorutine)
        .Subscribe(_ => Debug.Log("OnNext"),() =>Debug.Log("Completed")).AddTo(gameObject); // 이 클래스가 Destory 되면 구독 중단 되도록 AddTo로 수명관리


        Observable.FromCoroutineValue<int>(MyCorutineReturn)
        .Subscribe(x => Debug.Log(x))
        .AddTo(gameObject);
    }

    IEnumerator MyCorutine(){
        Debug.Log("START CORUTINE");
        yield return null;
        Debug.Log("End CORUTINE");
    }

    private List<int> intList;

    IEnumerator MyCorutineReturn(){
        foreach(var a in intList)
            yield return a;
    }
}
