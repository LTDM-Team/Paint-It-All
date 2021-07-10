using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoViewer : MonoBehaviour
{
    [Header("Количество жизней")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Slider _healthProgressBar;

    [Header("Количество патрон")]
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private Slider _ammoProgressBar;

    [Header("Время раунда")]
    [SerializeField] private int _roundTime;
    [SerializeField] private TextMeshProUGUI _timeText;

    private const int _secondsInMinute = 60;

    private void Update()
    {
        ConvertTime(_roundTime, out int minutes, out int seconds);
        _timeText.text = string.Format("{0:d2}:{1:d2}", minutes, seconds);

        _ammoProgressBar.value = (float)_currentAmmo / _maxAmmo;
        _healthProgressBar.value = (float)_currentHealth / _maxHealth;
    }
    
    public void SetHealth(int maxHealth, int currentHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
    }
    public void SetAmmo(int maxAmmo, int currentAmmo)
    {
        _maxAmmo = maxAmmo;
        _currentAmmo = currentAmmo;
    }
    public void UpdateHealth(int currentHealth)
    {
        _currentHealth = currentHealth;
    }    
    public void UpdateAmmo(int currentAmmo)
    {
        _currentAmmo = currentAmmo;
    }
    public void ReduceTime()
    {
        _roundTime--;
    }

    private void ConvertTime(int timeInSeconds, out int minutes, out int seconds)
    {
        minutes = (int)Mathf.Floor(timeInSeconds / _secondsInMinute);
        seconds = timeInSeconds - minutes * _secondsInMinute;
    }
}
