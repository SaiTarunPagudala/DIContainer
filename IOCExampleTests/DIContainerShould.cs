using IOCExample.Classes;
using IOCExample.Container;
using IOCExample.Enums;
using IOCExample.Interfaces;

namespace IOCExampleTests;

public class DIContainerShould
{
    [Fact]
    public void CheckOnlySingleTon()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Singleton);
        diContainer.LoadAll();
        var result = diContainer.Resolve<ITemp1>();
        result.temp1Name = "Checking Singleton";
        result = diContainer.Resolve<ITemp1>();
        Assert.Equal("Checking Singleton", result.temp1Name);
    }

    [Fact]
    public void CheckOnlyTransient()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Transient);
        diContainer.LoadAll();
        var result = diContainer.Resolve<ITemp1>();
        result.temp1Name = "Checking Singleton";
        result = diContainer.Resolve<ITemp1>();
        Assert.Null(result.temp1Name);
    }

    [Fact]
    public void CheckSingleTonInsideSingleTon()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Singleton);
        diContainer.Register<ITemp2, Temp2>(LifeTime.Singleton);
        diContainer.Register<ITemp3, Temp3>(LifeTime.Singleton);
        diContainer.LoadAll();
        var temp3result = diContainer.Resolve<ITemp3>();
        Assert.Null(temp3result.temp3Name);
        Assert.Null(temp3result.temp1obj.temp1Name);
        Assert.Null(temp3result.temp2obj.temp2Name);
        temp3result.temp3Name = "Changed by Temp3";
        var temp1result = diContainer.Resolve<ITemp1>();
        temp1result.temp1Name = "Changed by Temp1";
        var temp2result = diContainer.Resolve<ITemp2>();
        temp2result.temp2Name = "Changed by Temp2";
        temp3result = diContainer.Resolve<ITemp3>();
        Assert.Equal("Changed by Temp1", temp3result.temp1obj.temp1Name);
        Assert.Equal("Changed by Temp2", temp3result.temp2obj.temp2Name);
        Assert.Equal("Changed by Temp3", temp3result.temp3Name);
    }

    [Fact]
    public void CheckTransientInsideSingleTon()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Transient);
        diContainer.Register<ITemp2, Temp2>(LifeTime.Transient);
        diContainer.Register<ITemp3, Temp3>(LifeTime.Singleton);
        diContainer.LoadAll();
        var temp3result = diContainer.Resolve<ITemp3>();
        Assert.Null(temp3result.temp3Name);
        Assert.Null(temp3result.temp1obj.temp1Name);
        Assert.Null(temp3result.temp2obj.temp2Name);
        temp3result.temp3Name = "Changed by Temp3";
        temp3result.temp1obj.temp1Name = "Changed by Temp1";
        temp3result.temp2obj.temp2Name = "Changed by Temp2";
        temp3result = diContainer.Resolve<ITemp3>();
        var temp1result = diContainer.Resolve<ITemp1>();
        var temp2result = diContainer.Resolve<ITemp2>();
        Assert.Null(temp1result.temp1Name);
        Assert.Null(temp2result.temp2Name);
        Assert.Equal("Changed by Temp1", temp3result.temp1obj.temp1Name);
        Assert.Equal("Changed by Temp2", temp3result.temp2obj.temp2Name);
        Assert.Equal("Changed by Temp3", temp3result.temp3Name);
    }

    [Fact]
    public void CheckSingleTonInsideTransient()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Singleton);
        diContainer.Register<ITemp2, Temp2>(LifeTime.Singleton);
        diContainer.Register<ITemp3, Temp3>(LifeTime.Transient);
        diContainer.LoadAll();
        var temp1result = diContainer.Resolve<ITemp1>();
        var temp2result = diContainer.Resolve<ITemp2>();
        var temp3result = diContainer.Resolve<ITemp3>();
        temp1result.temp1Name = "Changed by Temp1";
        temp2result.temp2Name = "Changed by Temp2";
        Assert.Null(temp3result.temp3Name);
        Assert.Null(temp3result.temp1obj.temp1Name);
        Assert.Null(temp3result.temp2obj.temp2Name);
        temp3result.temp3Name = "Changed by Temp3";
        temp3result.temp1obj.temp1Name = "Changed by Temp1";
        temp3result.temp2obj.temp2Name = "Changed by Temp2";
        temp3result = diContainer.Resolve<ITemp3>();

        Assert.Null(temp3result.temp1obj.temp1Name);
        Assert.Null(temp3result.temp2obj.temp2Name);
        Assert.Null(temp3result.temp3Name);
        Assert.Equal("Changed by Temp1", temp1result.temp1Name);
        Assert.Equal("Changed by Temp2", temp2result.temp2Name);

    }

}


