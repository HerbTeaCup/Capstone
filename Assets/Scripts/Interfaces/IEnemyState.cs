public interface IEnemyState
{
    void Enter(); // ���� ����
    void Execute(); // ���� Ȱ��ȭ
    void Exit(); // ���� ���
    void TakeDamage(int damage); // ���� ������ ���� ��
}