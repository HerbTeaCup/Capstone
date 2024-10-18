using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, IManager
{
    static GameManager _ins = null;
    InputManager _input = null;

    CameraManager _cam = new CameraManager();
    EnemyManager _enemy = new EnemyManager();
    StageManager _stage = new StageManager();

    // MonoBehaviour
    UIManager _ui;
    LoadingSceneManager _loadingScene;

    public static GameManager Instance { get { Init(); return _ins; } }
    public static InputManager Input { get { return Instance._input; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static EnemyManager Enemy { get { return Instance._enemy; } }
    public static StageManager Stage { get { return Instance._stage; } }
    public static UIManager UI { get { return Instance._ui; } set { Instance._ui = value; } }
    public static LoadingSceneManager LoadingScene { get { return Instance._loadingScene; } set { Instance._loadingScene = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Input.Updater();
        Enemy.Updater();
    }
    private void LateUpdate()
    {
        Input.LateUpdater();
        Enemy.LateUpdater();
    }

    // GameManager가 이미 존재하면 새로 생성되지 않도록 수정
    static void Init()
    {
        if (_ins == null)
        {
            GameObject temp = GameObject.Find("@GameManager");

            if (temp == null)
            {
                temp = new GameObject("@GameManager");
                DontDestroyOnLoad(temp);
            }

            temp.TryGetComponent<GameManager>(out _ins);
            if (_ins == null) { _ins = temp.AddComponent<GameManager>(); }

            temp.TryGetComponent<InputManager>(out _ins._input);
            if (_ins._input == null) { _ins._input = temp.AddComponent<InputManager>(); }

            // LoadingSceneManager 확인 및 추가
            if (_ins._loadingScene == null)
            {
                _ins._loadingScene = FindObjectOfType<LoadingSceneManager>();
                if (_ins._loadingScene == null)
                {
                    GameObject loadingObj = new GameObject("LoadingSceneManager");
                    _ins._loadingScene = loadingObj.AddComponent<LoadingSceneManager>();
                    DontDestroyOnLoad(loadingObj);
                }
            }
        }
    }

    // PlayerInput 유지: 씬 전환 후 PlayerInput 초기화 방지
    public void PreservePlayerInput()
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            DontDestroyOnLoad(playerInput.gameObject);
            Debug.Log("PlayerInput이 유지되었습니다.");
        }
        else
        {
            Debug.LogError("PlayerInput을 찾을 수 없습니다.");
        }
    }

    public void Clear()
    {
        Debug.Log("Clearing Camera...");
        if (Cam != null) Cam.Clear();

        Debug.Log("Clearing Enemies...");
        if (Enemy != null) Enemy.Clear();

        Debug.Log("Clearing Stage...");
        if (Stage != null) Stage.Clear();

        Debug.Log("Clearing UI...");
        if (UI != null) UI.Clear();

        Debug.Log("Clearing Loading Scene...");
        if (LoadingScene != null) LoadingScene.Clear();
    }
}
