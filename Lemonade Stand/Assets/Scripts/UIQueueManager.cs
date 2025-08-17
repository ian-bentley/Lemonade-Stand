using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQueueManager : MonoBehaviour
{
    [SerializeField] private List<Transform> QueueNodes;
    [SerializeField] private Transform ExitNode;
    [SerializeField] private CustomerPool CustomerPool;
    [SerializeField] private CustomerSpawner CustomerSpawner;

    private Dictionary<Customer, CustomerController> Map { get; set; }

    private void OnEnable() {
        CustomerQueue.QueueChanged += OnQueueChanged;
    }

    private void OnDisable() {
        CustomerQueue.QueueChanged -= OnQueueChanged;
    }

    private void Awake() {
        Map = new Dictionary<Customer, CustomerController>();
    }

    public void OnQueueChanged(IReadOnlyList<Customer> Customers) {
        foreach (Customer customer in Map.Keys.Except(Customers).ToList()) {
            CustomerController customerController = Map[customer];
            customerController.Target = ExitNode;
            Map.Remove(customer);
        }

        for (int i = 0; i < Customers.Count && i < QueueNodes.Count; i++) {
            Customer customer = Customers[i];

            if (!Map.ContainsKey(customer)) Map[customer] = CustomerPool.GetAvailableCustomerController();

            CustomerController customerController = Map[customer];
            Transform target = QueueNodes[i];
            customerController.Target = target;
        }
    }
}
