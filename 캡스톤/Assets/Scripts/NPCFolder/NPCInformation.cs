using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HJ.Dialogue;

//? 플레이메이커
using HutongGames.PlayMaker;
namespace HJ.NPC
{

    public enum NPCDialogue { SpeechBubble, Dialogue }
    [RequireComponent(typeof(NPCController))]
    public class NPCInformation : MonoBehaviour
    {
        [LabelText("NPC 이름")]
        public string _name;

        [LabelText("NPC 설명"), Multiline]
        public string _dec;

        [LabelText("NPC 식별번호")]
        public int _index;

        [LabelText("첫 인사말")]
        public string _msg;

        [LabelText("랜덤 인사말 목록")]
        public string[] _rndmsg;

        //? NPC Controller
        private NPCController controller;

        [Sirenix.OdinInspector.Title("사용하고 싶은 NPC 명령어"), LabelText("PlayMaker")]
        public PlayMakerFSM fsmBox;
        
        /// <summary>
        /// PlayMaker 전용 NPCInfo Template 에서 사용함
        /// InitNPC State 안에 넣어주면 된다.
        /// </summary>
        public void InitNPC()
        {
            if(fsmBox == null)
            {
                Debug.LogError("해당 NPC에 fsmBox가 안들어가 있습니다.");
                return;
            }

            fsmBox.FsmVariables.GetFsmString("npc_name").Value = _name;
            fsmBox.FsmVariables.GetFsmInt("npc_id").Value = _index;
            fsmBox.FsmVariables.GetFsmString("FirstMessage").Value = _msg;
        }

        /// <summary>
        /// NPC Controller 스크립트 전용 초기화
        /// </summary>
        public void InitScriptNPC()
        {
            controller = GetComponent<NPCController>();
            controller.name = _name;
            controller.index = _index;

            string[] m = new string[_rndmsg.Length + 1];
            m[0] = _msg;
            for(int i = 1; i < _rndmsg.Length + 1; i++)
            {
                m[i] = _rndmsg[i - 1];
            }

            controller.msg = m;

        }

        public void PlayerRayCast()
        {
            if(controller != null)
            {
                controller.RayStart();
            }
        }

        /// <summary>
        /// 랜덤 메세지
        /// </summary>
        public void RandomMessage()
        {
            if(_rndmsg.Length > 0)
            {
                //? 랜덤함수
                int rnd = Random.Range(0,_rndmsg.Length);

                //? 만약 배열크기랑 같은 경우
                if (rnd == _rndmsg.Length) rnd = 0;

                _msg = _rndmsg[rnd];
                fsmBox.FsmVariables.GetFsmString("FirstMessage").Value = _msg;
            }
        }

     
    }


}
