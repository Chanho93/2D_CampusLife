using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
namespace HJ.Manager
{
    #region 유틸리티
    public enum PlayerENUMSTATUS { STUDY, SOCIETY, LOVE }

    public interface IStatus
    {
        /// <summary>
        /// 돈을 추가한다.
        /// </summary>
        /// <param name="m">추가할 금액</param>
        /// <returns></returns>
        int AddMoney(int m);    
        /// <summary>
        /// 돈을 뺀다.
        /// </summary>
        /// <param name="m">지불할 돈</param>
        /// <returns></returns>
        int SubMoney(int m);
        /// <summary>
        /// 플레이어 능력치 증가
        /// </summary>
        /// <param name="status">올릴 능력치</param>
        /// <param name="addnumber">올라갈 양</param>
        void AddStatus(PlayerENUMSTATUS status, int addnumber);
        /// <summary>
        /// 플레이어 능력치 감소
        /// </summary>
        /// <param name="status">감소할 능력치</param>
        /// <param name="subnumber">감소할 량</param>
        void SubStatus(PlayerENUMSTATUS status, int subnumber);
    }
    #endregion

    #region 개인정보
    public delegate int MyMoneyDelegate(int n);
    public delegate void MyStatusDelegate(PlayerENUMSTATUS type, int n);

    [Serializable]
    public class PlayerInformation : IStatus
    {
        public PlayerInformation(string name, int age, string major, int money,int study, int society, int love, int hp)
        {
            this._name = name;
            this._age = age;
            this._major = major;
            _money = money;
            _study = study;
            _society = society;
            _love = love;
            HP = hp;
        }
        [Title("Player Information")]
        [LabelText("플레이어 이름")]
        public string _name;

        [LabelText("플레이어 학년")]
        public int _age;

        [LabelText("플레이어 학과")]
        public string _major;

        [LabelText("플레이어 건강")]
        public int _hp;

        [LabelText("플레이어 자금")]
        public int _money;

        [LabelText("플레이어 지식")]
        public int _study;

        [LabelText("플레이어 인싸력")]
        public int _society;

        [LabelText("플레이어 연애력")]
        public int _love;

        [LabelText("플레이어 학점")]
        public float _avgGrades;

        //? _hp 프로퍼티
        public int HP
        {
            get
            {
                if(_hp <= 0)
                {
                    return 0;
                }
                else
                {
                    return _hp;
                }
            }
            set
            {
                _hp = value;
            }
        }

        #region 인터페이스 설정
        public int AddMoney(int m)
        {
            int sum = _money + m;
            if (sum >= 9999999)
            {
                sum = 9999999;
            }
            else
            {
                return sum;
            }

            return sum;
        }

        public int SubMoney(int m)
        {
            int sub = _money - m;
            if (sub <= 0)
            {
                sub = 0;
            }
            else
            {
                return sub ;
            }

            return sub;
        }

        public void AddStatus(PlayerENUMSTATUS status, int addnumber)
        {
            switch(status)
            {
                case PlayerENUMSTATUS.STUDY:
                    _study += addnumber;
                    break;
                case PlayerENUMSTATUS.SOCIETY:
                    _society += addnumber;
                    break;
                case PlayerENUMSTATUS.LOVE:
                    _love += addnumber;
                    break;
            }
        }

        public void SubStatus(PlayerENUMSTATUS status, int subnumber)
        {
            switch (status)
            {
                case PlayerENUMSTATUS.STUDY:
                    _study -= subnumber;
                    break;
                case PlayerENUMSTATUS.SOCIETY:
                    _society -= subnumber;
                    break;
                case PlayerENUMSTATUS.LOVE:
                    _love -= subnumber;
                    break;
            }
        }
        #endregion
    }

    #endregion

    public class PlayerManager : Singleton<PlayerManager>
    {
        // 플레이어 정보 시스템
        [SerializeField] PlayerInformation playerInformation;

        //? 이어하기 버튼을 누를 경우 True, 새로하기는 false
        public static bool is_Load = false;
        // 플레이어 인터페이스 시스템
        public IStatus status;

        //################################################
        //!? 이벤트 기능
        public event MyMoneyDelegate OnADDMoney;       //? 돈 증가 이벤트
        public event MyStatusDelegate OnADDStatus;     //? 능력치 증가 이벤트
        public event MyMoneyDelegate OnSubMoney;        //? 돈 감소 이벤트
        public event MyStatusDelegate OnSubStatus;      //? 능력치 감소 이벤트
        //################################################
        
       
        private void Start()
        {
            if(!is_Load )
            {
                //? 새로하기
                NewPlayer();
            }
            else
            {
                //? 불러오기
                LoadPlayer("Load", 1, "fsf", 3, 4, 5, 6,5);
            }
            
            
        }

        void LoadStatus()
        {
            //if(OnADDMoney.Method != null) OnADDMoney -= status.AddMoney;
            //if(OnADDStatus.Method != null) OnADDStatus -= status.AddStatus;
            //if(OnSubMoney.Method != null) OnSubMoney -= status.SubMoney;
            //if(OnSubStatus.Method != null) OnSubStatus -= status.SubStatus;




            status = playerInformation;

            OnADDMoney += status.AddMoney;
            OnSubMoney += status.SubMoney;
            OnADDStatus += status.AddStatus;
            OnSubStatus += status.SubStatus;

            
        }
        #region 플레이어 불러오기
        /// <summary>
        /// 새로운 캐릭터 생성
        /// </summary>
        public void NewPlayer()
        {
            playerInformation = new PlayerInformation("한림이", 20, "융합소프트웨어학과",1000, 5, 5, 5,100);
            LoadStatus();
        }

        /// <summary>
        /// 플레이어를 불러온다.
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="age">나이</param>
        /// <param name="major">전공</param>
        /// <param name="money">돈</param>
        /// <param name="study">지식</param>
        /// <param name="society">인싸력</param>
        /// <param name="love">사랑의 매</param>
        public void LoadPlayer(string name, int age, string major, int money, int study, int society, int love, int hp)
        {
            playerInformation = new PlayerInformation(name, age, major, money, study, society, love,hp);
            LoadStatus();
        }

        #endregion

        #region 버튼용 이벤트
        //? 돈 증가 메서드
        public void AddMoneyMethod(int n) =>
            OnADDMoney(n);
        //? 돈 감소 메서드
        public void SubMoneyMethod(int n) =>
            OnSubMoney(n);
        //? 지식 증가 메서드
        public void AddStudyStatusMethod(int n) =>
            OnADDStatus(PlayerENUMSTATUS.STUDY, n);
        //? 지식 감소 메서드
        public void SubStudyStatusMethod(int n) =>
            OnSubStatus(PlayerENUMSTATUS.STUDY, n);
        //? 인싸력 증가 메서드
        public void AddSocietyStatusMethod(int n) =>
            OnADDStatus(PlayerENUMSTATUS.SOCIETY, n);
        //? 인싸력 감소 메서드
        public void SubSocietyStatusMethod(int n) => 
            OnSubStatus(PlayerENUMSTATUS.SOCIETY, n);
        //? 연애력 증가 메서드
        public void AddLoveStatusMethod(int n) =>
            OnADDStatus(PlayerENUMSTATUS.LOVE, n);
        //? 연애력 감소 메서드
        public void SubLoveStatusMethod(int n) =>
            OnSubStatus(PlayerENUMSTATUS.LOVE, n);
        #endregion


    }
}

