using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIronHack.Observers;

public interface IObserver
{
    void Update(string message);
}

public class ConcreteObserver : IObserver
{
    private readonly string name;

    public ConcreteObserver(string name)
    {
        this.name = name;
    }

    public void Update(string message) 
    {
        Console.WriteLine($"{this.name} received: {message}"); 
    }
}

public class Subject
{
    private readonly List<IObserver> observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        this.observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this.observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (var observer in this.observers)
        {
            observer.Update(message);
        }
    }
}

