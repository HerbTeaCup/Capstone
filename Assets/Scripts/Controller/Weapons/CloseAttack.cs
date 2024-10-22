using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        IUnitDamageable enemy;

        if (other.TryGetComponent<IUnitDamageable>(out enemy))
        {
            enemy.TakeDamage(5);
            this.transform.parent.GetComponent<PlayerStatus>().Sound.HitSoundPlay();
        }
    }
}
