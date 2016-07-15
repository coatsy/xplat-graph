namespace PropertyManager.Services
{
    public interface IDialogService 
    {
        IDialogHandle ShowProgress(string title, string message);
    }
}
