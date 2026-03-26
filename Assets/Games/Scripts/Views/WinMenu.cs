using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private TextMeshProUGUI pointText;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextLevel);
        replayButton.onClick.AddListener(OnReplayLevel);
    }

    public void ShowMenu(bool isOpened)
    {
        menuPanel.gameObject.SetActive(isOpened);

        pointText.text = $"Stack: {GameManager.Instance.LevelManager.StackPoint}";
    }

    private void OnNextLevel()
    {
        LevelManager.OnLevelAction?.Invoke(LevelAction.Next);
        menuPanel.gameObject.SetActive(false);
    }

    private void OnReplayLevel()
    {
        LevelManager.OnLevelAction?.Invoke(LevelAction.Replay);
        menuPanel.gameObject.SetActive(false);
    }
}