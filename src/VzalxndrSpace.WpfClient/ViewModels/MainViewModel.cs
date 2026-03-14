using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public readonly GoalApiService _goalApiClient;
        [ObservableProperty]
        private ObservableCollection<GoalDto> _goals = new();

        public MainViewModel(GoalApiService goalApiClient)
        { 
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
    }
}
