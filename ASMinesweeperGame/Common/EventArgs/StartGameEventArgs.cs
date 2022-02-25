using System;

namespace ASMinesweeperGame {
    public class StartGameEventArgs : EventArgs {
        public GameDifficult StartInfo { get; }

        public StartGameEventArgs(GameDifficult startInfo) {
            StartInfo = startInfo;
        }
    }
}
