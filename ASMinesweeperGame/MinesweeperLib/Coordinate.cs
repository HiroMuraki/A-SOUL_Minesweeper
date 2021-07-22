namespace ASMinesweeperGame.MinesweeperLib {
    public struct Coordinate {
        private int _row;
        private int _column;

        public int Row {
            get {
                return _row;
            }
            set {
                _row = value;
            }
        }
        public int Column {
            get {
                return _column;
            }
            set {
                _column = value;
            }
        }

        public Coordinate(int row, int column) {
            _row = row;
            _column = column;
        }

        public static bool operator ==(Coordinate left, Coordinate right) {
            return left._row == right._row && left._column == right._column;
        }
        public static bool operator !=(Coordinate left, Coordinate right) {
            return !(left == right);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override string ToString() {
            return $"[{_row}, {_column}]";
        }
    }
}
