using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;

namespace locky2
{
	public partial class NutsPage : ContentPage
	{
		private TodoItemManager manager;
		private MobileServiceClient client;
		private IMobileServiceTable<Nuts> nutsTable;

		// Track whether the user has authenticated.
		bool authenticated = false;

		public NutsPage()
		{
			InitializeComponent();

			manager = TodoItemManager.DefaultManager;
			client = manager.CurrentClient;
			nutsTable = client.GetTable<Nuts>();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Refresh items only when authenticated.
			if (authenticated == true)
			{
				// Set syncItems to true in order to synchronize the data
				// on startup when running in offline mode.
				//await RefreshItems(true, syncItems: false);
				Debug.WriteLine("authenticated");

				// Hide the Sign-in button.
				this.loginButton.IsVisible = false;
			}
		}

		public async Task<String> GetNutAsync() //<ObservableCollection<Nuts>> GetNutsAsync()
		{
			List<Nuts> items = await nutsTable
				.OrderBy<int>(n=>n.Score)
				.ToListAsync();

			items.Reverse();
			return items[0].Score.ToString();//new ObservableCollection<Nuts>(items);	
		}

		public async void Display(object sender, EventArgs e)
		{
			await nutsTable.InsertAsync(new Nuts { Score = 1001 });
			//var nuts = new Nuts { Score = 1000 };
			nutsLabel.Text = await GetNutAsync();

			//ObservableCollection<Nuts> nutsCollection;
			//await GetNutAsync();
		}

		async void loginButton_Clicked(object sender, EventArgs e)
		{
			if (App.Authenticator != null)
				authenticated = await App.Authenticator.Authenticate();

			// Set syncItems to true to synchronize the data on startup when offline is enabled.
			if (authenticated == true)
			{
			//	await RefreshItems(true, syncItems: false);
			}
		}


	}
}
