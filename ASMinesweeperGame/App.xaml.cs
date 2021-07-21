using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ASMinesweeperGame {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static ResourceDictionary ColorDict = new ResourceDictionary() {
            Source = new Uri("/ASMinesweeperGame;component/Resources/PresetColors.xaml", UriKind.Relative)
        };

        private void Application_Startup(object sender, StartupEventArgs e) {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
