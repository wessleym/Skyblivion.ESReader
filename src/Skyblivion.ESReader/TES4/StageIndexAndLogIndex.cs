using System;

namespace Skyblivion.ESReader.TES4
{
	public class StageIndexAndLogIndex : IEquatable<StageIndexAndLogIndex>
	{
		public int StageIndex { get; }
		public int LogIndex { get; }
		public StageIndexAndLogIndex(int stageIndex, int logIndex)
		{
			StageIndex = stageIndex;
			LogIndex = logIndex;
		}

		public override int GetHashCode()
		{
			int hashCode = -1966952206;
			hashCode = hashCode * -1521134295 + StageIndex.GetHashCode();
			hashCode = hashCode * -1521134295 + LogIndex.GetHashCode();
			return hashCode;
		}

		public bool Equals(StageIndexAndLogIndex? other)
		{
			return other != null && StageIndex == other.StageIndex && LogIndex == other.LogIndex;
		}
		public override bool Equals(object? other)
		{
			StageIndexAndLogIndex? otherCasted = other as StageIndexAndLogIndex;
			return otherCasted != null && Equals(otherCasted);
		}

		public static bool operator ==(StageIndexAndLogIndex? left, StageIndexAndLogIndex? right)
		{
			if (left is null) { return right is null; }
			return right != null && left.Equals(right);
		}

		public static bool operator !=(StageIndexAndLogIndex? left, StageIndexAndLogIndex? right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return nameof(StageIndex) + " " + StageIndex + ", " + nameof(LogIndex) + " " + LogIndex;
		}
	}
}
