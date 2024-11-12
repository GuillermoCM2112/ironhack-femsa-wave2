using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIronHack.Factories;

public interface IComponent
{
    void Operation();
}

public class Button : IComponent
{
    public void Operation()
    {
        Console.WriteLine("Push button.");
    }
}

public class TextBox : IComponent
{
    public void Operation()
    { 
        Console.WriteLine("Write in textbox."); 
    }
}

public class ObjectFactory
{
    public static IComponent CreateComponent(string type)
    {
        return type switch
        {
            "Button" => new Button(),
            "Textbox" => new TextBox(),
            _ => throw new ArgumentException("Invalid type")
        };
    }
}
