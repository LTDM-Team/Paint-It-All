﻿using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

class GameLevel : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Player _botPrefab;
    [SerializeField] private int _countBots;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Color32[] _colors;
    [Header("Time")]
    [SerializeField] private Timer _timer;
    [SerializeField] private int _roundTimeSeconds;
    [Header("Hooks")]
    [SerializeField] private CameraFollowAssigner _cameraFollow;
    [SerializeField] private ColorCounter _colorCounter;
    [SerializeField] private EndRoundTitle _endRoundTitle;
    [SerializeField] private AudioSource _audioSource;

    private List<Color32> _usedColors = new List<Color32>();

    private void Start()
    {
        SpawnEntities();
        StartTimer();
        StartColorCounter();
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
    private void StartTimer()
    {
        _timer.StartTimer(_roundTimeSeconds, OnRoundTimeEnd);
    }
    private void StartColorCounter()
    {
        _colorCounter.StartCount(_usedColors.ToArray());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            _audioSource.mute = !_audioSource.mute;
    }

    private void OnRoundTimeEnd()
    {
        var bestColor = _colorCounter.GetBestColor();
        _endRoundTitle.ShowTitle(bestColor);

        Invoke(nameof(RestartLevel), 3);
    }
    private void RestartLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void SpawnPlayer(Vector2 position, Color32 color)
    {
        var player = SpawnPlayerObject(_playerPrefab, position, color);
        _cameraFollow.AssignToPlayer(player);
    }
    private void SpawnBot(Vector2 position, Color32 color)
    {
        SpawnPlayerObject(_botPrefab, position, color);
    }
    private Player SpawnPlayerObject(Player prefab, Vector2 position, Color32 color)
    {
        _usedColors.Add(color);

        var player = Instantiate(prefab, position, Quaternion.identity);
        player.Initialize(color);

        return player;
    }
}