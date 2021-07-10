using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
class PaintBallProjectile : MonoBehaviour
{
    [Header("Fly")]
    [SerializeField] private float _speed;
    [SerializeField] private float _minFlyDistance;
    [SerializeField] private float _maxFlyDistance;
    [Header("Blow")]
    [SerializeField] private int _blowSizeMin;
    [SerializeField] private int _blowSizeMax;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

    private float _targetFlyDistance;
    private Vector2 _startPosition;
    private Color32 _color;

    public void Initialize(Color32 color, Vector2 direction)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();

        _targetFlyDistance = Random.Range(_minFlyDistance, _maxFlyDistance);
        _startPosition = transform.position;
        _color = color;

        transform.right = direction;
        _rigidbody.velocity = direction * _speed;
        _renderer.color = color;
    }

    private void FixedUpdate()
    {
        var distance = GetFlyDistance();
        if (distance >= _targetFlyDistance)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Blow();
    }

    private void Blow()
    {
        var blowSize = Random.Range(_blowSizeMin, _blowSizeMax + 1);
        PaintCanvas.Instance.DrawCircle(_color, transform.position, blowSize);
    }
    private float GetFlyDistance()
    {
        return Vector2.Distance(transform.position, _startPosition);
    }
}