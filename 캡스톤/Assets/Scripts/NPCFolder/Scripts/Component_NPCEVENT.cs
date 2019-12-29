using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using HJ.Manager;
namespace HJ.NPC
{
    public interface IEVENT
    {
        void Play();
        IEnumerator PlayCorutine();
    }
    /// <summary>
    /// NPC EVENT 실행
    /// </summary>
    public class Component_NPCEVENT : SerializedMonoBehaviour 
    {

        public IEVENT EventInterface;

        [LabelText("Loop")]
        public bool is_loop = false;        //? 무한반복?

        [LabelText("Play On Wake")]
        public bool is_playOnWake = false;


        protected virtual void Start(){
            if (EventInterface == null) return;
            if (is_playOnWake)
            {
                EventInterface.Play();
                StartCoroutine(EventInterface.PlayCorutine());
            }
        }
        protected virtual void Update() {
            if (EventInterface == null) return;

            if (is_loop)
            {
                EventInterface.Play();
                StartCoroutine(EventInterface.PlayCorutine());
            }
        }

        public virtual void PlayEvent()
        {
            if (EventInterface == null) return;

            EventInterface.Play();
            StartCoroutine(EventInterface.PlayCorutine());
        }

        /// <summary>
        /// 인터페이스 변경
        /// </summary>
        protected virtual void ChangeIEVENT(IEVENT e) { EventInterface = e; }
    }

    [Serializable, LabelText("텔레포트이벤트")]
    public class EventPage_NPC1 : IEVENT
    {
        public Vector2 MovePos;
        
        public void Play()
        {
           
            
        }

        public IEnumerator PlayCorutine()
        {
            GameManager.Instance.Fade(1f);
            yield return new WaitForSeconds(0.8f);
            GameManager.Instance._Player.transform.position = MovePos;
            GameManager.Instance._MainCamera.transform.position = new Vector3(MovePos.x, MovePos.y, -10f);
        }
    }

    [Serializable,LabelText("대화 이벤트")]
    public class EventPage_NPC2 :IEVENT
    {
        [SerializeField] Dialogue.Dialogue dialogue;
        [SerializeField] Dialogue.ChoiceDialogue choiceDialogue;
        [SerializeField] Dialogue.FlowNode.FLOW flowEnum;

        private bool is_start = false;
        public void Play()
        {
            if(flowEnum == Dialogue.FlowNode.FLOW.DIALOGUE)
            {
                if (dialogue == null) return;
                HJ.Manager.DialogueManager.Instance.LoadDialogue(dialogue);
                is_start = true;
            }
            else if(flowEnum == Dialogue.FlowNode.FLOW.CHOICEDIALOGUE)
            {
                if (choiceDialogue == null) return;
                HJ.Manager.DialogueManager.Instance.LoadChoiceDialogue(choiceDialogue);
                is_start = true;
            }
        }
        public IEnumerator PlayCorutine()
        {
           while(!is_start)
            {
                yield return new WaitForEndOfFrame();
            }
            if (flowEnum == Dialogue.FlowNode.FLOW.DIALOGUE)
                HJ.Manager.DialogueManager.Instance.SetDialogue();
            else if (flowEnum == Dialogue.FlowNode.FLOW.CHOICEDIALOGUE)
                HJ.Manager.DialogueManager.Instance.SetChoiceDialogue();
        }
    }
    [Serializable, LabelText("페이드 아웃")]
    public class EventPage_NPC3 : IEVENT
    {

        public float timer = 1f;
        public void Play()
        {
            
        }
        public IEnumerator PlayCorutine()
        {
            GameManager.Instance._FadeOutImage.DOFade(1f, timer);
            yield return null;
        }
    }


}

