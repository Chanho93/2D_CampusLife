using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneLoding : MonoBehaviour
{
    public Slider LodingSlider;

    private void Start()
    {
        StartCoroutine(Loding());
    }

    IEnumerator Loding()
    {
        AsyncOperation async;
        
        async = SceneManager.LoadSceneAsync("02.MainMap", LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return new WaitForEndOfFrame();
            LodingSlider.value = async.progress;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("02.MainMap"));
        
    }
}
