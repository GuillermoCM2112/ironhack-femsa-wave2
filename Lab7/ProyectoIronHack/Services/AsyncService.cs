using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIronHack.Services;

public class AsyncService
{
    private readonly SemaphoreSlim _semaphore;

    public AsyncService(int maxConcurrentTasks)
    {
        _semaphore = new SemaphoreSlim(maxConcurrentTasks);
    }

    public async Task ExecuteAsync(IEnumerable<Func<Task>> tasks)
    {
        var taskList = new List<Task>();

        foreach (var task in tasks)
        {
            await _semaphore.WaitAsync();

            taskList.Add(Task.Run(async () =>
            {
                try
                {
                    await task();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    _semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(taskList);
    }
}
