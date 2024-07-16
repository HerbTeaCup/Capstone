public interface IEnemyState
{
    float FindDistance { get; set; }
    float AttackDistance { get; set; }
    float MoveSpeed { get; set; }
    int ATK { get; set; }
    int HP { get; set; }
    float AttackDelay { get; set; }
    float CurrentTime { get; set; }

    void Enter(); // 상태 진입
    void Execute(); // 상태 활성화
    void Exit(); // 상태 벗어남
    void TakeDamage(int damage); // 적이 데미지 받을 때
}