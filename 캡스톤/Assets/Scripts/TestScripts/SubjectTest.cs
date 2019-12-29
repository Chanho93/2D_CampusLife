using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;
//? Subject<T> 형태로 원하는 자료형을 Subject 타입의 인스턴스로 생성하면 UniRx 의 스트림을 사용할 수 있다.

public class SubjectTest : MonoBehaviour
{
    void Start(){
        Subject<string> _subject = new Subject<string>();   // Subject 타입 string 객체 생성

        // 메세지 발생시 3회 출력
        //_subject.Subscribe(msg => Debug.Log("Subscribe1 " + msg));
        //_subject.Subscribe(msg => Debug.Log("Subscribe2 " + msg));
        //_subject.Subscribe(msg => Debug.Log("Subscribe3 " + msg));
        

        // 이벤트 메세지 전달
        //_subject.OnNext("Frist");
        //_subject.OnNext("Seconde");

        //? Subject에 OnNext로 메세지 전달하고 그 메세지 수령하면 Subscribe에서 지정한 함수를 수행한다.
        //? Subject는 event와 상위호환이다.


        //? Observer (관찰자) & Observable (관찰 가능한)
        //? IObserver 인터페이스와 IObservable 인터페이스의 메소드를 구현하고 있는 Subject

        // IObserver (OnNext, OnError, OnCompleted) ==> Subject ==> IObservable (Subscribe)

        //? Observable 객체 생성 방법 (관찰가능한)
        //? 주로 ReactiveProperty, UniRx.Triggers, UGUI 이벤트에서 생성하는 방법이다.

        //? Subject 시리즈로 생성하는 방법 [Observable]
        //! Subject<T> --> 가장 단순한 형태이며 자주사용하고, OnNext로 이벤트 전달한다.
        //! BehaviorSubject<T> --> 마지막으로 전달한 이벤트를 캐싱하고 이후에 Subscribe 될 때 그 값을 전달한다.
        //! ReplaySubject<T> --> 과거에 전달한 모든 이벤트를 캐싱하고 이후에 Subscribe 될 때 그 값을 모두 모와서 전달한다.
        //! AsyncSubject<T> --> OnNext 사용시 즉시 전달하지 않고 내부에 캐싱한 후 OnCompleted 가 실행되면 마지막 OnNext를 전달한다.

        //? ReactiveProperty 시리즈로 생성하는 방법 [Observable]
        var rp = new ReactiveProperty<int>(10);

        //NOTE .Value을 사용하면 일반 변수처럼 대입하거나 값을 읽을 수 있다
        rp.Value = 5;       // 값을 수정하면 즉시 OnNext가 발생하여 Subscribe 로 통보
        var currentValue = rp.Value;   

        //rp.Subscribe(x => Debug.Log(x));

        //rp.Value = 10;  // 값 수정시 즉시 Subscribe로 이동

        //? 결과는 5  10

        //? UniRX.Trigger 을 사용하여 생성하는 방법 [Observable]
        // using UniRx.Trigger 있어야 하며
        //! void Update() 대용으로 주로 쓰게 될 함수다.
        //this.UpdateAsObservable().Subscribe(_ => Debug.Log("Update"));

        //? 코루틴으로 사용하여 생성하는 방법 [Observable]
        //Coroutine 에서 IObservable 로 변환하려면 Observable.FromCoroutine 을 이용한다.

        //Observable.FromCoroutine<int>(obserber => GameTimer(obserber, 60)).Subscribe(t => Debug.Log(t));
        
        //? UGUI 사용하여 이벤트 변환사용[Observable]
        //var button = GetComponent<Button>();
        //button.OnClickAsObservable().Subscribe();       //Note Button::OnClick 에 해당하는 Observable 생성함수

        //var inputField = GetComponent<InputField>();
        //inputField.OnValueChangedAsObservable().Subscribe();    //Note Inputfiled::OnValueChange 해당하는 옵시디블 생성함수
        //inputField.OnEndEditAsObservable().Subscribe();         //Note Inputfiled::OnEndedit()에 해당하는 옵시디블 생성함수

        //var slider = GetComponent<Slider>();
        //slider.OnValueChangedAsObservable().Subscribe(); 


        //? 그외
        //! ObservableWWW -> WWW 스트림을 취급할수 있도록 만든 것으로 코루틴 사용하지 않고 비동기 다운로드 처리를 할 수 있어 매우 유용한기능
        //! 하지만 WWW가 Deprecate 되면서 작동은 하지만 언제 사라질지 모르므로 쓰지 않는게 좋다.

        //! Observable.NextFrame -> 다음 프레임에 메세지를 전달하는 Observable을 생성한다.
        Observable.NextFrame().Subscribe(_=>Debug.Log("NextFrame"));
    }

        IEnumerator GameTimer(IObserver<int> observer, int initialCount)
        {
            var count = 100;
            while(count > 0)
            {
                observer.OnNext(count--);
                yield return new WaitForSeconds(1);
            }

            observer.OnNext(0);
            observer.OnCompleted();
        }
}
