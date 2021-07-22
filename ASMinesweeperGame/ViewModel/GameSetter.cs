using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ASMinesweeperGame.MinesweeperLib;

namespace ASMinesweeperGame.ViewModel {
    public class GameSetter : INotifyPropertyChanged {
        #region 后备字段
        private static GameSetter _singletonInstance;
        private readonly static object _singletonLocker = new object();
        private int _rowSize;
        private int _columnSize;
        private int _mineSize;
        private GameTheme _gameTheme;
        #endregion

        #region 公开事件
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 公共属性
        public GameTheme GameTheme {
            get {
                return _gameTheme;
            }
            set {
                _gameTheme = value;
                OnPropertyChanged(nameof(GameTheme));
            }
        }
        public int RowSize {
            get {
                return _rowSize;
            }
            set {
                _rowSize = value;
                OnPropertyChanged(nameof(RowSize));
                OnPropertyChanged(nameof(MaxMinesSize));
                OnPropertyChanged(nameof(MineSize));
            }
        }
        public int ColumnSize {
            get {
                return _columnSize;
            }
            set {
                _columnSize = value;
                OnPropertyChanged(nameof(ColumnSize));
                OnPropertyChanged(nameof(MaxMinesSize));
                OnPropertyChanged(nameof(MineSize));
            }
        }
        public int MineSize {
            get {
                return _mineSize;
            }
            set {
                _mineSize = value;
                OnPropertyChanged(nameof(MineSize));
            }
        }
        public int MinRowSize { get { return 6; } }
        public int MaxRowSize { get { return 20; } }
        public int MinColumnSize { get { return 6; } }
        public int MaxColumnSize { get { return 30; } }
        public int MinMinesSize { get { return _rowSize * _columnSize / 20; } }
        public int MaxMinesSize { get { return _rowSize * _columnSize / 4; } }
        #endregion

        private GameSetter() {
            _rowSize = 9;
            _columnSize = 9;
            _mineSize = 10;
            GameTheme = GameTheme.AS;
        }
        public static GameSetter GetInstance() {
            if (_singletonInstance == null) {
                lock (_singletonLocker) {
                    if (_singletonInstance == null) {
                        _singletonInstance = new GameSetter();
                    }
                }
            }
            return _singletonInstance;
        }

        public static GameTheme GetRandomTheme() {
            Random rnd = new Random();
            switch (rnd.Next(0, 6)) {
                case 0:
                    return GameTheme.AS;
                case 1:
                    return GameTheme.Ava;
                case 2:
                    return GameTheme.Bella;
                case 3:
                    return GameTheme.Carol;
                case 4:
                    return GameTheme.Diana;
                case 5:
                    return GameTheme.Eileen;
                default:
                    return GameTheme.AS;
            }
        }
        public static int[,] LoadLayout(string layoutFilePath) {
            List<int[]> layoutInf = new List<int[]>();
            int rowSize = 0;
            int colSize = 0;
            using (StreamReader reader = new StreamReader(layoutFilePath)) {
                int row = 0;
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine().Trim();
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }
                    string[] subLayout = Regex.Split(line, @"[\s]{1,}");
                    // 以第一行的列数为列数
                    if (row == 0) {
                        colSize = subLayout.Length;
                    }
                    // 从第二行开始，如果出现列数和第一行不同的行，抛出异常
                    else if (colSize != subLayout.Length) {
                        throw new FormatException();
                    }
                    layoutInf.Add(new int[colSize]);
                    for (int col = 0; col < colSize; col++) {
                        layoutInf[row][col] = Convert.ToInt32(subLayout[col]);
                    }
                    row++;
                }
                rowSize = row;
            }
            // 转化为二维数组并返回
            int[,] layout = new int[rowSize, colSize];
            for (int row = 0; row < rowSize; row++) {
                for (int col = 0; col < colSize; col++) {
                    layout[row, col] = layoutInf[row][col];
                }
            }
            return layout;
        }
        public static void SaveLayout(int[,] layout, string filePath) {
            int rowSize = layout.GetLength(0);
            int colSize = layout.GetLength(1);
            using (StreamWriter writer = new StreamWriter(filePath)) {
                for (int row = 0; row < rowSize; row++) {
                    for (int col = 0; col < colSize; col++) {
                        writer.Write($"{layout[row, col]}");
                        if (col < colSize - 1) {
                            writer.Write(' ');
                        }
                    }
                    if (row < rowSize - 1) {
                        writer.Write('\n');
                    }
                }
            }
        }
        public static void SaveLayout(IEnumerable<IBlock> layoutArray, int rowSize, int colSize, string filePath) {
            using (StreamWriter writer = new StreamWriter(filePath)) {
                int col = 0;
                int row = 0;
                foreach (var block in layoutArray) {
                    if (block.Type == BlockType.Mine) {
                        writer.Write(1);
                    }
                    else {
                        writer.Write(0);
                    }
                    col++;
                    writer.Write(' ');
                    if (col >= colSize) {
                        col = 0;
                        row += 1;
                        writer.Write('\n');
                    }
                    if (row >= rowSize) {
                        break;
                    }
                }
            }
        }
        public void SwitchDiffcult(StartGameInfo gameInfo) {
            switch (gameInfo) {
                case StartGameInfo.Easy:
                    RowSize = 9;
                    ColumnSize = 9;
                    MineSize = 10;
                    break;
                case StartGameInfo.Normal:
                    RowSize = 16;
                    ColumnSize = 16;
                    MineSize = 40;
                    break;
                case StartGameInfo.Hard:
                    RowSize = 16;
                    ColumnSize = 30;
                    MineSize = 99;
                    break;
                case StartGameInfo.Custom:
                    break;
                default:
                    break;
            }
        }

        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
