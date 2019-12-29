using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class left_move : MonoBehaviour
{
    public GameObject[] npc;
    public Animator[] npc_ani;

    bool love;
    bool run1;
    // Start is called before the first frame update
    void Start()
    {
        love = true;
        run1 = true;
        Invoke("Left",2.5f);
        Invoke("Front_Ani", 7.5f);
        StartCoroutine(Love_True());
        StartCoroutine(Run());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    void Left()
    {
       
        npc[0].SetActive(true);
        npc[2].SetActive(true);
       
      
        npc[0].transform.DOMove(new Vector3(-4.36f, -3.33f, 3.233073f), 5.0f).SetEase(Ease.Linear);
        npc[2].transform.DOMove(new Vector3(20.36f, -3.33f, 3.233073f), 3.0f).SetEase(Ease.Linear);
    }

    void Front_Ani()
    {
        npc_ani[0].SetBool("Front", true);
    }



    IEnumerator Run()
    {
        yield return new WaitForSeconds(2.5f);
        npc[3].SetActive(true);

        for(int i=0; i< 100; i++){

            if(run1 == true) {
               
                npc[3].transform.DOMove(new Vector3(5.92f, -3.33f, 3.233073f), 1.5f).SetEase(Ease.Linear);
                run1 = false;
                yield return new WaitForSeconds(1.75f);
            }
            else {
                npc_ani[1].SetBool("Run", true);
                npc[3].transform.DOMove(new Vector3(8.04f, -3.33f, 3.233073f), 1.5f).SetEase(Ease.Linear);
                run1 = true;
                yield return new WaitForSeconds(1.75f);
                npc_ani[1].SetBool("Run", false);
            }
        }
    }

    IEnumerator Love_True()
    {
        yield return new WaitForSeconds(3.0f);
        npc[1].SetActive(true);
     
        for(int i =0; i<100;i++)
        {
            yield return new WaitForSeconds(0.3f);
            

            if(love == false)
            {
                npc[1].SetActive(false);
                love = true;
            }
            else
            {
                npc[1].SetActive(true);
                love = false;
            }

        }
    }
}
