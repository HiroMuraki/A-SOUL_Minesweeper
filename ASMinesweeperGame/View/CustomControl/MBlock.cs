using System.Windows;
using System.Windows.Controls;

namespace ASMinesweeperGame.MinesweeperLib {
    public class MBlock : Button, IBlock {
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(BlockType), typeof(MBlock), new PropertyMetadata(BlockType.Blank));
        public static readonly DependencyProperty CoordinateProperty =
            DependencyProperty.Register(nameof(Coordinate), typeof(Coordinate), typeof(MBlock), new PropertyMetadata(new Coordinate(0, 0)));
        public static readonly DependencyProperty IsFlagedProperty =
            DependencyProperty.Register(nameof(IsFlaged), typeof(bool), typeof(MBlock), new PropertyMetadata(false));
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(MBlock), new PropertyMetadata(false));
        public static readonly DependencyProperty NearMinesNumProperty =
            DependencyProperty.Register(nameof(NearMinesNum), typeof(int), typeof(MBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(GameTheme), typeof(MBlock), new PropertyMetadata(GameTheme.AS));


        public GameTheme Theme {
            get { return (GameTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }
        public BlockType Type {
            get { return (BlockType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public Coordinate Coordinate {
            get { return (Coordinate)GetValue(CoordinateProperty); }
            set { SetValue(CoordinateProperty, value); }
        }
        public bool IsFlaged {
            get { return (bool)GetValue(IsFlagedProperty); }
            set { SetValue(IsFlagedProperty, value); }
        }
        public bool IsOpen {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public int NearMinesNum {
            get { return (int)GetValue(NearMinesNumProperty); }
            set { SetValue(NearMinesNumProperty, value); }
        }

        public void Reset() {
            Type = BlockType.Blank;
            IsFlaged = false;
            IsOpen = false;
        }

        static MBlock() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MBlock), new FrameworkPropertyMetadata(typeof(MBlock)));
        }
        public MBlock() {
            Coordinate = new Coordinate(0, 0);
            IsFlaged = false;
            IsOpen = false;
            NearMinesNum = 0;
            Type = BlockType.Blank;
        }
    }
}
