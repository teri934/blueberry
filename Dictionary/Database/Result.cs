using SQLite;
using System;

namespace Dictionary.Database
{
	class Result : Java.Lang.Object
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public DateTime date { get; set; }
		public int Score { get; set; }
		public int Rounds { get; set; }
	}
}