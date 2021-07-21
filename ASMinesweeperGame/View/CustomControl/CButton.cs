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
    public class CButton : Button {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CButton), new PropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(GameTheme), typeof(CButton), new PropertyMetadata(GameTheme.AS));

        public GameTheme Theme {
            get { return (GameTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }
        public CornerRadius CornerRadius {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static CButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CButton), new FrameworkPropertyMetadata(typeof(CButton)));
        }
    }
}
