using System;

using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
class Timer : MonoBehaviour
{
    [SerializeField] private string _format = "{0:00}:{1:00}";

    private TextMeshProUGUI _text;
    private int _seconds;
    private Action _callback;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer(int seconds, Action callback)
    {
        StopTimer();
        gameObject.SetActive(true);

        _seconds = seconds;
        _callback = callback;

        InvokeRepeating(nameof(Tick), 0, 1);
    }
    public void StopTimer()
    {
        CancelInvoke(nameof(Tick));
        gameObject.SetActive(false);
    }

    private void Tick()
    {
        _seconds--;
        UpdateText();

        if (_seconds == 0)
        {
            _callback();
            StopTimer();
        }
    }
    private void UpdateText()
    {
        var minutes = _seconds / 60;
        var seconds = _seconds % 60;

        _text.text = string.Format(_format, minutes, seconds);
    }
}