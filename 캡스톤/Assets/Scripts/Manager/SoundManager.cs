using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;        // 오디오 믹서

#region 사운드 데이터 베이스
[System.Serializable]
//TODO: 곡 저장하는 클래스
public class Sound
{
	//TODO: 변수 선언
	[Title("File Name")]
	[Tooltip("Mp3 파일 닉네임을 설정합니다. 나중에 음악 재생이 필요할 때 이 닉네임을 사용해서 작동시킨다.")]
    [InlineButton("AutoName", "A")]
    public string name;         //? Mp3 파일 닉네임 (이걸로 음악 실행)

	[Title("File List")]
	[Tooltip("Mp3 파일을 담는 곡입니다.")]
    [InlineEditor]
	public AudioClip clip;      //? Mp3 File 을 집어넣습니다.

    [Title("Infomation")]
    [Tooltip("사운드 설명")]
    [HideLabel]
    public string info;         //? MP3 File의 설명서

    // InlineButton
    private void AutoName()
    {
        name = clip.name;
    }
}
#endregion


[TypeInfoBox("사운드 메니져 시스템\nBGM,SE,Voice,3D ADD Sound\n 시스템 이 있는 스크립트이다.")]
public class SoundManager : SerializedMonoBehaviour
{

	//TODO: 사운드 매니져 인스턴스
	static public SoundManager _instance;    //! 싱글턴화 시킨다. (공유자원)

    const int SOUND_LENGTH = 7;                     //! 배열 크기

