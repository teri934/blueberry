using SQLite;
using System;

namespace Dictionary.Database
{
	class Result
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public DateTime date { get; set; }
		public int Score { get; set; }
	}
}