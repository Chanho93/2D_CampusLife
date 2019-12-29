using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using HJ.Manager;
namespace HJ.Manager
{
    public enum TIMEENUM
    {
        MORNING,          //? 아침
        LUNCH,              //? 점심
        NIGHT,              //? 저녘
        DAWN                //? 새벽
    }
    public enum WEATHERENUM
    {
        SUN,
        RAIN,
        SNOW
    }
    public interface ITime
    {
        int Day { get; set; }
        int Time { get; set; }
        TIMEENUM TIMESET { get; set; }
        WEATHERENUM WEATHERSET { get; set; }
        void SetDay(int n);
        
    }
    public class TimeManager : MonoBehaviour, ITime
    {
        // 요일
        private int _day;

        // 열거형 타입의 오늘의 시간
        [SerializeField, EnumPaging]
        private TIMEENUM _timenum;

        [SerializeField, EnumPaging]
        private WEATHERENUM _weathernum;
        // 시간초
        private int _time;

        // 인터페이스
        public ITime Itime;

        // 날씨 페이드
        [LabelText("날씨 페이드")]
        public Image _WeatherFade;

        [Title("효과음")]
        [LabelText("아침")]
        public AudioClip _sunclip;
        [LabelText("점심")]
        public AudioClip _lunchclip;
        [LabelText("저녘")]
        public AudioClip _nightclip;

        [LabelText(" 비 ")]
        public GameObject _Rain;

        [LabelText(" 눈 ")]
        public GameObject _Snow;

        [LabelText("사운드 소스")]
        public AudioSource _SoundSource;

        public PlayerState playerState;
        public DialogueManager dialogueManager;

        #region 싱글턴
        // 인스턴스
        private static TimeManager instance;

        public static TimeManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);

            Itime = this;

