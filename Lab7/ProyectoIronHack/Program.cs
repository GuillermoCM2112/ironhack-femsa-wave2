using ProyectoIronHack.Config;
using ProyectoIronHack.Factories;
using ProyectoIronHack.Observers;
using ProyectoIronHack.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = AppConfig.Instance;
        Console.WriteLine($"MaxConcurrentTasks: {config.MaxConcurrentTasks}");

        var button = ObjectFactory.CreateComponent("Button");
        var textbox = ObjectFactory.CreateComponent("Textbox");

        button.Operation();
        textbox.Operation();

        var subject = new Subject();
        var observer1 = new ConcreteObserver("User 1");
        var observer2 = new ConcreteObserver("User 2");

        subject.Attach(observer1);
        subject.Attach(observer2);
        subject.Notify("New Update!");

        var asyncService = new AsyncService(3);

        var tasks = new List<Func<Task>>
        {
            () => SimulateTask("Task 1", 2000),
            () => SimulateTask("Task 2", 3000),
            () => SimulateTask("Task 3", 1000),
            () => SimulateTask("Task 4", 1500),
            () => SimulateTask("Task 5", 2500)
        };

        await asyncService.ExecuteAsync(tasks);

        Console.WriteLine("All tasks finished.");

        static async Task SimulateTask(string taskName, int delay)
        {
            Console.WriteLine($"{taskName} started.");
            await Task.Delay(delay);
            Console.WriteLine($"{taskName} finished.");
        }
    }
}