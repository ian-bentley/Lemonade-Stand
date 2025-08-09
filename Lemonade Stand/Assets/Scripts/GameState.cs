using UnityEngine;

public abstract class GameState : IState
{
    protected GameManager GameManager { get; set; }

    public GameState(GameManager gameManager)
    {
        GameManager = gameManager;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick();
}
