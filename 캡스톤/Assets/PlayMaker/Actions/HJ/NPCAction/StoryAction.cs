using UnityEngine;
using HJ.Dialogue;
using HJ.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("HJ/NPCAction")]
	[Tooltip("이야기 진행 스크립트")]
	public class StoryAction : FsmStateAction
	{
        [RequiredField]
        [UIHint(UIHint.Variable), Readonly]
        public FsmInt _StoryNumber;

        public FsmEvent[] _StoryEvent;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            if(_StoryEvent.Length > _StoryNumber.Value)
            {
                Fsm.Event(_StoryEvent[_StoryNumber.Value]);
            }
            else
                Finish();
        }



	}

}
