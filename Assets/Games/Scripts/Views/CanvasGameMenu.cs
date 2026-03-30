using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameMenu : CanvasUI
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button replayButton;


    private void Awake()
    {
        LevelManager.OnLevelChanged += OnLevelChanged;

        replayButton.onClick.AddListener(OnReplayLevel);
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelChanged -= OnLevelChanged;
    }

    private void Start()
    {
        OnLevelChanged(0);
    }

    public void StartPointPosition()
    {

    }

    public void EndPointPosition()
    {

    }

    private void OnReplayLevel()
    {
        LevelManager.OnLevelAction?.Invoke(LevelAction.Replay);
    }

    private void OnLevelChanged(int level)
    {
        levelText.text = $"Level {level + 1}";
    }
}