using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
namespace HJ.NPC
{
    [Serializable]
    public class MOVEUtility
    {
        [LabelText("NPC 이동 가능여부")]
        public bool is_npcmove = false;

        [LabelText("무한 루프")]
        public bool is_loop = false;

        [LabelText("NPC 이동할 방향")]
        public NPCPOSITION[] direction;

        [LabelText("NPC 이동 주기 설정"), Range(1, 5), InfoBox(" 1 ~ 5 까지 있으며 \n1이 4초마다 움직인다.\n2는 3초 \n3은 2초\n4는 1초\n5는 0초")]
        public int frequency;
    }
    public class Component_NPCMOVE : Utility_NPC_MoveSystem
    {
        [SerializeField] MOVEUtility npcmove;

        #region 설정
        private void Start()
        {
            
            StartCoroutine(MoveCoroutine());
        }
        public void SetMove(int frequency, bool is_loop = false, params NPCPOSITION[] pos) {
            npcmove.frequency = frequency;
            npcmove.is_loop = is_loop;
            npcmove.direction = pos;
            npcmove.is_npcmove = true;
            StartCoroutine(MoveCoroutine());
        }
        public void SetNotMove() {
            npcmove.is_npcmove = false;
        }
        IEnumerator MoveCoroutine()
        {
            bool is_directionCheck = npcmove.direction.Length != 0;

            if(is_directionCheck)
            {
                for(int i = 0; i < npcmove.direction.Length; i++)
                {
                    // NPC 이동속도 빈도 설정
                    switch (npcmove.frequency)
                    {
                        case 1: //? 천천히
                            yield return new WaitForSeconds(4f);
                            break;
                        case 2: //? 조금 천천히
                            yield return new WaitForSeconds(3f);
                            break;
                        case 3: //? 중간
                            yield return new WaitForSeconds(2f);
                            break;
                        case 4: //? 빠름
                            yield return new WaitForSeconds(1f);
                            break;
                        case 5: //? 매우 빠름
                            break;
                    }
                    
                    

                    if(npcmove.is_npcmove)
                    {
                        
                        base.Move(npcmove.direction[i]);
                    }

                    //? 무한루프
                    if(npcmove.is_loop)
                        if (i == npcmove.direction.Length - 1) i = -1;
                }

            }
        }
        #endregion
    }
}

