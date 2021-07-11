using UnityEngine;

interface IInput
{
    bool CanShoot();

    Vector2 GetMoveDirection();
    Vector2 GetLookDirection();
    Vector2 GetLookVector();
}