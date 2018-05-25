using System;

namespace GameOfLife.Domain
{
    public struct Position : IEquatable<Position>
    {
        public int Column { get; }
        public int Row { get; }

        public Position(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position) obj);
        }

        public bool Equals(Position other)
        {
            return Column == other.Column &&
                   Row == other.Row;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + Column.GetHashCode();
            hashCode = hashCode * -1521134295 + Row.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Position position1, Position position2)
        {
            return position1.Equals(position2);
        }

        public static bool operator !=(Position position1, Position position2)
        {
            return !(position1 == position2);
        }

        public override string ToString()
        {
            return $"Column: {this.Column}, Row: {this.Row}";
        }
    }
}
