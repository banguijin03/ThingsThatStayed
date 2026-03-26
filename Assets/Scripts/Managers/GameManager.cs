using System.Collections;
using UnityEngine;


public delegate void InitializeEvent();
public delegate void UpdateEvent(float deltaTime);
public delegate void DestroyEvent();

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    UIManager _ui;
    public UIManager UI => _ui;

    DataManager _data;
    public DataManager Data => _data;

    ObjectManager _objectM;
    public ObjectManager ObjectM => _objectM;

    SaveManager _save;
    public SaveManager Save => _save;

    SettingManager _setting;
    public SettingManager Setting => _setting;

    LanguageManager _language;
    public LanguageManager Language => _language;

    AudioManager _audio;
    public AudioManager Audio => _audio;

    CameraManager _camera;
    public CameraManager Camera => _camera;

    InputManager _input;
    public InputManager Input => _input;

    IEnumerator initializing;


    public static event InitializeEvent OnInitializeManager;
    public static event InitializeEvent OnInitializeController;
    public static event InitializeEvent OnInitializeCharacter;
    public static event InitializeEvent OnInitializeObject;

    public static event UpdateEvent OnUpdateManager;
    public static event UpdateEvent OnUpdateController;
    public static event UpdateEvent OnUpdateCharacter;
    public static event UpdateEvent OnUpdateObject;

    public static event DestroyEvent OnDestroyManager;
    public static event DestroyEvent OnDestroyController;
    public static event DestroyEvent OnDestroyCharacter;
    public static event DestroyEvent OnDestroyObject;

    bool isLoading = true;
    bool isPlaying = true;

    void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        initializing = InitializeManagers();

        StartCoroutine(initializing);
    }
    void OnDestroy()
    {
        if (initializing != null) StopCoroutine(initializing);
        DeleteManagers();
    }
    IEnumerator InitializeManagers()
    {
        int totalLoadCount = 0;
        totalLoadCount += CreateManager(ref _ui).LoadCount;
        totalLoadCount += CreateManager(ref _data).LoadCount;
        totalLoadCount += CreateManager(ref _objectM).LoadCount;
        totalLoadCount += CreateManager(ref _save).LoadCount;
        totalLoadCount += CreateManager(ref _setting).LoadCount;
        totalLoadCount += CreateManager(ref _language).LoadCount;
        totalLoadCount += CreateManager(ref _audio).LoadCount;
        totalLoadCount += CreateManager(ref _camera).LoadCount;
        totalLoadCount += CreateManager(ref _input).LoadCount;


        yield return CreateManager(ref _ui).Connect(this);
        UIBase loadingUI = UIManager.ClaimOpenUI(UIType.Loading);
        IProgress<int> loadingProgress = loadingUI as IProgress<int>;

        loadingProgress?.Set(0, totalLoadCount);
        yield return _data.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _objectM.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _save.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _setting.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _language.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _audio.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _camera.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return _input.Connect(this);
        loadingProgress?.AddCurrent(1);
        yield return new WaitForSeconds(1.0f);
        UIManager.ClaimCloseUI(UIType.Loading);
        isLoading = false;
        /* 노가다하면 이렇게됨
        if (_ui == null)
        {
            _ui= gameObject.AddComponent<UIManager>();
            _ui.Connect(this);
        }
        if (_data == null)
        {   
            _data = gameObject.AddComponent<DataManager>();
            _data.Connect(this);
        }
        if (_save == null)
        {   
            _save = gameObject.AddComponent<SaveManager>();
            _save.Connect(this);
        }
        if (_setting == null)
        {    
            _setting = gameObject.AddComponent<SettingManager>();
            _setting.Connect(this);
        }
        if (_language == null)
        {    
            _language = gameObject.AddComponent<LanguageManager>();
            _language.Connect(this);
        }
        if (_audio == null)
        {    
            _audio = gameObject.AddComponent<AudioManager>();
            _audio.Connect(this);
        }
        if (_camera == null)
        {    
            _camera = gameObject.AddComponent<CameraManager>();
            _camera.Connect(this);
        }
        if (_input == null)
        {    
            _input = gameObject.AddComponent<InputManager>();
            _input.Connect(this);
        }*/

    }
    void DeleteManagers()
    {
        //유저입력
        Input?.Disconnect();
        //오브젝트
        ObjectM?.Disconnect();
        //오디오
        Audio?.Disconnect();
        //언어
        Language?.Disconnect();
        //세팅
        Setting?.Disconnect();
        //세이브
        Save?.Disconnect();
        //카메라
        Camera?.Disconnect();
        //ui
        UI.Disconnect();
        //데이터파일
        Data?.Disconnect();
    }
    ManagerType CreateManager<ManagerType>(ref ManagerType targetVariable) where ManagerType : ManagerBase
    {
        if (targetVariable == null)
        {
            targetVariable = this.TryAddComponent<ManagerType>();
        }
        return targetVariable;

    }
    public static void Pause()
    {
        Instance.isPlaying = false;
    }
    public static void Unpause()
    {
        Instance.isPlaying = true;
    }

    public void InvokeInitializeEvent(ref InitializeEvent OriginEvent)
    {
        if (OriginEvent != null)
        {
            InitializeEvent currentEvent = OriginEvent;
            OriginEvent = null;
            currentEvent.Invoke();
        }
    }
    public void InvokeDestroyEvent(ref DestroyEvent OriginEvent)
    {
        if (OriginEvent != null)
        {
            DestroyEvent currentEvent = OriginEvent;
            OriginEvent = null;
            currentEvent.Invoke();
        }
    }
    
    void Update()
    {
        if (isLoading) return;

        //매니저를 초기화
        InvokeInitializeEvent(ref OnInitializeManager);
        //캐릭터를 초기화
        InvokeInitializeEvent(ref OnInitializeCharacter);
        //컨트롤러를 초기화
        InvokeInitializeEvent(ref OnInitializeController);
        //오브젝트를 초기화
        InvokeInitializeEvent(ref OnInitializeObject);

        if (isPlaying)
        {
            //프레임 사이에 몇초가 지났는지
            float deltaTime=Time.deltaTime;
            //매니저가 업테이트하는 경우
            OnUpdateManager?.Invoke(deltaTime);
            //컨트롤러를 업데이트한다
            OnUpdateController?.Invoke(deltaTime);
            //캐릭터를 업데이트한다
            OnUpdateCharacter?.Invoke(deltaTime);
            //오브젝트를 업데이트한다
            OnUpdateObject?.Invoke(deltaTime);
        }

        //오브젝트를 제거
        InvokeDestroyEvent(ref OnDestroyObject);
        //컨트롤러를 제거
        InvokeDestroyEvent(ref OnDestroyController);
        //캐릭터를 제거
        InvokeDestroyEvent(ref OnDestroyCharacter);
        //매니저를 제거
        InvokeDestroyEvent(ref OnDestroyManager);
    }
}