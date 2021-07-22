using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMinesweeperGame.MinesweeperLib {
    public interface IBlock {
        public GameTheme Theme { get; set; }
        public BlockType Type { get; set; }
        public Coordinate Coordinate { get; set; }
        public bool IsFlaged { get; set; }
        public bool IsOpen { get; set; }
        public int NearMinesNum { get; set; }
    }
}
