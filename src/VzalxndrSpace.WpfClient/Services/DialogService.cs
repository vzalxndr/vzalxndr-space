using System;
using System.Threading.Tasks;

namespace VzalxndrSpace.WpfClient.Services;

public class DialogService : IDialogService
{
    public Func<string, Task<bool>>? ShowDialogHandler { get; set; }
    public Action<string, bool>? ShowToastHandler { get; set; }

    public async Task<bool> ConfirmAsync(string message)
    {
        if (ShowDialogHandler != null)
        {
            return await ShowDialogHandler.Invoke(message);
        }
        return false;
    }

    public void ShowToast(string message, bool isError = false)
    {
        ShowToastHandler?.Invoke(message, isError);
    }
}