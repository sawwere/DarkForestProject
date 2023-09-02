using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    float GetSpeed();
    Vector3 GetPosition();

    void SpeedResetToZero();
    void SpeedRestore();
}
