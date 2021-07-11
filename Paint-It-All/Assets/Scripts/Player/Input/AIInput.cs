using UnityEngine;

[RequireComponent(typeof(Player))]
class AIInput : MonoBehaviour, IInput
{
    [SerializeField] private float _lookEnemyRange;
    [SerializeField] private float _lookPaintRange;
    [SerializeField] private int _lookPaintRotationStep;
    [SerializeField] private float _cursorSpeed;
    [SerializeField] private float _cantPaintMoveCooldown;
    [SerializeField] private float _changeDirectionTime;

    private bool _hasEnemyInRange;
    private Player _closestEnemy;

    private bool _canShoot;
    private Vector2 _cursor;
    private Vector2 _cursorTarget;

    private float _cantPaintTime;
    private float _moveInCurrentDirectionTime;

    private bool _canMove;
    private Vector2 _moveDirection;

    private Color32 _color;
    private int _playerLayerMask;

    private void Start()
    {
        _playerLayerMask = LayerMask.GetMask("Players");
        _color = GetComponent<Player>().Color;
        _cursor = transform.position;
    }

    public bool CanShoot() => _canShoot;
    public Vector2 GetMoveDirection() => _moveDirection;
    public Vector2 GetLookDirection()
    {
        return GetLookVector().normalized;
    }
    public Vector2 GetLookVector()
    {
        return _cursor - (Vector2)transform.position;
    }

    private void FixedUpdate()
    {
        _hasEnemyInRange = GetClosestEnemyInRange(out var enemy, _lookEnemyRange);
        if (_hasEnemyInRange)
            _closestEnemy = enemy;

        _canShoot = FindPaintTarget(_lookPaintRotationStep, _lookPaintRange);
        if (_canShoot == false)
        {
            _cantPaintTime += Time.fixedDeltaTime;
            if (_cantPaintTime >= _cantPaintMoveCooldown)
            {
                _canMove = true;
                _cantPaintTime = 0;
            }
        }
        else
        {
            _cantPaintTime = 0;
            _canMove = false;
        }

        Move();
        MoveCursor();
    }

    private bool FindPaintTarget(int rotationStep, float maxRange)
    {
        for (var rotation = 0; rotation < 360; rotation += rotationStep)
        {
            var direction = Quaternion.Euler(0, 0, rotation) * Vector2.up;
            for (var range = 0; range < maxRange; range++)
            {
                if (HasMyColorInDirection(direction, range) == false)
                {
                    var offset = direction * range;
                    _cursorTarget = transform.position + offset;

                    return true;
                }
            }
        }

        return false;
    }
    private void MoveCursor()
    {
        _cursor = Vector2.Lerp(_cursor, _cursorTarget, Time.fixedDeltaTime * _cursorSpeed);
    }

    private void Move()
    {
        if (_canMove)
        {
            _moveInCurrentDirectionTime += Time.fixedDeltaTime;
            if (_moveDirection == Vector2.zero || _moveInCurrentDirectionTime >= _changeDirectionTime)
            {
                _moveDirection = GetRandomDirection();
                _moveInCurrentDirectionTime = 0;
            }

            _cursorTarget = (Vector2)transform.position + (_moveDirection * _lookEnemyRange);
        }
        else
        {
            _moveInCurrentDirectionTime = 0;
            _moveDirection = Vector2.zero;
        }
    }

    //Vector2 test2;
    //Vector2 test3;
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = _color;
    //    Gizmos.DrawSphere(_cursor, 0.5f);

    //    Gizmos.DrawSphere(_cursorTarget, 0.25f);
    //    Gizmos.DrawSphere(test2, 1f);

    //    //Gizmos.color = Color.black;
    //    //Gizmos.DrawSphere(test3, 3);
    //}

    private Vector2 GetRandomDirection()
    {
        var position = PaintCanvas.Instance.GetRandomWorldPosition();
        var vector = position - (Vector2)transform.position;

        var direction = vector.normalized;
        return direction;
    }
    private bool GetClosestEnemyInRange(out Player enemy, float range)
    {
        enemy = null;

        var closestEnemyCollider = Physics2D.OverlapCircle(transform.position, range, _playerLayerMask);
        if (closestEnemyCollider == null)
            return false;

        enemy = closestEnemyCollider.GetComponent<Player>();
        if (enemy == null)
            return false;

        return true;
    }
    private bool HasMyColorInDirection(Vector2 direction, float range)
    {
        var offset = direction * range;
        var position = (Vector2)transform.position + offset;

        return HasMyColorIn(position);
    }
    private bool HasMyColorIn(Vector2 position)
    {
        return PaintCanvas.Instance.HasColorIn(position, _color);
    }
}