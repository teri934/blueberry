using SQLite;
using System;

namespace Database
{
	class Result
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public DateTime date { get; set; }
		public int Score { get; set; }
		public int Rounds { get; set; }
	}
}