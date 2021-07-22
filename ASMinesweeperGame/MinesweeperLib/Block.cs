using System.ComponentModel;

namespace ASMinesweeperGame.MinesweeperLib {
    public class Block : IBlock, INotifyPropertyChanged {
        #region 后备字段
        private GameTheme _theme;
        private BlockType _type;
        private Coordinate _coordinate;
        private bool _isFlaged;
        private bool _isOpen;
        private int _nearMinesNum;
        #endregion

        #region 公共属性
        public GameTheme Theme {
            get {
                return _theme;
            }
            set {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }
        public BlockType Type {
            get {
                return _type;
            }
            set {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public Coordinate Coordinate {
            get {
                return _coordinate;
            }
            set {
                _coordinate = value;
                OnPropertyChanged(nameof(Coordinate));
            }
        }
        public bool IsFlaged {
            get {
                return _isFlaged;
            }
            set {
                _isFlaged = value;
                OnPropertyChanged(nameof(IsFlaged));
            }
        }
        public bool IsOpen {
            get {
                return _isOpen;
            }
            set {
                _isOpen = value;
                OnPropertyChanged(nameof(IsOpen));
            }
        }
        public int NearMinesNum {
            get {
                return _nearMinesNum;
            }
            set {
                _nearMinesNum = value;
                OnPropertyChanged(nameof(NearMinesNum));
            }
        }
        #endregion

        #region 构造方法
        public Block() {

        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
