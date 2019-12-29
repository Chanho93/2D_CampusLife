using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ObserverTest : MonoBehaviour
{
    //! Subscribe 메서드
    //? Subscribe(IObserber observer)   기본형
    //? Subscribe()                     모든 메세지를 무시한다.
    //? Subscribe(Action onNext)        onNext 만 처리
    //? Subscribe(Action onNext, Action onError)    OnNext 처리, 예외시 OnError 처리
    //? Subscribe(Action onNext, Action onCompleted) OnNext 처리, 완료시 onCompleted 처리
    //? Subscribe(Action onNext, Action onError, Action onCompleted)    3개 처리

    #region OnNext 관련
    //! OnNext
    //? 기본 사용에 있어서는 OnNext만 사용해도 큰 문제는 없다.
    void OnNextTest(){
        var intTest = new Subject<int>();

        intTest.Subscribe(x => Debug.Log(x));
        intTest.OnNext(2);
        intTest.OnNext(5);
    }
    #endregion
    #region OnError 관련
    //! OnError
    //? 예외처리 발생시 통지 가능, 만약 통지시 그 시점에서 바로 구독이 종료된다.
    void OnErrorTest(){
        var stringSubject = new Subject<string>();

        stringSubject
            .Select(str => int.Parse(str))  //? int 형이 아닌경우 예외 발생
            .Subscribe(
              x => Debug.Log("성공" + x),
              ex => Debug.Log("예외발생" + ex) 
            
            )
            ;  //? Subscribe(onNext, onError);
        
        //! 예외처리
        //? Retry --> OnError 발생 시 다시 Subscribe 한다.
        //? Catch --> OnError 발생 시 예외 처리 후 다른 스트림으로 대체한다.
        //? Catchlgnore --> OnError 발생 시 예외처리 후 OnError 무시하고 OnCompleted 대체
        //? OnErrorRetry --> OnError 발생 시 예외 처리 후 Subscribe 한다. (시간 지정 가능)

        stringSubject.OnNext("1");
        stringSubject.OnNext("3");
        stringSubject.OnNext("Hello");
        stringSubject.OnNext("5");  // 구독 취소가 되어 실행 안됨

    }
    #endregion
    #region OnCompleted 관련
    //! OnCompleted
    //? 스트림이 완료 되었으므로 이후 메세지 발생하지 않겠다 라는 사실을 통지하는 메세지이다.
    //? 구독 종료된 Subject는 재사용이 불가능 하며, Subscribe를 호출해도 OnCompleted가 발생한다.
    void OnCompletedTest(){
        var subject = new Subject<int>();

        subject.Subscribe(
            x => Debug.Log(x),
            () => Debug.Log("Completed")
        ); //? Subscribe(onNext, onCompleted)

        subject.OnNext(5);
        subject.OnNext(6);
        subject.OnCompleted();
        subject.OnNext(7);  //? 7은 안나온다.
    }
    #endregion
    #region Dispose
    //! Dispose
    //? Subscribe의 반환값인 IDisposable은 가비지 컬렉터가 관리하지 않는 리소스를 해제해 주기 위한 C# 기본 인터페이스이다.
    //? Subscribe가 완료 될 때 이 IDisposable을 반환한다는 것이 곧 IDisposable의 Dispose를 실행하면 스트림이 구독이 종료된다는 뜻이다.

    void DisposeTest(){
        var subject = new Subject<int>();
        var disposable = subject.Subscribe(x => Debug.Log(x), ()=>Debug.Log("OnCompleted"));
        
    }
    #endregion





    void Start(){
        
    }
}
