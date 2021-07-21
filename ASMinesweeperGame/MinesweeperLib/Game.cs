using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMinesweeperGame.MinesweeperLib {
    public class Game : INotifyPropertyChanged {
        #region 后备字段
        private static Game _singletonInstance;
        private int _rowSize;
        private int _columnSize;
        private int _minesSize;
        private int _flagsCount;
        private bool _isFirstOpen;
        private DateTime _startTime;
        private Dictionary<Coordinate, MBlock> _blocks;
        #endregion

        #region 公共事件
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<GameStartedEventArgs> GameStarted;
        public event EventHandler<GameLayoutRestedEventArgs> GameLayoutRested;
        public event EventHandler<GameCompletedEventArgs> GameCompleted;
        #endregion

        #region 公共属性
        public int RowSize {
            get {
                return _rowSize;
            }
            private set {
                _rowSize = value;
                OnPropertyChanged(nameof(RowSize));
            }
        }
        public int ColumnSize {
            get {
                return _columnSize;
            }
            private set {
                _columnSize = value;
                OnPropertyChanged(nameof(ColumnSize));
            }
        }
        public int MineSize {
            get {
                return _minesSize;
            }
            private set {
                _minesSize = value;
                OnPropertyChanged(nameof(MineSize));
            }
        }
        public int GameSize {
            get {
                return _rowSize * _columnSize;
            }
        }
        public int FlagsCount {
            get {
                return _flagsCount;
            }
        }
        public string GameProcess {
            get {
                return $"{_minesSize - _flagsCount}";
            }
        }
        public IEnumerable<MBlock> BlockArray {
            get {
                foreach (var block in _blocks.Values) {
                    yield return block;
                }
            }
        }
        #endregion

        #region 构造函数
        private Game() {

        }
        public static Game GetInstance() {
            if (_singletonInstance == null) {
                _singletonInstance = new Game();
            }
            return _singletonInstance;
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 设置游戏的行数，列数和雷数
        /// </summary>
        /// <param name="rowSize">行数</param>
        /// <param name="columnSize">列数</param>
        /// <param name="mineSize">雷数</param>
        public void Start(int rowSize, int columnSize, int mineSize) {
            // 设置基本游戏信息
            _rowSize = rowSize;
            _columnSize = columnSize;
            _minesSize = mineSize;
            _flagsCount = 0;
            _isFirstOpen = true;
            // 初始化方块序列
            _blocks = new Dictionary<Coordinate, MBlock>();
            for (int row = 0; row < _rowSize; row++) {
                for (int col = 0; col < _columnSize; col++) {
                    Coordinate coordinate = new Coordinate(row, col);
                    // 创建block
                    _blocks[coordinate] = new MBlock(coordinate);
                    // 前n个方块设置为地雷
                    if (row * ColumnSize + col < MineSize) {
                        _blocks[coordinate].Type = BlockType.Mine;
                    }
                }
            }
            // 打乱方块顺序
            Shuffle();
            // 引发GameStarted事件
            GameStarted?.Invoke(this, new GameStartedEventArgs(_rowSize, _columnSize, _minesSize));
            GameLayoutRested?.Invoke(this, new GameLayoutRestedEventArgs());
            OnPropertyChanged(nameof(GameProcess));
        }
        /// <summary>
        /// 使用二维数组布局信息加载游戏
        /// </summary>
        /// <param name="layout">布局信息</param>
        public void Start(int[,] layout) {
            // 设置基本游戏信息
            _rowSize = layout.GetLength(0);
            _columnSize = layout.GetLength(1);
            _minesSize = 0;
            _flagsCount = 0;
            _isFirstOpen = false;
            // 初始化方块序列
            _blocks = new Dictionary<Coordinate, MBlock>();
            for (int row = 0; row < _rowSize; row++) {
                for (int col = 0; col < _columnSize; col++) {
                    Coordinate coordinate = new Coordinate(row, col);
                    // 根据布局信息创建block
                    _blocks[coordinate] = new MBlock(coordinate);
                    if (layout[row, col] == 0) {
                        _blocks[coordinate].Type = BlockType.Blank;
                    }
                    else {
                        _blocks[coordinate].Type = BlockType.Mine;
                        _minesSize++;
                    }
                }
            }
            ReSyncLayout();
            // 引发GameStarted和GameLayoutRested事件
            GameStarted?.Invoke(this, new GameStartedEventArgs(_rowSize, _columnSize, _minesSize));
            GameLayoutRested?.Invoke(this, new GameLayoutRestedEventArgs());
            OnPropertyChanged(nameof(GameProcess));
        }
        /// <summary>
        /// 重新开始当前游戏
        /// </summary>
        public void Restart() {
            // 设置基本游戏信息
            _flagsCount = 0;
            _isFirstOpen = false;
            // 重置方块序列
            for (int row = 0; row < _rowSize; row++) {
                for (int col = 0; col < _columnSize; col++) {
                    Coordinate coordinate = new Coordinate(row, col);
                    _blocks[coordinate].IsOpen = false;
                    _blocks[coordinate].IsFlaged = false;
                }
            }
            // 引发GameStarted和GameLayoutRested事件
            GameStarted?.Invoke(this, new GameStartedEventArgs(_rowSize, _columnSize, _minesSize));
            GameLayoutRested?.Invoke(this, new GameLayoutRestedEventArgs());
            OnPropertyChanged(nameof(GameProcess));
        }
        /// <summary>
        /// 打开指定方块，为递归打开（扫雷左键）
        /// </summary>
        /// <param name="block">待打开的方块坐标</param>
        public void OpenBlock(MBlock block) {
            if (_isFirstOpen) {
                _startTime = DateTime.Now;
                Coordinate safeCoordinate = block.Coordinate;
                SetSafeArea(safeCoordinate);
                _isFirstOpen = false;
                block = _blocks[safeCoordinate];
            }
            OpenBlockHelper(block);
            var isGameCompleted = IsGameCompleted();
            if (isGameCompleted != null) {
                TimeSpan time = DateTime.Now - _startTime;
                GameCompleted?.Invoke(this, new GameCompletedEventArgs(isGameCompleted.Value, time));
            }
        }
        /// <summary>
        /// 打开指定方块周围的方块（扫雷左键双击）
        /// </summary>
        /// <param name="block">中心方块</param>
        public void OpenNearBlocks(MBlock block) {
            // 仅已经打开的方块可触发
            if (block.IsOpen) {
                int nearFlagedBlocks = GetNearCounts(block.Coordinate, (Coordinate nCoordinate) => _blocks[nCoordinate].IsFlaged);
                // 如果标旗数大于了该方块的周围雷数，则触发
                if (nearFlagedBlocks >= block.NearMinesNum) {
                    foreach (Coordinate nearCoordinate in GetNearCoordinates(block.Coordinate)) {
                        OpenBlockHelper(_blocks[nearCoordinate]);
                    }
                }
            }
            var isGameCompleted = IsGameCompleted();
            if (isGameCompleted != null) {
                TimeSpan time = DateTime.Now - _startTime;
                GameCompleted?.Invoke(this, new GameCompletedEventArgs(isGameCompleted.Value, time));
            }
        }
        /// <summary>
        /// 为指定方块插旗（扫雷右键）
        /// </summary>
        /// <param name="block">待插旗的方块坐标</param>
        public void FlagBlock(MBlock block) {
            // 已打开的方块不可插旗
            if (block.IsOpen) {
                return;
            }
            // 切换是否标记，并更新_flagsCount
            if (block.IsFlaged) {
                block.IsFlaged = false;
                _flagsCount--;
            }
            else {
                block.IsFlaged = true;
                _flagsCount++;
            }
            OnPropertyChanged(nameof(FlagsCount));
            OnPropertyChanged(nameof(GameProcess));
        }
        #endregion

        #region 辅助方法
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 重新获取布局信息
        /// </summary>
        private void ReSyncLayout() {
            // 重设方块状态
            foreach (Coordinate coordinate in GetAllCoordinates()) {
                _blocks[coordinate].NearMinesNum = GetNearCounts(coordinate, (Coordinate nCoordinate) => _blocks[nCoordinate].Type == BlockType.Mine);
            }
            OnPropertyChanged(nameof(BlockArray));
        }
        /// <summary>
        /// 打开指定方块，为递归打开（扫雷左键）
        /// </summary>
        /// <param name="block">待打开的方块坐标</param>
        public void OpenBlockHelper(MBlock block) {
            // 已经打开或被标记的块不可打开
            if (block.IsOpen || block.IsFlaged) {
                return;
            }
            // 将IsOpen设置为True
            block.IsOpen = true;
            // 如果是雷的话，打开后返回
            if (block.Type == BlockType.Mine) {
                return;
            }
            // 如果该方块周围没有雷的话，则递归打开四周的八个方块
            if (block.NearMinesNum == 0) {
                foreach (Coordinate nearCoordinate in GetNearCoordinates(block.Coordinate)) {
                    OpenBlockHelper(_blocks[nearCoordinate]);
                }
            }
        }
        /// <summary>
        /// 启用方块保护，指定方块周围的8个坐标与自身坐标将不会有雷，用于首开保护
        /// </summary>
        /// <param name="block">待保护的坐标方块</param>
        private void SetSafeArea(Coordinate safeCenter) {
            // 如果block不是雷且周围雷数为0，则跳过
            if (_blocks[safeCenter].Type == BlockType.Blank && _blocks[safeCenter].NearMinesNum == 0) {
                return;
            }
            // 记录安全区坐标
            List<Coordinate> safeArea = new List<Coordinate>(); // 安全区，此区域类都是白块
            safeArea.Add(safeCenter);
            safeArea.AddRange(GetNearCoordinates(safeCenter));
            // 用于交换的安全坐标
            List<Coordinate> safeCoordinate = new List<Coordinate>(); // 允许的安全区s
            foreach (Coordinate nCoordinate in GetAllCoordinates()) {
                if (_blocks[nCoordinate].Type == BlockType.Blank && !safeArea.Contains(nCoordinate)) {
                    safeCoordinate.Add(nCoordinate);
                }
            }
            // 从safeCoordinate随机选择一个坐标，用其方块替换nearbyPos的坐标的方块
            Random random = new Random();
            foreach (var nCoordinate in safeArea) {
                int rndPosIndex = random.Next(0, safeCoordinate.Count);
                SwapBlock(nCoordinate, safeCoordinate[rndPosIndex]);
                safeCoordinate.RemoveAt(rndPosIndex);
            }
            ReSyncLayout();
            GameLayoutRested?.Invoke(this, new GameLayoutRestedEventArgs());
        }
        /// <summary>
        /// 打乱方块位置（洗牌算法）
        /// </summary>
        private void Shuffle() {
            Random random = new Random();
            for (int i = 0; i < GameSize; i++) {
                int indexA = i;
                int indexB = random.Next(i, GameSize);
                Coordinate coordinateA = new Coordinate(indexA / ColumnSize, indexA % ColumnSize);
                Coordinate coordinateB = new Coordinate(indexB / ColumnSize, indexB % ColumnSize);
                SwapBlock(coordinateA, coordinateB);
            }
            ReSyncLayout();
        }
        /// <summary>
        /// 交换两个方块的位置
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void SwapBlock(Coordinate a, Coordinate b) {
            MBlock T = _blocks[a];
            _blocks[a] = _blocks[b];
            _blocks[a].Coordinate = a;
            _blocks[b] = T;
            _blocks[b].Coordinate = b;
        }
        /// <summary>
        /// 获取指定方块周围符合predicate的方块的数量
        /// </summary>
        /// <param name="nCoordinate">中心方块</param>
        /// <param name="predicate">判定方法</param>
        /// <returns></returns>
        private int GetNearCounts(Coordinate nCoordinate, Predicate<Coordinate> predicate) {
            int count = 0;
            foreach (Coordinate coordinate in GetNearCoordinates(nCoordinate)) {
                if (predicate(coordinate)) {
                    ++count;
                }
            }
            return count;
        }
        /// <summary>
        /// 获取指定方块周围8个方块的坐标
        /// </summary>
        /// <param name="coordinate">中心方块坐标</param>
        /// <returns></returns>
        private IEnumerable<Coordinate> GetNearCoordinates(Coordinate coordinate) {
            int cRow = coordinate.Row;
            int cCol = coordinate.Column;
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    int nRow = cRow + i;
                    int nCol = cCol + j;
                    // 跳过中心，负坐标，越界坐标
                    if ((nRow == cRow && nCol == cCol)
                        || nRow < 0 || nRow >= RowSize
                        || nCol < 0 || nCol >= ColumnSize) {
                        continue;
                    }
                    yield return new Coordinate(nRow, nCol);
                }
            }
        }
        /// <summary>
        /// 获取当前游戏所有可用的坐标值
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Coordinate> GetAllCoordinates() {
            foreach (Coordinate coordinate in _blocks.Keys) {
                yield return coordinate;
            }
        }
        /// <summary>
        /// 检查游戏是否完成
        /// </summary>
        /// <returns></returns>
        private bool? IsGameCompleted() {
            // 默认游戏为完成状态
            bool? completed = true;
            foreach (var item in BlockArray) {
                // 如果有雷被打开，直接判定为false
                if (item.Type == BlockType.Mine && item.IsOpen) {
                    completed = false;
                    break;
                }
                // 如果有一个空白方块没有被打开，判定为null
                if (item.Type == BlockType.Blank && !item.IsOpen) {
                    completed = null;
                }
            }
            return completed;
        }
        /// <summary>
        /// 字符串格式化方法
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < RowSize; row++) {
                for (int col = 0; col < ColumnSize; col++) {
                    Coordinate coordinate = new Coordinate(row, col);
                    sb.Append($"{(_blocks[coordinate].Type == BlockType.Mine ? 1 : 0)}{_blocks[coordinate].Coordinate} ");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        #endregion
    }
}

