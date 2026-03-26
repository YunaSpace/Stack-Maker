using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public LevelManager LevelManager;
    public ViewManager ViewManager;

    private void Start()
    {
        Application.targetFrameRate = 120;
    }

    public void HandlePauseGame()
    {

    }

    public void HandleLoseGame()
    {

    }

    public void HandleWinGame()
    {

    }
}