using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VzalxndrSpace.WpfClient.Services;
using VzalxndrSpace.WpfClient.ViewModels;

namespace VzalxndrSpace.WpfClient.Views;

public partial class MainWindow : Window
{
    private TaskCompletionSource<bool>? _dialogTcs;
    private int _toastCounter = 0;

    public MainWindow(MainViewModel viewModel, IDialogService dialogService)
    {
        InitializeComponent();
        DataContext = viewModel;

        if (dialogService is DialogService ds)
        {
            ds.ShowDialogHandler = ShowDialogAsync;
            ds.ShowToastHandler = ShowToast;
        }
    }

    private Task<bool> ShowDialogAsync(string message)
    {
        DialogMessageText.Text = message;
        DialogOverlay.Visibility = Visibility.Visible;

        _dialogTcs = new TaskCompletionSource<bool>();
        return _dialogTcs.Task;
    }

    private void DialogConfirm_Click(object sender, RoutedEventArgs e)
    {
        DialogOverlay.Visibility = Visibility.Collapsed;
        _dialogTcs?.TrySetResult(true);
    }

    private void DialogCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogOverlay.Visibility = Visibility.Collapsed;
        _dialogTcs?.TrySetResult(false);
    }

    private async void ShowToast(string message, bool isError)
    {
        _toastCounter++;
        var currentToast = _toastCounter;

        Dispatcher.Invoke(() =>
        {
            ToastText.Text = message;
            var bgColor = isError ? "#D73A4A" : "#2EA043";
            ToastPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bgColor));

            var fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(300));
            var slideUp = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300))
            { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } };

            ToastPanel.BeginAnimation(OpacityProperty, fadeIn);
            ToastTransform.BeginAnimation(TranslateTransform.YProperty, slideUp);
        });

        await Task.Delay(3000);

        if (_toastCounter == currentToast)
        {
            Dispatcher.Invoke(() =>
            {
                var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));
                var slideDown = new DoubleAnimation(50, TimeSpan.FromMilliseconds(300))
                { EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn } };

                ToastPanel.BeginAnimation(OpacityProperty, fadeOut);
                ToastTransform.BeginAnimation(TranslateTransform.YProperty, slideDown);
            });
        }
    }
}