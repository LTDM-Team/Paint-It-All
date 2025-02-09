﻿using UnityEngine;
using TMPro;

class ColorCounter : MonoBehaviour
{
    [SerializeField] private string _format = "{0}%";
    [SerializeField] private TextMeshProUGUI[] _texts;

    private Color32[] _colors;
    private int _allColorsCount;
    private int[] _colorsCount;

    public void StartCount(Color32[] colors)
    {
        _colors = colors;
        _colorsCount = new int[colors.Length];

        UpdateColors();

        CancelInvoke(nameof(Tick));
        InvokeRepeating(nameof(Tick), 0, 1);
    }
    public Color32 GetBestColor()
    {
        var max = _colorsCount[0];
        var bestColor = _colors[0];

        for (var i = 1; i < _colorsCount.Length; i++)
        {
            if (_colorsCount[i] > max)
            {
                max = _colorsCount[i];
                bestColor = _colors[i];
            }
        }

        return bestColor;
    }

    private void UpdateColors()
    {
        for (var i = 0; i < _colors.Length; i++)
            _texts[i].color = _colors[i];
    }

    private void Tick()
    {
        CountColors();
        UpdateTexts();
    }
    private void CountColors()
    {
        _allColorsCount = 0;

        for (var i = 0; i < _colors.Length; i++)
        {
            var count = PaintCanvas.Instance.GetCountColor(_colors[i]);
            _colorsCount[i] = count;
            _allColorsCount += count;
        }
    }
    private void UpdateTexts()
    {
        for (var i = 0; i < _colors.Length; i++)
        {
            var colorProcent = (byte)((double)_colorsCount[i] / _allColorsCount * 100);
            _texts[i].text = string.Format(_format, colorProcent);
        }
    }
}