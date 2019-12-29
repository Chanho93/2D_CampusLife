using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MoveScript : MonoBehaviour
{
    /// 조이스틱 사용여부
    public static bool is_JoyStick = true;        //? 조이스틱으로 플레이 할건가?
    
    [Title("JoyStick")]
    public Transform _JoyStick;     //? 조이스틱

    [Title("Character"),LabelText("플레이어")]
    public Transform _Player;       //? 플레이어


    private Vector3 _StickFirstPos;     //? 조이스틱 처음 위치
    private Vector3 _JoyVec;            //? 조이스틱의 방향(벡터)
    private float _Radius;              //? 조이스틱 배경의 반지름

    private bool is_MoveFlag = false;       //? 이동 여부

    void Start(){
        _Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;     //? 조이스틱 배경 반의 y축 길이의 값
        _StickFirstPos = _JoyStick.transform.position;                  //? 조이스틱의 위치

        //? 캔버스 크기에 대한 바지름 조절
        float _Can = transform.parent.GetComponent<RectTransform>().localScale.x;

        _Radius *= _Can;

        is_MoveFlag = false;

        
    }

    public void Drag(BaseEventData _data)
    {
        if(is_JoyStick){
            PointerEventData _Data = _data as PointerEventData; //? 마우스 및 터치에 대한 이벤트
            Vector3 _Pos = _Data.position;

            //? 조이스틱을 이동시킬 방향을 구함
            _JoyVec = (_Pos - _StickFirstPos).normalized;

            //? 조이스틱의 처음 위치와 현재 내가 터치하고 있는 위치의 거리를 구한다.
            float _dis = Vector3.Distance(_Pos,_StickFirstPos);

            //? 거리가 반지름 보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동
            if(_dis < _Radius)
            {
                _JoyStick.position = _StickFirstPos + _JoyVec * _dis;
                
            }else{
                // 거리가 반지름 보다 커지면 조이스틱을 반지름 크기만큼 이동
                _JoyStick.position = _StickFirstPos + _JoyVec * _Radius;
            }
        }
        


    }

    // 드래그 끝
    public void DragEnd(){
        _JoyStick.position = _StickFirstPos;        // 원래 위치
        _JoyVec = Vector3.zero;
    }


    
    
}
