using System;
using System.Collections.Generic;
using System.Linq;

public class CustomerQueue {
    public static event Action<IReadOnlyList<Customer>> QueueChanged;
    public static event Action<string, float> CustomerAdded;
    public static event Action<string> CustomerRemoved;

    private List<Customer> Customers;

    public CustomerQueue() {
        Customers = new List<Customer>();
    }

    public int Count => Customers.Count;
    public bool HasCustomerInQueue => Customers.Count > 0;

    private void OnCustomerLeave(Customer customer) => ExitCustomerFromPatience(customer);
    private void OnCustomerReceiveDrink(Customer customer) => ExitCustomerFromServed(customer);

    public void Update() {
        for (int i = Customers.Count - 1; i >= 1; i--) {
            Customer customer = Customers[i];
            customer.Update();
        }
    }

    public void Enqueue(Customer customer) {
        Customers.Add(customer);
        customer.Leave += OnCustomerLeave;
        customer.ReceiveDrink += OnCustomerReceiveDrink;
        QueueChanged?.Invoke(Customers);
        CustomerAdded?.Invoke(customer.Id, customer.PatienceTimer.current_time);
    }

    private void Dequeue(Customer customer) {
        customer.Leave -= OnCustomerLeave;
        customer.ReceiveDrink -= OnCustomerReceiveDrink;
        Customers.Remove(customer);
        QueueChanged?.Invoke(Customers);
        CustomerRemoved?.Invoke(customer.Id);
    }

    public Customer ElementAt(int index) => Customers.ElementAt(index);

    private void ExitCustomerFromPatience(Customer customer) => Dequeue(customer);
    private void ExitCustomerFromServed(Customer customer) => Dequeue(customer);

    public void UnbindCustomers() {
        foreach (Customer customer in Customers) {
            customer.Leave -= OnCustomerLeave;
            customer.ReceiveDrink -= OnCustomerReceiveDrink;
        }
    }
}