            DontDestroyOnLoad(gameObject);
        }
        #endregion




        #region 인터페이스 구현
        int ITime.Day { get => _day; set => _day = value; }
        public TIMEENUM TIMESET { get => _timenum; set =>_timenum = value; }
        public int Time { get => _time; set => _time = value; }
        public WEATHERENUM WEATHERSET { get =>_weathernum; set => _weathernum = value; }


        public int GetDay
        {
            get { return Itime.Day; }
        }

        /// <summary>
        /// 시간에 따른 하늘 상태 변경
        /// </summary>
        /// <param name="n"></param>
        public void SetDay(int n)
        {
             if(n >= 0 && n < 6)
            {
                TIMESET = TIMEENUM.MORNING;
                //StartCoroutine(dialogueManager.AlermShow("아침 !!"));
            }
            else if(n >= 6 && n < 12)
            {
                TIMESET = TIMEENUM.LUNCH;
                //StartCoroutine(dialogueManager.AlermShow("점심 !!"));
            }
            else if(n >= 12 && n < 18)
            {
                TIMESET = TIMEENUM.NIGHT;
                //StartCoroutine(dialogueManager.AlermShow("저녘 !!"));
            }
            else if(n >= 18 && n < 24)
            {
                TIMESET = TIMEENUM.DAWN;
                //StartCoroutine(dialogueManager.AlermShow("세벽 !!"));
            }

             
        }
        #endregion

        #region 메서드
        /// <summary>
        /// 다음날 시작
        /// </summary>
        private void NextDay()
        {
            Itime.Time = 0;
            ++Itime.Day;
            StartCoroutine(dialogueManager.AlermShow(string.Format("{0} Day",Itime.Day)));
            int rand = UnityEngine.Random.Range(1, 34);

            if (rand <= 25)
            {
                WEATHERSET = WEATHERENUM.SUN;
            }
            else
            {
                WEATHERSET = WEATHERENUM.RAIN;
            }

            if(playerState.Sleep == 0)
            {
                StartCoroutine(dialogueManager.AlermShow("잠을 안잤습니다.. HP 40감소"));
                playerState.Sleep = 0;
                playerState.CurrentHPSub = 40;
            }

            //? 수금 타임
            if( Itime.Day >= 30 && Itime.Day % 30 == 0)
            {
                StartCoroutine( dialogueManager.AlermShow("수금시간입니다. 30만 감소"));

                playerState.SubMoney = 300000;

                GameManager.isDead = playerState.MoneyCheck;


            }
            else
            {
                int pay = (int)(UnityEngine.Random.Range(0.5f, 2.0f) * 10000);
                StartCoroutine(dialogueManager.AlermShow(string.Format("하루 지출 : {0} 감소",pay)));

                playerState.SubMoney = pay;

                GameManager.isDead = playerState.MoneyCheck;
            }

            
        }
        
        /// <summary>
        /// 시간 확인
        /// </summary>
        public void CheckDay()
        {
            // 24 숫자가 되면 하루 지남
            if(Itime.Time >= 24)
            {
                NextDay();
            }

            // 상태변경
            SetDay(Itime.Time);
        }

        /// <summary>
        /// 시간 증가
        /// </summary>
        /// <param name="t">시간</param>
        public void AddTime(int t)
        {
            Itime.Time += t;

            CheckDay();
        }
        //? tlwkr
        public void WeatherCorutine(float t)
        {
            StartCoroutine(WeatherFade(t));
        }

        /// <summary>
        /// 날씨 조절
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IEnumerator WeatherFade(float t)
        {
            WaitForSeconds wait = new WaitForSeconds(t);

            CheckDay();

            //? 아침
            if(TIMESET == TIMEENUM.MORNING && WEATHERSET == WEATHERENUM.SUN)
            {
                //? 날씨가 아침이고 태양이 뜨면
                _WeatherFade.DOFade(0.15f, t);
                SoundSet(_sunclip);
                _Rain.SetActive(false);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }else if(TIMESET == TIMEENUM.MORNING && WEATHERSET == WEATHERENUM.RAIN)
            {
                //? 날씨가 아침이고 비가 오면
                _WeatherFade.DOFade(0.55f, t);
                _Rain.SetActive(true);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.MORNING && WEATHERSET == WEATHERENUM.SNOW)
            {
                //? 날씨가 아침이고 눈이 오면
                _WeatherFade.DOFade(0.45f, t);
                _Rain.SetActive(false);
                _Snow.SetActive(true);
                yield return new WaitForSeconds(t + 0.9f);
            }
            //? 점심
            else if (TIMESET == TIMEENUM.LUNCH && WEATHERSET == WEATHERENUM.SUN)
            {
                //? 날씨가 점심이고 해가 쨍쨍하면
                _WeatherFade.DOFade(0f, t);
                SoundSet(_lunchclip);
                _Rain.SetActive(false);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.LUNCH && WEATHERSET == WEATHERENUM.RAIN)
            {
                //? 날씨가 점심이고 비오면
                _WeatherFade.DOFade(0.35f, t);
                _Rain.SetActive(true);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.LUNCH && WEATHERSET == WEATHERENUM.SNOW)
            {
                //? 시간이 점심이고 눈이 오면
                _WeatherFade.DOFade(0.35f, t);
                _Rain.SetActive(false);
                _Snow.SetActive(true);
                yield return new WaitForSeconds(t + 0.9f);
            }
            //? 저녘
            else if (TIMESET == TIMEENUM.NIGHT && WEATHERSET == WEATHERENUM.SUN)
            {
                //? 시간이 저녘이고 해가 쨍쨍하면
                _WeatherFade.DOFade(0.5f, t);
                SoundSet(_nightclip);
                _Rain.SetActive(false);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.NIGHT && WEATHERSET == WEATHERENUM.RAIN)
            {
                //? 시간이 저녘이고 비오면
                _WeatherFade.DOFade(0.6f, t);
                _Rain.SetActive(true);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.NIGHT && WEATHERSET == WEATHERENUM.SNOW)
            {
                //? 시간이 저녘이고 눈오면
                _WeatherFade.DOFade(0.5f, t);
                _Rain.SetActive(false);
                _Snow.SetActive(true);
                yield return new WaitForSeconds(t + 0.9f);
            }
            //? 세벽
            else if (TIMESET == TIMEENUM.DAWN && WEATHERSET == WEATHERENUM.SUN)
            {
                //? 시간이 세벽이고 해가 쨍쨍하면
                _WeatherFade.DOFade(0.67f, t);
                SoundSet(_nightclip);
                _Rain.SetActive(false);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.DAWN && WEATHERSET == WEATHERENUM.RAIN)
            {
                //? 시간이 세벽이고 비오면
                _WeatherFade.DOFade(0.75f, t);
                _Rain.SetActive(true);
                _Snow.SetActive(false);
                yield return new WaitForSeconds(t + 0.9f);
            }
            else if (TIMESET == TIMEENUM.DAWN && WEATHERSET == WEATHERENUM.SNOW)
            {
                //? 시간이 세벽이고 눈오면
                _WeatherFade.DOFade(0.6f, t);
                _Rain.SetActive(false);
                _Snow.SetActive(true);
                yield return new WaitForSeconds(t + 0.9f);
            }

        }

        void SoundSet(AudioClip clip)
        {
            if (clip == null) return;
            if(_SoundSource == null)
            {
                _SoundSource = gameObject.AddComponent<AudioSource>();
                _SoundSource.playOnAwake = false;
            }
            _SoundSource.Stop();
            _SoundSource.loop = false;
            _SoundSource.clip = clip;
            _SoundSource.Play();
        }

        void LoopSoundSet(AudioClip clip)
        {
            if (clip == null) return;
            if(_SoundSource == null)
            {
                _SoundSource = gameObject.AddComponent<AudioSource>();
                _SoundSource.playOnAwake = false;
            }

            _SoundSource.Stop();
            _SoundSource.loop = true;
            _SoundSource.clip = clip;
            _SoundSource.Play();
        }
        #endregion
    }
}

