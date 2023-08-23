using IOCExample.Classes;
using IOCExample.Container;
using IOCExample.Enums;
using IOCExample.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Singleton);
        diContainer.Register<ITemp2, Temp2>(LifeTime.Singleton);
        diContainer.Register<ITemp3, Temp3>(LifeTime.Singleton);
        diContainer.Register<ITemp1, Temp4>(LifeTime.Singleton, "Temp4");
        diContainer.LoadAll();
        var temp4 = diContainer.Resolve<ITemp1>("Temp4");
        temp4.Name = "I'm in Temp4";
        Console.WriteLine(temp4.Name);
        var res = diContainer.Resolve<ITemp1>();
        res.Name = "changed by temp1";
        var res1 = diContainer.Resolve<ITemp3>();
        Console.WriteLine(res1.tempobj.Name);
        res1.tempobj.Name = "Changed by temp3";
        res = diContainer.Resolve<ITemp1>();
        Console.WriteLine(res.Name);
        temp4 = diContainer.Resolve<ITemp1>("Temp4");
        Console.WriteLine(temp4.Name);

        System.Threading.Thread.Sleep(1000000);
    }
}