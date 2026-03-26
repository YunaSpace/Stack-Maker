using System;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static Action<bool> OnNextLevelPanelShowed { get; set; }
    public static Action OnNextLevelPanelMasked { get; set; }

    [SerializeField] private WinMenu winMenu;
    [SerializeField] private LoseMenu loseMenu;
    [SerializeField] private NextMenu nextMenu;

    private void Awake()
    {
        LevelManager.OnLevelStateChanged += HandleLevelStateChanged;
        ViewManager.OnNextLevelPanelShowed += nextMenu.ShowMask;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelStateChanged -= HandleLevelStateChanged;
        ViewManager.OnNextLevelPanelShowed -= nextMenu.ShowMask;
    }

    private void HandleLevelStateChanged(LevelState state)
    {
        winMenu.ShowMenu(state == LevelState.Win);
        loseMenu.ShowMenu(state == LevelState.Lose);
    }
}