using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ.Manager;

public class WarpScript : MonoBehaviour
{
    public Transform pos;       // 해당 위치 이동
    public Transform cmera; // 카메라
    public BoxCollider2D bound;
    public string _audioBGMName;
    public void TelePort()
    {
        if (pos == null) return;

        CameraManager cam = cmera.GetComponent<CameraManager>();
        
        GameManager.Instance.Fade(1.0f);
        GameManager.Instance._Player.transform.position = pos.position;
        if(_audioBGMName != string.Empty)
        {

            SoundManager._instance.Play_BGM(_audioBGMName);
        }
        cam.enabled = false;
        cmera.transform.position = new Vector3( pos.position.x,pos.position.y,cmera.position.z);
        cam.SetBound(bound);

        cam.enabled = true;

    }
}
