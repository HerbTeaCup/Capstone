using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("Battle")]
    public int Hp;
    public int MaxHP;
    public float MoveSpeed;
    public float AttackDistance;
    public float FindDistance;
    public float AttackDelay;
    public int ATK;
    public bool isDead = false;

    [HideInInspector]
    public float initialAttackDelay;  // 초기 공격 지연 시간

    private void Start()
    {
        StatInit();
    }

    void StatInit()
    {
        Hp = MaxHP;
        initialAttackDelay = AttackDelay;  // 초기 공격 지연 시간 설정
    }
}
