using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LemonadeStand : MonoBehaviour
{
    Player player = new Player();
    World world = new World();
    [SerializeField] UI ui = new UI();
    CustomerManager customer_manager = new CustomerManager();

    public Player Player => player;
    public World World => world;
    public UI UI => ui;
    public CustomerManager CustomerManager => customer_manager;

    private void Start()
    {
        InitializeUI();
    }

    public void InitializeUI()
    {
        ui.SetCashText(player.cash);
        ui.SetStockText(player.inventory.stock);
        ui.SetCustomersText(customer_manager.customer_queue);
        ui.SetServedText(player.served);
        ui.SetDayCounterText(world.day_count);
        ui.SetDayTimerText(world.day_timer.current_time);
    }

    public void StartDay()
    {
        StartCoroutine(SellLemonade());
    }

    public void EndDay()
    {
        // Raise day count and update display
        world.day_count++;
        ui.SetDayCounterText(world.day_count);

        // expire stock and update display
        player.inventory.ExpireStock();
        ui.SetStockText(player.inventory.stock);
    }

    public void ResetDay()
    {
        player.Reset(); // reset player
        world.Reset(); // reset world
        customer_manager.Reset(); // reset customer manager

        // reset display
        ui.SetCustomersText(customer_manager.customer_queue);
        ui.SetServedText(player.served);
    }

    public void StartDayTimers()
    {
        customer_manager.spawn_delay_timer.Start();
        player.serve_timer.Start();
        world.day_timer.Start();
    }

    IEnumerator SellLemonade()
    {
        ResetDay();
        StartDayTimers();

        while (world.day_running)
        {
            world.Update();
            player.Update();
            customer_manager.Update();

            // update UI
            ui.SetDayTimerText(world.day_timer.current_time);
            ui.SetCustomersText(customer_manager.customer_queue);
            ui.SetCashText(player.cash);
            ui.SetStockText(player.inventory.stock);
            ui.SetServedText(player.served);

            // if there are no more customers, run end early timer unless it has already been completed
            if (!world.end_early_timer.elapsed && customer_manager.QueueIsEmpty() && !customer_manager.CanSpawnMoreCustomers())
            {
                world.end_early_timer.Start();
            }

            // if not serving, have stock, and there is a customer
            if (!player.serving && customer_manager.HasCustomerInQueue() && player.inventory.HasStock())
            {
                player.StartServing(); // start serving customer
            }

            // if serving is done
            if (player.serving && player.serve_timer.elapsed)
            {
                player.Serve();

                // remove top customer from queue
                customer_manager.DequeueCustomer(0);
            }

            yield return null; // pause loop till next frame so timers work
        }

        EndDay();
    }

    public void BuyStock()
    {
        player.BuyStock();
        ui.SetCashText(player.cash);
        ui.SetStockText(player.inventory.stock);
    }
}
