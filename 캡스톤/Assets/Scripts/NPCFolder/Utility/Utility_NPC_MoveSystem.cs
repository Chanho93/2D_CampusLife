using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
namespace HJ.NPC
{
    /// <summary>
    ///  NPC 움직이는 위치에 대한 열거형
    ///  UP - 위, Down - 아래, Left - 왼쪽, Right - 오른쪽, FirstWait - 1초 대기
    /// </summary>
    public enum NPCPOSITION { UP, DOWN, LEFT, RIGHT, FIRSTWAIT}

    public interface INPCMOVE { void Move(NPCPOSITION _pos); }

    public class Utility_NPC_MoveSystem : MonoBehaviour, INPCMOVE
    {
        [LabelText("이동속도")]
        public float speed;
        public int walkCount;
        protected int currentwalkCount;
        protected Vector3 vector;       // 위치

        private BoxCollider2D boxCollider2D;
        public LayerMask layerMask;
        public Animator animator;

        public void Move(NPCPOSITION _pos)
        {
            StartCoroutine(MoveCoroutine(_pos));
        }
        
        bool CheckCollsion()
        {
            RaycastHit2D hit;

            Vector2 Start = transform.position;
            Vector2 End = Start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            boxCollider2D.enabled = false;
            hit = Physics2D.Linecast(Start, End, layerMask);
            boxCollider2D.enabled = true;

            if(hit.transform != null)
            {
                return true;
            }
            return false;
        }

        IEnumerator MoveCoroutine(NPCPOSITION _pos)
        {
            // 초기화
            vector.Set(0, 0, vector.z);

            // 캐릭터가 이동할 방향을 지정
            switch (_pos)
            {
                case NPCPOSITION.UP:
                    vector.y = 1f;
                    break;
                case NPCPOSITION.DOWN:
                    vector.y = -1f;
                    break;
                case NPCPOSITION.RIGHT:
                    vector.x = 1f;
                    break;
                case NPCPOSITION.LEFT:
                    vector.x = -1f;
                    break;
            }

            // 에니메이터가 있는 경우
            if(animator != null)
            {
                // 이동 모션 실행
                animator.SetFloat("DirX", vector.x);
                animator.SetFloat("DirY", vector.y);

                while(true)
                {
                    bool checkCollsionFlag = CheckCollsion();
                    if (checkCollsionFlag)
                    {
                        // 움직이지 않도록 설정
                        animator.SetBool("Walking", false);
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        break;  // 플레이어가 비킬 경우 무한반복문에서 빠져나오기
                    }
                }
                
                animator.SetBool("Walking", true);
            }
            
            while(currentwalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentwalkCount++;
                yield return new WaitForSeconds(0.01f);
            }

            currentwalkCount = 0;
            animator.SetBool("Walking", false);
        }
    }
}

