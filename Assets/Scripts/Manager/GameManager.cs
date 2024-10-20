using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IManager
{
    // MonoBehaviour
    static GameManager _ins = null;
    InputManager _input = null;
    UIManager _ui;
    LoadingSceneManager _loadingScene;

    CameraManager _cam = new CameraManager();
    EnemyManager _enemy = new EnemyManager();
    StageManager _stage = new StageManager();

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

    // GameManager�� �̹� �����ϸ� ���� �������� �ʵ��� ����
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

            // LoadingSceneManager Ȯ�� �� �߰�
            if (_ins._loadingScene == null)
            {
                _ins._loadingScene = FindObjectOfType<LoadingSceneManager>();
                if (_ins._loadingScene == null)
                {
                    // ���⿡�� ���ο� ������Ʈ�� ������ �ʰ� ���� ������Ʈ Ȯ��
                    GameObject loadingSceneObj = GameObject.Find("@LoadingSceneManager");
                    if (loadingSceneObj == null)
                    {
                        // LoadingSceneManager�� ���� ���� ���� ����
                        loadingSceneObj = new GameObject("@LoadingSceneManager");
                        _ins._loadingScene = loadingSceneObj.AddComponent<LoadingSceneManager>();
                    }
                    else
                    {
                        // �̹� �����ϴ� ��� ã��
                        _ins._loadingScene = loadingSceneObj.GetComponent<LoadingSceneManager>();
                    }
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
    }
}
