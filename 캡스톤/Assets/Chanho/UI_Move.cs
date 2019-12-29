using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UI_Move : MonoBehaviour
{
    public GameObject[] UII;
    
    // Start is called before the first frame update
    void Start()
    {
        SoundManager._instance.Play_BGM("BGM1",false);
        Invoke("UI_Moving", 0.5f);
        Invoke("UI_SA", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UI_Moving()
    {
        UII[0].transform.DOMove(new Vector3(0.1514541f, -0.08536935f, 0f), 2.0f).SetEase(Ease.Linear);
        UII[1].transform.DOMove(new Vector3(0.08899997f, 3.355f, 0f), 2.0f).SetEase(Ease.Linear);
       
       // UII[2].transform.DOMove(new Vector3(20f, 218f, 0f), 2.0f).SetEase(Ease.Linear);
       // UII[3].transform.DOMove(new Vector3(-0.02000003f, 0.4099998f, 0f), 2.0f).SetEase(Ease.Linear);
       // UII[4].transform.DOMove(new Vector3(0f, -1.01537f, 0f), 2.0f).SetEase(Ease.Linear);
       // UII[5].transform.DOMove(new Vector3(0f, -2.32f, 0f), 2.0f).SetEase(Ease.Linear);
    }

    public void UI_SA()
    {
        UII[2].SetActive(true);
        UII[3].SetActive(true);
        UII[4].SetActive(true);
        UII[5].SetActive(true);

    }

    public void ButtonClick_Play()
    {
        StartCoroutine(SoundManager._instance.Play_SE("Click"));
        SceneManager.LoadScene("01.Loding", LoadSceneMode.Single);

        SoundManager._instance.StopBGM();
    }
}
