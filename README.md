# 2D_CampusLife
 
졸업작품_캡스톤 디자인 동상

1.메인화면

![image](https://user-images.githubusercontent.com/48191157/71571288-e76d4c80-2b1c-11ea-9eb2-7bc3216c305e.png)

2.학교에 대한 정보 및 NPC플레이메이커 활용

![image](https://user-images.githubusercontent.com/48191157/71571306-fe13a380-2b1c-11ea-841f-24e11aaccfce.png)

    using HutongGames.PlayMaker;
    namespace HJ.NPC
    {

    public enum NPCDialogue { SpeechBubble, Dialogue }
    [RequireComponent(typeof(NPCController))]
    public class NPCInformation : MonoBehaviour
    {
        [LabelText("NPC 이름")]
        public string _name;

        [LabelText("NPC 설명"), Multiline]
        public string _dec;

        [LabelText("NPC 식별번호")]
        public int _index;

        [LabelText("첫 인사말")]
        public string _msg;

        [LabelText("랜덤 인사말 목록")]
        public string[] _rndmsg;

        //? NPC Controller
        private NPCController controller;

        [Sirenix.OdinInspector.Title("사용하고 싶은 NPC 명령어"), LabelText("PlayMaker")]
        public PlayMakerFSM fsmBox;
        
        /// <summary>
        /// PlayMaker 전용 NPCInfo Template 에서 사용함
        /// InitNPC State 안에 넣어주면 된다.
        /// </summary>
        public void InitNPC()
        {
            if(fsmBox == null)
            {
                Debug.LogError("해당 NPC에 fsmBox가 안들어가 있습니다.");
                return;
            }

            fsmBox.FsmVariables.GetFsmString("npc_name").Value = _name;
            fsmBox.FsmVariables.GetFsmInt("npc_id").Value = _index;
            fsmBox.FsmVariables.GetFsmString("FirstMessage").Value = _msg;
        }

        /// <summary>
        /// NPC Controller 스크립트 전용 초기화
        /// </summary>
        public void InitScriptNPC()
        {
            controller = GetComponent<NPCController>();
            controller.name = _name;
            controller.index = _index;

            string[] m = new string[_rndmsg.Length + 1];
            m[0] = _msg;
            for(int i = 1; i < _rndmsg.Length + 1; i++)
            {
                m[i] = _rndmsg[i - 1];
            }

            controller.msg = m;
        }

        public void PlayerRayCast()
        {
            if(controller != null)
            {
                controller.RayStart();
            }
        }
        /// <summary>
        /// 랜덤 메세지
        /// </summary>
        public void RandomMessage()
        {
            if(_rndmsg.Length > 0)
            {
                //? 랜덤함수
                int rnd = Random.Range(0,_rndmsg.Length);

                //? 만약 배열크기랑 같은 경우
                if (rnd == _rndmsg.Length) rnd = 0;

                _msg = _rndmsg[rnd];
                fsmBox.FsmVariables.GetFsmString("FirstMessage").Value = _msg;
            }
        }     
    }
}

2_1.조이스틱 조작

![image](https://user-images.githubusercontent.com/48191157/71571319-0835a200-2b1d-11ea-8afe-b9f99bc8c171.png)

    public JoyStick joystick;
    public GameObject _Joypad;  //? 조이패드
    public bool is_Joystick = true;
    //? 플레이어 이동속도
    public float Speed { get { return speed; } set { speed = value; } }
    float vertical, horizontal;
    bool is_canMove = true;            //? 이동가능?
    float speed;
    float runspeed;
    int walkCount = 2;
    int currentWalkCount = 0;
    Vector2 _PlayerVector;
    //? 플레이어 컴퍼넌트
    BoxCollider2D _PlayerBoxColider2D;
    [SerializeField] LayerMask _PlayerLayMask;
    Animator _PlayerAnimator;
    private void Awake()
    {          

    }
    private void Start()
    {
        //!? 플레이어 이동속도 설정
        Speed = 0.1f;
        _PlayerBoxColider2D = GetComponent<BoxCollider2D>();
        _PlayerAnimator = GetComponent<Animator>();

        //Debug.Log(_PlayerLayMask.value);

       // Observable.EveryFixedUpdate()
       //.Where(_ => Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") !=0)
       //.Subscribe(_ =>
       //{
       //    vertical = Input.GetAxisRaw("Vertical");
       //    horizontal = Input.GetAxisRaw("Horizontal");

       //    if (is_canMove)
       //    {
       //        if (horizontal != 0 || vertical != 0)
       //        {
       //            is_canMove = false;
       //            StartCoroutine(ComputerMoveCoroutine());
       //        }
       //    }
       //});
    }
    
    private void Update()
    {
        if(!is_Joystick)    // 키보드
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else  // 조이스틱
        {
            vertical = joystick.GetVerticalValue();
            horizontal = joystick.GetHorizontalValue();
        }     
         if (is_canMove && DialogueManager.is_keypad && !DialogueManager.is_Msg)
        {
            if(horizontal != 0 || vertical != 0)
            {
                is_canMove = false;
                StartCoroutine(ComputerMoveCoroutine());
            }
        }
    }
    
    IEnumerator ComputerMoveCoroutine()
    {
        while(vertical != 0 || horizontal != 0)
        {            
            _PlayerVector.Set(horizontal, vertical);
            _PlayerVector.Normalize();
            //Debug.Log(_PlayerVector);
            //if (_PlayerVector.x != 0) _PlayerVector.y = 0;
            _PlayerAnimator.SetFloat("DirX", _PlayerVector.x);
            _PlayerAnimator.SetFloat("DirY", _PlayerVector.y);
            _PlayerAnimator.SetBool("Walking", true);

            //? 이동 불가 지역 설정
            RaycastHit2D hit;
            Vector2 _start = transform.position;
            Vector2 _end = _start + new Vector2(_PlayerVector.x * speed * walkCount, _PlayerVector.y * speed *walkCount);

            _PlayerBoxColider2D.enabled = false;
            hit = Physics2D.Linecast(_start, _end, _PlayerLayMask);
            _PlayerBoxColider2D.enabled = true;

            if(hit.transform != null)
            {               
                break;
            }
            //Debug.Log(hit.transform);
            while(currentWalkCount < walkCount)
            {
                transform.Translate(_PlayerVector.x * (speed + runspeed), _PlayerVector.y * (speed + runspeed), 0);
               // if (is_applyRunFlag) currentWalkCount++;
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
                        
        }
        _PlayerAnimator.SetBool("Walking", false);
        is_canMove = true;
            }

3.캐릭터와 상호작용 및 선택답안1

![image](https://user-images.githubusercontent.com/48191157/71571323-108ddd00-2b1d-11ea-9f29-8f8f03256b08.png)

3_1.다이얼로그를 이용한 스토리 전개

![image](https://user-images.githubusercontent.com/48191157/71571329-184d8180-2b1d-11ea-88c7-c22b7e69c441.png)

    namespace HJ.Manager
    {
    public enum FACEPOS { LEFT, RIGHT}
    public enum BACKGROUNDUI { CAMPUS, SCHOOL, COFFEE, LIBRARY, PCROOM, HOME, BAR }
    public class DialogueManager : Singleton<DialogueManager>
    {
        #region Delegate
        public delegate void ClickDelegate(int n);                          //? 클릭이벤트
        public delegate void ChangeTextDelegate(Text t, string s);  //? 택스트 변경 이벤트
        #endregion

        #region EVENT
        private event ClickDelegate OnEvent_ClickButton;
        private event ChangeTextDelegate OnEvent_ChangeText;
        #endregion

        #region Variables
        //? Dialogue UI Variables
        public GameObject _DialogueBox;

        public Text _DialogueText;

        public Image _FaceLeft;
        public Image _FaceRight;

        [Header("BackGround UI")]
        public GameObject _BackGroundObject;

        [Header("이미지 Null 값 UI")]
        public Sprite _NullMask;

        //? Choice UI Variables
        public GameObject _ChoiceBox;

        public Text _ChoiceMainText;
        public Text _ChoiceText1;
        public GameObject _ChoiceText1object;
        public Text _ChoiceText2;
        public GameObject _ChoiceText2object;
        public Text _ChoiceText3;
        public GameObject _ChoiceText3object;
        
        //? 알람
        public Text _Alerm;

        [Header("다이얼로그 배경화면")]
        public Sprite _CampuseBackGround;       //? 캠퍼스
        public Sprite _SchoolBackGround;           //? 학교
        public Sprite _PCRoomBackGround;        //? PC방
        public Sprite _MyHomeBackGround;        //? 집
        public Sprite _LibraryBackGround;       //? 도서관 
        public Sprite _BarBackGround;           //? 술집
        public Sprite _CoffeeBackGround;        //? 커피집
        public Image _BackGrounUI;                   //? 배경 UI

        //? Component Script
        [HideInInspector]
        public ComponentDialogueShow _componentDialogue;

        public static bool is_Msg = false;     //! 대화중인가?
        public static bool is_keypad = true;  //! 방향키 이동 가능한가?
        public static bool is_nextmsg = false;  //! 다음 대화 내용 있나?
        private static bool is_choice = false;  //! 선택창이 떳는가?
        private static bool is_Wait = false;    //! 대기중 인가?
        public Queue<string> _msgDialogueList = new Queue<string>();
        private Queue<float> _msgtimer = new Queue<float>();
        private Queue<Sprite> _msgSprite = new Queue<Sprite>();
        private BACKGROUNDUI backui;

        [HideInInspector]
        public List<string> _choiceList = new List<string>();

        private int _count = 0;
        private string _m;
        private string _msgTitle;
        private Dialogue.Dialogue tempdb;
        private Dialogue.ChoiceDialogue tempchoicedb;
        private Dialogue.ChoiceDialogue temp2chicedb;
        private Dialogue.Dialogue temp2db;
        private FACEPOS facepos;
        #endregion

        /// <summary>
        /// 초기화
        /// </summary>
        private void Awake()
        {
            if (_DialogueText != null) _DialogueText.text = "";
            facepos = FACEPOS.LEFT;
            // 이벤트 등록
            OnEvent_ClickButton += OnClickNumber;
            OnEvent_ChangeText += ChangeText;
            }

        /// <summary>
        /// 다이얼로그 파일 불러오기
        /// </summary>
        public void LoadDialogue(Dialogue.Dialogue db)
        {
            _msgDialogueList.Clear();       //! 청소
            _msgtimer.Clear();
            _msgSprite.Clear();
            _msgTitle = null;
            tempdb = null;
            _count = 0;
            if (db == null) return;
            var n =
                from set in db.setting
                where set.description != null
                select set.description;
            //? UI 설정
            backui = db.backgroundUI;
            for (int m = 0; m < db.setting.Length; m++){
                _msgtimer.Enqueue(db.setting[m].descriptionTime);
                _msgSprite.Enqueue(db.setting[m].description_Image);
            }
            foreach (var msg in n)
            {
                _msgDialogueList.Enqueue(msg);
            }
            _msgTitle = db.dialogueTitle;
            tempdb = db;
            Debug.Log(string.Format("Load Dialogue : {0}" ,_msgDialogueList.Count));
        }
        public void LoadChoiceDialogue(ChoiceDialogue db)
        {
            _choiceList.Clear();
            _msgTitle = null;
            tempchoicedb = null;
            List<string> s = db.ChoiceMsgList;
            if(s.Count >= 1)
            {
                _msgTitle = db.dialogueTitle;
                for (int i = 0; i < s.Count; i++)
                {
                    _choiceList.Add(s[i]);
                }
                tempchoicedb = db;
            }
        }
        public void NextButton()
        {
            StartCoroutine(SoundManager._instance.Play_SE("Click1"));
            if (is_Msg)
            {                
                _DialogueText.DOKill();
                _DialogueText.text = "";
                _DialogueText.text = _m;
                is_Msg = false;
                if(is_nextmsg)
                {
                    StartCoroutine(ShowDialogue());
                }
            }
            else if(is_nextmsg)
            {
                StartCoroutine(ShowDialogue());
                }            
        }

        /// <summary>
        /// 다이얼로그 보여주기
        /// </summary>
        public void SetDialogue()
        {
            StartCoroutine(ShowDialogue());
        }
        /// <summary>
        /// 선택창 보여주기
        /// </summary>
        public void SetChoiceDialogue()
        {
            StartCoroutine(ShowChoiceDialogue());
        }

        /// <summary>
        /// 다이얼로그 보여주기
        /// </summary>
        /// <returns>다이얼로그 에셋</returns>
        public IEnumerator ShowDialogue()
        {
            float t;
            Sprite sp;
            is_Msg = true;
            is_nextmsg = true;
            temp2db = tempdb;
            temp2chicedb = tempchoicedb;
            if (_msgDialogueList.Count <= 0 || _msgtimer.Count <= 0)
            {
                //Debug.LogError(_msgDialogueList.Count +" " + _msgtimer.Count);
                try { 
                //? 이벤트 실행
                if(tempdb.subEvnet != null)
                    StartCoroutine(PlayEvent(0));
                    
                //? 플레이메이커 이벤트 전송
                if(tempdb._fsmFile != null)
                {
                    tempdb._fsmFile.SendEvent(tempdb._fsmevent);
                }                           
                switch (tempdb.nextNODE._flow)
                {
                    case FlowNode.FLOW.DIALOGUE:
                        //? 다음 노드가 다이얼로그 인 경우
                        if(tempdb.nextNODE._NextNode != null)
                        {
                            LoadDialogue(tempdb.nextNODE._NextNode);

                            StartCoroutine(ShowDialogue());
                            yield break;
                            }                    
                            break;
                    case FlowNode.FLOW.CHOICEDIALOGUE:
                        //? 다음 노드가 선택로그 인 경우
                        if (tempdb.nextNODE._ChoiceNode != null)
                        {
                            LoadChoiceDialogue(tempdb.nextNODE._ChoiceNode);

                            StartCoroutine(ShowChoiceDialogue());
                            yield break;
                        }
                        break;
                }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                /*
                if(tempdb.subEvnet.GetPersistentEventCount() > 0)
                    tempdb.subEvnet.Invoke();
                    */

                is_keypad = true;
                is_Msg = false;
                is_nextmsg = false;
                HideDialogueObject();     //? 오브젝트 비활성화          
                yield break;                
            }
            ShowDialogueObject();   //? 오브젝트 표시
            BackGroundChange();
            t = _msgtimer.Dequeue();
            _m = _msgDialogueList.Dequeue();
            _DialogueText.text = "";
            _DialogueText.DOText(_m,t);
            sp = _msgSprite.Dequeue();
            //? 이미지 표시
            if(sp != null)
                switch(facepos)
                {
                    case FACEPOS.LEFT:
                        _FaceLeft.sprite = sp;
                        _FaceRight.sprite = _NullMask;
                        facepos = FACEPOS.RIGHT;
                        break;
                    case FACEPOS.RIGHT:
                        _FaceRight.sprite = sp;
                        _FaceLeft.sprite = _NullMask;
                        facepos = FACEPOS.LEFT;
                        break;
                }
            yield return new WaitForSeconds(t + 0.7f);
            _m = null;
            is_Msg = false;
        }        
        /// <summary>
        /// 선택창 보여주기
        /// </summary>
        /// <returns>선택창 에셋</returns>
        public IEnumerator ShowChoiceDialogue()
        {
            int cnt = _choiceList.Count;            
            is_choice = false;
            is_Msg = true;

            //? 선택창 뛰우기
            ShowChoiceDialogueObject();

            switch(tempchoicedb._choicenumber)
            {
                case 2:
                    _ChoiceText3object.gameObject.SetActive(false);
                    break;
                case 1:
                    _ChoiceText3object.gameObject.SetActive(false);
                    _ChoiceText2object.gameObject.SetActive(false);
                    break;
            }

            //? 이벤트 실행 : 텍스트 변경
            OnEvent_ChangeText(_ChoiceMainText, tempchoicedb.dialogueTitle);
            OnEvent_ChangeText(_ChoiceText1, _choiceList[0]);
            OnEvent_ChangeText(_ChoiceText2, _choiceList[1]);
            OnEvent_ChangeText(_ChoiceText3, _choiceList[2]);
            yield return new WaitForSeconds(2f);
            is_choice = true;         
            yield return null;
        }
        #region 다이얼로그창
        void ShowDialogueObject()
        {
            StartCoroutine(SoundManager._instance.Play_SE("Dialogue1"));
            _DialogueBox.SetActive(true);
            _DialogueBox.transform.DOShakeScale(0.2f, 0.2f, 8);
            _FaceLeft.sprite = _NullMask;
            _FaceRight.sprite = _NullMask;
            _DialogueText.text = "";
        }

        void HideDialogueObject()
        {
            _DialogueBox.transform.DOShakeScale(1f, 0.5f, 8);
            _DialogueText.text = "";
            _FaceLeft.sprite = _NullMask;
            _FaceRight.sprite = _NullMask;
            _BackGroundObject.SetActive(false);
            _DialogueBox.SetActive(false);
        }

        void ShowChoiceDialogueObject()
        {
            _ChoiceBox.transform.DOShakeScale(0.2f, 0.2f, 8);
            _ChoiceBox.SetActive(true);
            _ChoiceMainText.text = "";
            _ChoiceText1.text = "";
            _ChoiceText2.text = "";
            _ChoiceText3.text = "";
            _ChoiceText3object.SetActive(true);
            _ChoiceText2object.SetActive(true);
        }

        void HideChoiceDialogueObject()
        {
            _ChoiceBox.transform.DOShakeScale(0.2f, 0.2f, 8);
            _ChoiceBox.SetActive(false);
            _ChoiceMainText.text = "";
            _ChoiceText1.text = "";
            _ChoiceText2.text = "";
            _ChoiceText3.text = "";
        }
        void ChangeText(Text t,string s)
        {
            t.DOText(s, 1.6f);

            if(s == string.Empty || s == null)
            {
                t.gameObject.SetActive(false);
            }
            else
            {
                t.gameObject.SetActive(true);
            }
        }

        void BackGroundChange()
        {
            switch(backui)
            {
                case BACKGROUNDUI.SCHOOL:
                    if (_SchoolBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _SchoolBackGround;
                    }
                    break;
                case BACKGROUNDUI.PCROOM:
                    if (_PCRoomBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _PCRoomBackGround;
                    }
                    break;
                case BACKGROUNDUI.LIBRARY:
                    if (_LibraryBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _LibraryBackGround;
                    }
                    break;
                case BACKGROUNDUI.HOME:
                    if (_MyHomeBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _MyHomeBackGround;
                    }
                    break;
                case BACKGROUNDUI.COFFEE:
                    if (_CoffeeBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _CoffeeBackGround;
                    }
                    break;
                case BACKGROUNDUI.CAMPUS:
                    if (_CampuseBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _CampuseBackGround;
                    }
                    break;
                case BACKGROUNDUI.BAR:
                    if (_BarBackGround != null)
                    {
                        _BackGroundObject.SetActive(true);
                        _BackGrounUI.sprite = _BarBackGround;
                    }
                    break;
            }
        }
        #endregion

        //? Choice 선택
        public void Choice1()
        {
            StartCoroutine(SoundManager._instance.Play_SE("Click2"));
            if (is_choice)
            {
                OnEvent_ClickButton(1);                
                HideChoiceDialogueObject();
            }
            else
            {
                Debug.Log("Button SEtting Waitting....");
            }
        }
        public void Choice2()
        {
            StartCoroutine(SoundManager._instance.Play_SE("Click2"));
            if (is_choice)
            {
                OnEvent_ClickButton(2);                   
                HideChoiceDialogueObject();
            }
            else
            {
                Debug.Log("Button SEtting Waitting....");
            }
        }
        public void Choice3()
        {
            StartCoroutine(SoundManager._instance.Play_SE("Click2"));
            if (is_choice)
            {
                OnEvent_ClickButton(3);
                   
                HideChoiceDialogueObject();
            }
            else
            {
                Debug.Log("Button SEtting Waitting....");
            }
        }
        void OnClickNumber(int n)
        {
            FlowNode node = null;            
            if (n == 1) node = tempchoicedb._NextChoice1Node;
            else if (n == 2) node = tempchoicedb._NextChoice2Node;
            else if (n == 3) node = tempchoicedb._NextChoice3Node;
            else
            {
                Debug.LogError("OnClickNumber Method Node null");
                return;
            }

            //? 이벤트 실행
            switch (n)
            {
                case 1:
                    //? 이벤트 실행
                    if (tempchoicedb.subevent1 != null)
                    {
                        Debug.Log("PlayEvent 1");
                        StartCoroutine(PlayEvent(1));
                    }
                    break;
                case 2:
                    //? 이벤트 실행
                    if (tempchoicedb.subevent2 != null)
                    {
                        Debug.Log("PlayEvent 2");
                        StartCoroutine(PlayEvent(2));
                    }
                    break;
                case 3:
                    //? 이벤트 실행
                    if (tempchoicedb.subevent3 != null)
                    {   
                        Debug.Log("PlayEvent 3");
                        StartCoroutine(PlayEvent(3));
                    }
                    
                    break;
            }
            switch (node._flow)
            {
                case FlowNode.FLOW.DIALOGUE:
                    LoadDialogue(node._NextNode);
                    tempchoicedb = null;
                    StartCoroutine(ShowDialogue());
                    break;
                case FlowNode.FLOW.CHOICEDIALOGUE:
                    LoadChoiceDialogue(node._ChoiceNode);
                    StartCoroutine(ShowChoiceDialogue());
                    break;
            }                              
            is_Msg = false;
            is_choice = false;
        }

        //? 이벤트 시작 코루틴
        IEnumerator PlayEvent(int n)
        {
            switch(n)
            {
                case 0:     // Dialogue
                    for (int i = 0; i < tempdb.subEvnet.Length; i++)
                    {
                        while (is_Wait)
                        {
                            yield return null;
                        }
                        Debug.Log(tempdb.subEvnet[i]);
                        EventPageManager.Instance.OnPlay(tempdb.subEvnet[i]);
                    }
                    break;
                case 1:     // Choice 1
                    for (int i = 0; i < tempchoicedb.subevent1.Length; i++)
                    {
                        while(is_Wait)
                        {
                            yield return null;
                        }
                        Debug.Log(tempchoicedb.subevent1[i]);
                        EventPageManager.Instance.OnPlay(tempchoicedb.subevent1[i]);
                    }
                    break;
                case 2:     // Choice2
                    for (int i = 0; i < tempchoicedb.subevent2.Length; i++)
                    {
                        while (is_Wait)
                        {
                            yield return null;
                        }
                        Debug.Log(tempchoicedb.subevent2[i]);
                        EventPageManager.Instance.OnPlay(tempchoicedb.subevent2[i]);
                    }
                    break;
                case 3:     // Choice 3
                    for (int i = 0; i < tempchoicedb.subevent3.Length; i++)
                    {
                        while (is_Wait)
                        {
                            yield return null;
                        }
                        Debug.Log(tempchoicedb.subevent3[i]);
                        EventPageManager.Instance.OnPlay(tempchoicedb.subevent3[i]);
                    }
                    break;
                default:    // Null
                    break;
            }
        }

        /// <summary>
        /// 대기 시간
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IEnumerator WaitMethod(float t =0.1f)
        {
            is_Wait = true;
            WaitForSeconds wait = new WaitForSeconds(t);
            yield return wait;
            is_Wait = false;
            wait = null;
        }

        /// <summary>
        /// 알람 표시
        /// </summary>
        /// <param name="msg">메세지 내용</param>
        /// <returns></returns>
        public IEnumerator AlermShow(string msg)
        {
            WaitForSeconds wait = new WaitForSeconds(4f);

            while (is_Wait)
            {
                yield return null;
            }

            _Alerm.DOText(msg, 1.2f);
            yield return wait;
            _Alerm.text = "";
            wait = null;
        }
    }
}

4.스탯에 따른 다른 스토리 및 결말

![image](https://user-images.githubusercontent.com/48191157/71571337-1edbf900-2b1d-11ea-8f4f-a35c1ab0c9be.png)
