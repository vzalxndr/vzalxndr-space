using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VzalxndrSpace.WpfClient.Models;
using VzalxndrSpace.WpfClient.Services;

namespace VzalxndrSpace.WpfClient.ViewModels;

public partial class SessionViewModel : ObservableObject
{
    private readonly GoalApiService _apiClient;
    private readonly DispatcherTimer _timer;
    private int _secondsLeft;
    private const int DefaultSessionMinutes = 25;

    private Guid? _currentSessionId;

    [ObservableProperty]
    private GoalDto? _currentGoal;

    [ObservableProperty]
    private string _timeLeftDisplay = "25:00";

    [ObservableProperty]
    private bool _isStartButtonEnabled = true;

    public SessionViewModel(GoalApiService apiClient)
    {
        _apiClient = apiClient;
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
    }

    public void Initialize(GoalDto goal)
    {
        CurrentGoal = goal;
        _secondsLeft = DefaultSessionMinutes * 60;
        _currentSessionId = null;
        UpdateTimeDisplay();

        IsStartButtonEnabled = true;
    }

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        if (_secondsLeft > 0)
        {
            _secondsLeft--;
            UpdateTimeDisplay();
        }
        else
        {
            _timer.Stop();
            await StopServerSessionAsync();
        }
    }

    [RelayCommand]
    private async Task StartTimerAsync()
    {
        if (CurrentGoal != null)
        {
            IsStartButtonEnabled = false;

            _currentSessionId = await _apiClient.StartSessionAsync(CurrentGoal.Id, DefaultSessionMinutes);
        }

        _timer.Start();
    }

    [RelayCommand]
    private async Task StopSessionAsync(System.Windows.Window window)
    {
        _timer.Stop();

        await StopServerSessionAsync();

        window.Close();
    }

    private async Task StopServerSessionAsync()
    {
        if (_currentSessionId != null)
        {
            await _apiClient.StopSessionAsync(_currentSessionId.Value);
            _currentSessionId = null;
        }
    }

    public void ForceStopSession()
    {
        _timer.Stop();
        _ = StopServerSessionAsync();
    }

    private void UpdateTimeDisplay()
    {
        TimeSpan time = TimeSpan.FromSeconds(_secondsLeft);
        TimeLeftDisplay = time.ToString(@"mm\:ss");
    }
}