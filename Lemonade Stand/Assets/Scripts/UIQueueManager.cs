using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQueueManager : MonoBehaviour
{
    [SerializeField] private List<Transform> QueueNodes;
    [SerializeField] private CustomerPool CustomerPool;
    [SerializeField] private CustomerSpawner CustomerSpawner;
    [SerializeField] private Transform LeaveNode;
    [SerializeField] private Transform ExitNode;

    private Dictionary<Customer, CustomerController> Map { get; set; }

    private void OnEnable() {
        CustomerQueue.QueueChanged += OnQueueChanged;
        World.OnDayStart += OnDayStart;
        World.OnDayStart += OnDayEnd;
    }

    private void OnDisable() {
        CustomerQueue.QueueChanged -= OnQueueChanged;
        World.OnDayStart -= OnDayEnd;
    }

    private void Awake() {
        Map = new Dictionary<Customer, CustomerController>();
    }

    private void OnDayStart() {
        Map = new Dictionary<Customer, CustomerController>();
    }

    private void OnDayEnd() {
        foreach (Customer customer in Map.Keys) {
            // unsubscribe queue mananager to customer exit events
            customer.Leave -= OnCustomerLeave;
            customer.ReceiveDrink -= OnCustomerReceiveDrink;

            // unsubscribe customer to controller
            CustomerController customerController = Map[customer];
            customerController.ReachedStand -= customer.OnReachedStand;
        }
    }

    private void OnCustomerLeave(Customer customer) {
        customer.Leave -= OnCustomerLeave;
        customer.ReceiveDrink -= OnCustomerReceiveDrink;
        CustomerController customerController = Map[customer];
        customerController.ReachedStand -= customer.OnReachedStand;
        customerController.DisableCollision();
        customerController.Leave();
        Map.Remove(customer);
    }

    private void OnCustomerReceiveDrink(Customer customer) {
        customer.Leave -= OnCustomerLeave;
        customer.ReceiveDrink -= OnCustomerReceiveDrink;
        CustomerController customerController = Map[customer];
        customerController.ReachedStand -= customer.OnReachedStand;
        customerController.DisableCollision();
        customerController.Exit();
        Map.Remove(customer);
    }

    public void OnQueueChanged(IReadOnlyList<Customer> Customers) {

        // iterate through all customers currently in the queue
        for (int i = 0; i < Customers.Count && i < QueueNodes.Count; i++) {
            
            // get the current customer
            Customer customer = Customers[i];

            // if this customer is not in our map
            if (!Map.ContainsKey(customer)) {

                // get an available controller (one that's not active)
                CustomerController newCustomerController = CustomerPool.GetAvailableCustomerController();

                // set exit and leave nodes
                newCustomerController.LeaveNode = LeaveNode;
                newCustomerController.ExitNode = ExitNode;

                // map it with the customer
                Map[customer] = newCustomerController;

                // subscribe customer to when controller reaches stand
                newCustomerController.ReachedStand += customer.OnReachedStand;

                // subscribe queue mananager to customer exit events
                customer.Leave += OnCustomerLeave;
                customer.ReceiveDrink += OnCustomerReceiveDrink;
            }

            // get the controller of the current customer
            CustomerController customerController = Map[customer];

            // get the node whose index matches the customers index
            Transform target = QueueNodes[i];

            // set the customer's controller to this target
            customerController.Target = target;
        }
    }
}
