using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("HJ/NPCAction")]
	[Tooltip("말풍선 박스와 말풍선 텍스트로 이루어진 스크립트NPC 사용하기 가장 좋다.")]
	public class SpeechBubbleAction : FsmStateAction
	{
        public FsmGameObject _SpeechBox;
        public Text _SpeechText;
        
        
        public FsmFloat _timer;
        public FsmString _msg;
        public override void Reset()
        {
            _SpeechBox = null;

        }

        // Code that runs on entering the state.
        public override void OnEnter()
		{
            _SpeechText = _SpeechBox.Value.GetComponentInChildren<Text>();


            StartCoroutine(MessageShow());
            
			Finish();
		}

		private IEnumerator MessageShow()
        {
            WaitForSeconds wait = new WaitForSeconds( _timer.Value + 0.5f);
            _SpeechText.text = "";
            
            _SpeechBox.Value.SetActive(true);
            _SpeechBox.Value.transform.DOShakeScale(0.2f, 0.2f, 3);
            _SpeechText.DOText(_msg.Value, _timer.Value);

            yield return wait;
          
            _SpeechBox.Value.SetActive(false);
            _SpeechText.text = "";

        }

        
	}

}
