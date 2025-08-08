# TO DO List

## Pending Items
[ ] Attraction stat and upgrade UI which affects popularity
[ ] Efficiency stat and upgrade UI which affects patience
[ ] Change price UI
[ ] Reputation stat and upgrade UI which affects value
[ ] Report screen with basic end stats
[ ] Hint conditions and hint report
[ ] customer object and cost preferences / price rejection
[ ] reipe and preference score, value stat influence on EOD score
[ ] location: neighborhood (+pop, +patience, -value), downtown (+pop, -patience, +value)
[ ] weather: prob table, rng class, normal, rainy (-pop, -patience, -value, -ice), sunny (+pop, 
    -patience, +value, +ice)
[ ] customer comments

## Completed
[X] Day counter
[X] Spawn in groups and end when no groups
[X] Patience
[X] Tune game so customer count is low and people leave at high rate
[X] refactor to group code and variables
[X] Debug screen
[X] Update UI

## Future Plans
- 


day_process
    set day_timer
    set serve_timer
    set spawn_timer based on popularity
    set earnings to 0
    set served to 0

    set customer_queue to 0
    set spawn_delay_timer to 5s
    set max_customers to 5
    set customers_spawned to 0
    set spawn_duration to 5s
    set spawn_interval to spawn_duration / max_customers
    set spawn_timer t0 spawn_interval
    set end_early_timer to 15s

    reset queue text and served text

    while not end_day:
        if day_timer or end_early_timer is up:
            end_day

        decrease day_timer
        decrease spawn_timer

        update day timer text

        if spawn_delay_timer is not done
            decrease spawn_delay_timer
    
        if spawn_delay_timer is done and customers_spawned is not at max_customers:
            decrease spawn_timer
            
            if spawn_timer is done:
                spawn customers
                reset spawn timer
        
        if customer_queue is 0 and customers_spawned is at max_customerss
            decrease end_early_timer
        
        if customer_queue has customer and there is stock:
            decrease serve_timer
        
        if serve_timer is complete:
            reset serve_timer
            serve customer
                add price to earnings and cash
                decrease stock
                remove customer from queue
                increase served count
                update cash, stock, served and queue display
    
    increase day_count
    update day_count display

    expire stock
        set stock to 0
        update stock display