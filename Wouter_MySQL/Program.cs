using MySql.Data.MySqlClient;
using Wouter_MySQL;  // Importeer de benodigde namespace voor het werken met MySQL databases.

namespace Wouter  // Definieer een namespace genaamd 'Wouter' om de code logisch te groeperen.
{
    class Program  // Definieer een klasse genaamd 'Program'.
    {
        // Definieer enkele statische variabelen die in de hele klasse gebruikt zullen worden.
        static string connectionString = Constants.ConnectionString;  // Verbindingsreeks voor de database, verondersteld opgeslagen in een constante.
        static string databaseName = "WouterDatabase";  // Naam van de database.
        static string tableName = "Student";  // Naam van de tabel.

        static void Main(string[] args)  // Het hoofdprogramma dat wordt uitgevoerd wanneer de applicatie start.
        {
            while (true)  // Oneindige lus om het programma in een continu werkende staat te houden.
            {
                Console.Clear();  // Maak het consolevenster leeg.
                Console.WriteLine("Choose an action:");  // Toon het menu met opties.
                Console.WriteLine("1. Create Database and Table");
                Console.WriteLine("2. Add Data");
                Console.WriteLine("3. Show Data");
                Console.WriteLine("4. Update Data");
                Console.WriteLine("5. Delete Data");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();  // Lees de keuze van de gebruiker.

                switch (choice)  // Voer een actie uit op basis van de keuze van de gebruiker.
                {
                    case "1":
                        try
                        {
                            CreateDatabaseAndTable();  // Creëer de database en tabel.
                        }
                        catch (Exception ex)  // Foutafhandeling
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        Console.WriteLine("Press a key to continue ...");
                        Console.ReadKey();  // Wacht op een toetsdruk om door te gaan.
                        break;
                    case "2":
                        Console.WriteLine("Enter Name:");
                        string name = Console.ReadLine();  // Lees de naam van de gebruiker.
                        Console.WriteLine("Enter Age:");
                        int age = int.Parse(Console.ReadLine());  // Lees de leeftijd van de gebruiker en converteer deze naar een integer.
                        try
                        {
                            AddData(name, age);  // Voeg de data toe aan de tabel.
                        }
                        catch (Exception ex)  // Foutafhandeling
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        Console.WriteLine("Press a key to continue ...");
                        Console.ReadKey();  // Wacht op een toetsdruk om door te gaan.
                        break;
                    case "3":
                        ShowData();  // Toon de gegevens uit de tabel.
                        Console.WriteLine("Press a key to continue ...");
                        Console.ReadKey();  // Wacht op een toetsdruk om door te gaan.
                        break;
                    case "4":
                        Console.WriteLine("Enter Id:");
                        int updateId = int.Parse(Console.ReadLine());  // Lees het id van de te updaten rij.
                        Console.WriteLine("Enter Name:");
                        string updateName = Console.ReadLine();  // Lees de nieuwe naam.
                        Console.WriteLine("Enter Age:");
                        int updateAge = int.Parse(Console.ReadLine());  // Lees de nieuwe leeftijd.
                        try
                        {
                            UpdateData(updateId, updateName, updateAge);  // Werk de gegevens bij.
                        }
                        catch (Exception ex)  // Foutafhandeling
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        Console.WriteLine("Press a key to continue ...");
                        Console.ReadKey();  // Wacht op een toetsdruk om door te gaan.
                        break;
                    case "5":
                        Console.WriteLine("Enter Id:");
                        int deleteId = int.Parse(Console.ReadLine());  // Lees het id van de te verwijderen rij.
                        try
                        {
                            DeleteData(deleteId);  // Verwijder de gegevens.
                        }
                        catch (Exception ex)  // Foutafhandeling
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        Console.WriteLine("Press a key to continue ...");
                        Console.ReadKey();  // Wacht op een toetsdruk om door te gaan.
                        break;
                    case "0":
                        return;  // Beëindig het programma.
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");  // Toon een foutmelding bij een ongeldige keuze.
                        Console.ReadKey();  // Wacht op een toetsdruk om door te gaan.
                        break;
                }
            }
        }

