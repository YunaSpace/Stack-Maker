using UnityEngine;
using UnityEngine.UI;

public class CanvasMainMenu : CanvasUI
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private Button playButton;

    public void PlayGame()
    {
        this.gameObject.SetActive(false);

        ViewManager.Instance.OpenCanvas<CanvasLoadMenu>();
    }
}