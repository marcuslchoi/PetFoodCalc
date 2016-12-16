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

		private List<DogFood> _dogFoodsFromDb;
		private List<string> _dogSizes;
		private List<string> _animalTypes;
		private string _animal;
		private List<string> _activityLevels;
		private List<string> _weightUnits;

		public DogFoodPage()
		{
			InitializeComponent();

			_dogSizes = GetDogSizes();
			_animalTypes = GetAnimals();
			_activityLevels = GetActivityLevels();
			_weightUnits = GetWeightUnits();

			manager = TodoItemManager.DefaultManager;
			client = manager.CurrentClient;
			dogFoodTable = client.GetTable<DogFood>();

			background.Source = ImageSource.FromResource("locky2.Images.FatDog1.jpeg");

			foreach (string animal in _animalTypes)
				animalPicker.Items.Add(animal);

			foreach (string activityLevel in _activityLevels)
				activityLevelPicker.Items.Add(activityLevel);

			//populate dog sizes
			foreach (string dogSize in _dogSizes)
				dogSizesPicker.Items.Add(dogSize);

			foreach (string weightUnit in _weightUnits)
				weightUnitPicker.Items.Add(weightUnit);

			weightUnitPicker.SelectedIndex = 0;
			AddNewFoodsToDb();

			AddFoodsToListView();
		}

		public async Task<List<DogFood>> GetDogFoodAsync()
		{
			List<DogFood> dogFoods = await dogFoodTable
				.OrderBy<string>(dogFood => dogFood.Brand)
				.ToListAsync();

			return dogFoods;
		}

		public async void AddNewFoodsToDb()
		{
			//await dogFoodTable.InsertAsync(new DogFood { Brand="Taste Of The Wild"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Nature's Variety"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Natural Balance"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Organix"});
			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Hill's Science Diet"});

			//await dogFoodTable.InsertAsync(new DogFood { Brand = "Hill's Science Diet", Type = "Some Food Name" });
		}

		public async void AddFoodsToListView()
		{
			//populate dog foods
			_dogFoodsFromDb = await GetDogFoodAsync();

			var dogFoodGroups = new List<FoodGroup>();
			foreach (var alphabetLetter in Constants.alphabetLetters)
			{
				var dogFoodGroup = new FoodGroup(alphabetLetter);

				foreach (var food in GetFoodSubList(alphabetLetter))
					dogFoodGroup.Add(food);

				dogFoodGroups.Add(dogFoodGroup);
			}

			dogFoodsListView.ItemsSource = dogFoodGroups;
			dogFoodsListView.IsGroupingEnabled = true;
			dogFoodsListView.GroupDisplayBinding = new Binding("Title");
			dogFoodsListView.GroupShortNameBinding = new Binding("Title");
		}

		//return the sub list of dog foods whose brand begins with groupTitle
		private List<DogFood> GetFoodSubList(string groupTitle)
		{
			var foodSubList = _dogFoodsFromDb.FindAll(dogfood => dogfood.Brand.Substring(0, groupTitle.Length) == groupTitle);
			return foodSubList;
		}

		public async void Display(object sender, EventArgs e)
		{
			howMuchToFeed.Text = "10 oz every meal";
			//ObservableCollection<Nuts> nutsCollection;
			//await GetNutAsync();
		}

		void AnimalSelectedIndexChanged(object sender, System.EventArgs e)
		{
			_animal = animalPicker.Items[animalPicker.SelectedIndex];

			//TODO: fix crash when age already chosen and animal changed
			agePicker.Items.Clear();
			foreach (var age in GetAges())
				agePicker.Items.Add(age);
		}

		void AgeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			var age = agePicker.Items[agePicker.SelectedIndex];
		}

		void ActivityLevelSelectedIndexChanged(object sender, System.EventArgs e)
		{

			var activityLevel = activityLevelPicker.Items[activityLevelPicker.SelectedIndex];
		}

		void WeightUnitSelectedIndexChanged(object sender, System.EventArgs e)
		{

			var weightUnit = weightUnitPicker.Items[weightUnitPicker.SelectedIndex];
		}

		void ChooseFoodButtonClicked(object sender, System.EventArgs e)
		{
			dogFoodsListView.IsVisible = !dogFoodsListView.IsVisible;
		}

		void FoodSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			dogFoodsListView.IsVisible = false;
			var food = e.SelectedItem as DogFood;
			chooseFoodButton.Text = food.Brand + " " + food.Type;
		}

		void DogSizeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			var dogSize = dogSizesPicker.Items[dogSizesPicker.SelectedIndex];
			//DisplayAlert("Dog Size", dogSize, "OK");
		}

		private List<string> GetDogSizes()
		{
			return new List<string> { "<10 lb", "10-20 lb", "20-30 lb", "30-40 lb", "40-50 lb", "50+ lb" };
		}

		private List<string> GetAnimals()
		{
			return new List<string> { "Dog","Cat" };
		}

		private List<string> GetAges()
		{
			string infant="";
			if (_animal == "Dog")
				infant = "Puppy";
			else if (_animal == "Cat")
				infant = "Kitten";
			
			return new List<string> { infant,"Adult","Senior" };
		}

		private List<string> GetActivityLevels()
		{
			return new List<string> { "Sedentary","Normal","Active" };
		}

		private List<string> GetWeightUnits()
		{
			return new List<string> { "lb","kg" };
		}
	}
}

