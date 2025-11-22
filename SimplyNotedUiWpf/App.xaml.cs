using System.IO;
using System.Windows;

namespace SimplyNotedUiWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            Properties["PathFile"] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SimplyNoted", "__sn_notes__.json");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _ = new ViewModels.MainViewModel();
        }
    }
}