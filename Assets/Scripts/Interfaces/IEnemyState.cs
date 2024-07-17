public interface IEnemyState
{
    void Enter(); // 상태 진입
    void Execute(); // 상태 활성화
    void Exit(); // 상태 벗어남
    void TakeDamage(int damage); // 적이 데미지 받을 때
}