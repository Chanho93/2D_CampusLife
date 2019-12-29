using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;

namespace HJ.Manager
{
    public class NPCManager : Singleton<NPCManager>
    {
        [LabelText("NPC 목록")]
        [SerializeField] NPCLIST[] npclist;

        /// <summary>
        /// NPC List 배열에 있는 캐릭터를 보여준다.
        /// </summary>
        /// <param name="index"></param>
        public void ShowNPC(int index)
        {
            if(npclist.Length > index)
            {
                StartCoroutine(npclist[index].ShowObject());
            }
        }


        /// <summary>
        /// NPC를 숨긴다.
        /// </summary>
        /// <param name="index"></param>
        public void HideNPC(int index)
        {
            if(npclist.Length > index)
            {
                StartCoroutine(npclist[index].HideObejct());
            }
        }

        /// <summary>
        /// 플레이메이커 : StoryNumber 값을 1증가시킨다.
        /// </summary>
        /// <param name="index">npclist index</param>
        public void AddStoryNumber(int index)
        {
            if(npclist.Length > index)
            {
                //? StoryNumber (전역 이벤트)를 실행한다.
                npclist[index].fsm_dialogue.SendEvent("OnAddStory");
            }
        }
        /// <summary>
        /// 플레이메이커 : StoryNumber 값을 value만큼 대입한다.
        /// </summary>
        /// <param name="index">npclist 배열 인덱스 값</param>
        /// <param name="value">SToryNumber에 들어갈 값</param>
        public void SetStoryNumber(int index, int value)
        {
            if(npclist.Length > index)
            {
                //? FSM 안의 StoryNumber 값을 대입한다.
                npclist[index].fsm_dialogue.FsmVariables.GetFsmInt("StoryNumber").Value = value;
            }
        }
    }
    [System.Serializable]
    public class NPCLIST
    {
        [LabelText("INDEX")]
        public int index = 0;
        [LabelText("NPC 객체")]
        public GameObject npc_object;
        [LabelText("FSM NpcInformation")]
        public PlayMakerFSM fsm_dialogue;

        [LabelText("FSM EpisodeCheckList")]
        public PlayMakerFSM fsm_value;

        public IEnumerator HideObejct()
        {
            npc_object.GetComponent<SpriteRenderer>().DOFade(0f, 1.0f);
            yield return new WaitForSeconds(1.5f);
            npc_object.SetActive(false);
        }

        public IEnumerator ShowObject()
        {
            npc_object.SetActive(true);
            npc_object.GetComponent<SpriteRenderer>().DOFade(1f, 1.0f);
            yield return new WaitForSeconds(1.5f);
        }
    }
}