using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CanvasNextMenu : CanvasUI
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Image _maskImage;
    [SerializeField] private float _maskingDuration;

    [SerializeField] private float _time;

    private Material _maskMaterial;

    private bool _isShowingMask;

    private void Awake()
    {
        _maskMaterial = _maskImage.material;
    }

    private void Start()
    {
        _menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (_time < 0 || _time > _maskingDuration)
        {
            if (_time < 0)
            {
                ViewManager.OnNextLevelPanelMasked?.Invoke();
            }
            else if (_time > _maskingDuration && _menuPanel.activeInHierarchy)
            {
                _menuPanel.SetActive(false);
            }

            return;
        }

        _time += _isShowingMask ? -Time.deltaTime : Time.deltaTime;

        var percent = math.clamp(_time / _maskingDuration, 0, 1);
        var size = math.remap(0, 1, 0.0001f, 3f, percent);
        _maskMaterial.SetFloat("_MaskSize", size);
    }

    public void ShowMask(bool toShow)
    {
        _menuPanel.SetActive(true);

        _isShowingMask = toShow;

        _time = toShow ? _maskingDuration : 0;
    }
}