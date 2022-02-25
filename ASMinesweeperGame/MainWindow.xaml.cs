using ASMinesweeperGame.MinesweeperLib;
using ASMinesweeperGame.ViewModel;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ASMinesweeperGame {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public GameTheme Theme {
            get { return (GameTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(GameTheme), typeof(MainWindow), new PropertyMetadata(GameTheme.AS));

        public RoutedCommand SaveGame { get; } = new RoutedCommand();
        public RoutedCommand LoadGame { get; } = new RoutedCommand();
        public RoutedCommand MinimumWindow { get; } = new RoutedCommand();
        public RoutedCommand MaximumWindow { get; } = new RoutedCommand();
        public RoutedCommand CloseWindow { get; } = new RoutedCommand();
        public RoutedCommand ExpandSetterPanel { get; } = new RoutedCommand();

        public Game Game { get; } = Game.Instance;
        public GameSetter GameSetter { get; } = GameSetter.Instance;
        public GameSoundPlayer GameSound { get; } = GameSoundPlayer.Instance;

        public MainWindow() {
            RegistCommands();
            Game.BlockCreater = () => {
                return new MBlock();
            };
            Game.GameLayoutRested += Game_GameLayoutRested;
            Game.GameCompleted += Game_GameCompleted;
            InitializeComponent();
            GridRoot.MaxHeight = SystemParameters.WorkArea.Height + 1;
            GridRoot.MaxWidth = SystemParameters.WorkArea.Width + 1;
            StartGame_Click(null!, new StartGameEventArgs(GameDifficult.Easy));
            ExpandSetterPanelHelper();
            GameSound.PlayMusic();
        }

        private void StartGame_Click(object sender, StartGameEventArgs e) {
            try {
                Theme = GameSetter.GetRandomTheme();
                GameSetter.SwitchDifficult(e.StartInfo);
                Game.Start(GameSetter.RowSize, GameSetter.ColumnSize, GameSetter.MineSize);
                GameStatistics.Hide();
                GameRemaker.Hide();
            }
            catch (Exception exp) {
                MessageTip.DisplayTip($"{exp.Message}", TimeSpan.FromSeconds(2));
            }
        }
        private void RestoreGame_Click(object sender, DragEventArgs e) {
            string layoutFile = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            RestoreGame(layoutFile);
        }
        private void RemakeGame_Remake(object sender, RemakeEventArgs e) {
            Game.Restart();
            GameRemaker.Hide();
            MessageTip.DisplayTip("/remake", TimeSpan.FromSeconds(1));
        }
        private void RemakeGame_Error(object sender, RemakeEventArgs e) {
            MessageTip.DisplayTip("/remake出错，请检查/remake指令", TimeSpan.FromSeconds(2));
        }
        private void Game_GameCompleted(object? sender, GameCompletedEventArgs e) {
            if (e.IsGameFinished) {
                GameStatistics.Display(Game.RowSize, Game.ColumnSize, Game.MineSize, e.Time);
            }
            else {
                GameRemaker.Display();
                GameSound.PlayMineFXSound();
            }
        }
        private void Game_GameLayoutRested(object? sender, GameLayoutRestedEventArgs e) {
            GameLayout.Children.Clear();
            GameLayout.Rows = Game.RowSize;
            GameLayout.Columns = Game.ColumnSize;
            foreach (var block in Game.BlockArray) {
                MBlock mBlock = (MBlock)block;
                mBlock.Theme = Theme;
                mBlock.PreviewMouseLeftButtonDown += Block_MouseLeftButtonDown;
                mBlock.PreviewMouseRightButtonDown += Block_MouseRightButtonDown;
                GameLayout.Children.Add(mBlock);
            }
        }

        private void Block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount >= 2) {
                Game.OpenNearBlocks((MBlock)sender);
                GameSound.PlayQuickOpenFXSound();
            }
            else {
                Game.OpenBlock((MBlock)sender);
                GameSound.PlayOpenFXSound();
            }
            e.Handled = true;
        }
        private void Block_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            Game.FlagBlock((MBlock)sender);
            GameSound.PlayFlagFXSound();
            e.Handled = true;
        }

        private void Window_Move(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount >= 2) {
                if (WindowState == WindowState.Normal) {
                    WindowState = WindowState.Maximized;
                }
                else {
                    WindowState = WindowState.Normal;
                }
            }
            else {
                DragMove();
            }
        }
        private void Window_DragEnter(object sender, DragEventArgs e) {
            FileLoader.IsHitTestVisible = true;
        }
        private void Window_DragLeave(object sender, DragEventArgs e) {
            FileLoader.IsHitTestVisible = false;
        }
        private void Window_Drop(object sender, DragEventArgs e) {
            FileLoader.IsHitTestVisible = false;
        }

        private void RegistCommands() {
            var saveGameBinding = new CommandBinding();
            saveGameBinding.Command = SaveGame;
            saveGameBinding.CanExecute += (s, e) => e.CanExecute = true;
            saveGameBinding.Executed += (s, e) => {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "MWLayout.txt";
                if (sfd.ShowDialog() == true) {
                    GameSetter.SaveLayout(Game.BlockArray, Game.RowSize, Game.ColumnSize, sfd.FileName);
                    MessageTip.DisplayTip($"保存完成：{sfd.FileName}", TimeSpan.FromSeconds(2));
                }
            };
            CommandBindings.Add(saveGameBinding);

            var loadGameBinding = new CommandBinding();
            loadGameBinding.Command = LoadGame;
            loadGameBinding.CanExecute += (s, e) => e.CanExecute = true;
            loadGameBinding.Executed += (s, e) => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.Filter = "存档文件|*.txt";
                if (ofd.ShowDialog() == true) {
                    RestoreGame(ofd.FileName);
                }
            };
            CommandBindings.Add(loadGameBinding);

            var minimumWindowBinding = new CommandBinding();
            minimumWindowBinding.Command = MinimumWindow;
            minimumWindowBinding.CanExecute += (s, e) => e.CanExecute = true;
            minimumWindowBinding.Executed += (s, e) => WindowState = WindowState.Minimized;
            CommandBindings.Add(minimumWindowBinding);

            var maximumWindowBinding = new CommandBinding();
            maximumWindowBinding.Command = MaximumWindow;
            maximumWindowBinding.CanExecute += (s, e) => e.CanExecute = true;
            maximumWindowBinding.Executed += (s, e) => {
                if (WindowState == WindowState.Normal) {
                    WindowState = WindowState.Maximized;
                }
                else {
                    WindowState = WindowState.Normal;
                }
            };
            CommandBindings.Add(maximumWindowBinding);

            var closeWindowBinding = new CommandBinding();
            closeWindowBinding.Command = CloseWindow;
            closeWindowBinding.CanExecute += (s, e) => e.CanExecute = true;
            closeWindowBinding.Executed += (s, e) => Application.Current.Shutdown();
            CommandBindings.Add(closeWindowBinding);

            var expandSetterPanelBinding = new CommandBinding();
            expandSetterPanelBinding.Command = ExpandSetterPanel;
            expandSetterPanelBinding.CanExecute += (s, e) => e.CanExecute = true;
            expandSetterPanelBinding.Executed += (s, e) => {
                if (SetterArea.Width != 240) {
                    ExpandSetterPanelHelper();
                }
                else {
                    FoldSetterPanelHelper();
                }
            };
            CommandBindings.Add(expandSetterPanelBinding);
        }
        private void FoldSetterPanelHelper() {
            DoubleAnimation widthAnimation = new DoubleAnimation() {
                To = 50,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                Duration = TimeSpan.FromMilliseconds(150)
            };
            DoubleAnimation opacityAnimation = new DoubleAnimation() {
                To = 0,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                Duration = TimeSpan.FromMilliseconds(150)
            };
            SetterArea.BeginAnimation(WidthProperty, widthAnimation);
            SetterPanel.BeginAnimation(OpacityProperty, opacityAnimation);
            SetterPanel.IsHitTestVisible = false;
        }
        private void ExpandSetterPanelHelper() {
            DoubleAnimation widthAnimation = new DoubleAnimation() {
                To = 240,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                Duration = TimeSpan.FromMilliseconds(150)
            };
            DoubleAnimation opacityAnimation = new DoubleAnimation() {
                To = 1,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                Duration = TimeSpan.FromMilliseconds(150)
            };
            SetterArea.BeginAnimation(WidthProperty, widthAnimation);
            SetterPanel.BeginAnimation(OpacityProperty, opacityAnimation);
            SetterPanel.IsHitTestVisible = true;
        }
        private void RestoreGame(string layoutFile) {
            try {
                int[,] layoutInf = GameSetter.LoadLayout(layoutFile);
                Game.Start(layoutInf);
                MessageTip.DisplayTip("游戏加载完成", TimeSpan.FromSeconds(1));
                GameStatistics.Hide();
                GameRemaker.Hide();
            }
            catch (Exception exp) {
                MessageTip.DisplayTip($"文件读取错误：{exp.Message}", TimeSpan.FromSeconds(2));
            }
        }
    }
}
