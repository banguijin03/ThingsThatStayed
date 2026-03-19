using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    UIManager _ui;
    public UIManager UI => _ui;

    DataManager _data;
    public DataManager Data => _data;

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
        totalLoadCount+=CreateManager(ref _ui).LoadCount;
        totalLoadCount+=CreateManager(ref _data).LoadCount;
        totalLoadCount+=CreateManager(ref _save).LoadCount;
        totalLoadCount+=CreateManager(ref _setting).LoadCount;
        totalLoadCount+=CreateManager(ref _language).LoadCount;
        totalLoadCount+=CreateManager(ref _audio).LoadCount;
        totalLoadCount+=CreateManager(ref _camera).LoadCount;
        totalLoadCount+=CreateManager(ref _input).LoadCount;
        

        yield return CreateManager(ref _ui).Connect(this);
        UIManager.ClaimOpenUI(UIType.Loading);
        if(UIManager.ClaimOpenUI(UIType.Loading) is IProgress<int> loadingProgress) loadingProgress.Set(0, totalLoadCount);

        yield return _data.Connect(this);
        yield return _save.Connect(this);
        yield return _setting.Connect(this);
        yield return _language.Connect(this);
        yield return _audio.Connect(this);
        yield return _camera.Connect(this);
        yield return _input.Connect(this);
        UIManager.ClaimCloseUI(UIType.Loading);
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
        Input?.Disconnect();
        Audio?.Disconnect();
        Language?.Disconnect();
        Setting?.Disconnect();
        Save?.Disconnect();
        Camera?.Disconnect();
        UI.Disconnect();
        Data?.Disconnect();
    }

    ManagerType CreateManager<ManagerType>(ref ManagerType targetVariable) where ManagerType:ManagerBase
    {
        if (targetVariable == null)
        {
            targetVariable= this.TryAddComponent<ManagerType>();
        }
        return targetVariable;
    }
    void Update()
    {

    }
}
