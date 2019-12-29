using UnityEngine;
using System;
using Sirenix.OdinInspector;
/*
 * 이 스크립트는 다이얼로그 연결 흐름을 만들기 위해 사용된 스크립트이다.
 */ 
 
 namespace HJ.Dialogue
{
    
    [Serializable]
    public class FlowNode 
    {
        public FlowNode()
        {
            _NextNode = null;
            _ChoiceNode = null;
            _flow = FLOW.DIALOGUE;
        }
        public FlowNode(Dialogue node)
        {
            _NextNode = node;
            _flow = FLOW.DIALOGUE;
        }
        public FlowNode(ChoiceDialogue node)
        {
            _ChoiceNode = node;
            _flow = FLOW.CHOICEDIALOGUE;
        }
        public FlowNode(FLOW f)
        {
            _flow = f;
        }
        [LabelText("이동할 다음 대화 노드")]
        public Dialogue _NextNode;

        [LabelText("이동할 다음 선택창 노드")]
        public ChoiceDialogue _ChoiceNode;

        public enum FLOW { DIALOGUE, CHOICEDIALOGUE}
        [LabelText("선택할 옵션"), EnumToggleButtons]
        public FLOW _flow;
    }

}
