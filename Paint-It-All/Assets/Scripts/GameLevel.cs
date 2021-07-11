using System.Collections.Generic;
using System.Linq;

using UnityEngine;

class GameLevel : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Player _botPrefab;
    [SerializeField] private int _countBots;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Color32[] _colors;
    [SerializeField] private CameraFollowAssigner _cameraFollow;

    private void Start()
    {
        SpawnEntities();
    }
    private void SpawnEntities()
    {
        var shuffledSpawnPointsOrder = _spawnPoints.OrderBy(spawnPoint => System.Guid.NewGuid());
        var shuffledSpawnPoints = new Stack<Transform>(shuffledSpawnPointsOrder);

        var shuffledColorsOrder = _colors.OrderBy(color => System.Guid.NewGuid());
        var shuffledColors = new Stack<Color32>(shuffledColorsOrder);

        var playerPosition = shuffledSpawnPoints.Pop().position;
        var playerColor = shuffledColors.Pop();
        SpawnPlayer(playerPosition, playerColor);

        var countBots = Mathf.Min(_spawnPoints.Length, _countBots);
        for (var i = 0; i < countBots; i++)
        {
            var position = shuffledSpawnPoints.Pop().position;
            var color = shuffledColors.Pop();

            SpawnBot(position, color);
        }
    }

    private void SpawnPlayer(Vector2 position, Color32 color)
    {
        var player = SpawnPlayerObject(_playerPrefab, position, color);
        var cameraFollow = FindObjectOfType<CameraFollowAssigner>();

        cameraFollow.AssignToPlayer(player);
    }
    private void SpawnBot(Vector2 position, Color32 color)
    {
        SpawnPlayerObject(_botPrefab, position, color);
    }
    private Player SpawnPlayerObject(Player prefab, Vector2 position, Color32 color)
    {
        var player = Instantiate(prefab, position, Quaternion.identity);
        player.Initialize(color);

        return player;
    }
}