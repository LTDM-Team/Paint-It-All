using UnityEngine;

[RequireComponent(typeof(IInput))]
[RequireComponent(typeof(Rigidbody2D))]
class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

    private IInput _input;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _input = GetComponent<IInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Move();
        Look();

        if (_input.CanShoot())
            Shoot();
    }

    private void Move()
    {
        var direction = _input.GetMoveDirection();
        _rigidbody.velocity = direction * _speed;
    }
    private void Look()
    {
        var direction = _input.GetLookDirection();
        transform.right = direction;
    }
    private void Shoot()
    {
        
    }
}