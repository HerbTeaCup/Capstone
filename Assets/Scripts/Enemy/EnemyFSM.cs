using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour, IUnitDamageable
{
    // State Variable
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }
    EnemyState _state;

    [SerializeField] float findDistance = 8f; // Find the distance of player
    [SerializeField] float attackDistance = 2f; // Attackable range
    [SerializeField] float moveSpeed = 5f; // Move speed
    [SerializeField] int ATK = 5; // Attack damage 
    [SerializeField] int hp = 100;

    // 일정 시간 간격으로 공격하기 위한 변수들
    float currentTime = 0; // cumulative time
    float attackDelay = 2f; // attack delay time

    CharacterController cc;
    Transform player;

    void Start()
    {
        // Initial state is Idle state.
        _state = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        cc = GetComponent<CharacterController>();
    }


    void Update()
    {
        // 현재 Enemy 어떤 state?
        switch (_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged: // 상태 전환 시 1회만 실행
                //Damaged();
                break;
            case EnemyState.Die: // 상태 전환 시 1회만 실행
                //Die();
                break;
        }
    }
    
    void Idle()
    {
        // If, distance between player and enemy in action start range -> convert move state
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            _state = EnemyState.Move;
            print("Idle -> Move"); // 잘 작동하는지 확인하기 위한 출력문
        }
    }

    void Move()
    {
        // If there is distance between player and enemy out of attack range, the enemy moves forward to the player.
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized; // Setting move direction
            
            cc.Move(dir * moveSpeed * Time.deltaTime); // move using the Character Controller Component
        }
        else // The current state convert to the attack state.
        {
            _state = EnemyState.Attack;
            print("Move -> Attack"); // 잘 작동하는지 확인하기 위한 출력문

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓음
            currentTime = attackDelay;
        }
    }

    public void Attack()
    {
        // If there is distance between player and enemy in attack range, the enemy attacks the player.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().TakeDamage(ATK); PlayerTemp파일 지워서 임시 주석처리
                currentTime = 0;

                print("공격"); // 잘 작동하는지 확인하기 위한 출력문
            }
        }
        else // The current state convert to the move state. (Rechase)
        {
            _state = EnemyState.Move;
            print("Atack -> Move"); // 잘 작동하는지 확인하기 위한 출력문
            currentTime = 0;
        }
    }

    void Damaged()
    {
        // 특정 코루틴 함수가 끝날 때까지 대기
        StartCoroutine(DmgProcess());
    }

    public void TakeDamage(int dmg)
    {
        // 이미 피격 상태이거나 사망 상태라면 아무런 처리 X 함수 종료
        if (_state == EnemyState.Damaged || _state == EnemyState.Die)
        {
            return;
        }

        // The enemy hp decreased by the player ATK
        hp -= dmg;
        print($"적 hp = {hp}");

        if (hp > 0) // If the enemy hp is bigger than 0, any state converts to the damaged state.
        {
            _state = EnemyState.Damaged;
            print("Any state -> Damaged"); // 잘 작동하는지 확인하기 위한 출력문
            Damaged();
        }
        else
        {
            _state = EnemyState.Die;
            print("Any state -> Damaged"); // 잘 작동하는지 확인하기 위한 출력문
            Die();
        }
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DmgProcess()
    {
        // 피격 모션 시간만큼 기다림
        yield return new WaitForSeconds(0.5f);

        // The current state converts to the move state.
        _state = EnemyState.Move;
        print("Damaged -> Move");
    }
    
    void Die()
    {
        StopAllCoroutines(); // 진행 중인 피격 코루틴 중지
        StartCoroutine(DieProcess()); // 죽음 상태를 처리하기 위한 코루틴 실행
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false; // Disabled the Character Contoller Component 

        // Remove the enemy after 2 seconds
        yield return new WaitForSeconds(2f); // 지정된 시간(초) 동안 대기
        print("적 제거");
        Destroy(gameObject); // Remove the enemy
    }
}
