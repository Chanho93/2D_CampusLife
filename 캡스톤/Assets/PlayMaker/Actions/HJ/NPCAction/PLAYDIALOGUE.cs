using UnityEngine;
using HJ.Dialogue;
using HJ.Manager;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("HJ/NPCAction")]
	[Tooltip("플레이메이커 전용 다이얼로그 실행 스크립트")]
	public class PLAYDIALOGUE : FsmStateAction
	{
        [RequiredField]
        [UIHint(UIHint.Variable), Readonly]
        public FsmObject objectVariable;

        [Tooltip("NULL 값이면 nullEvent 실행")]
        public FsmEvent nullEvent;

        [Tooltip("Null 값이 아닐 때 showEvent 실행")]
        public FsmEvent showEvent;


        // 처음 시작
        public override void OnEnter()
		{

            CheckDB();

			Finish();
		}

        void CheckDB()
        {

            StartCoroutine(LoadingDB());
            //? 이벤트 실행
            Fsm.Event(objectVariable.Value == null ? nullEvent : showEvent);
        }

        IEnumerator LoadingDB()
        {
            //? 만약 NULL 값일 경우
            if(objectVariable.Value == null)
            {
                Fsm.Event(nullEvent);
                yield break;
            }

            //? DB 로딩
            DialogueManager.Instance.LoadDialogue((Dialogue)objectVariable.Value);

            yield return new WaitForSeconds(1f);
            
            yield return StartCoroutine( DialogueManager.Instance.ShowDialogue());

            Fsm.Event(showEvent);
        }

	}

}
