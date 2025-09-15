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
            Properties["PathFile"] = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "SimplyNoted", "__sn_notes__.json");
        }
    }
}