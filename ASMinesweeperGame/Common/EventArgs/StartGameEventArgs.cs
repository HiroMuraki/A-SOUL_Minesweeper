using System;

namespace ASMinesweeperGame {
    public class StartGameEventArgs : EventArgs {
        private readonly StartGameInfo _startInfo;
        public StartGameInfo StartInfo {
            get {
                return _startInfo;
            }
        }

        public StartGameEventArgs(StartGameInfo startInfo) {
            _startInfo = startInfo;
        }
    }
}
