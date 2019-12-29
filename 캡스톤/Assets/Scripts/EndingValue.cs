using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingValue : MonoBehaviour
{
    private static EndingValue instance;
    public static EndingValue Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    public enum ENDINGTYPE
    {
        GRADE, SOCIAL, LOVE, MONEY
    }
    public ENDINGTYPE endingtype;
    public int Winner;

    public PlayerState playerState;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 엔딩 값 설정
    /// </summary>
    /// <param name="e"></param>
    public void SetEnding()
    {
        int study = (int)playerState.Study;
        int social = (int)playerState.Social;
        int love = (int)playerState.Love;
        

        int maxsolution = Mathf.Max(study,social);
        ENDINGTYPE type = (study >= social) ? ENDINGTYPE.GRADE : ENDINGTYPE.SOCIAL;
        int maxsolution2 = Mathf.Max(maxsolution, love);
        ENDINGTYPE type2 = (love >= maxsolution) ? ENDINGTYPE.LOVE : (type == ENDINGTYPE.GRADE) ? ENDINGTYPE.GRADE : ENDINGTYPE.SOCIAL;


        endingtype = type2;
        Winner = maxsolution2;

        study = 0;
        social = 0;
        love = 0;
        maxsolution = 0;
        maxsolution2 = 0;
        
    }

    public void MoneyEnding()
    {
        int money = playerState.AddMoney;
        endingtype = ENDINGTYPE.MONEY;
        Winner = money;

        money = 0;
    }

}
