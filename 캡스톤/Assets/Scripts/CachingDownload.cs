using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CachingDownload : MonoBehaviour
{
    //? 번들 받을 주소
    public string _BundleURL;
    public int version; //? 번들 버전

    void Start(){
        StartCoroutine(DownloadAndCache());
    }

    IEnumerator DownloadAndCache(){
        //? Cache 폴더에 AssetBundle을 담을 것이므로 캐싱시스템 준비 될 때 까지 기다림
        while(!Caching.ready) 
            yield return null;

        //? 에셋번들이 캐시에 있으면 로드하고, 없으면 다운로드하여 캐시폴더에 저장한다.
        using (WWW www = WWW.LoadFromCacheOrDownload(_BundleURL,version)){
            yield return www;
            if(www.error != null)
            {
                throw new Exception("WWW 다운로드에 문제 생겼다. " + www.error);
            }
        }
    }
}
