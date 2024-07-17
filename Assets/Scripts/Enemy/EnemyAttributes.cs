using UnityEngine;

[System.Serializable]
public class EnemyAttributes
{
    [SerializeField] private float findDistance = 8f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int atk = 5;
    [SerializeField] private int hp = 100;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float currentTime = 0f;

    public float FindDistance
    {
        get { return findDistance; }
        set { findDistance = value; }
    }

    public float AttackDistance
    {
        get { return attackDistance; }
        set { attackDistance = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public int ATK
    {
        get { return atk; }
        set { atk = value; }
    }

    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public float AttackDelay
    {
        get { return attackDelay; }
        set { attackDelay = value; }
    }

    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
}