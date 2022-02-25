using System;

namespace ASMinesweeperGame.MinesweeperLib {
    public class GameCompletedEventArgs : EventArgs {
        public bool IsGameFinished { get; }
        public TimeSpan Time { get; }

        public GameCompletedEventArgs(bool gameFinished, TimeSpan time) {
            IsGameFinished = gameFinished;
            Time = time;
        }
    }
}
