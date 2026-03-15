using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VzalxndrSpace.WpfClient.Models;
using VzalxndrSpace.WpfClient.Services;

namespace VzalxndrSpace.WpfClient.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly GoalApiService _goalApiClient;
        private readonly AuthService _authService;

        [ObservableProperty]
        private ObservableCollection<GoalDto> _goals = new();

        public MainViewModel(GoalApiService goalApiClient, AuthService authService)
        { 
            _authService = authService;
            _goalApiClient = goalApiClient;

            LoadGoalsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadGoalsAsync()
        { 
            var loadedGoals = await _goalApiClient.GetGoalsAsync();

            Goals.Clear();
            foreach (var goal in loadedGoals)
            { 
                Goals.Add(goal);
            }
        }

        [RelayCommand]
        private void Logout()
        { 
            _authService.Logout();

            var loginWindow = App.AppHost!.Services.GetRequiredService<Views.LoginWindow>();
            loginWindow.DataContext = App.AppHost!.Services.GetRequiredService<LoginViewModel>();

            loginWindow.Show();

            var mainWindow = System.Windows.Application.Current.Windows
            .OfType<Views.MainWindow>()
            .FirstOrDefault();

            mainWindow?.Close();
        }
    }
}
