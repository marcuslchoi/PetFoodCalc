using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace locky2
{
	public class Nuts
	{

		[Newtonsoft.Json.JsonProperty("Id")]
		public string Id { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string AzureVersion { get; set; }

		public DateTime DateUtc { get; set; }
		//public bool MadeAtHome { get; set; }

		public int Score
		{
			get;
			set;
		}

		[Newtonsoft.Json.JsonIgnore]
		public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }

		[Newtonsoft.Json.JsonIgnore]
		public string TimeDisplay { get { return DateUtc.ToLocalTime().ToString("t"); } }
	}

	//public class AzureDataService
	//{
	//	public MobileServiceClient MobileService { get; set; }
	//	IMobileServiceSyncTable nutsTable;

	//	public async Task Initialize()
	//	{
	//		//Create our client
	//		MobileService = new MobileServiceClient("https://locky2.azurewebsites.net");

	//		const string path = "syncstore.db";
	//		//setup our local sqlite store and intialize our table
	//		var store = new MobileServiceSQLiteStore(path);
	//		store.DefineTable(); 
	//		await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

	//		//Get our sync table that will call out to azure
	//		nutsTable = MobileService.GetSyncTable();

	//	}

	//	public async Task<IEnumerable> GetNuts()
	//	{
	//	}

	//	public async Task AddNut(bool madeAtHome)
	//	{
	//	}

	//	public async Task SyncNut()
	//	{
	//		//pull down all latest changes and then push current nut up
	//		await nutsTable.PullAsync("allCoffees", nutsTable.CreateQuery());
	//		await MobileService.SyncContext.PushAsync();

	//	}
	//}
}