        static void ShowData()  // Methode om gegevens uit de tabel te tonen.
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))  // Maak een nieuwe verbinding met de MySQL database.
            {
                connection.Open();  // Open de verbinding.
                string selectQuery = $"USE {databaseName};SELECT * FROM {tableName}";  // SQL-query om alle gegevens uit de tabel te selecteren.
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))  // Maak een nieuw MySQL-commando.
                {
                    var reader = command.ExecuteReader();  // Voer de query uit en krijg een reader-object om door de resultaten te itereren.
                    while (reader.Read())  // Lees elke rij in de resultaten.
                    {
                        Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, Age: {reader["Age"]}");  // Toon de gegevens van de huidige rij.
                    }
                }
            }
        }

        static void CreateDatabaseAndTable()  // Methode om de database en tabel te creëren.
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))  // Maak een nieuwe verbinding met de MySQL database.
            {
                connection.Open();  // Open de verbinding.
                string createDatabaseQuery = $"CREATE DATABASE IF NOT EXISTS {databaseName}";  // SQL-query om de database te creëren als deze nog niet bestaat.
                using (MySqlCommand command = new MySqlCommand(createDatabaseQuery, connection))  // Maak een nieuw MySQL-commando.
                {
                    command.ExecuteNonQuery();  // Voer de query uit.
                    Console.WriteLine("Database created successfully!");
                }

                string createTableQuery = @$"
                USE {databaseName};
                CREATE TABLE IF NOT EXISTS {tableName} (
                    Id INT PRIMARY KEY AUTO_INCREMENT,  
                    Name NVARCHAR(50),
                    Age INT
                );";

                using (MySqlCommand command = new MySqlCommand(createTableQuery, connection))  // Maak een nieuw MySQL-commando.
                {
                    command.ExecuteNonQuery();  // Voer de query uit.
                    Console.WriteLine("Table created successfully!");
                }
            }
        }

        static void AddData(string name, int age)  // Methode om gegevens toe te voegen aan de tabel.
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))  // Maak een nieuwe verbinding met de MySQL database.
                {
                    connection.Open();  // Open de verbinding.
                    string insertQuery = $"USE {databaseName};INSERT INTO {tableName} (Name, Age) VALUES (@Name, @Age)";  // SQL-query om gegevens in te voegen.

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))  // Maak een nieuw MySQL-commando.
                    {
                        command.Parameters.AddWithValue("@Name", name);  // Voeg de parameter voor naam toe.
                        command.Parameters.AddWithValue("@Age", age);  // Voeg de parameter voor leeftijd toe.
                        command.ExecuteNonQuery();  // Voer de query uit.
                        Console.WriteLine("Data added successfully!");
                    }
                }
            }
            catch (Exception ex)  // Foutafhandeling
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void UpdateData(int id, string name, int age)  // Methode om gegevens in de tabel bij te werken.
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))  // Maak een nieuwe verbinding met de MySQL database.
                {
                    connection.Open();  // Open de verbinding.
                    string updateQuery = $"USE {databaseName};UPDATE {tableName} SET Name = @Name, Age = @Age WHERE Id = @Id";  // SQL-query om gegevens bij te werken.

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))  // Maak een nieuw MySQL-commando.
                    {
                        command.Parameters.AddWithValue("@Id", id);  // Voeg de parameter voor id toe.
                        command.Parameters.AddWithValue("@Name", name);  // Voeg de parameter voor naam toe.
                        command.Parameters.AddWithValue("@Age", age);  // Voeg de parameter voor leeftijd toe.
                        command.ExecuteNonQuery();  // Voer de query uit.
                        Console.WriteLine("Data updated successfully!");
                    }
                }
            }
            catch (Exception ex)  // Foutafhandeling
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void DeleteData(int id)  // Methode om gegevens uit de tabel te verwijderen.
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))  // Maak een nieuwe verbinding met de MySQL database.
                {
                    connection.Open();  // Open de verbinding.
                    string deleteQuery = $"USE {databaseName};DELETE FROM {tableName} WHERE Id = @Id";  // SQL-query om gegevens te verwijderen.

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))  // Maak een nieuw MySQL-commando.
                    {
                        command.Parameters.AddWithValue("@Id", id);  // Voeg de parameter voor id toe.
                        command.ExecuteNonQuery();  // Voer de query uit.
                        Console.WriteLine("Data deleted successfully!");
                    }
                }
            }
            catch (Exception ex)  // Foutafhandeling
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
