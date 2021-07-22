using System;

namespace ASMinesweeperGame.MinesweeperLib {
    public class GameStartedEventArgs : EventArgs {
        private readonly int _rowSize;
        private readonly int _columnSize;
        private readonly int _mineSize;

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


        public GameStartedEventArgs(int rowSize, int columnSize, int mineSize) {
            _rowSize = rowSize;
            _columnSize = columnSize;
            _mineSize = mineSize;
        }
    }
}
