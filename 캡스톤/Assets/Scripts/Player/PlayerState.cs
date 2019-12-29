using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using HJ.Manager;
//? 플레이어의 스텟 설정
public class PlayerState : Singleton<PlayerState>
{
    #region 변수
    public string character_Name = "한돌이";
    private uint study_stat;     //? 공부력 
    private uint social_stat;    //? 인싸력
    private uint love_stat;      //? 사량
    private int money;              //? 돈


    private uint current_MapID;
    private int current_hp = 100;             //? 현재건강
    private int max_hp = 100;                  //? 최대 건강력

    private int sleepy = 0;                     //? 잠

    public Dictionary<string,int> status_arrayList = new Dictionary<string,int>();
    public Dictionary<string, Text> status_TextList = new Dictionary<string, Text>();

    [Title("UI Setting")]
    public GameObject ui_statusBox;         //? 스테이터스 부모 창
    public Text[] ui_stateText = new Text[7];                 //? 스테이스 능력 텍스트 상자


    //? UI 창
    public bool is_true = false;

    public enum STATUSENUM
    {
        NONE = 0,Study =1, Social, Love, Money, MapID, MaxHP, CurrHP
    }

    //public STATUSENUM statusenum;

    public static bool is_tutorial;         //? 튜토리얼을 끝냈는가?
    #endregion


    /// <summary>
    /// 눙력치
    /// </summary>
    /// <param name="t">요소</param>
    /// <param name="n">값</param>
    public void SettingStatus(STATUSENUM t, uint n)
    {
        switch(t)
        {
            case 0:
                break;
            case STATUSENUM.Study:
                Study = n;
                break;
            case STATUSENUM.Social:
                Social = n;
                break;
            case STATUSENUM.Love:
                Love = n;
                break;
            case STATUSENUM.Money:
                AddMoney = (int)n;
                break;
            case STATUSENUM.CurrHP:
                CurrentHpADD = (int)n;
                break;

        }
    }


    #region 프로퍼티
    public uint Study
    {
        get { return study_stat; }
        set { study_stat += value; }
    }
    public uint Social
    {
        get { return social_stat; }
        set { social_stat += value; }
    }
    public uint Love
    {
        get { return love_stat; }
        set { love_stat += value;}
    }
    public int Sleep
    {
        get { return sleepy; }
        set { sleepy = value; }
    }
    public bool MoneyCheck
    {
        get
        {
            bool isdead = (money < 0);

            return isdead;
        }
    }

    /// <summary>
    /// 돈 증가
    /// </summary>
    public int AddMoney
    {
        get { return money; }
        set { money += value; }
    }

    /// <summary>
    /// 돈 감소
    /// </summary>
    public int SubMoney
    {
        get { return money; }
        set { money -= value; }
    }

    public uint MapID
    {
        get { return current_MapID; }
        set { current_MapID = value; }
    }
    public int MaxHP
    {
        get { return max_hp; }
        set { max_hp = value; }
    }
    public int CurrentHpADD
    {
        get { return current_hp; }
        set
        {
            int n = current_hp + value;
            if( n > MaxHP)  //? 최대체력이 높으면?
            {
                current_hp = MaxHP; //? 최대 채력만큼 회복
            }
            else
            {
                current_hp = n; //? 아니면 증가
            }
        }
    }
    public int CurrentHPSub
    {
        get { return current_hp; }
        set
        {
            int n = current_hp - value;

            if(n > 0)
            {
                current_hp = n;
            }
            else
            {
                current_hp = 0;
                GameManager.isDead = true;      //? 죽음 확정
            }

        }
    }
    #endregion

    private void Update()
    {
        if (is_true)
        {
            ui_stateText[0].text = string.Format("{0}", Study);
            ui_stateText[1].text = string.Format("{0}", Social);
            ui_stateText[2].text = string.Format("{0}", Love);
            ui_stateText[3].text = string.Format("{0}", money);
            ui_stateText[4].text = string.Format("{0}", current_hp);
            ui_stateText[5].text = string.Format("{0}", TimeManager.Instance.GetDay);
            ui_stateText[6].text = string.Format("{0}", TimeManager.Instance.Time);
        }
        
    }

    public void MenuButtonClick()
    {
        if (HJ.Manager.DialogueManager.is_Msg) return;
        is_true = !is_true;
        if (is_true) ui_statusBox.SetActive(true);
        else ui_statusBox.SetActive(false);

       
    }

    void InitGame()
    {
        status_arrayList.Clear();

        status_arrayList.Add("Study", (int)Study);
        status_arrayList.Add("Social", (int)Social);
        status_arrayList.Add("Love", (int)Love);
        if(money >= 0)
            status_arrayList.Add("Money", (int)money);
        else
            status_arrayList.Add("Money", 0);
        status_arrayList.Add("MapID", (int)current_MapID);
        status_arrayList.Add("MaxHP", (int)max_hp);
        status_arrayList.Add("CurrHP", (int)current_hp);


        //statusenum = STATUSENUM.NONE;
    }
    /// <summary>
    /// 스테이터스 능력치 추가
    /// </summary>
    /// <param name="status"></param>
    /// <param name="n"></param>
    public void ADDStatusList(STATUSENUM status, int n)
    {
        status_arrayList[status.ToString()] += n;
    }

    

}
