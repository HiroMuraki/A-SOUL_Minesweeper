using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ASMinesweeperGame.View {
    public class RSlider : Slider {
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(Brush), typeof(RSlider), new PropertyMetadata(null));
        public Brush Theme {
            get { return (Brush)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        static RSlider() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RSlider), new FrameworkPropertyMetadata(typeof(RSlider)));
        }
    }
}
