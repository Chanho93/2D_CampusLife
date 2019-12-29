using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.STATE;

public class NPCMove : MonoBehaviour
{
    public Transform _npcobject;

    //? 이동 빈도
    public NPCMoveRATE moverate;

    //? 이동 방향
    public NPCMovePetton[] movepetton;

    //? 반복?
    public bool is_loop = false;

    public float pixel = 46f;

    private Animator anim;

    public IEnumerator MoveNPC()
    {
        if (movepetton.Length <= 0) yield break;
        if (_npcobject == null) _npcobject = gameObject.transform;

        anim = GetComponent<Animator>();

        //? 이동 빈도 설정
        WaitForSeconds wait = new WaitForSeconds(3f);
        

        while(true)
        {
            switch (moverate)
            {
                case NPCMoveRATE.NONE:
                    yield break;
                case NPCMoveRATE.VERYSLOW:
                    wait = new WaitForSeconds(4f);
                    break;
                case NPCMoveRATE.SLOW:
                    wait = new WaitForSeconds(3f);
                    break;
                case NPCMoveRATE.NORMAL:
                    wait = new WaitForSeconds(2f);
                    break;
                case NPCMoveRATE.FAST:
                    wait = new WaitForSeconds(1f);
                    break;
                case NPCMoveRATE.VERYFAST:
                    wait = new WaitForSeconds(0.2f);
                    break;
            }

            for (int i = 0; i < movepetton.Length; i++)
            {
                switch (movepetton[i])
                {
                    case NPCMovePetton.STOP:
                        yield return wait;
                        break;
                    case NPCMovePetton.UP:
                        _npcobject.Translate(Vector2.up * pixel * Time.deltaTime);
                        if (anim != null) anim.SetFloat("DirY", 1);
                        yield return wait;
                        break;
                    case NPCMovePetton.DOWN:
                        _npcobject.Translate(Vector2.down * pixel * Time.deltaTime);
                        if (anim != null) anim.SetFloat("DirY", -1);
                        yield return wait;
                        break;
                    case NPCMovePetton.LEFT:
                        _npcobject.Translate(Vector2.left * pixel * Time.deltaTime);
                        if (anim != null) anim.SetFloat("DirX", -1);
                        yield return wait;
                        break;
                    case NPCMovePetton.RIGHT:
                        _npcobject.Translate(Vector2.right * pixel * Time.deltaTime);
                        if (anim != null) anim.SetFloat("DirX", 1);
                        yield return wait;
                        break;
                }   // Switch

            } // for

           while(!is_loop)
            {
                yield return null;
            }
        }
    }


    public void STARTMOVE()
    {
        StartCoroutine(MoveNPC());
    }
}
