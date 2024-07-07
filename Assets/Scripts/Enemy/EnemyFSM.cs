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

    // ���� �ð� �������� �����ϱ� ���� ������
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
        // ���� Enemy � state?
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
            print("Idle -> Move"); // �� �۵��ϴ��� Ȯ���ϱ� ���� ��¹�
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
            print("Move -> Attack"); // �� �۵��ϴ��� Ȯ���ϱ� ���� ��¹�

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������� ����
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
                print("����"); // �� �۵��ϴ��� Ȯ���ϱ� ���� ��¹�
            }
        }
        else // The current state convert to the move state. (Rechase)
        {
            _state = EnemyState.Move;
            print("Atack -> Move"); // �� �۵��ϴ��� Ȯ���ϱ� ���� ��¹�
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
