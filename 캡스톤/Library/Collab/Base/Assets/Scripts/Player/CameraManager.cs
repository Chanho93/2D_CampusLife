using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    #region Variables
    [Title("Setting"),LabelText("카메라 이동속도")]
    [Range(0f, 5f)]
    public float _CameraMoveSpeed = 2.0f;       //? 카메라 이동속도

    [LabelText("쫓아갈 캐릭터")]
    public Transform _Target;                   //? 캐릭터


    private Vector3 _TargetPosition;            //? 타겟 위치


    [LabelText("카메라 범위 콜라이더")]
    public BoxCollider2D bound;

    //? 박스 콜라이더 영역의 최소 최대 xyz 값을 지님.
    private Vector3 minBound;
    private Vector3 maxBound;

    //? 카메라의 반 너비, 반 높이 지닐 수 있는 변수
    float halfWidth;
    float halfHeight;

    Camera theCamera;       //? 카메라의 반높이 값을 구할 속성을 이용한 변수
    #endregion

    #region 싱글턴
    private void Awake()
    {
        if(instance != null) { Destroy(this.gameObject); }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // 기본셋팅
        theCamera = GetComponent<Camera>();
      //  minBound = bound.bounds.min;
      //  maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
        this.UpdateAsObservable()
        .Where(x => _Target.gameObject != null)
        .Subscribe(_ => {

            _TargetPosition.Set(_Target.transform.position.x,_Target.transform.position.y,this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, _TargetPosition, _CameraMoveSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        
        }, ex => Debug.LogError("CameraManager 문제 발생 _Target 변수 Null ")
        
        );

    }

    //? 카메라 지정영역 설정
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }



}
