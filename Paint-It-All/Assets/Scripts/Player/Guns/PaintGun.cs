using UnityEngine;

class PaintGun : MonoBehaviour, IGun
{
    [SerializeField] private PaintBallProjectile _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _cooldown;

    private float _lastShootTime = float.NegativeInfinity;

    public void Shoot(Vector2 direction)
    {
        if (CanShoot() == false) return;
        else _lastShootTime = Time.time;

        SpawnProjectile(direction);
    }
    private bool CanShoot()
    {
        var nextPossibleShootTime = _lastShootTime + _cooldown;
        return Time.time >= nextPossibleShootTime;
    }

    private void SpawnProjectile(Vector2 direction)
    {
        var projectile = Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity, null);
        projectile.Initialize(Color.yellow, direction);
    }
}