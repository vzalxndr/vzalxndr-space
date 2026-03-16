using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VzalxndrSpace.WpfClient;
using VzalxndrSpace.WpfClient.Services;
using VzalxndrSpace.WpfClient.ViewModels;

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
                    client.BaseAddress = new Uri("http://localhost:5089/");
                });

                services.AddHttpClient<GoalApiService>(client =>
                {
                    client.BaseAddress = new Uri("http://localhost:5089/");
                });

                services.AddTransient<LoginViewModel>();
                services.AddTransient<Views.LoginWindow>();

                services.AddTransient<MainViewModel>();
                services.AddTransient<Views.MainWindow>();

                services.AddTransient<AddGoalViewModel>();
                services.AddTransient<Views.AddGoalWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    { 
        await AppHost!.StartAsync();
        base.OnStartup(e);
        var loginWindow = AppHost.Services.GetRequiredService<Views.LoginWindow>();

        loginWindow.DataContext = AppHost.Services.GetRequiredService<LoginViewModel>();

        loginWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}

