using UnityEngine;
using UnityEngine.UI;

public class CanvasLoseMenu : CanvasUI
{
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button replayButton;

    private void Awake()
    {
        replayButton.onClick.AddListener(OnReplayLevel);
    }

    public void ShowMenu(bool isOpened)
    {
        menuPanel.gameObject.SetActive(isOpened);
    }

    private void OnReplayLevel()
    {
        LevelManager.OnLevelAction?.Invoke(LevelAction.Replay);
        menuPanel.gameObject.SetActive(false);
    }
}