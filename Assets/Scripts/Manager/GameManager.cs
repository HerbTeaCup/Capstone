using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IManager
{
    static GameManager _ins = null;
    InputManager _input = null;
    OptionManager _option = null; // 옵션 UI (나중에 컴포넌트로 추가할 예정)

    CameraManager _cam = new CameraManager();
    EnemyManager _enemy = new EnemyManager();
    StageManager _stage = new StageManager();
   
    public static GameManager Instance { get { Init(); return _ins; } }
    public static InputManager Input { get { return Instance._input; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static EnemyManager Enemy { get { return Instance._enemy; } }
    public static StageManager Stage { get { return Instance._stage; } }
    public static OptionManager Option {  get { return Instance._option; } }

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
    static void Init()
    {
        if (_ins != null) return; // 이미 초기화한 경우 다시 호출되지 않도록 방지

        GameObject temp = GameObject.Find("@GameManager");

        if (temp == null)
        {
            temp = new GameObject("@GameManager");
        }

        temp.TryGetComponent<GameManager>(out _ins);
        if (_ins == null) { _ins = temp.AddComponent<GameManager>(); }
        temp.TryGetComponent<InputManager>(out _ins._input);
        if (_ins._input == null) { _ins._input = temp.AddComponent<InputManager>(); }

        // OptionManager 컴포넌트 추가
        _ins._option = temp.AddComponent<OptionManager>();

        // 게임 상태 초기화 및 시작 준비
        Option.InitGameState();
        _ins.StartCoroutine(Option.ReadyToStart());
    }
    public void Clear()
    {
        Input.Clear();
        Cam.Clear();
        Enemy.Clear();
        Stage.Clear();
        Option.Clear();
    }
}
