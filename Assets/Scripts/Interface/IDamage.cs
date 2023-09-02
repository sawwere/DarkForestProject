using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { wall, projectile, close_combat, stakes};

public interface IDamage
{
    void RecieveDamage(int amount, DamageType dt);

    void DoDamage(IDamage obj);

    int GetLine();
}
