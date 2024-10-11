using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 moveDir;
    private void Start()
    {
        moveDir = transform.forward;
    }
    private void OnCollisionEnter(Collision collision)
    {
        IUnitDamageable unit;
        EnemyLook enemyLook;
        if (collision.transform.TryGetComponent<IUnitDamageable>(out unit))
        {
            unit.TakeDamage(6);
        }
        if (collision.transform.TryGetComponent<EnemyLook>(out enemyLook))
        {
            enemyLook.LookFor(moveDir);
        }
    }
}
