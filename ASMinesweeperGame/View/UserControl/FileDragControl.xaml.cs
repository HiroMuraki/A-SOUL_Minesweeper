using System.Windows;
using System.Windows.Controls;

namespace ASMinesweeperGame.View {
    /// <summary>
    /// FileDragControl.xaml 的交互逻辑
    /// </summary>
    public partial class FileDragControl : UserControl {
        public event DragEventHandler? FileDraged;
        public FileDragControl() {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e) {
            FileDraged?.Invoke(this, e);
        }
    }
}
