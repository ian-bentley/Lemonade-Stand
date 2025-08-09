using UnityEngine;

public interface IState
{
    void Enter(); // runs when you enter the state
    void Exit(); // runs when you exit the state
    void Tick(); // runs every update tick
}
