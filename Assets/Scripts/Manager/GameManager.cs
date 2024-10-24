using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IManager
{
    static GameManager _ins = null;
    InputManager _input = null;
    UIManager _ui;
    LoadingSceneManager _loadingScene;

    CameraManager _cam = new CameraManager();
    EnemyManager _enemy = new EnemyManager();
    StageManager _stage = new StageManager();
    ObjectManager _object = null;
    MissionManager _mission = null;

    public static GameManager Instance { get { Init(); return _ins; } }
    public static InputManager Input { get { return Instance._input; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static EnemyManager Enemy { get { return Instance._enemy; } }
    public static StageManager Stage { get { return Instance._stage; } }
    public static UIManager UI { get { return Instance._ui; } set { Instance._ui = value; } }
    public static LoadingSceneManager LoadingScene { get { return Instance._loadingScene; } set { Instance._loadingScene = value; } }
    public static ObjectManager Object { get { return Instance._object; } }
    public static MissionManager Mission { get { return Instance._mission; } set { Instance._mission = value; } }

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
        Object.Updater();
    }

    private void LateUpdate()
    {
        Input.LateUpdater();
        Enemy.LateUpdater();
    }

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

          
            if (_ins._mission == null)
            {
                _ins._mission = FindObjectOfType<MissionManager>();
                if (_ins._mission == null)
                {
                    GameObject missionManager = new GameObject("@MissionManager");
                    _ins._mission = missionManager.AddComponent<MissionManager>();  // AddComponent ���
                }
            }

            // LoadingSceneManager Ȯ�� �� �߰�
            if (_ins._loadingScene == null)
            {
                _ins._loadingScene = FindObjectOfType<LoadingSceneManager>();
                if (_ins._loadingScene == null)
                {
                    GameObject loadingManager = new GameObject("@LoadingSceneManager");
                    _ins._loadingScene = loadingManager.AddComponent<LoadingSceneManager>();  // AddComponent ���
                }
            }

            if (_ins._object == null)
            {
                _ins._object = FindObjectOfType<ObjectManager>();
                if (_ins._object == null)
                {
                    GameObject objectManagerObj = new GameObject("@ObjectManager");
                    _ins._object = objectManagerObj.AddComponent<ObjectManager>();  // AddComponent ���
                }
            }
        }
    }

    // PlayerInput ����: �� ��ȯ �� PlayerInput �ʱ�ȭ ����
    public void PreservePlayerInput()
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            DontDestroyOnLoad(playerInput.gameObject);
            Debug.Log("PlayerInput�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("PlayerInput�� ã�� �� �����ϴ�.");
        }
    }

    public void StartMissionManager()
    {
        if (_mission != null)
        {
            _mission.InitializeMissions(); // �� �κ��� ���� ȣ��� �� ����
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

        if (LoadingScene != null) LoadingScene.Clear();

        if (Mission != null) Mission.Clear();
    }
}
