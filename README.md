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
    }
    
    private void Update()
    {
      
        if  // 조이스틱
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
            currentWalkCount = 0;                        
        }
        _PlayerAnimator.SetBool("Walking", false);
        is_canMove = true;
            }

3.캐릭터와 상호작용 및 선택답안1

![image](https://user-images.githubusercontent.com/48191157/71571323-108ddd00-2b1d-11ea-9f29-8f8f03256b08.png)

3_1.다이얼로그를 이용한 스토리 전개

![image](https://user-images.githubusercontent.com/48191157/71571329-184d8180-2b1d-11ea-88c7-c22b7e69c441.png)
   
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
}

4.스탯에 따른 다른 스토리 및 결말

![image](https://user-images.githubusercontent.com/48191157/71571337-1edbf900-2b1d-11ea-8f4f-a35c1ab0c9be.png)
