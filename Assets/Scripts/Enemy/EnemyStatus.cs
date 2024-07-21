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
    public float initialAttackDelay;  // �ʱ� ���� ���� �ð�

    private void Start()
    {
        StatInit();
    }

    void StatInit()
    {
        Hp = MaxHP;
        initialAttackDelay = AttackDelay;  // �ʱ� ���� ���� �ð� ����
    }
}
