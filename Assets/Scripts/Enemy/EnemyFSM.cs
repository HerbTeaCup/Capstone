using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
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
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
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
    void Attack()
    {
        // If there is distance between player and enemy in attack range, the enemy attacks the player.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
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

    }
    void Die()
    {

    }
}
