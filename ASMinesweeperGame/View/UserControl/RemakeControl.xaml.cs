using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ASMinesweeperGame.View {
    /// <summary>
    /// RemakeControl.xaml 的交互逻辑
    /// </summary>
    public partial class RemakeControl : UserControl {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(RemakeControl), new PropertyMetadata(""));
        public static string RemakePassword { get; } = "/REMAKE";

        public event EventHandler<RemakeEventArgs>? Remake;
        public event EventHandler<RemakeEventArgs>? PasswordError;
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Password
        /// </summary>
        public string Password {
            get {
                return (string)GetValue(PasswordProperty);
            }
            set {
                SetValue(PasswordProperty, value);
            }
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

        public RemakeControl() {
            InitializeComponent();
        }
        private void Remake_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (Password.ToUpper() == RemakePassword) {
                    Password = "";
                    Remake?.Invoke(this, new RemakeEventArgs());
                }
                else {
                    PasswordError?.Invoke(this, new RemakeEventArgs());
                }
            }
        }
    }
}
