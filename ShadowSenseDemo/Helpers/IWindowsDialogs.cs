namespace ShadowSenseDemo.Helpers
{
    public interface IWindowsDialogs
    {
        string[] ShowOpenFileDialog(string caption, string filter = null, string initialDirectory = null);

    }
}
