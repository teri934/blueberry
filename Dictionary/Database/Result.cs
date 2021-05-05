using SQLite;
using System;
using System.Xml.Serialization;

namespace Dictionary.Database
{
	[XmlType("result")]
	public class Result : Java.Lang.Object
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public DateTime date { get; set; }
		public int Score { get; set; }
		public int Rounds { get; set; }
	}
}