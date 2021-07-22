using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ASMinesweeperGame.View {
    /// <summary>
    /// RemakeControl.xaml 的交互逻辑
    /// </summary>
    public partial class RemakeControl : UserControl, INotifyPropertyChanged {
        private static readonly string _remakePassword = "/REMAKE";
        private string _password;

        public string Password {
            get {
                return _password;
            }
            set {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public event EventHandler<RemakeEventArgs> Remake;
        public event EventHandler<RemakeEventArgs> Error;
        public event PropertyChangedEventHandler PropertyChanged;

        public RemakeControl() {
            _password = "";
            InitializeComponent();
        }

        public void Display() {
            IsHitTestVisible = true;
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

        private void Remake_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (Password.ToUpper() == _remakePassword) {
                    Password = "";
                    Remake?.Invoke(this, new RemakeEventArgs());
                }
                else {
                    Error?.Invoke(this, new RemakeEventArgs());
                }
            }
        }
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
