using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VzalxndrSpace.WpfClient;

namespace VzalxndrSpace.WpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient<AuthService>(client =>
                {
                    client.BaseAddress = new Uri("http://localhost:5000/");
                });
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    { 
        await AppHost!.StartAsync();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}

