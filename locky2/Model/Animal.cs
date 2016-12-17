using System;
using SQLite;

namespace locky2
{
	public class Animal
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }
		public DateTime DateSaved { get; set; }

		[MaxLength(255)]
		public string Name { get; set; }
		public string Species { get; set; }
		public string Age { get; set; }
		public string ActivityLevel { get; set; }
		public float Weight { get; set; }
		public string FoodName { get; set; }
	}
}
