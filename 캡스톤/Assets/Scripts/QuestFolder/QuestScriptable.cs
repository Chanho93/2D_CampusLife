using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.Dialogue;
using Sirenix.OdinInspector;
namespace HJ.Quest
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest/New Quest")]
    [TypeInfoBox("Quest File")]
    public class QuestScriptable : ScriptableObject
    {
        [InfoBox("퀘스트 고유 번호로 이어갈 때 NodeID를 이용하여 이동한다. \n -1은 ID 설정 사용을 안하겠다는 뜻이다.")]
        [Title("Scriptable 고유번호")]

        public int NodeID = -1;

        [FoldoutGroup("Setting"), LabelText("퀘스트 제목")]
        public string _QuestName;


        [FoldoutGroup("Setting"), LabelText("퀘스트 내용"),Multiline]
        public string _QuestDescription;


        [FoldoutGroup("Setting"), LabelText("퀘스트 제목")]
        public bool[] is_QuestStagesituation;                     //? 전부 True 상태시 퀘스트 완료 할 수 있음.


        [FoldoutGroup("Completed"), LabelText("보상 내용"), Multiline]
        public string _QuestCompensation;                           //? 퀘스트 보상 내용


    }
}

