using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows;
using VzalxndrSpace.WpfClient.Models;
using VzalxndrSpace.WpfClient.Services;

namespace VzalxndrSpace.WpfClient.ViewModels;

public partial class EditGoalViewModel : ObservableObject
{
    private readonly GoalApiService _apiClient;

    private Guid _goalId;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public EditGoalViewModel(GoalApiService apiClient)
    {
        _apiClient = apiClient;
    }

    public void Initialize(GoalDto goal)
    {
        _goalId = goal.Id;
        Title = goal.Title;
        Description = goal.Description;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync(Window window)
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Title cannot be empty.";
            return;
        }

        if (_goalId == Guid.Empty)
        {
            ErrorMessage = "Error: Goal ID is missing.";
            return;
        }

        ErrorMessage = string.Empty;

        try
        {
            var updatedGoal = await _apiClient.UpdateGoalAsync(_goalId, Title, Description);

            if (updatedGoal != null)
            {
                window.DialogResult = true;
                window.Close();
            }
            else
            {
                ErrorMessage = "Failed to update the goal. Server returned an error.";
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Connection error. Please check your internet.";
        }
    }
}