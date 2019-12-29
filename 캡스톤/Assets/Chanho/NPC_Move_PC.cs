using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPC_Move_PC : MonoBehaviour
{

    public GameObject[] npc;
    public Animator[] npc_ani;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move()
    {

        for (int i = 0; i < 1000; i++)
        {
            yield return new WaitForSeconds(1.0f);
            npc[0].transform.DOMove(new Vector3(-42.5f, 14.05f, 0f), 3.0f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(3.0f);
            

            npc_ani[0].SetBool("Turn", true);
            npc[0].transform.DOMove(new Vector3(-42.5f, 16.73f, 0f), 4.0f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(4.0f);

            npc_ani[0].SetBool("Turn", false);

        }
    }
}
