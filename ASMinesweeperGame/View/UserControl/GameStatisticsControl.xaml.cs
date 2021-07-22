using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ASMinesweeperGame.View {
    /// <summary>
    /// GameStatisticsControl.xaml 的交互逻辑
    /// </summary>
    public partial class GameStatisticsControl : UserControl, INotifyPropertyChanged {
        private int _rowSize;
        private int _columnSize;
        private int _mineSize;
        private TimeSpan _time;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int RowSize {
            get {
                return _rowSize;
            }
        }
        public int ColumnSize {
            get {
                return _columnSize;
            }
        }
        public int MineSize {
            get {
                return _mineSize;
            }
        }
        public double Time {
            get {
                return _time.TotalSeconds;
            }
        }

        public GameStatisticsControl() {
            InitializeComponent();
        }

        public void Display(int rowSize, int columnSize, int mineSize, TimeSpan time) {
            IsHitTestVisible = true;
            _rowSize = rowSize;
            _columnSize = columnSize;
            _mineSize = mineSize;
            _time = time;
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
    }
}
