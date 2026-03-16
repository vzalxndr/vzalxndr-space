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

        [RelayCommand]
        private async Task OpenAddGoalWindowAsync()
        {
            var addGoalWindow = App.AppHost!.Services.GetRequiredService<VzalxndrSpace.WpfClient.Views.AddGoalWindow>();
            addGoalWindow.DataContext = App.AppHost!.Services.GetRequiredService<VzalxndrSpace.WpfClient.ViewModels.AddGoalViewModel>();

            addGoalWindow.Owner = System.Windows.Application.Current.Windows
                .OfType<VzalxndrSpace.WpfClient.Views.MainWindow>()
                .FirstOrDefault();

            bool? result = addGoalWindow.ShowDialog();

            if (result == true)
            {
                await LoadGoalsAsync();
            }
        }

        //[RelayCommand]
        //private async Task DeleteGoalAsync(GoalDto? goal)
        //{
        //    if (goal == null)
        //    {
        //        return;
        //    }

        //    bool success = await _goalApiClient.DeleteGoalAsync(goal.Id);

        //    if (success)
        //    {
        //        Goals.Remove(goal);
        //    }
        //    else
        //    { 
        //        //TODO: MessageBox with an error 
        //    }
        //}

        [RelayCommand]
        private async Task CompleteGoalAsync(GoalDto? goal)
        {
            if (goal == null || goal.Status == 1)
            {
                return;
            }

            bool success = await _goalApiClient.CompleteGoalAsync(goal.Id);

            if (success)
            { 
                int index = Goals.IndexOf(goal);
                if (index != -1)
                {
                    var updatedGoal = goal with { Status = 1 };
                    Goals[index] = updatedGoal;
                }
            }
        }
    }
}
