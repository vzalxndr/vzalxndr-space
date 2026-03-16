using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VzalxndrSpace.WpfClient.ViewModels;

namespace VzalxndrSpace.WpfClient.ViewModels
{
    public partial class LoginViewModel : ObservableObject 
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public LoginViewModel(AuthService authService)
        { 
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;

            bool success = await _authService.LoginAsync(Password);

            if (success)
            {
                var mainWindow = App.AppHost!.Services.GetRequiredService<Views.MainWindow>();

                mainWindow.DataContext = App.AppHost!.Services.GetRequiredService<MainViewModel>();

                mainWindow.Show();

                System.Windows.Application.Current.Windows[0]?.Close();
            }
            else
            {
                ErrorMessage = "Invalid password or server is currently unavailable :(";
            }

            IsLoading = false;
        }
    }
}
