using ASMinesweeperGame.MinesweeperLib;
using System.Collections.Generic;

namespace ASMinesweeperGame {
    public class ObjectPool<T> where T : new() {
        public int InitCount { get; set; }
        public int MaxCount { get; set; }

        public ObjectPool() {

        }
        public ObjectPool(int initCount, int maxCount) {
            InitCount = initCount;
            MaxCount = maxCount;
        }

        public void Initialization() {
            _queue.Clear();
            for (int i = 0; i < MaxCount; i++) {
                _queue.Enqueue(new T());
            }
        }
        public T Fetch() {
            if (_queue.Count > 0) {
                return _queue.Dequeue();
            }
            else {
                return new T();
            }
        }
        public bool TryFetch(out T item) {
            if (_queue.Count > 0) {
                item = _queue.Dequeue();
                return true;
            }
            else {
                item = new T();
                return false;
            }
        }
        public bool TryReturn(T item) {
            if (_queue.Count < MaxCount) {
                (item as IResetable)?.Reset();
                _queue.Enqueue(item);
                return true;
            }
            return false;
        }

        private readonly Queue<T> _queue = new Queue<T>();
    }
}
