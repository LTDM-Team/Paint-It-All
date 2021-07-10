using UnityEngine;

class PlayerInput : MonoBehaviour, IInput
{
    public bool CanShoot() => Input.GetMouseButton(0);

    public Vector2 GetLookDirection()
    {
        var mousePosition = GetWorldMousePosition();

        var lookVector = mousePosition - (Vector2)transform.position;
        var lookDirection = lookVector.normalized;

        return lookDirection;
    }
    public Vector2 GetMoveDirection()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        return new Vector2(horizontal, vertical);
    }

    private Vector2 GetWorldMousePosition()
    {
        var mousePosition = Input.mousePosition;
        return CachedCamera.Camera.ScreenToWorldPoint(mousePosition);
    }
}