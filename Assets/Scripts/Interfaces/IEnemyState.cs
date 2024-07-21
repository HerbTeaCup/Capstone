public interface IEnemyState
{
    void Enter(EnemyFSM enemy); // ���¿� ������ �� ȣ��˴ϴ�.
    void Execute(); // ���°� Ȱ��ȭ�� ���� �ݺ������� ȣ��˴ϴ�.
    void Exit(); // ���¸� ������ �� ȣ��˴ϴ�.
    void TakeDamage(int damage); // �������� ���� �� ȣ��˴ϴ�.
}
