﻿using UnityEngine;

class AIInput : MonoBehaviour, IInput
{
    public bool CanShoot() => false;

    public Vector2 GetLookDirection() => GetMoveDirection();
    public Vector2 GetMoveDirection()
    {
        var horizontal = Mathf.Sin(Time.time) / 5f;
        var vertical = Mathf.Cos(Time.time) / 5f;

        return new Vector2(horizontal, vertical);
    }

}