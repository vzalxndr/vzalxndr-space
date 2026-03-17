using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VzalxndrSpace.WpfClient.Models;
using VzalxndrSpace.WpfClient.Services;

namespace VzalxndrSpace.WpfClient.ViewModels
{
    public partial class AddGoalViewModel : ObservableObject
    {
        private readonly GoalApiService _apiClient;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public AddGoalViewModel(GoalApiService apiClient)
        {
            _apiClient = apiClient;
        }

        [RelayCommand]
        private async Task SaveAsync(Window window)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorMessage = "Title cannot be empty";
                return;
            }

            ErrorMessage = string.Empty;

            bool success = await _apiClient.CreateGoalAsync(Title, Description);

            if (success)
            {
                window.DialogResult = true;
                window.Close();
            }
            else 
            {
                ErrorMessage = "Goal saving failed, server is unreacheble.";
            }
        }
    }
}
