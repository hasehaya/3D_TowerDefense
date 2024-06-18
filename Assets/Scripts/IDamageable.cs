using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface IDamageable
{
    Damageable damageable { get; set; }
    public void TakeDamage(int damage);
}
