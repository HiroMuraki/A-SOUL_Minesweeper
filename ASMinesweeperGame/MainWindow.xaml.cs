using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ASMinesweeperGame.MinesweeperLib;
using ASMinesweeperGame.ViewModel;
using Microsoft.Win32;

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

        public Game Game { get; set; }
        public GameSetter GameSetter { get; set; }
        public GameSoundPlayer GameSound { get; set; }

        public MainWindow() {
            Game = Game.GetInstance();
            Game.BlockCreater = () => {
                return new MBlock();
            };
            GameSetter = GameSetter.GetInstance();
            Game.GameLayoutRested += Game_GameLayoutRested;
            Game.GameCompleted += Game_GameCompleted;
            GameSound = GameSoundPlayer.GetInstance();
            InitializeComponent();
            GridRoot.MaxHeight = SystemParameters.WorkArea.Height + 1;
            GridRoot.MaxWidth = SystemParameters.WorkArea.Width + 1;
            StartGame_Click(null, new StartGameEventArgs(StartGameInfo.Custom));
            ExpandSetterPanel();
            GameSound.PlayMusic();
        }

        private void StartGame_Click(object sender, StartGameEventArgs e) {
            try {
                Theme = GameSetter.GetRandomTheme();
                GameSetter.SwitchDiffcult(e.StartInfo);
                Game.Start(GameSetter.RowSize, GameSetter.ColumnSize, GameSetter.MineSize);
                GameStatistics.Hide();
                GameRemaker.Hide();
            }
            catch (Exception exp) {
                MessageTip.DisplayTip($"文件读取错误：{exp.Message}", TimeSpan.FromSeconds(2));
            }
        }
        private void RestoreGame_Click(object sender, DragEventArgs e) {
            string layoutFile = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            RestoreGame(layoutFile);
        }
        private void RestoreGame_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "存档文件|*.txt";
            if (ofd.ShowDialog() == true) {
                RestoreGame(ofd.FileName);
            }
        }
        private void RemakeGame_Remake(object sender, RemakeEventArgs e) {
            Game.Restart();
            GameRemaker.Hide();
            MessageTip.DisplayTip("/remake", TimeSpan.FromSeconds(1));
        }
        private void RemakeGame_Error(object sender, RemakeEventArgs e) {
            MessageTip.DisplayTip("/remake出错，请检查/remake指令", TimeSpan.FromSeconds(2));
        }
        private void SaveGame_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "MWLayout.txt";
            if (sfd.ShowDialog() == true) {
                GameSetter.SaveLayout(Game.BlockArray, Game.RowSize, Game.ColumnSize, sfd.FileName);
                MessageTip.DisplayTip($"保存完成：{sfd.FileName}", TimeSpan.FromSeconds(2));
            }
        }
        private void Game_GameCompleted(object sender, GameCompletedEventArgs e) {
            if (e.IsGameFinished) {
                GameStatistics.Display(Game.RowSize, Game.ColumnSize, Game.MineSize, e.Time);
            }
            else {
                GameRemaker.Display();
                GameSound.PlayMineFXSound();
            }
        }
        private void Game_GameLayoutRested(object sender, GameLayoutRestedEventArgs e) {
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
        private void ExpandSetterPanel_Click(object sender, RoutedEventArgs e) {
            if (SetterArea.Width != 240) {
                ExpandSetterPanel();
            }
            else {
                FoldSetterPanel();
            }
        }

        private void Block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount >= 2) {
                Game.OpenNearBlocks(sender as MBlock);
                GameSound.PlayOpenFXSound();
            }
            else {
                Game.OpenBlock(sender as MBlock);
                GameSound.PlayQuickOpenFXSound();
            }
            e.Handled = true;
        }
        private void Block_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            Game.FlagBlock(sender as MBlock);
            GameSound.PlayFlagFXSound();
            e.Handled = true;
        }

        private void Window_Minimum(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }
        private void Window_Maximum(object sender, RoutedEventArgs e) {
            if (WindowState == WindowState.Normal) {
                WindowState = WindowState.Maximized;
            }
            else {
                WindowState = WindowState.Normal;
            }
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
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

        private void FoldSetterPanel() {
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
        }
        private void ExpandSetterPanel() {
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
