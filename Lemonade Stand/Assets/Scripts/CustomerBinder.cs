//using System.Collections.Generic;
//using UnityEngine;

//public class CustomerBinder : MonoBehaviour {
//    [SerializeField] CustomerPool CustomerPool;

//    private List<Customer> WaitingList;
//    //private Dictionary<Customer, CustomerController> Binds;

//    private void Start() {
//        WaitingList = new List<Customer>();
//        CustomerQueue.Enqueued += OnCustomerEnqueued;
//    }

//    public void OnCustomerEnqueued(Customer customer) => TryBindNextCustomer(customer);

//    private void TryBindNextCustomer(Customer customer) {
//        Debug.Log($"Trying to bind customer {customer.Id}");

//        CustomerController customerController = CustomerPool.GetAvailableCustomerController();

//        if (customerController == null) {
//            WaitingList.Add(customer);
//            Debug.Log("No available controller. Added to wait list");
//            return;
//        }

//        Bind(customerController, customer);
//    }

//    private void Bind(CustomerController customerController, Customer customer) {
//        customer.Leave += customerController.OnCustomerLeave; // TODO make Leave event fire on leave
//        // customerController.ReachedStand += OnReachedStand; // TODO wire ordering to reaching stand event

//        Debug.Log($"Customer {customer.Id} has been bound to controller");
//    }
//}
