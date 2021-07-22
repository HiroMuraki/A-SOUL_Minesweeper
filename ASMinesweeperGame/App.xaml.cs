using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ASMinesweeperGame.ViewModel;

namespace ASMinesweeperGame {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static ResourceDictionary ColorDict = new ResourceDictionary() {
            Source = new Uri("/ASMinesweeperGame;component/Resources/PresetColors.xaml", UriKind.Relative)
        };

        private void Application_Startup(object sender, StartupEventArgs e) {
            ResolveLaunchArguments(e.Args);
            MainWindow window = new MainWindow();
            window.Show();
        }
        private void ResolveLaunchArguments(string[] args) {
            GameSetter setter = GameSetter.GetInstance();
            GameSoundPlayer gameSound = GameSoundPlayer.GetInstance();
            setter.SwitchDiffcult(StartGameInfo.Normal);
            try {
                for (int i = 0; i < args.Length; i++) {
                    string currentArg = args[i].ToUpper();
                    if (currentArg == "-ROW") {
                        setter.RowSize = Convert.ToInt32(args[i + 1]);
                        i += 1;
                    }
                    else if (currentArg == "-COLUMN" || currentArg == "-COL") {
                        setter.ColumnSize = Convert.ToInt32(args[i + 1]);
                        i += 1;
                    }
                    else if (currentArg == "-JI" || currentArg == "MINE") {
                        setter.MineSize = Convert.ToInt32(args[i + 1]);
                        i += 1;
                    }
                    else if (currentArg == "-SOUND" || currentArg == "SOUNDPACK") {
                        gameSound.GameSoundDirectory = args[i + 1];
                    }
                }
                gameSound.LoadSounds();
            }
            catch {
                setter.SwitchDiffcult(StartGameInfo.Normal);
                MessageBox.Show("启动参数解析错误，使用默认值");
            }
        }
    }
}
