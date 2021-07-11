using System.Collections.Generic;
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
    [SerializeField] private float _attackEnemyTime;

    private bool _hasEnemyInRange;
    private bool _isAttacking;
    private Player _closestEnemy;

    private bool _canShoot;
    private Vector2 _cursor;
    private Vector2 _cursorTarget;

    private float _currentAttackTime;
    private float _cantPaintTime;
    private float _moveInCurrentDirectionTime;

    private bool _canMove;
    private Vector2 _moveDirection;

    private Color32 _color;
    private ContactFilter2D _enemyContactFilter;

    private void Start()
    {
        _enemyContactFilter = new ContactFilter2D();
        _enemyContactFilter.layerMask = LayerMask.GetMask("Players");

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
        {
            _closestEnemy = enemy;
            _isAttacking = true;
        }

        if (_isAttacking)
        {
            AttackEnemy();
        }
        else
        {
            Paint();
            Move();
        }
        MoveCursor();
    }

    private void AttackEnemy()
    {
        if (_currentAttackTime >= _attackEnemyTime)
        {
            _isAttacking = false;
            _currentAttackTime = 0;
        }
        _currentAttackTime += Time.fixedDeltaTime;

        _cursorTarget = _closestEnemy.transform.position;
        _canShoot = true;

        _canMove = false;
        _moveDirection = Vector2.zero;
    }
    private void Paint()
    {
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
            var isOutOfBoundsArea = PaintCanvas.Instance.ContainsInCanvas(transform.position) == false;
            var canChangeDirection = isOutOfBoundsArea
                || _moveDirection == Vector2.zero
                || _moveInCurrentDirectionTime >= _changeDirectionTime;

            if (canChangeDirection)
            {
                _moveDirection = GetRandomDirection();
                _moveInCurrentDirectionTime = 0;
            }
            _moveInCurrentDirectionTime += Time.fixedDeltaTime;
            
            _cursorTarget = (Vector2)transform.position + (_moveDirection * _lookEnemyRange);
        }
        else
        {
            _moveInCurrentDirectionTime = 0;
            _moveDirection = Vector2.zero;
        }
    }

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

        var closestEnemyColliders = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, range, _enemyContactFilter, closestEnemyColliders);

        for (var i = 0; i < closestEnemyColliders.Count; i++)
        {
            if (closestEnemyColliders[i].gameObject != gameObject)
            {
                enemy = closestEnemyColliders[i].GetComponent<Player>();
                break;
            }
        }

        return enemy != null;
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