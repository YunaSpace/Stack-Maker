using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : Singleton<ViewManager>
{
    public static Action<bool> OnNextLevelPanelShowed { get; set; }
    public static Action OnNextLevelPanelMasked { get; set; }

    [SerializeField] private CanvasWinMenu winMenu;
    [SerializeField] private CanvasLoseMenu loseMenu;
    [SerializeField] private CanvasNextMenu nextMenu;
    [SerializeField] private CanvasGameMenu gameMenu;

    [SerializeField] private List<CanvasUI> canvasList = new();

    private Dictionary<Type, CanvasUI> canvases = new();

    protected override void Awake()
    {
        base.Awake();

        GetAllCanvas();

        LevelManager.OnLevelStateChanged += HandleLevelStateChanged;
        ViewManager.OnNextLevelPanelShowed += nextMenu.ShowMask;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelStateChanged -= HandleLevelStateChanged;
        ViewManager.OnNextLevelPanelShowed -= nextMenu.ShowMask;
    }

    public void OpenCanvas<T>() where T : CanvasUI
    {
        if (canvases.ContainsKey(typeof(T)))
        {
            var canvas = canvases[typeof(T)];
            canvas.gameObject.SetActive(true);
        }
    }

    private void GetAllCanvas()
    {
        foreach (var canvas in canvasList)
        {
            canvases.Add(canvas.GetType(), canvas);
        }
    }

    private void HandleLevelStateChanged(LevelState state)
    {
        winMenu.ShowMenu(state == LevelState.Win);
        loseMenu.ShowMenu(state == LevelState.Lose);
    }
}