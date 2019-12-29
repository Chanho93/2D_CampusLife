using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.Dialogue;
using Sirenix.OdinInspector;
namespace HJ.NPC
{
    public interface IDialogueShow
    {
        IEnumerator ShowDialogue(string n, float timer);
        void HideDialogue();
    }
    public interface IChoiceShow
    {
        IEnumerator ShowChoice(string n1, string n2, string n3, float timer);
        void HideChoice();
    }
    public interface INormalShow
    {
        IEnumerator ShowNormal(string n, float timer);
        void HideNormal();
    }
    public class Utility_Dialogue : MonoBehaviour, IDialogueShow, IChoiceShow, INormalShow
    {
        [ LabelText("인사말")]
        public string _msgNormal;
        [LabelText("다이얼로그 형태")]
        public Dialogue.Dialogue _msgDialogue;
        [LabelText("선택창 형태")]
        public Dialogue.ChoiceDialogue _msgChoice;

        public enum DialoguePettonEnum {NORMALMSG, STORYMSG, CHOICEMSG}
        [EnumToggleButtons]
        public DialoguePettonEnum _dialogueenum;

        #region 인터페이스
        public virtual void HideChoice()
        {
            
        }

        public virtual void HideDialogue()
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerator ShowChoice(string n1, string n2, string n3, float timer)
        {
            yield return null;
        }

        public virtual IEnumerator ShowDialogue(string n, float timer)
        {
            yield return null;
        }

        public virtual IEnumerator ShowNormal(string n, float timer)
        {
            yield return null;
        }

        public virtual void HideNormal()
        {
            
        }
        #endregion
    }

}
