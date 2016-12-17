using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using SQLite;

namespace locky2
{

	public partial class DogFoodPage : ContentPage
	{

		private TodoItemManager manager;
		private MobileServiceClient client;
		private IMobileServiceTable<DogFood> dogFoodTable;
		private List<DogFood> _dogFoodsFromDb;

		private List<string> _animalTypes;
		private List<string> _activityLevels;
		private List<string> _weightUnits;

		private string _species;
		private DogFood _food;
		private string _age;
		private string _activityLevel;
		private float _weight;
		private string _weightUnit;

		private SQLiteAsyncConnection _connection;

		public DogFoodPage()
		{
			InitializeComponent();

			//this is the gateway to the database (has crud operations)
			_connection = DependencyService.Get<ISQLiteDb>().GetConnection();


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

			foreach (string weightUnit in _weightUnits)
				weightUnitPicker.Items.Add(weightUnit);

			weightUnitPicker.SelectedIndex = 0;
			AddNewFoodsToDb();

			AddFoodsToListView();
		}

		protected override async void OnAppearing()
		{
			await _connection.CreateTableAsync<Animal>();

			var animals = await _connection.Table<Animal>().ToListAsync();
			//animalslistview.itemsource = animals;

			base.OnAppearing();
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

		public void DisplayFoodAmount(object sender, EventArgs e)
		{
			float cups = 1.5f;
			//howMuchToFeed.Text = "10 oz every meal";
			DisplayAlert("Food!",
						 "Your " + _species.ToLower() + " needs " + cups + " cups of food per meal. Save info?",
						 "Yes","No");

			SaveAnimalToSqlLite("some name");
		}

		private async void SaveAnimalToSqlLite(string name)
		{
			var pet = new Animal
			{
				DateSaved = DateTime.Now,
				Name = name,
				Species = _species,
				Age = _age,
				ActivityLevel = _activityLevel,
				Weight = _weight,
				FoodName = _food.Brand+" "+_food.Type
			};
			await _connection.InsertAsync(pet);
		}

		void AnimalSelectedIndexChanged(object sender, System.EventArgs e)
		{
			_species = animalPicker.Items[animalPicker.SelectedIndex];

			//TODO: fix crash when age already chosen and animal changed
			agePicker.Items.Clear();
			foreach (var age in GetAges())
				agePicker.Items.Add(age);
		}

		void AgeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			_age = agePicker.Items[agePicker.SelectedIndex];
		}

		void ActivityLevelSelectedIndexChanged(object sender, System.EventArgs e)
		{

			_activityLevel = activityLevelPicker.Items[activityLevelPicker.SelectedIndex];
		}

		void WeightEntryCompleted(object sender, System.EventArgs e)
		{
			float weightEntered = float.Parse(weightEntry.Text);
			if (_weightUnit == "lb")
				_weight = weightEntered / Constants.lbsPerKg;
			else
				_weight = weightEntered;
			
		}

		void WeightUnitSelectedIndexChanged(object sender, System.EventArgs e)
		{

			_weightUnit = weightUnitPicker.Items[weightUnitPicker.SelectedIndex];
		}

		void ChooseFoodButtonClicked(object sender, System.EventArgs e)
		{
			dogFoodsListView.IsVisible = !dogFoodsListView.IsVisible;
		}

		void FoodSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			dogFoodsListView.IsVisible = false;
			_food = e.SelectedItem as DogFood;
			chooseFoodButton.Text = _food.Brand + " " + _food.Type;
		}

		private List<string> GetAnimals()
		{
			return new List<string> { "Dog","Cat" };
		}

		private List<string> GetAges()
		{
			string infant="";
			if (_species == "Dog")
				infant = "Puppy";
			else if (_species == "Cat")
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

