using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager
{
    static GameManager _ins = null;
    InputManager _input = null;

    CameraManager _cam = new CameraManager();
    EnemyManager _enemy = new EnemyManager();
    StageManager _stage = new StageManager();
    UIManager _ui = new UIManager();

    public static GameManager Instance { get { Init(); return _ins; } }
    public static InputManager Input { get { return Instance._input; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static EnemyManager Enemy { get { return Instance._enemy; } }
    public static StageManager Stage { get { return Instance._stage; } }
    public static UIManager UI { get {  return Instance._ui; } }

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
        if (_ins == null)
        {
            GameObject temp = GameObject.Find("@GameManager");

            if (temp == null)
            {
                temp = new GameObject("@GameManager");
            }

            temp.TryGetComponent<GameManager>(out _ins);
            if (_ins == null) { _ins = temp.AddComponent<GameManager>(); }
            temp.TryGetComponent<InputManager>(out _ins._input);
            if (_ins._input == null) { _ins._input = temp.AddComponent<InputManager>(); }
        }
    }
    public void Clear()
    {
        Input.Clear();
        Cam.Clear();
        Enemy.Clear();
        Stage.Clear();
        UI.Clear();
    }
}
