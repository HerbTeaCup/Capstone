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
            Debug.Log($"{collision.gameObject.name}");
            unit.TakeDamage(6);
        }
    }
}
