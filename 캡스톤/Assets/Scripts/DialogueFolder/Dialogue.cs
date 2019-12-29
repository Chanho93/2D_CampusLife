using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HJ.Manager;
using HutongGames.PlayMaker;
namespace HJ.Dialogue
{

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
    [TypeInfoBox("Dialogue File")]
    public class Dialogue: ScriptableObject
    {
        [InfoBox("다음 대화로 이어갈 때 NodeID를 이용하여 이동한다. \n -1은 ID 설정 사용을 안하겠다는 뜻이다.")]
        [Sirenix.OdinInspector.Title("Scriptable 고유번호")]
    
        public int NodeID = -1;

        [LabelText("파일 제목")]
        public string dialogueTitle;

        [LabelText("배경화면")]
        public BACKGROUNDUI backgroundUI;

        [Sirenix.OdinInspector.Title("다이얼로그 설정"),InfoBox("대화내용 저장된 배열이 다 끝나면 nextNode로 이동")]
        [LabelText("대화내용"),BoxGroup("Setting")]
        public Message[] setting;

        [Sirenix.OdinInspector.Title("이벤트"),LabelText("일반적 이벤트"),SerializeField]
        public MessageCommend[] subEvnet;

        [Sirenix.OdinInspector.Title("플레이메이커 이벤트")]
        [LabelText("FSM File")]
        public PlayMakerFSM _fsmFile;
        [LabelText("FsM EVENT")]
        public string _fsmevent = "OnLoad";



        [Button("이벤트 초기화")]
        private void OnClearButton()
        {
            subEvnet = null;
        }

        [LabelText("다음 대화 내용"),BoxGroup("Setting")]
        public FlowNode nextNODE = new FlowNode();

    }

    [System.Serializable]
    public class Message{
        [LabelText("대화내용")]public string description;
        [LabelText("대화이미지")]public Sprite description_Image;
        [LabelText("대화내용 뛰우는 속도"),Range(1f,5f)]public float descriptionTime = 1f;
    }
}


