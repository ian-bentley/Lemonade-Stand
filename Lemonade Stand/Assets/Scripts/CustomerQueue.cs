using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerQueue {
    public static event Action<IReadOnlyList<Customer>> QueueChanged;

    private List<Customer> Customers;

    public CustomerQueue() {
        Customers = new List<Customer>();
    }

    public int Count => Customers.Count;

    private void OnCustomerLeave(Customer customer) => Dequeue(customer);

    public void Update() {
        for (int i = Customers.Count - 1; i >= 0; i--) {
            Customer customer = Customers[i];
            customer.Update();
        }
    }

    public void Enqueue(Customer customer) {
        Customers.Add(customer);
        customer.Leave += OnCustomerLeave;
        QueueChanged?.Invoke(Customers);
    }

    private void Dequeue(Customer customer) {
        customer.Leave -= OnCustomerLeave;
        Customers.Remove(customer);
        QueueChanged?.Invoke(Customers);
    }

    public Customer ElementAt(int index) => Customers.ElementAt(index);
}
