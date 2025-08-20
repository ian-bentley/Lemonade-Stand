using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : MonoBehaviour {
    [SerializeField] private CustomerController Prefab;
    [SerializeField] private int InitialSize = 32;
    [SerializeField] private int ExpandBatchSize = 8;

    private List<CustomerController> CustomerControllers;

    private void OnEnable() {
        World.OnDayStart += OnDayStart;
    }

    private void OnDisable() {
        World.OnDayStart -= OnDayStart;
    }

    private void Awake() {
        CustomerControllers = new List<CustomerController>();
    }

    public CustomerController GetAvailableCustomerController() {

        foreach (CustomerController customerController in CustomerControllers) {
            if (customerController.gameObject.activeInHierarchy) continue;
            customerController.transform.position = Vector3.zero;
            customerController.Activate();
            customerController.EnableCollision();
            return customerController;
        }

        int nextIndex = CustomerControllers.Count;
        for (int i = 0; i < ExpandBatchSize; i++) {
            IncreasePool();
        }

        CustomerController createdController = CustomerControllers[nextIndex];
        createdController.Activate();
        createdController.EnableCollision();
        return createdController;
    }

    private void IncreasePool() {
        CustomerController customerController = Instantiate(Prefab, transform, false);
        customerController.gameObject.SetActive(false);
        CustomerControllers.Add(customerController);
    }

    private void OnDayStart() {
        foreach (CustomerController customerController in CustomerControllers) {
            customerController.Deactivate();
        }

        CustomerControllers = new List<CustomerController>();

        for (int i = 0; i < InitialSize; i++) {
            IncreasePool();
        }
    }
}
