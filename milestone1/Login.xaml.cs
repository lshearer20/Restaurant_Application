using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace milestone1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            //Users.Items.Add("123");
            addUsers();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Port=5436; Database = milestone2db; password=liverpool20";
        }


        // Execute Query Function by argument and action.
        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            // Establish a connection.
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                // Open the Connection
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read(); // Dont need to loop since business ID"s are unique
                        myf(reader);

                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addUsers()
        {   // Defining the connection.
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {   // Open the Connection.
                connection.Open();
                // Create the npgsql command.
                using (var cmd = new NpgsqlCommand())
                {
                    // Set the connection of the command to the connection we created.
                    cmd.Connection = connection;
                    // Specify query to execute.
                    //cmd.CommandText = "SELECT DISTINCT userid FROM USERS";
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";

                    // Try/catch to handle errors so teh appli cation does crash.
                    try
                    {   // execute the query
                        var reader = cmd.ExecuteReader();
                        // Give query results to the reader.
                        while (reader.Read())
                            // Iterate over them.
                            Users.Items.Add(reader.GetString(0));
                    }
                    // If it fails then catch.
                    catch (NpgsqlException ex)
                    {   // Write message to screen if error occurs
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {   // Close the connection.
                        connection.Close();
                    }
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
