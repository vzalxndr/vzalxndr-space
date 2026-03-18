using System.Threading.Tasks;

namespace VzalxndrSpace.WpfClient.Services;

public interface IDialogService
{
    Task<bool> ConfirmAsync(string message);
    void ShowToast(string message, bool isError = false);
}