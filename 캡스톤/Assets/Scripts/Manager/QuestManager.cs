using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.Quest;
using Sirenix.OdinInspector;
public class QuestManager : MonoBehaviour
{
    #region 싱글턴
    //? 싱글턴
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    public static Dictionary<int, QuestScriptable> QuestPrograss = new Dictionary<int, QuestScriptable>();
    public static Dictionary<int, QuestScriptable> QuestCompleted = new Dictionary<int, QuestScriptable>();
    public static Dictionary<int, QuestScriptable> QuestFailed = new Dictionary<int, QuestScriptable>();

    public enum QuestSelect { Prograss, Completed, Failed } //? 진행중, 완료, 실패

    #region 변수
    //? UI
    public GameObject ui_QuestBox;
    

    #endregion


    public QuestSelect GetQuestSelect { get; set; }
    /// <summary>
    /// 퀘스트를 찾는것을 목표로한다.
    /// </summary>
    /// <param name="n">NodeID</param>
    /// <param name="temp">퀘스트 상태</param>
    /// <returns></returns>
    public QuestScriptable SearchQuestion(int n, QuestSelect temp)
    {
        QuestScriptable t = null;
        switch(temp)
        {
            case QuestSelect.Prograss: //? 진행형일 경우
                if (QuestPrograss.ContainsKey(n))
                    t = QuestPrograss[n];
                break;
            case QuestSelect.Completed: //? 완료형일 경우
                if (QuestCompleted.ContainsKey(n))
                    t = QuestCompleted[n];
                break;
            case QuestSelect.Failed:
                if (QuestFailed.ContainsKey(n))
                    t = QuestFailed[n];
                break;
        }

        return t;
    }


}
