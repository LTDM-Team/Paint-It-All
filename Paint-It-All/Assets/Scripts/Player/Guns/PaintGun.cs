﻿using UnityEngine;

class PaintGun : MonoBehaviour
{
    [SerializeField] private PaintBallProjectile _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _cooldown;

    private Color32 _color;
    private float _lastShootTime = float.NegativeInfinity;

    public void Initialize(Color32 color)
    {
        _color = color;
    }

    public void Shoot(Vector2 vector)
    {
        if (CanShoot() == false) return;
        else _lastShootTime = Time.time;

        SpawnProjectile(vector);
    }
    private bool CanShoot()
    {
        var nextPossibleShootTime = _lastShootTime + _cooldown;
        return Time.time >= nextPossibleShootTime;
    }

    private void SpawnProjectile(Vector2 direction)
    {
        var projectile = Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity, null);
        projectile.Initialize(_color, direction);
    }
}