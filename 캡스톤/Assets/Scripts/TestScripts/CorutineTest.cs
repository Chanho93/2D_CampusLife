
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class CorutineTest : MonoBehaviour
{
    public bool IsPaused;

    void Start(){
        //Observable.FromCoroutineValuelong>(observer => CountCorutine(observer)) //IObserver<T> 를 인자로 넘겨준다 
        //.Subscribe(x => Debug.Log(x)) 
       // .AddTo(gameObject); 

    }

    IEnumerator CountCorutine(IObserver<long,long> observer)
    {
        long current = 0;
        float deltaTime = 0;

        while(true)
        {
            if(!IsPaused)
            {
                deltaTime = Time.deltaTime;
                if(deltaTime >= 1.0f)
                {
                    var intergerpart = (int)Mathf.Floor(deltaTime);
                    current += intergerpart;
                    deltaTime -= intergerpart;
                    Debug.Log("Count");
                    observer.OnNext(current);   // 코루틴 내에서 직접 발새애하여 Sub scribe에 통지
                }
            }
            yield return null;
        }
        
    }
}
