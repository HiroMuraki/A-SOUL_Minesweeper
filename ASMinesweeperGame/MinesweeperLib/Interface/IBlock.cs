namespace ASMinesweeperGame.MinesweeperLib {
    public interface IBlock {
        GameTheme Theme { get; set; }
        BlockType Type { get; set; }
        Coordinate Coordinate { get; set; }
        bool IsFlaged { get; set; }
        bool IsOpen { get; set; }
        int NearMinesNum { get; set; }
    }
}
