using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIronHack.Config;

public class AppConfig
{
    private static AppConfig? instance;

    public string DatabaseConnectionString { get; private set; }
    public int MaxConcurrentTasks { get; private set; }

    private AppConfig()
    {
        DatabaseConnectionString = "Server=myServer;Database=myDb;";
        MaxConcurrentTasks = 5;
    }

    public static AppConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppConfig();
            }
            return instance;
        }
    }
}
