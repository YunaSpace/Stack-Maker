using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Image _loadingFill;
    [SerializeField] private TextMeshProUGUI _loadingText;

    [SerializeField] private float _loadingSpeed = 10;
    [SerializeField] private float _stallingDuration;
    [SerializeField] private float _loadingDuration;

    private float _time;
    private bool _isLoadDone;

    private void Awake()
    {
        _loadingFill.fillAmount = 0;
        _loadingText.text = $"0%";
    }

    private void Update()
    {
        if (_isLoadDone)
        {
            return;
        }

        ProcessLoading();
    }

    private void ProcessLoading()
    {
        _time += Time.deltaTime * _loadingSpeed;

        if (_time < _stallingDuration)
        {
            return;
        }

        var percent = (_time - _stallingDuration) / _loadingDuration;

        if (percent > 1)
        {
            _isLoadDone = true;

            _menuPanel.SetActive(false);
        }

        _loadingFill.fillAmount = percent;
        _loadingText.text = $"{(int)(percent * 100)}%";
    }
}