public interface IEnemyState
{
    float FindDistance { get; set; }
    float AttackDistance { get; set; }
    float MoveSpeed { get; set; }
    int ATK { get; set; }
    int HP { get; set; }
    float AttackDelay { get; set; }
    float CurrentTime { get; set; }

    void Enter(); // ���� ����
    void Execute(); // ���� Ȱ��ȭ
    void Exit(); // ���� ���
    void TakeDamage(int damage); // ���� ������ ���� ��
}