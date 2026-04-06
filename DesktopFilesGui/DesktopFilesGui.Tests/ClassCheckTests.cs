using System.Reflection;

namespace DesktopFilesGui.Tests;

public class ClassCheckTests
{
    [Fact]
    public void Services_AllMustBeSealed()
    {
        //Arrange
        var services = Assembly
            .GetAssembly(typeof(App))!
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, Namespace: "DesktopFilesGui.Services" });
        //Act
        var result = services.Where(@class => !@class.IsSealed);
        //Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void Converters_AllMustBeSealed()
    {
        //Arrange
        var services = Assembly
            .GetAssembly(typeof(App))!
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, Namespace: "DesktopFilesGui.Converters" });
        //Act
        var result = services.Where(@class => !@class.IsSealed);
        //Assert
        Assert.Empty(result);
    }
    
    
    [Fact]
    public void Attributes_AllMustBeSealed()
    {
        //Arrange
        var services = Assembly
            .GetAssembly(typeof(App))!
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, Namespace: "DesktopFilesGui.Attributes" });
        //Act
        var result = services.Where(@class => !@class.IsSealed);
        //Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void DataAnnotations_AllMustBeSealed()
    {
        //Arrange
        var services = Assembly
            .GetAssembly(typeof(App))!
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, Namespace: "DesktopFilesGui.DataAnnotations" });
        //Act
        var result = services.Where(@class => !@class.IsSealed);
        //Assert
        Assert.Empty(result);
    }
}