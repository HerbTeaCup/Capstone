public interface IEnemyState
{
    void Enter(EnemyFSM enemy); // 상태에 진입할 때 호출됩니다.
    void Execute(); // 상태가 활성화된 동안 반복적으로 호출됩니다.
    void Exit(); // 상태를 종료할 때 호출됩니다.
    void TakeDamage(int damage); // 데미지를 받을 때 호출됩니다.
}
