using System;

namespace ASMinesweeperGame.MinesweeperLib {
    public class GameStartedEventArgs : EventArgs {
        public int RowSize { get; }
        public int ColumnSize { get; }
        public int MineSize { get; }

        public GameStartedEventArgs(int rowSize, int columnSize, int mineSize) {
            RowSize = rowSize;
            ColumnSize = columnSize;
            MineSize = mineSize;
        }
    }
}
