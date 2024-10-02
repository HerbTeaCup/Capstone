using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        IUnitDamageable unit;
        if (collision.transform.TryGetComponent<IUnitDamageable>(out unit))
        {
            unit.TakeDamage(6);
        }
    }
}
