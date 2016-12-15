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
	public partial class DogFoodPage : ContentPage
	{

		private TodoItemManager manager;
		private MobileServiceClient client;
		private IMobileServiceTable<DogFood> dogFoodTable;

		private List<string> _dogSizes;

		public DogFoodPage()
		{
			InitializeComponent();

			_dogSizes = GetDogSizes();

			manager = TodoItemManager.DefaultManager;
			client = manager.CurrentClient;
			dogFoodTable = client.GetTable<DogFood>();

			background.Source = ImageSource.FromResource("locky2.Images.FatDog1.jpeg");

			//populate dog sizes
			foreach (string dogSize in _dogSizes)
				dogSizesPicker.Items.Add(dogSize);

			//AddFoodsToDb();

			AddFoodsToPicker();
		}

		public async Task<List<DogFood>> GetDogFoodAsync() //<ObservableCollection<Nuts>> GetNutsAsync()
		{
			List<DogFood> dogFoods = await dogFoodTable
				.OrderBy<string>(dogFood => dogFood.Brand)
				.ToListAsync();

			return dogFoods;//new ObservableCollection<Nuts>(items);	
		}

		public async void AddFoodsToDb()
		{ 
			//await dogFoodTable.InsertAsync(new DogFood { Brand="Taste Of The Wild"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Nature's Variety"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Natural Balance"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Organix"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Hill's Science Diet"});

			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Hill's Science Diet", Type = "Soft" });
		}

		public async void AddFoodsToPicker()
		{ 
			//populate dog foods
			var dogFoodsList = await GetDogFoodAsync();
			foreach (var dogFood in dogFoodsList)
			{
				dogFoodsPicker.Items.Add(dogFood.Brand);

			}
		}

		public async void Display(object sender, EventArgs e)
		{
			howMuchToFeed.Text = "10 oz every meal";
			//ObservableCollection<Nuts> nutsCollection;
			//await GetNutAsync();
		}

		void DogSizeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			var dogSize = dogSizesPicker.Items[dogSizesPicker.SelectedIndex];
			//DisplayAlert("Dog Size", dogSize, "OK");
		}

		void DogFoodSelectedIndexChanged(object sender, System.EventArgs e)
		{
			var dogFood = dogFoodsPicker.Items[dogFoodsPicker.SelectedIndex];
		}

		private List<string> GetDogSizes()
		{
			return new List<string> { "<10 lb", "10-20 lb", "20-30 lb","30-40 lb","40-50 lb","50+ lb"};
		}
	}
}
