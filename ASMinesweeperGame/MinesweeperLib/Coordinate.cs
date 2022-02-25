using System;

namespace ASMinesweeperGame.MinesweeperLib {
    public readonly struct Coordinate : IEquatable<Coordinate> {
        public int Row { get; }
        public int Column { get; }

        public Coordinate(int row, int column) {
            Row = row;
            Column = column;
        }

        public static bool operator ==(Coordinate left, Coordinate right) {
            return left.Row == right.Row && left.Column == right.Column;
        }
        public static bool operator !=(Coordinate left, Coordinate right) {
            return !(left == right);
        }
        public bool Equals(Coordinate other) {
            return Row == other.Row && Column == other.Column;
        }
        public override bool Equals(object? obj) {
            if (obj == null) {
                return false;
            }
            if (typeof(Coordinate) != obj.GetType()) {
                return false;
            }
            return Equals((Coordinate)obj);
        }
        public override int GetHashCode() {
            return Row ^ Column;
        }
        public override string ToString() {
            return $"({Row}, {Column})";
        }
    }
}
