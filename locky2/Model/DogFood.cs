using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace locky2
{
	public class DogFood
	{

		[Newtonsoft.Json.JsonProperty("Id")]
		public string Id { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string AzureVersion { get; set; }

		public DateTime DateUtc { get; set; }
		//public bool MadeAtHome { get; set; }

		public string Brand
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		//[Newtonsoft.Json.JsonIgnore]
		//public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }

		//[Newtonsoft.Json.JsonIgnore]
		//public string TimeDisplay { get { return DateUtc.ToLocalTime().ToString("t"); } }
	}
}
