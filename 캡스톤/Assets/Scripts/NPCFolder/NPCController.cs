using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HJ.Manager;
using UnityEngine.UI;
using DG.Tweening;
namespace HJ.NPC
{
    
    //? NPCInformation 스크립트 없이는 사용 불가
    [RequireComponent(typeof(NPCInformation))]
    public class NPCController : MonoBehaviour
    {
        [LabelText("대화형식"), EnumToggleButtons]
        public NPCDialogue dialoguetype;

        private GameObject _SpeechBox;
        private GameObject _SpeechBubbleBox;
        private Text _SpeechBubbleText;

        private GameObject instance;    //? 해당 NPC

        private NPCInformation infomation;      //? NPC Information

        [LabelText("스토리 번호")]
        public int _StoryNumber;

        [HideInInspector]
        public string name;
        [HideInInspector]
        public int index;

        [DisplayAsString, LabelText("안건드려도됨")]
        [InfoBox("NPC Information에서 수정한거 이곳에 올라온다.")]
        public string[] msg;

        [LabelText("다이얼로그")]
        public Dialogue.Dialogue[] dialogue;

        private bool is_msg = false;    //? 메세지 중?

        public bool is_dialogueLoop = false;        //? 무한 다이얼로그?
        //? 말풍선
        private int count = 0;
        [LabelText("말풍선 최대 대화갯수")]
        public int maxcount = 3;

        private bool episodeclear = false;
        /// <summary>
        /// 초기화
        /// </summary>
        private void Awake()
        {
            instance = gameObject;
            infomation = GetComponent<NPCInformation>();

            _SpeechBox = gameObject.transform.GetChild(0).gameObject;

            _SpeechBubbleBox = _SpeechBox.transform.GetChild(0).gameObject;
            _SpeechBubbleText = _SpeechBubbleBox.transform.GetChild(0).gameObject.GetComponent<Text>();

        }

        private void Start()
        {
            
            infomation.InitScriptNPC();
        }

       
        /// <summary>
        /// 레이케스트 시작
        /// </summary>
        public void RayStart()
        {
            //? 말풍선
            if(dialoguetype == NPCDialogue.SpeechBubble)
            {
                if(!is_msg)
                    StartCoroutine(SpeechBubblePlay(ReturnRandomString()));
            }else if(dialoguetype == NPCDialogue.Dialogue)
            {
                if(dialogue.Length <= 0) { dialoguetype = NPCDialogue.SpeechBubble; count = 0; maxcount = 999; }
                if (!is_msg && dialogue.Length >= 1 && _StoryNumber < dialogue.Length)
                    StartCoroutine(DailoguePlay(dialogue[_StoryNumber]));
            }

        }
        #region 말풍선 대화
        //? 랜덤값 반환
        string ReturnRandomString()
        {
            int rand = Random.Range(0, msg.Length + 1);
            if (rand == msg.Length) rand = 0;

            return msg[rand];
        }
        /// <summary>
        /// 말풍선 출력
        /// </summary>
        /// <param name="msg">대화메세지</param>
        /// <returns></returns>
        IEnumerator SpeechBubblePlay(string msg)
        {
            WaitForSeconds wait = new WaitForSeconds(2.2f);
            is_msg = true;
            _SpeechBubbleText.text = "";

            if (!episodeclear)
                count++;
            else count = 0;

            yield return null;

            if(_SpeechBubbleText != null)
            {
                _SpeechBubbleBox.SetActive(true);

                _SpeechBubbleText.DOText(msg, 1.4f);
               
            }

            yield return wait;
            if(count >= maxcount) { dialoguetype = NPCDialogue.Dialogue; }
            _SpeechBubbleText.text = "";
            _SpeechBubbleBox.SetActive(false);
            is_msg = false;
        }
        IEnumerator DailoguePlay(Dialogue.Dialogue db)
        {
            is_msg = true;
            

            WaitForSeconds wait = new WaitForSeconds(1f);
            DialogueManager.Instance.LoadDialogue(db);
            DialogueManager.is_Msg = true;

            yield return wait;

            yield return StartCoroutine( DialogueManager.Instance.ShowDialogue());

            DialogueManager.is_Msg = false;

            
            _StoryNumber++;

            if(_StoryNumber >= dialogue.Length && !is_dialogueLoop) { dialoguetype = NPCDialogue.SpeechBubble;  episodeclear = true; }
            else if(_StoryNumber >= dialogue.Length && is_dialogueLoop)
            {
                _StoryNumber = 0;
            }
            is_msg = false;
        }
        #endregion
    }

}
