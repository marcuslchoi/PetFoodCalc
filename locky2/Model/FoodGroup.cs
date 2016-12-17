using System;
using System.Collections.Generic;
namespace locky2
{
	public class FoodGroup : List<DogFood>
	{
		public string Title
		{
			get;
			set;
		}

		public FoodGroup(string title)
		{
			Title = title;
		}
	}
}
