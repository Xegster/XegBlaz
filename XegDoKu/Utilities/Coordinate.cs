using System;

namespace XegDoKu.Utilities
{
	public class Coordinate : IEquatable<Coordinate>, ICloneable
	{
		public int Row { get; set; }
		public int Column { get; set; }
		public override string ToString()
		{
			return string.Format("{0},{1}", Row, Column);
		}

		public object Clone()
		{
			return new Coordinate
			{
				Row = this.Row,
				Column = this.Column
			};
		}

		#region Implement IEquatable
		public bool Equals(Coordinate other)
		{
			// Complex is a value type, thus we don't have to check for null
			// if (other == null) return false;

			return (this.Row == other.Row)
				&& (this.Column == other.Column);
		}

		public override bool Equals(object other)
		{
			// other could be a reference type, the is operator will return false if null
			if (other is Coordinate)
				return this.Equals((Coordinate)other);
			else
				return false;
		}

		public override int GetHashCode()
		{
			return this.Row.GetHashCode() ^ this.Column.GetHashCode();
		}
		public static bool operator ==(Coordinate term1, Coordinate term2)
		{
			return term1.Equals(term2);
		}

		public static bool operator !=(Coordinate term1, Coordinate term2)
		{
			return !term1.Equals(term2);
		}
		#endregion
	}

}
