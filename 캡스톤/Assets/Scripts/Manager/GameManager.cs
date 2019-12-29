using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace HJ.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public Transform _Player;                   //? 플레이어
        public Transform _MainCamera;           //? 메인카메라

        public Image _FadeOutImage;        //? FadeOut


        public int episodenumber;              //? 에피소드 번호

        public PlayerState playerState;
        public TimeManager timeManager;
        public EndingValue endingValue;
        //? 캐릭터가 죽었는가?
        public static bool isDead = false;



        /// <summary>
        /// 페이드 아웃
        /// </summary>
        /// <param name="t">시간</param>
        public void Fade(float t)
        {
            StartCoroutine(FadeOutCorutine(t));
        }


        private void Update()
        {
            if (isDead)
            {
                SceneManager.LoadScene("GameOver");
                isDead = false;
            }
            if(playerState.MoneyCheck)
            {
                SceneManager.LoadScene("GameOver");
                isDead = false;
            }
            if(timeManager.GetDay >= 40)
            {
                endingValue.SetEnding();
                SceneManager.LoadScene("Ending");
            }
            if(playerState.AddMoney >= 2000000)
            {
                endingValue.MoneyEnding();
                SceneManager.LoadScene("Ending");
            }
            
        }


        private IEnumerator FadeOutCorutine(float t)
        {
            if (_FadeOutImage == null) { yield break; }
            _FadeOutImage.DOFade(1f, t);

            yield return new WaitForSeconds(t);

            yield return StartCoroutine(FadeInCorutine(t));
        }
        private IEnumerator FadeInCorutine(float t)
        {
            if (_FadeOutImage == null) { yield break; }
            _FadeOutImage.DOFade(0f, t);

            yield return null;
        }

        /// <summary>
        /// 에피소드 번호
        /// </summary>
        public int EpisodeNumber
        {
            get { return episodenumber; }
            set { episodenumber = value; }
        }
    }

}
