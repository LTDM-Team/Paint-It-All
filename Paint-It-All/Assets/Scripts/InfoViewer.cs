using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoViewer : MonoBehaviour
{
    public int CurrentHealth
    {
        set
        {
            _currentHealth = value;
            _healthProgressBar.value = _currentHealth / _maxHealth;
        }
    }
    private int _currentHealth;

    public int CurrentAmmo
    {
        set
        {
            _currentAmmo = value;
            _ammoProgressBar.value = _currentAmmo / _maxAmmo;
        }
    }
    private int _currentAmmo;

    public int RoundTime
    {
        set
        {
            _roundTime = value;
            ConvertTime(_roundTime, out int minutes, out int seconds);
            _timeText.text = string.Format("Время: {0:d2}:{1:d2}", minutes, seconds);
        }
    }
    private int _roundTime;

    [Header("Жизни")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private Slider _healthProgressBar;

    [Header("Патроны")]
    [SerializeField] private int _maxAmmo;
    [SerializeField] private Slider _ammoProgressBar;

    [Header("Время")]
    [SerializeField] private TextMeshProUGUI _timeText;

    private const int _secondsInMinute = 60;

    private void Start()
    {
        CurrentAmmo = 5;
        CurrentHealth = 50;
        RoundTime = 360;
    }

    public void SetHealth(int maxHealth, int currentHealth)
    {
        _maxHealth = maxHealth;
        CurrentHealth = currentHealth;
    }
    public void SetAmmo(int maxAmmo, int currentAmmo)
    {
        _maxAmmo = maxAmmo;
        CurrentAmmo = currentAmmo;
    }
    public void UpdateHealth(int currentHealth)
    {
        CurrentHealth = currentHealth;
    }    
    public void UpdateAmmo(int currentAmmo)
    {
        CurrentAmmo = currentAmmo;
    }
    public void ReduceTime(int seconds)
    {
        _roundTime -= seconds;
    }

    private void ConvertTime(int timeInSeconds, out int minutes, out int seconds)
    {
        minutes = (int)Mathf.Floor(timeInSeconds / _secondsInMinute);
        seconds = timeInSeconds - minutes * _secondsInMinute;
    }
}
