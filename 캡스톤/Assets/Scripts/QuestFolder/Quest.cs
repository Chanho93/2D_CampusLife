using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace HJ.Quest
{
    public enum QuestStatus
    {
        STATUS_HUMAN,        //? 휴먼 상태 -> 퀘스트 받을 수 없는 상태 (조건이 달성되지 않았거나 할 경우 나타내는 상태)
        STATUS_STANDBY,      //? 대기 상태 -> 퀘스트를 받을 수 있지만 아직 안 받은 상태
        STATUS_PROGRASS,   //? 진행 상태 -> 퀘스트를 진행중인 상태
        STATUS_COMPLETED,   //? 완료 상태 -> 퀘스트를 완료한 상태
        STATUS_FAILED           //? 퀘스트 실패 상태
    }
    public class Quest : MonoBehaviour
    {
        public IQuestUI _QuestInterface;


        #region 가상메서드
        /// <summary>
        /// 게임 시작할 때 한번 실행
        /// </summary>
        public virtual void Start()
        {

        }
        /// <summary>
        /// 매 프레임마다 반복
        /// </summary>
        public virtual void Update()
        {

        }

       

        #endregion
    }

    public interface IQuestUI
    {
        /// <summary>
        /// 퀘스트UI를 보여준다.
        /// </summary>
        void ShowQuest();   
        /// <summary>
        /// 퀘스트 UI를 숨긴다.
        /// </summary>
        void HideQuest();
        /// <summary>
        /// 퀘스트 현재 상태를 변경한다.
        /// </summary>
        /// <param name="status">퀘스트 상태 변경</param>
        /// <returns></returns>
        QuestStatus ChangeQuest(QuestStatus status);
    }
}

