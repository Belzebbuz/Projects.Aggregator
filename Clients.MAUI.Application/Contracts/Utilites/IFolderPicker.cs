namespace Clients.MAUI.Application.Contracts.Utilites;

public interface IFolderPicker
{
    public Task<string> PickFolder();
}
