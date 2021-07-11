using UnityEngine;

class PlayerInput : MonoBehaviour, IInput
{
    public bool CanShoot() => Input.GetMouseButton(0);

    public Vector2 GetMoveDirection()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        return new Vector2(horizontal, vertical);
    }

    public Vector2 GetLookDirection()
    {
        var lookDirection = GetLookVector().normalized;
        return lookDirection;
    }
    public Vector2 GetLookVector()
    {
        var mousePosition = GetWorldMousePosition();
        var lookVector = mousePosition - (Vector2)transform.position;

        return lookVector;
    }

    private Vector2 GetWorldMousePosition()
    {
        var mousePosition = Input.mousePosition;
        return CachedCamera.Camera.ScreenToWorldPoint(mousePosition);
    }
}