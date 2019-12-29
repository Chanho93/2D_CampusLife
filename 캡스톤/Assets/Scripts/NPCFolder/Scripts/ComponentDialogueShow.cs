using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;
using UnityEngine.UI;
namespace HJ.NPC
{
    public class ComponentDialogueShow : Utility_Dialogue
    {
        public GameObject _normalbox;
        public Text _normalmsg;

        [Range(1f,4f)]
        public float _timer;

        /// <summary>
        /// 노멀 대화 상자 실행
        /// </summary>
        /// <param name="n">대화내용</param>
        public void StartNormalMsg()
        {
            if(_dialogueenum == DialoguePettonEnum.NORMALMSG)
                StartCoroutine(ShowNormal(_msgNormal, _timer));
        }
        public override IEnumerator ShowNormal(string n, float timer)
        {
            if(_normalbox != null)
            {
                _normalbox.SetActive(true);
                _normalbox.transform.DOShakeScale(0.2f, 0.1f, 3);

                _normalmsg.DOText(n, timer);

                yield return new WaitForSeconds(timer + 0.9f);

                HideDialogue();
            }
        }

        public void StartDialogueMsg(Dialogue.Dialogue db)
        {
            if(_dialogueenum == DialoguePettonEnum.STORYMSG)
            {
                //! 대화중이므로 이동 금지
                HJ.Manager.DialogueManager.is_keypad = false;

                //! 대화내용 DB 로드
                HJ.Manager.DialogueManager.Instance.LoadDialogue(db);

                //! 대화내용 DB 완료시 대화 시작
                StartCoroutine(HJ.Manager.DialogueManager.Instance.ShowDialogue());

            }
        }
        public void StartChoiceDialogueMsg(Dialogue.ChoiceDialogue db)
        {
            if(_dialogueenum == DialoguePettonEnum.CHOICEMSG)
            {
                //! 대화중이면 이동 금지
                HJ.Manager.DialogueManager.is_keypad = false;

                //! 대화내용 DB 로드
                HJ.Manager.DialogueManager.Instance.LoadChoiceDialogue(db);

                //! 대화내용 DB 완료시 대화시작
                StartCoroutine(HJ.Manager.DialogueManager.Instance.ShowChoiceDialogue());
            }
        }

        public override IEnumerator ShowDialogue(string n, float timer)
        {
            return base.ShowDialogue(n, timer);
        }

        public override void HideDialogue()
        {
            _normalmsg.text = "";
            _normalbox.SetActive(false);
        }
    }
}