	#region 싱글턴
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);      // 씬이 로드 될 때 자동으로 파괴되지 않는 오브젝트

        }
		else
			Destroy(this.gameObject);           // 연결되 있는 오브젝트를 삭제한다. ( SoundManager 2개가 될경우 발동)
	}

    void Start()
    {
        StartCoroutine(CheckSE());
        audioSource_SE_name = new string[SOUND_LENGTH];

        bgm_volume = PlayerPrefs.GetFloat("bgm", 0f);
        se_volume = PlayerPrefs.GetFloat("se", 0f);
        voice_volume = PlayerPrefs.GetFloat("voice", 0f);
        master_volume = PlayerPrefs.GetFloat("master", 0f);

        StartCoroutine(StartVolume());
    }

    #endregion

    #region 변수
    //TODO: 전역변수 설정
    [Title("오디오 컴포넌트")]
    [InfoBox("Audio Component 을 넣어놓는 곳입니다. \n 화이팅 !!")]
    [FoldoutGroup("Audio Component"), LabelText("배경음악 컨트롤러")]
	public AudioSource audioSource_BGM;    //! 배경음악 전용 컴퍼넌트

    [FoldoutGroup("Audio Component"), LabelText("효과음 컨트롤러")]
    public AudioSource[] audioSource_SE =new AudioSource[7];       //! 효과음 전용 컴퍼넌트

    [FoldoutGroup("Audio Component"), LabelText("보이스 컨트롤러")]
    public AudioSource audioSource_Voice;

    [FoldoutGroup("Audio Mixer"), LabelText("오디오 그룹")]
    public AudioMixer audiomixer;

    [Title("음악파일")]
    [InfoBox("Audio File 을 넣어놓는 곳입니다. \n 화이팅 !!")]
    
    [TabGroup("BGM Sound File")]
    [TableList, LabelText("BGM File")]
    public Sound[] bgm_file;

    [TabGroup("SE Sound File")]
    [TableList, LabelText("SE File")]
    public Sound[] se_file;

    [TabGroup("Voice Sound File")]
    [TableList, LabelText("Voice File")]
    public Sound[] voice_file;

    [InfoBox("안 건드려도 됩니다.")]
    public string[] audioSource_SE_name;    

    #region 사운드 소리 조절
    [BoxGroup("Infomation")]
    [InfoBox("마스터 볼륨 설정 ")]
    [Title("Master Volume"), HideLabel]
    [ProgressBar(-80f, 0f, ColorMember = "GetMasterColor", BackgroundColorMember = "GetBackgroundColor", DrawValueLabel = false)]
    public float master_volume = 0f;

    [BoxGroup("Infomation")]
    [InfoBox("배경음악 볼륨 설정 ")]
    [Title("BGM Volume"),HideLabel]
    [ProgressBar(-80f,0f,ColorMember ="GetBgmColor",BackgroundColorMember ="GetBackgroundColor",DrawValueLabel =false)]
    public float bgm_volume = 0f;

    [BoxGroup("Infomation")]
    [InfoBox("효과음 볼륨 설정 ")]
    [Title("SE Volume"), HideLabel]
    [ProgressBar(-80f, 0f, ColorMember = "GetSEColor", BackgroundColorMember = "GetBackgroundColor", DrawValueLabel = false)]
    public float se_volume = 0f;

    [BoxGroup("Infomation")]
    [InfoBox("목소리 볼륨 설정 ")]
    [Title("Voice Volume"),HideLabel]
    [ProgressBar(-80f, 0f, ColorMember = "GetVoiceColor", BackgroundColorMember = "GetBackgroundColor", DrawValueLabel = false)]
    public float voice_volume = 0f;


    protected bool is_sound = false;

    // Audio Component Check  [True = Check OK, False = Check Not]
    private bool se_component = false;

    [BoxGroup("Infomation")]
    [Button("설정",ButtonSizes.Medium)]
    private void SetSoundVolume()
    {
        if(!is_sound)
        StartCoroutine(StartVolume());
    }

    [BoxGroup("Infomation")]
    [Button("음소거", ButtonSizes.Medium)]
    private void SetSoundNullVolume()
    {
        master_volume = 0f;
        bgm_volume = 0f;
        se_volume = 0f;
        voice_volume = 0f;
        if (!is_sound)
            StartCoroutine(StartVolume());
        
    }
    [BoxGroup("Infomation")]
    [Button("최대", ButtonSizes.Medium)]
    private void SetSoundMaxVolume()
    {
        bgm_volume = 1f;
        se_volume = 1f;
        voice_volume = 1f;
        master_volume = 1f;
        if (!is_sound)
            StartCoroutine(StartVolume());

    }

    /// <summary>
    /// 볼륨을 설정한다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartVolume()
    {
        is_sound = true;        // Start Corutine

        yield return null;

        audiomixer.SetFloat("MasterVolume", master_volume);
        audiomixer.SetFloat("BGMVolume", bgm_volume);
        audiomixer.SetFloat("SEVolume", se_volume);
        audiomixer.SetFloat("VoiceVolume", voice_volume);
       
        yield return new WaitForSeconds(1f);

        is_sound = false;
    }

    //Todo: MainMenu BGM, SE, Voice, Master Audio Setting
    public void ChangeMaster(float f)
    {
        // Master Volume Setting
        master_volume = (80 * f) - 80;
        PlayerPrefs.SetFloat("master", master_volume);
        audiomixer.SetFloat("MasterVolume", master_volume );
    }
    public void ChangeBGM(float f)
    {
        // Master Volume Setting
        bgm_volume = (80 * f) - 80;
        PlayerPrefs.SetFloat("bgm", bgm_volume);
        audiomixer.SetFloat("BGMVolume",bgm_volume);
    }
    public void ChangeSE(float f)
    {
        // Master Volume Setting
        se_volume = (80 * f) - 80;
        PlayerPrefs.SetFloat("se", se_volume);
        audiomixer.SetFloat("SEVolume", se_volume);
    }
    public void ChangeVoice(float f)
    {
        // Master Volume Setting
        voice_volume = (80 * f) - 80;
        PlayerPrefs.SetFloat("voice", voice_volume);
        audiomixer.SetFloat("VoiceVolume", voice_volume);
    }
    #endregion

    // 프로그래스바 메소드 실행
    #region PrograssBar Method
    private Color GetBgmColor()
    {
        return bgm_volume > 0.7f ? Color.green : bgm_volume > 0.4f ? new Color(0f, 1f, 0.6f) : new Color(0f, 1f, 1f);
    }
    private Color GetBackgroundColor() => Color.white;

    private Color GetSEColor()
    {
        return se_volume > 0.7f ? Color.blue : se_volume > 0.4f ? new Color(0.5f, 0f, 1f) : new Color(1f, 0f, 1f);
    }
    private Color GetVoiceColor()
    {
        return voice_volume > 0.7f ? Color.red : voice_volume > 0.4f ? new Color(1f, 0.4f, 0.4f) : new Color(1f, 0.8f, 0.7f);
    }
    private Color GetMasterColor()
    {
        return Color.black;
    }
    #endregion








    #endregion
    //TODO 사용방법
    //? 다른 C# 스크립트로 가서 string 변수로 음악 닉네임을 적은 다음
    //!? 재생시키는건  StartCorutine 을 사용하여 실행하지만
    //!? 멈추는거는 코루틴이 아닌 void 형으로 되어 있길 때문에 StartCorutine 을 사용 안한다.
    //x SoundManager._instance.PlaySE

    #region 배경음악

    public void Play_BGM(string _n, bool b = true, float _volume = 1f) 
    {
            StartCoroutine(BGM(_n,b,_volume));
    }
    public IEnumerator BGM(string _name, bool is_loop = true, float _v = 1f)
    {
       
        //!? BGM AudioComponent 확인
        if(audioSource_BGM == null)
        {
            Debug.LogError("AudioSource BGM Component 가 비어있는 상태입니다.");
            yield break;
        }

        //!? BGM 실행 중인가?
        if (audioSource_BGM.isPlaying)
        {
            //ConsoleProDebug.Watch("Before BGM Music", audioSource_BGM.clip.name);
            //ConsoleProDebug.Watch("After BGM Music Change", _name);

            yield return StartCoroutine(UpdateBGMFade());        // BGM Fade out in
            audioSource_BGM.clip = null;        // BGM Clip 빼기
        }

        //!? MP3 파일 찾기
        for(int i =0; i < bgm_file.Length; i++)
        {
            //!? Sound Class에 저장되어 있는 name 변수랑 같은지 확인
            if(_name == bgm_file[i].name)
            {
                audioSource_BGM.clip = bgm_file[i].clip;        //!? BGM 클립 삽입
                audioSource_BGM.loop = is_loop;                     //!? 무한루프 설정
                audioSource_BGM.volume = _v;
                audioSource_BGM.Play();                                     //!? 재생
                //ConsoleProDebug.Watch("BGM Music", "Play");
                yield break;
            }
        }

        yield break;
    }
    //Todo: BGM  페이드
    IEnumerator UpdateBGMFade()
    {
        while(audioSource_BGM.volume >= 0.4f)
        {
            audioSource_BGM.volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
        }
        audioSource_BGM.Stop();
        audioSource_BGM.volume = 1f;
    }
    IEnumerator UpdateVoiceFade()
    {
        while (audioSource_Voice.volume >= 0.4f)
        {
            audioSource_Voice.volume -= 0.1f;
            yield return new WaitForSeconds(0.2f);
        }
        audioSource_Voice.Stop();
        audioSource_Voice.volume = 1f;
    }

    //Todo: BGM Stop
    public void StopBGM()
    {
        StartCoroutine(VolumeAnimation());
    }

    public void STOPPLAYBGM(string msg)
    {

    }

    IEnumerator VolumeAnimation()
    {
        while(true)
        {
            if (audioSource_BGM.volume <= 0.2)
                break;

            audioSource_BGM.volume -= 0.05f;
            yield return null;
        }

        audioSource_BGM.volume = 1f;
        audioSource_BGM.Stop(); //! 배경음악을 정지시킨다.
    }

    //Todo: BGM Loop 해제
    public void BGMLoopFalse()
    {
        audioSource_BGM.loop = false;
    }

    #endregion

    #region 효과음
    public IEnumerator Play_SE(string _name, bool is_loop = false, int n = -1, float _volume = 1f)
    {
        int index = 0;          // AudioSound Component Index
        int audionumber = 0;

        //!? 컴퍼넌트 확인
        if (!se_component) {yield return StartCoroutine(CheckSE()); }

        //!? 컴퍼넌트 아직도 확인 안 될 경우
        if (!se_component) {  yield break; }

        //!? 사라질 경우
        if(audioSource_SE_name.Length != SOUND_LENGTH) { audioSource_SE_name = new string[SOUND_LENGTH]; }

        //!? MP3 파일 검색
        for (int i = 0; i < se_file.Length; i++)
        {
            if (_name == se_file[i].name)
            {
                //? 파일이름이 같은 경우
                audionumber = i;
                //ConsoleProDebug.Watch("SoundManager MP3 File Find Index", string.Format("{0}",audionumber));
                break;
            }
        }

        //!? 재생중이지 않는 오디오 찾기
        if (n == -1)
        {
            for (int i = 0; i < audioSource_SE.Length; i++)
            {
                if (!audioSource_SE[i].isPlaying)
                {
                    //? SE 파일이 재생중이 아닐 때
                    if (audioSource_SE[i].clip != null)
                        audioSource_SE[i].clip = null;

                    audioSource_SE_name[i] = null;
                    audioSource_SE[i].loop = false; // 반복해제
                    audioSource_SE[i].volume = 1f;

                    //? 인덱스 값 조정
                    index = i;

                    //ConsoleProDebug.Watch("Clear SE Music", audioSource_SE[i].clip.name);

                    break;
                }
            }

            yield return null;

            audioSource_SE[index].clip = se_file[audionumber].clip;       // 클립 등록
            audioSource_SE_name[index] = se_file[audionumber].name;       // 이름 등록
            audioSource_SE[index].loop = is_loop;
            audioSource_SE[index].volume = _volume;
            //? 파일 재생
            audioSource_SE[index].Play();           // SE Play

        }
        else
        {
            if( n < audioSource_SE.Length)
            {
                audioSource_SE[n].clip = null;
                audioSource_SE_name[n] = null;
                audioSource_SE[n].loop = is_loop;

                
            }
            else
            {
                //ConsoleProDebug.Watch("SoundManager Error", string.Format("경고 !! n :{0} 값이 audioSource_SE 인덱스 값을 넘어섰습니다.",n));
            }

            audioSource_SE[n].clip = se_file[audionumber].clip;       // 클립 등록
            audioSource_SE_name[n] = se_file[audionumber].name;       // 이름 등록
            audioSource_SE[n].loop = is_loop;
            //? 파일 재생
            audioSource_SE[n].Play();           // SE Play
        }
        

        

        yield return null;
    }

    



    #endregion
    #region 보이스
    //Todo: Voice Play
    public void Play_Voice(string _name)
    {
            StartCoroutine(Voice(_name));
    }

    //Todo: Voice Home
    public IEnumerator Voice(string _name)
    {
        //!? 컴퍼넌트 확인
        if (audioSource_Voice == null)
        {
            Debug.LogError("Voice AudioComponent 등록이 안되어있습니다.");
            yield break;
        }

        yield return null;

        //!? Voice Mp3 파일 찾기
        for(int i = 0; i < voice_file.Length; i++)
        {
            if(_name ==voice_file[i].name)
            {
                yield return StartCoroutine(UpdateVoiceFade());
                audioSource_Voice.clip = voice_file[i].clip;
                audioSource_Voice.Play();

            } // if
        } // for

    }


    public void StopVoice()
    {
        StartCoroutine(UpdateVoiceFade());
    }
    #endregion







   

    /// <summary>
    /// 실행중인 효과음을 모두 정지시킨다.
    /// void 형식이라 StartCorutine 안써도 됨.
    /// </summary>
    public void StopAllSE()
	{
		for (int i = 0; i < audioSource_SE.Length; i++)
		{
			audioSource_SE[i].Stop();           //! 효과음 정지
            audioSource_SE[i].loop = false;
            audioSource_SE[i].volume = 1f;
            audioSource_SE_name[i] = null;
		}

	}

    /// <summary>
    /// SceneManager.cs 파일에서 _name 에 해당하는 효과음파일을 찾아
    /// 정지시킨다. StartCorutine 사용
    /// </summary>
    /// <param name="_name"></param>
    public IEnumerator StopSE(string _name = null, bool loop = false, int n = -1)
	{
        if (_name == null) yield break;

        if (n == -1)
        {
            for (int i = 0; i < audioSource_SE_name.Length; i++)
            {
                if (_name == audioSource_SE_name[i])
                {
                    audioSource_SE[i].Stop();
                    audioSource_SE[i].loop = loop;
                    audioSource_SE[i].volume = 1f;
                    audioSource_SE_name[i] = null;
                    yield return null;

                }
            }
        }
        else
        {
            audioSource_SE[n].Stop();
            audioSource_SE[n].loop = loop;
            audioSource_SE_name[n] = null;
        }
	}


	//Todo 오디오 소스 청소
	public void AudioSource_clear()
	{
		//! BGM 과 SE 전용 오디오 소스 컴퍼넌트를  비운다.
		if (audioSource_BGM.clip != null)
		{
			//! 청소
			audioSource_BGM.clip = null;
		}
		for (int i = 0; i < audioSource_SE.Length; i++)
		{
			if(audioSource_SE[i].clip != null)
			{
				//! 청소
				audioSource_SE[i].clip = null;
            }
		}

        if (audioSource_Voice.clip != null)
        {
            audioSource_Voice.clip = null;
        }
    }

    //Todo: SE AudioSource Component Check
    IEnumerator CheckSE()
    {
        for(int n = 0; n < audioSource_SE.Length; n++)
            if(audioSource_SE[n] == null)
            {
                Debug.LogError("효과음 전용 AudioComponent 등록되지 않았습니다.");
                yield break;
            }

        se_component = true;
        yield return null;
    }
}
