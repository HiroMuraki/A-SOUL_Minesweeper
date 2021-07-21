﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMinesweeperGame.MinesweeperLib {
    public class GameCompletedEventArgs : EventArgs {
        private readonly bool _isGameFinished;
        private readonly TimeSpan _time;

        public bool IsGameFinished {
            get {
                return _isGameFinished;
            }
        }
        public TimeSpan Time {
            get {
                return _time;
            }
        }

        public GameCompletedEventArgs(bool gameFinished, TimeSpan time) {
            _isGameFinished = gameFinished;
            _time = time;
        }
    }
}