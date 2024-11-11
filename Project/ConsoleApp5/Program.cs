using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using ConsoleMenu;
using static RealestateWarsaw.RealEstateDatabase;
using System.Reflection.Metadata.Ecma335;


namespace RealestateWarsaw
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] possibilities = { "display data from Database", "predict price for your property", "save your property" };
            string selectedpossibility = DisplayInteractiveMenu(possibilities);


            do
            {
                switch (selectedpossibility)
                {
                    case "display data from Database":

                        string[] cities = { "Warsaw", "Krakow" };
                        string selectedCity = DisplayInteractiveMenu(cities);

                        RealEstateDatabase database;
                        if (selectedCity == "Warsaw")
                            database = new RealEstateWarsawDatabase();
                        else if (selectedCity == "Krakow")
                            database = new RealEstateKrakowDatabase();
                        else
                        {
                            Console.WriteLine("Invalid city choice. Exiting program.");
                            return;
                        }

                        Console.WriteLine("Enter number of properties to display:");
                        if (!int.TryParse(Console.ReadLine(), out int limit))
                        {
                            Console.WriteLine("Invalid input. Exiting program.");
                            return;
                        }

                        List<RealEstateDatabase.RealEstateProperty> properties = database.GetProperties(limit);

                        Console.WriteLine("Properties:");
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("| District              | Floor | Price       | Rooms | Square Meters | Construction Year | Building Age | Predicted Price |");
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                        foreach (var property in properties)
                        {
                            float predictedPrice;
                            if (property.Price == 0)
                            {
                                Console.Write($"| {property.District,-21} | {property.Floor,-5} | Not Provided | {property.Rooms,-5} | {property.SquareMeters,-13:F1} | {property.ConstructionYear,-17} | {property.BuildingAge,-12} | ");
                                predictedPrice = database.Predict(property);
                            }
                            else
                            {
                                Console.Write($"| {property.District,-21} | {property.Floor,-5} | {property.Price,-11:C0} | {property.Rooms,-5} | {property.SquareMeters,-13:F1} | {property.ConstructionYear,-17} | {property.BuildingAge,-12} | ");
                                predictedPrice = property.Price;
                            }
                            Console.WriteLine($"{predictedPrice,-15:C0} |");
                        }
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
                        DisplayExitOrRepeatMenu();
                        break;
                    case "predict price for your property":
                        string[] cities_1 = { "Warsaw", "Krakow" };
                        string selectedCity_1 = DisplayInteractiveMenu(cities_1);


                        if (selectedCity_1 == "Warsaw")
                        {
                            RealEstateProperty warsaw = new RealEstateProperty();
                            Console.WriteLine("Chose District:");
                            string[] districts = {  "Bemowo", "Bialoleka", "Bielany", "Mazowieckie", "Mokotow",
                "Ochota", "Praga_polnoc", "Praga_poludnie", "Rembertow", "SRDM",
                "Targowek", "Ursus", "Ursynow", "Wawer", "Wesola", "Wilanow",
                "Wlochy", "Wola", "Zoliborz" };
                            string selectedDistrict = DisplayInteractiveMenu(districts);
                            warsaw.District = selectedDistrict;
                            Console.WriteLine("input floor:");
                            string input = Console.ReadLine();
                            warsaw.Floor = int.Parse(input);
                            Console.WriteLine("input Rooms:");
                            input = Console.ReadLine();
                            warsaw.Rooms = int.Parse(input);
                            Console.WriteLine("input SquareMeters:");
                            input = Console.ReadLine();
                            warsaw.SquareMeters = float.Parse(input);
                            Console.WriteLine("input ConstructionYear:");
                            input = Console.ReadLine();
                            warsaw.ConstructionYear = int.Parse(input);
                            warsaw.BuildingAge = DateTime.Now.Year - warsaw.ConstructionYear;
                            float[] encodedVector = RealEstateWarsawDatabase.EncodeDistrict_waw(warsaw.District);
                            float resprise = RealEstateWarsawDatabase.Predict(warsaw, encodedVector);
                            Console.WriteLine("Predicted price " + resprise);
                            DisplayExitOrRepeatMenu();
                        }
                        else if (selectedCity_1 == "Krakow")
                        {
                            RealEstateProperty krakow = new RealEstateProperty();
                            Console.WriteLine("Chose District:");
                            string[] districts = {   "Bienczyce", "Biezanow_Prokocim", "Bronowice", "Czyzyny", "Debniki",
                "Grzegorzki", "Kazimierz", "Krowodrza", "Lagiewniki_Borek_Falecki",
                "Mistrzejowice", "Nowa_Huta", "Podgorze", "Pradnik_Bialy", "Pradnik_Czerwony",
                "Srodmiescie", "Stare_Miasto", "Swoszowice", "Wzgorza_Krzeslawickie",
                "Zwierzyniec" };
                            string selectedDistrict = DisplayInteractiveMenu(districts);
                            krakow.District = selectedDistrict;
                            Console.WriteLine("input floor:");
                            string input = Console.ReadLine();
                            krakow.Floor = int.Parse(input);
                            Console.WriteLine("input Rooms:");
                            input = Console.ReadLine();
                            krakow.Rooms = int.Parse(input);
                            Console.WriteLine("input SquareMeters:");
                            input = Console.ReadLine();
                            krakow.SquareMeters = float.Parse(input);
                            Console.WriteLine("input ConstructionYear:");
                            input = Console.ReadLine();
                            krakow.ConstructionYear = int.Parse(input);
                            krakow.BuildingAge = DateTime.Now.Year - krakow.ConstructionYear;
                            float[] encodedVector = RealEstateKrakowDatabase.EncodeDistrict_Kra(krakow.District);
                            float resprise = RealEstateKrakowDatabase.Predict(krakow, encodedVector);
                            Console.WriteLine("Predicted price " + resprise);
                            DisplayExitOrRepeatMenu();
                        }
                        else
                        {
                            Console.WriteLine("Invalid city choice");
                            DisplayExitOrRepeatMenu();
                        }
                        break;
                    case "save your property":
                        string[] cities_2 = { "Warsaw", "Krakow" };
                        string selectedCity_2 = DisplayInteractiveMenu(cities_2);


                        if (selectedCity_2 == "Warsaw")
                        {
                            RealEstateProperty warsaw = new RealEstateProperty();
                            Console.WriteLine("Chose District:");
                            string[] districts = {  "Bemowo", "Bialoleka", "Bielany", "Mazowieckie", "Mokotow",
                "Ochota", "Praga_polnoc", "Praga_poludnie", "Rembertow", "SRDM",
                "Targowek", "Ursus", "Ursynow", "Wawer", "Wesola", "Wilanow",
                "Wlochy", "Wola", "Zoliborz" };
                            string selectedDistrict = DisplayInteractiveMenu(districts);
                            warsaw.District = selectedDistrict;
                            Console.WriteLine("input floor:");
                            string input = Console.ReadLine();
                            warsaw.Floor = int.Parse(input);
                            Console.WriteLine("input Price, or write 0 if you want to predict it by model:");
                            input = Console.ReadLine();
                            if (int.Parse(input) == 0)
                            {
                                float[] encodedVector_1 = RealEstateKrakowDatabase.EncodeDistrict_Kra(warsaw.District);
                                warsaw.Price = RealEstateKrakowDatabase.Predict(warsaw, encodedVector_1);
                                Console.WriteLine("Predicted price " + warsaw.Price);
                            }
                            else
                            {
                                warsaw.Price = float.Parse(input);
                            }
                            Console.WriteLine("input Rooms:");
                            input = Console.ReadLine();
                            warsaw.Rooms = int.Parse(input);
                            Console.WriteLine("input SquareMeters:");
                            input = Console.ReadLine();
                            warsaw.SquareMeters = float.Parse(input);
                            Console.WriteLine("input ConstructionYear:");
                            input = Console.ReadLine();
                            warsaw.ConstructionYear = int.Parse(input);
                            warsaw.BuildingAge = DateTime.Now.Year - warsaw.ConstructionYear;
                            RealEstateWarsawDatabase.SaveProperty(warsaw);
                            Console.WriteLine("Property succesfully saved:");
                            DisplayExitOrRepeatMenu();
                        }
                        else if (selectedCity_2 == "Krakow")
                        {
                            RealEstateProperty krakow = new RealEstateProperty();
                            Console.WriteLine("Chose District:");
                            string[] districts = {   "Bienczyce", "Biezanow_Prokocim", "Bronowice", "Czyzyny", "Debniki",
                "Grzegorzki", "Kazimierz", "Krowodrza", "Lagiewniki_Borek_Falecki",
                "Mistrzejowice", "Nowa_Huta", "Podgorze", "Pradnik_Bialy", "Pradnik_Czerwony",
                "Srodmiescie", "Stare_Miasto", "Swoszowice", "Wzgorza_Krzeslawickie",
                "Zwierzyniec" };
                            string selectedDistrict = DisplayInteractiveMenu(districts);
                            krakow.District = selectedDistrict;
                            Console.WriteLine("input floor:");
                            string input = Console.ReadLine();
                            krakow.Floor = int.Parse(input);
                            Console.WriteLine("input Price, or write 0 if you want to predict it by model:");
                            input = Console.ReadLine();
                            if (int.Parse(input) == 0)
                            {
                                float[] encodedVector = RealEstateKrakowDatabase.EncodeDistrict_Kra(krakow.District);
                                krakow.Price = RealEstateKrakowDatabase.Predict(krakow, encodedVector);
                                Console.WriteLine("Predicted price " + krakow.Price);
                            }
                            else
                            {
                                krakow.Price = float.Parse(input);
                            }
                            Console.WriteLine("input Rooms:");
                            input = Console.ReadLine();
                            krakow.Rooms = int.Parse(input);
                            Console.WriteLine("input SquareMeters:");
                            input = Console.ReadLine();
                            krakow.SquareMeters = float.Parse(input);
                            Console.WriteLine("input ConstructionYear:");
                            input = Console.ReadLine();
                            krakow.ConstructionYear = int.Parse(input);
                            krakow.BuildingAge = DateTime.Now.Year - krakow.ConstructionYear;
                            RealEstateKrakowDatabase.SaveProperty(krakow);
                            Console.WriteLine("Property succesfully saved:");
                            DisplayExitOrRepeatMenu();
                        }

                        break;
                    default:

                        break;
                }

                selectedpossibility = DisplayInteractiveMenu(possibilities);
            } while (selectedpossibility != null); 
            

        }
        public static string  DisplayInteractiveMenu(string[] items)
        {
            int selectedItemIndex = 0;

            
            Console.WriteLine("Chose :");

            for (int i = 0; i < items.Length; i++)
            {
                if (i == selectedItemIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(items[i]);
                Console.ResetColor();
            }

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedItemIndex = Math.Max(0, selectedItemIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedItemIndex = Math.Min(items.Length - 1, selectedItemIndex + 1);
                        break;
                }

                Console.Clear();
                Console.WriteLine("Chose:");

                for (int i = 0; i < items.Length; i++)
                {
                    if (i == selectedItemIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(items[i]);
                    Console.ResetColor();
                }

            } while (key.Key != ConsoleKey.Enter);

            return items[selectedItemIndex];

        }

        public static void DisplayExitOrRepeatMenu()
        {
            string[] exitOrRepeatOptions = { "Repeat Operation", "Exit Program" };
            Console.WriteLine("What would you like to do next?");
            string selectedOption = DisplayInteractiveMenu(exitOrRepeatOptions);

            switch (selectedOption)
            {
                case "Repeat Operation":
                    
                    break;
                case "Exit Program":
                    
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting program.");
                    Environment.Exit(0);
                    break;
            }
        }

    }



       

    


        // Base class for interacting with the database
        class RealEstateDatabase
        {
            protected static string connectionString;

            public RealEstateDatabase(string server = "(local)", string database = "RealEstateDB", bool integratedSecurity = true)
            {
                connectionString = $@"Server={server};Database={database};Integrated Security={integratedSecurity};";
            }

            public class RealEstateProperty
            {
                public string District { get; set; }
                public int Floor { get; set; } 
                public float Price { get; set; }
                public int Rooms { get; set; }
                public float SquareMeters { get; set; }
                public int ConstructionYear { get; set; }
                public int BuildingAge { get; set; }
            }

            public virtual List<RealEstateProperty> GetProperties(int limit)
            {
                return new List<RealEstateProperty>();
            }

            public virtual float Predict(RealEstateProperty property)
            {
                return 0;
            }

            

        }

        // Class for interacting with the Warsaw database
        class RealEstateWarsawDatabase : RealEstateDatabase
        {
            public RealEstateWarsawDatabase(string server = "(local)", string database = "RealEstateDB", bool integratedSecurity = true) : base(server, database, integratedSecurity)
            {
            }

            public override List<RealEstateProperty> GetProperties(int limit)
            {
                List<RealEstateProperty> properties = new List<RealEstateProperty>();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = $"SELECT TOP {limit} * FROM Realestate_Warsaw";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    RealEstateProperty property = new RealEstateProperty
                                    {
                                        District = reader["District"].ToString(),
                                        Floor = Convert.ToInt32(reader["floor"]),
                                        Price = Convert.ToSingle(reader["price"]),
                                        Rooms = Convert.ToInt32(reader["rooms"]),
                                        SquareMeters = Convert.ToSingle(reader["SquereMeters"]),
                                        ConstructionYear = Convert.ToInt32(reader["year"]),
                                        BuildingAge = Convert.ToInt32(reader["AGE"])
                                    };
                                    properties.Add(property);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving properties: {ex.Message}");
                }

                return properties;
            }
            
        public static void SaveProperty(RealEstateProperty property)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Realestate_Warsaw ([District], [floor], [price], [rooms], [SquereMeters], [year], [AGE]) VALUES (@District, @Floor, @Price, @Rooms, @SquareMeters, @ConstructionYear, @BuildingAge)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@District", property.District);
                            command.Parameters.AddWithValue("@Floor", property.Floor);
                            command.Parameters.AddWithValue("@Price", property.Price);
                            command.Parameters.AddWithValue("@Rooms", property.Rooms);
                            command.Parameters.AddWithValue("@SquareMeters", property.SquareMeters);
                            command.Parameters.AddWithValue("@ConstructionYear", property.ConstructionYear);
                            command.Parameters.AddWithValue("@BuildingAge", property.BuildingAge);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while saving property: {ex.Message}");
                }
            }

            public static  float[] EncodeDistrict_waw(string district)
            {
                //One-hot-encoding district feuter
                var districts = new string[]
                {
                 "Bemowo", "Bialoleka", "Bielany", "Mazowieckie", "Mokotow",
                "Ochota", "Praga_polnoc", "Praga_poludnie", "Rembertow", "SRDM",
                "Targowek", "Ursus", "Ursynow", "Wawer", "Wesola", "Wilanow",
                "Wlochy", "Wola", "Zoliborz"
                };

                
                float[] encodedVector = new float[districts.Length];

               
                for (int i = 0; i < districts.Length; i++)
                {
                    if (districts[i] == district)
                    {
                        encodedVector[i] = 1.0f;
                        break;
                    }
                }

                return encodedVector;
            }

            protected static  float[] Scale(float[] data) // using Standart_scaler fitted on previos data params to scale new input tensor 
            {
                float[] means = {3.27956553e+00f, 2.62734748e+00f, 5.52943488e+01f, 4.85229926e-02f,
 8.93310324e-02f, 5.00456806e-02f, 5.07562684e-03f, 1.64957872e-01f,
 4.45640037e-02f, 2.83219978e-02f, 9.48127094e-02f, 1.68510811e-02f,
 9.81626231e-02f, 3.64430007e-02f, 2.86265354e-02f, 5.08577809e-02f,
 1.84752817e-02f, 3.95898894e-03f, 3.37021622e-02f, 2.43630088e-02f,
 1.33488986e-01f, 2.94386357e-02f, 2.63737692e+01f};
                float[] stds = {2.81515391f,  1.01280967f, 18.54096732f,  0.21486859f,  0.28522097f,  0.21803924f,
  0.0710624f,   0.37114252f,  0.2063445f,   0.16589112f,  0.29295607f,  0.12871333f,
  0.29753441f,  0.18738972f,  0.16675448f,  0.21970723f,  0.13466234f,  0.06279582f,
  0.18046143f,  0.15417345f,  0.34010245f,  0.16903255f, 25.94239377f};
                float[] scaledData = new float[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    scaledData[i] = (data[i] - means[i]) / stds[i];
                }
                return scaledData;
            }

            public static  float Predict(RealEstateProperty obj, float[] oneHotVector)
            {
                
                List<float> inputValues = new List<float>();
                inputValues.Add(obj.Floor);
                inputValues.Add(obj.Rooms);
                inputValues.Add(obj.SquareMeters);
                inputValues.AddRange(oneHotVector);
                inputValues.Add(obj.BuildingAge);


                //  input array
                float[] inputArray = inputValues.ToArray();
                int[] shape = new int[] { 1, inputArray.Length }; // Tensors shape 
                inputArray = Scale(inputArray);
                var inputTensor = new DenseTensor<float>(inputArray, shape, false);

                // loading the model and predicting 
                using (var session = new InferenceSession(@"C:\Users\vital\Downloads\extra_trees_regressor.onnx"))
                {
                    var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input", inputTensor) };
                    using (var results = session.Run(inputs))
                    {
                        var outputValue = results.First().AsEnumerable<float>().ToArray(); 
                        return outputValue[0]; 
                    }
                }
            }
        }

        // Class for interacting with the Krakow database
        class RealEstateKrakowDatabase : RealEstateDatabase
        {
            public RealEstateKrakowDatabase(string server = "(local)", string database = "RealEstateDB", bool integratedSecurity = true) : base(server, database, integratedSecurity)
            {
            }

            public override List<RealEstateProperty> GetProperties(int limit)
            {
                List<RealEstateProperty> properties = new List<RealEstateProperty>();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = $"SELECT TOP {limit} * FROM Realestate_Krakow";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    RealEstateProperty property = new RealEstateProperty
                                    {
                                        District = reader["District"].ToString(),
                                        Floor = Convert.ToInt32(reader["floor"]),
                                        Price = Convert.ToSingle(reader["price"]),
                                        Rooms = Convert.ToInt32(reader["rooms"]),
                                        SquareMeters = Convert.ToSingle(reader["SquereMeters"]),
                                        ConstructionYear = Convert.ToInt32(reader["year"]),
                                        BuildingAge = Convert.ToInt32(reader["AGE"])
                                    };
                                    properties.Add(property);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving properties: {ex.Message}");
                }

                return properties;
            }

            public static void SaveProperty(RealEstateProperty property)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Realestate_Krakow ([District], [floor], [price], [rooms], [SquereMeters], [year], [AGE]) VALUES (@District, @Floor, @Price, @Rooms, @SquareMeters, @ConstructionYear, @BuildingAge)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@District", property.District);
                            command.Parameters.AddWithValue("@Floor", property.Floor);
                            command.Parameters.AddWithValue("@Price", property.Price);
                            command.Parameters.AddWithValue("@Rooms", property.Rooms);
                            command.Parameters.AddWithValue("@SquareMeters", property.SquareMeters);
                            command.Parameters.AddWithValue("@ConstructionYear", property.ConstructionYear);
                            command.Parameters.AddWithValue("@BuildingAge", property.BuildingAge);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while saving property: {ex.Message}");
                }
            }

            public static float[] EncodeDistrict_Kra(string district)
            {
                var districts = new string[]
                {
                "Bienczyce", "Biezanow_Prokocim", "Bronowice", "Czyzyny", "Debniki",
                "Grzegorzki", "Kazimierz", "Krowodrza", "Lagiewniki_Borek_Falecki",
                "Mistrzejowice", "Nowa_Huta", "Podgorze", "Pradnik_Bialy", "Pradnik_Czerwony",
                "Srodmiescie", "Stare_Miasto", "Swoszowice", "Wzgorza_Krzeslawickie",
                "Zwierzyniec"
                };

                float[] encodedVector = new float[districts.Length];

                for (int i = 0; i < districts.Length; i++)
                {
                    if (districts[i] == district)
                    {
                        encodedVector[i] = 1.0f;
                        break;
                    }
                }

                return encodedVector;
            }

            protected static float[] Scale(float[] data)
            {
                float[] means = {2.54121049e+00f, 2.59812022e+00f, 5.37620048e+01f, 7.74633340e-03f,
 5.83557116e-02f, 3.94546581e-02f, 1.03284445e-04f, 9.26461475e-02f,
 9.64676720e-02f, 8.67589341e-03f, 5.35013427e-02f, 1.34269779e-02f,
 6.54823384e-02f, 7.64304896e-02f, 2.16277629e-01f, 8.26275563e-02f,
 4.60648626e-02f, 2.03470357e-02f, 6.96137162e-02f, 4.64780004e-03f,
 1.72485024e-02f, 3.08820492e-02f, 8.68425945e+00f};
                float[] stds = {2.17105846e+00f, 9.90936294e-01f, 1.71537945e+01f, 8.76717042e-02f,
 2.34414851e-01f, 1.94674056e-01f, 1.01623707e-02f, 2.89935922e-01f,
 2.95231537e-01f, 9.27395400e-02f, 2.25030996e-01f, 1.15094284e-01f,
 2.47375022e-01f, 2.65685660e-01f, 4.11705740e-01f, 2.75318440e-01f,
 2.09625597e-01f, 1.41184397e-01f, 2.54494885e-01f, 6.80161598e-02f,
 1.30195974e-01f, 1.72998116e-01f, 1.15424178e+01f};
                float[] scaledData = new float[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    scaledData[i] = (data[i] - means[i]) / stds[i];
                }
                return scaledData;
            }

            public static float Predict(RealEstateProperty obj, float[] oneHotVector)
            {
                List<float> inputValues = new List<float>();
                inputValues.Add(obj.Floor);
                inputValues.Add(obj.Rooms);
                inputValues.Add(obj.SquareMeters);
                inputValues.AddRange(oneHotVector);
                inputValues.Add(obj.BuildingAge);


                float[] inputArray = inputValues.ToArray();
                int[] shape = new int[] { 1, inputArray.Length }; 
                inputArray = Scale(inputArray);
                var inputTensor = new DenseTensor<float>(inputArray, shape, false);

                using (var session = new InferenceSession(@"C:\Users\vital\Downloads\extra_trees_regressor_krak.onnx"))
                {
                    var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<float>("input", inputTensor) };
                    using (var results = session.Run(inputs))
                    {
                        var outputValue = results.First().AsEnumerable<float>().ToArray(); 
                        return outputValue[0]; 
                    }
                }
            }
        }
    
}