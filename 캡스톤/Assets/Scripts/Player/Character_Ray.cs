using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.NPC;
using HJ.Manager;
public class Character_Ray : MonoBehaviour
{
    public GameObject btn_select;
    

    private Vector2 locate;

    private Collider2D collider2d;

    private bool is_btnSetactive = true;
    //? 버튼 ?
    private bool is_Trigger = false;
    //  Vector2 _PlayerVector;
    //  public int walkCount = 2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {  /*
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // RaycastHit2D hit;
        locate.Set(horizontal, vertical);

        Vector2 _start = transform.position;
        Vector2 _end = _start + new Vector2(locate.x * 0.5f, locate.y * 0.5f);
        
        RaycastHit2D hit = Physics2D.Raycast(_start, _end, 10.0f);
        Debug.DrawRay(_start, _end, Color.red, 0.3f);
        if (hit)
        {
         //   Debug.Log("good");
       }            
       */
        if(DialogueManager.is_Msg)
        {
            is_btnSetactive = false;
            btn_select.SetActive(false);
        }
        else
        {
            is_btnSetactive = true;
        }
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC" )
        {
            if(is_btnSetactive)
                btn_select.SetActive(true);
            is_Trigger = false;
            collider2d = collision;
        }else if(collision.gameObject.tag == "Warp")
        {
            is_Trigger = true;
            if (is_btnSetactive)
                btn_select.SetActive(true);
            collider2d = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            is_Trigger = false;
            btn_select.SetActive(false);
            collider2d = null;
        }
        else if (collision.gameObject.tag == "Warp")
        {
            is_Trigger = true;
            btn_select.SetActive(false);
            collider2d = null;
        }
    }

    /// <summary>
    /// 상호작용 버튼
    /// </summary>
    public void Speechup()
    {
        if (!is_Trigger)
        {
            if (HJ.Manager.DialogueManager.is_nextmsg && HJ.Manager.DialogueManager.is_Msg) return;

            NPCInformation info = collider2d.GetComponent<NPCInformation>();
            if (info == null) return;

            //? 플레이메이커
            //info.fsmBox.SendEvent("NPC");
            info.PlayerRayCast();           //? 플레이 레이케스트 실행
        }
        else
        {
            WarpScript info = collider2d.GetComponent<WarpScript>();

            if (info == null) return;

            info.TelePort();

            info = null;
        }
        
       // Debug.Log(info._name);
    }

}