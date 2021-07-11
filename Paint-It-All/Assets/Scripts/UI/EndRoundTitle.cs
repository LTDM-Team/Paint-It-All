using UnityEngine;
using UnityEngine.UI;
using TMPro;

class EndRoundTitle : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void ShowTitle(Color32 color)
    {
        gameObject.SetActive(true);

        _image.color = color;
        _text.color = color;
    }
}