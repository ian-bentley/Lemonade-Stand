using UnityEngine;

public class DaytimeState : GameState
{
    public DaytimeState(GameManager gameManager) : base(gameManager){ }

    public override void Enter()
    {
        GameManager.Player.StartDay();
        GameManager.World.StartDay();
        GameManager.CustomerManager.StartDay();
    }

    public override void Exit()
    {
        GameManager.EndDay();
    }

    public override void Tick()
    {
        if (!GameManager.World.day_running)
        {
            GameManager.StateMachine.ChangeState(new PrepDayState(GameManager));
            return;
        }

        GameManager.World.Update();
        GameManager.Player.Update();
        GameManager.CustomerManager.Update();

        // if there are no more customers, run end early timer unless it has already been completed
        if (!GameManager.World.end_early_timer.elapsed && GameManager.CustomerManager.QueueIsEmpty() && !GameManager.CustomerManager.CanSpawnMoreCustomers())
        {
            GameManager.World.end_early_timer.Start();
        }

        // if not serving, have stock, and there is a customer
        if (GameManager.CustomerManager.HasCustomerInQueue() && GameManager.Player.CanServe())
        {
            GameManager.Player.StartServing();
        }

        // if serving is done
        if (GameManager.Player.Serving && GameManager.Player.ServeTimer.elapsed)
        {
            GameManager.Player.Serve();
            GameManager.CustomerManager.DequeueFrontCustomer();
        }
    }
}
