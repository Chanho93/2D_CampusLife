using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HJ.Manager;

namespace HJ.Dialogue
{
    [CreateAssetMenu(fileName = "New ChoiceDialogue", menuName = "Dialogue/New ChoiceDialogue")]
    [TypeInfoBox("ChoiceDialogue File")]
    public class ChoiceDialogue : ScriptableObject
    {
        [InfoBox("다음 대화로 이어갈 때 NodeID를 이용하여 이동한다. \n -1은 ID 설정 사용을 안하겠다는 뜻이다.")]
        [Title("Scriptable 고유번호")]
        
        public int NodeID = -1;

        [LabelText("파일 제목")]
        public string dialogueTitle;

        [LabelText("선택 메세지 갯수"), Range(1,3)]
        public int _choicenumber = 3;

        [Title("선택메세지 설정"),InfoBox("선택 메세지 3개 입력칸.")]
        [LabelText("선택 메세지1"),BoxGroup("Setting")]
        public string _ChoiceMsg1;
        [LabelText("이동할 DB1"),BoxGroup("Setting")]
        public FlowNode _NextChoice1Node;
        [LabelText("이벤트 1"),SerializeField]
        public MessageCommend[] subevent1;

        [Title("")]
        [LabelText("선택 메세지2"),BoxGroup("Setting")]
        public string _ChoiceMsg2;
        [LabelText("이동할 DB2"),BoxGroup("Setting")]
        public FlowNode _NextChoice2Node;
        [LabelText("이벤트 2"), SerializeField]
        public MessageCommend[] subevent2;

        [Title("")]
        [LabelText("선택 메세지3"),BoxGroup("Setting")]
        public string _ChoiceMsg3;

        [LabelText("이동할 DB3"),BoxGroup("Setting")]
        public FlowNode _NextChoice3Node;
        [LabelText("이벤트 3"), SerializeField]
        public MessageCommend[] subevent3;

        [Button("이벤트 모두 초기화")]
        public void OnClearButton()
        {
            subevent1 = null;
            subevent2 = null;
            subevent3 = null;
        }

        public List<string> ChoiceMsgList{
            get{
                List<string> temp = new List<string>();
                temp.Clear();
                temp.Add(_ChoiceMsg1);
                temp.Add(_ChoiceMsg2);
                temp.Add(_ChoiceMsg3);


                return temp;
            }
        }

        public List<FlowNode> DialogueList{
            get{
                List<FlowNode> temp = new List<FlowNode>();
                temp.Clear();
                temp.Add(_NextChoice1Node);
                temp.Add(_NextChoice2Node);
                temp.Add(_NextChoice3Node);

                return temp;
            }
        }

        
    }
}

