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
        var temp1dependenciesCount = diContainer.GetDependencies<ITemp1>().Count();
        Assert.Equal(0, temp1dependenciesCount);
        var temp2dependenciesCount = diContainer.GetDependencies<ITemp2>().Count();
        Assert.Equal(0, temp2dependenciesCount);
        var temp3dependenciesCount = diContainer.GetDependencies<ITemp3>().Count();
        Assert.Equal(2, temp3dependenciesCount);
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
        var temp1dependenciesCount = diContainer.GetDependencies<ITemp1>().Count();
        Assert.Equal(0, temp1dependenciesCount);
        var temp2dependenciesCount = diContainer.GetDependencies<ITemp2>().Count();
        Assert.Equal(0, temp2dependenciesCount);
        var temp3dependenciesCount = diContainer.GetDependencies<ITemp3>().Count();
        Assert.Equal(2, temp3dependenciesCount);
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
        var temp1dependenciesCount = diContainer.GetDependencies<ITemp1>().Count();
        Assert.Equal(0, temp1dependenciesCount);
        var temp2dependenciesCount = diContainer.GetDependencies<ITemp2>().Count();
        Assert.Equal(0, temp2dependenciesCount);
        var temp3dependenciesCount = diContainer.GetDependencies<ITemp3>().Count();
        Assert.Equal(2, temp3dependenciesCount);
    }

    [Fact]
    public void CheckTransientInsideTransient()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Transient);
        diContainer.Register<ITemp2, Temp2>(LifeTime.Transient);
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
        temp1result = diContainer.Resolve<ITemp1>();
        temp2result = diContainer.Resolve<ITemp2>();
        Assert.Null(temp3result.temp1obj.temp1Name);
        Assert.Null(temp3result.temp2obj.temp2Name);
        Assert.Null(temp3result.temp3Name);
        Assert.Null(temp1result.temp1Name);
        Assert.Null(temp2result.temp2Name);
        var temp1dependenciesCount = diContainer.GetDependencies<ITemp1>().Count();
        Assert.Equal(0, temp1dependenciesCount);
        var temp2dependenciesCount = diContainer.GetDependencies<ITemp2>().Count();
        Assert.Equal(0, temp2dependenciesCount);
        var temp3dependenciesCount = diContainer.GetDependencies<ITemp3>().Count();
        Assert.Equal(2, temp3dependenciesCount);
    }

    [Fact]
    public void CheckSingleTonWithName()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Singleton);
        diContainer.Register<ITemp1, Temp4>(LifeTime.Singleton, "Temp4");
        diContainer.LoadAll();
        var temp1result = diContainer.Resolve<ITemp1>();
        var temp4result = diContainer.Resolve<ITemp1>("Temp4");
        temp1result.temp1Name = "Changed by Temp1";
        temp4result.temp1Name = "Changed by Temp4";
        temp1result = diContainer.Resolve<ITemp1>();
        temp4result = diContainer.Resolve<ITemp1>("Temp4");
        Assert.Equal("Changed by Temp1", temp1result.temp1Name);
        Assert.Equal("Changed by Temp4", temp4result.temp1Name);
    }

    [Fact]
    public void CheckTransientWithName()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Temp1>(LifeTime.Transient);
        diContainer.Register<ITemp1, Temp4>(LifeTime.Transient, "Temp4");
        diContainer.LoadAll();
        var temp1result = diContainer.Resolve<ITemp1>();
        var temp4result = diContainer.Resolve<ITemp1>("Temp4");
        temp1result.temp1Name = "Changed by Temp1";
        temp4result.temp1Name = "Changed by Temp4";
        temp1result = diContainer.Resolve<ITemp1>();
        temp4result = diContainer.Resolve<ITemp1>("Temp4");
        Assert.Null(temp1result.temp1Name);
        Assert.Null(temp4result.temp1Name);
    }

    [Fact]
    public void AssertCircularDependency()
    {
        var diContainer = new DIContainer();
        diContainer.Register<ITemp1, Class1>(LifeTime.Singleton);
        diContainer.Register<ITemp2, Class2>(LifeTime.Singleton);
        diContainer.Register<ITemp3, Class3>(LifeTime.Singleton);
        Action act = () => diContainer.LoadAll(); ;
        Exception exception = Assert.Throws<Exception>(act);
        Assert.Equal("Fail to Load due to Ciruclar dependency", exception.Message);
    }
}


