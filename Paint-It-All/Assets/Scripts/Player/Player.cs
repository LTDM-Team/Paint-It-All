using UnityEngine;

[RequireComponent(typeof(IInput))]
[RequireComponent(typeof(PaintGun))]
[RequireComponent(typeof(Rigidbody2D))]
class Player : MonoBehaviour
{
    public Color32 Color { get; private set; }

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private float _speed;

    private IInput _input;
    private PaintGun _gun;
    private Rigidbody2D _rigidbody;

    public void Initialize(Color32 color)
    {
        _input = GetComponent<IInput>();
        _gun = GetComponent<PaintGun>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _gun.Initialize(color);

        Color = color;
        _visual.color = color;
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
        if (direction.x == direction.y)
            direction = new Vector2(direction.x / 1.3f, direction.x / 1.3f);

        _rigidbody.velocity = direction * _speed;
    }
    private void Look()
    {
        var direction = _input.GetLookDirection();
        transform.right = direction;
    }
    private void Shoot()
    {
        _gun.Shoot(_input.GetLookVector());
    }
}