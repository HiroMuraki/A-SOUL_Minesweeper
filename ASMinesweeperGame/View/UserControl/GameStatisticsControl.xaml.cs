using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ASMinesweeperGame.View {
    /// <summary>
    /// GameStatisticsControl.xaml 的交互逻辑
    /// </summary>
    public partial class GameStatisticsControl : UserControl, INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int RowSize { get; private set; }
        public int ColumnSize { get; private set; }
        public int MineSize { get; private set; }
        public double Time { get; private set; }

        public void Display(int rowSize, int columnSize, int mineSize, TimeSpan time) {
            IsHitTestVisible = true;
            RowSize = rowSize;
            ColumnSize = columnSize;
            MineSize = mineSize;
            Time = time.TotalSeconds;
            OnPropertyChanged(nameof(RowSize));
            OnPropertyChanged(nameof(ColumnSize));
            OnPropertyChanged(nameof(MineSize));
            OnPropertyChanged(nameof(Time));
            DoubleAnimation animation = new DoubleAnimation() {
                To = 1,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            BeginAnimation(OpacityProperty, animation);
        }
        public void Hide() {
            IsHitTestVisible = false;
            DoubleAnimation animation = new DoubleAnimation() {
                To = 0,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            BeginAnimation(OpacityProperty, animation);
        }

        public GameStatisticsControl() {
            InitializeComponent();
        }
    }
}
