using ASMinesweeperGame.ViewModel;
using System;
using System.Windows;

namespace ASMinesweeperGame {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static ResourceDictionary ColorDict { get; } = new ResourceDictionary() {
            Source = new Uri("/ASMinesweeperGame;component/Resources/PresetColors.xaml", UriKind.Relative)
        };

        private void Application_Startup(object sender, StartupEventArgs e) {
            ResolveLaunchArguments(e.Args);
            MainWindow window = new MainWindow();
            window.Show();
        }
        private void ResolveLaunchArguments(string[] args) {
            GameSetter.Instance.SwitchDifficult(GameDifficult.Normal);
            try {
                for (int i = 0; i < args.Length; i++) {
                    string currentArg = args[i].ToUpper();
                    if (currentArg == "-ROW") {
                        GameSetter.Instance.RowSize = Convert.ToInt32(args[i + 1]);
                        i += 1;
                    }
                    else if (currentArg == "-COLUMN" || currentArg == "-COL") {
                        GameSetter.Instance.ColumnSize = Convert.ToInt32(args[i + 1]);
                        i += 1;
                    }
                    else if (currentArg == "-JI" || currentArg == "MINE") {
                        GameSetter.Instance.MineSize = Convert.ToInt32(args[i + 1]);
                        i += 1;
                    }
                    else if (currentArg == "-SOUND" || currentArg == "SOUNDPACK") {
                        GameSoundPlayer.Instance.GameSoundDirectory = args[i + 1];
                    }
                }
                GameSoundPlayer.Instance.LoadSounds();
            }
            catch {
                GameSetter.Instance.SwitchDifficult(GameDifficult.Normal);
                MessageBox.Show("启动参数解析错误，使用默认值");
            }
        }
    }
}
