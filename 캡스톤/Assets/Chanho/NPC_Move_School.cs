using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NPC_Move_School : MonoBehaviour
{

    public GameObject[] npc;
    public Animator[] npc_ani;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
      //  SoundManager._instance.Play_BGM("BGM1", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(1.0f);       
        npc[0].transform.DOMove(new Vector3(-0.05306906f, 31.58f, 0f), 2.0f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2.0f);

        
        npc_ani[0].SetBool("Turn", true);
        npc[0].transform.DOMove(new Vector3(-2.78f, 31.58f, 0f), 4.0f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(4.0f);
        
    

        for(int i = 0; i < 1000; i++)
        {

        npc_ani[0].SetBool("Turn1", true);
        npc[0].transform.DOMove(new Vector3(-0.05306906f, 31.58f, 0f), 4.0f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(4.0f);
        npc_ani[0].SetBool("Turn1", false);

        npc_ani[0].SetBool("Turn", true);
        npc[0].transform.DOMove(new Vector3(-2.78f, 31.58f, 0f), 4.0f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(4.0f);


        }

    }
}
