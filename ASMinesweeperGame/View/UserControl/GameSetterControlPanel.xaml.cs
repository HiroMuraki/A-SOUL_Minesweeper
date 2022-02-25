using ASMinesweeperGame.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ASMinesweeperGame.View {
    /// <summary>
    /// GameSetterControlPanel.xaml 的交互逻辑
    /// </summary>
    public partial class GameSetterControlPanel : UserControl {
        public event EventHandler<StartGameEventArgs>? GameStarted;

        public static readonly DependencyProperty GameThemeProperty =
            DependencyProperty.Register(nameof(GameTheme), typeof(GameTheme), typeof(GameSetterControlPanel), new PropertyMetadata(GameTheme.AS));

        public RoutedCommand StartGame { get; } = new RoutedCommand();

        public GameSetter GameSetter { get; } = GameSetter.Instance;
        public GameTheme GameTheme {
            get { return (GameTheme)GetValue(GameThemeProperty); }
            set { SetValue(GameThemeProperty, value); }
        }

        private void RegistCommands() {
            var sgBinding = new CommandBinding();
            sgBinding.Command = StartGame;
            sgBinding.CanExecute += (s, e) => e.CanExecute = true;
            sgBinding.Executed += (s, e) => {
                try {
                    GameStarted?.Invoke(this, new StartGameEventArgs((GameDifficult)e.Parameter));
                }
                catch (Exception) {
                    GameStarted?.Invoke(this, new StartGameEventArgs(GameDifficult.Custom));
                }
            };
            CommandBindings.Add(sgBinding);
        }

        public GameSetterControlPanel() {
            RegistCommands();
            InitializeComponent();
        }
    }
}
