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
    public partial class BusinessDetails : Window
    {
        private string bid = "";
        public BusinessDetails(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            loadBusinessDetails();
            loadBusinessNumbers();
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

        private void executeQuery2(string sqlstr, Action<NpgsqlDataReader> myf)
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
                        while (reader.Read())
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

        private void setBusinessDetails(NpgsqlDataReader R)
        {
            // Update the values of each of the text boxes with values we retrieved from the reader.
            bname.Text = R.GetString(0);
            state.Text = R.GetString(1);
            city.Text = R.GetString(2); // query result doesnt include 4 columns only 3. so it is (2) not (3)
            address.Text = R.GetString(3);
            currentDay.Text = R.GetString(4);
            openTime.Text = R.GetValue(5).ToString();
            closeTime.Text = R.GetValue(6).ToString();
        }

        private void setBusinessCategories(NpgsqlDataReader R)
        {
            businessCategory.Items.Add(R.GetString(0)); // adds items to zipcodeList combo box.
        }


        void setNumInState(NpgsqlDataReader R)
        {
            // Getting the count value
            numInState.Content = R.GetInt16(0).ToString();
        }

        void setNumInCity(NpgsqlDataReader R)
        {
            // Getting the count value
            numInCity.Content = R.GetInt16(0).ToString();
        }

        void setNumTips(NpgsqlDataReader R)
        {
            // Getting the count value
            numOfTip.Content = R.GetValue(0).ToString();
        }

        private void loadBusinessNumbers()
        {
            string sqlStr1 = "SELECT count(*) from business WHERE state = (SELECT state FROM business WHERE businessid = '" + this.bid + "');";
            executeQuery(sqlStr1, setNumInState);

            string sqlStr2 = "SELECT count(*) from business WHERE city = (SELECT city FROM business WHERE businessid = '" + this.bid + "');";
            executeQuery(sqlStr2, setNumInCity);

            string sqlStr3 = "SELECT numtips from business WHERE businessid = '" + this.bid + "';";
            executeQuery(sqlStr3, setNumTips);
        }

        private void loadBusinessDetails()
        {
            // build a query that selects name state city where id matchges
            string sqlStr = @"SELECT name, state, city, address, hoursday, hoursopen, hoursclose 
                              FROM business
                              join hours on business.businessid = hours.businessid
                              join categories on business.businessid = categories.businessid
                              join attributes on business.businessid = attributes.businessid 
                              WHERE business.businessid = '" + this.bid + "' AND hoursday = '" + DateTime.Now.DayOfWeek.ToString() + "';";

            // Then pass this to executeQuery
            executeQuery(sqlStr, setBusinessDetails);

            string sqlStr2 = @"SELECT categoriesname
                              FROM categories
                              WHERE categories.businessid = '" + this.bid + "';";
            executeQuery2(sqlStr2, setBusinessCategories);
        }


        /*
        private void TipText_TextInput(object sender, TextCompositionEventArgs e)
        {
            //INSERT INTO Tip(businessID, userID, tipText, tipDate, likes)VALUES('5KheTjYPu1HcQzQFtm4_vw', 'jRyO2V1pA4CdVVqCIOPc1Q', 'Good chips and salsa. Loud at times. Good service. Bathrooms AWFUL. So that tanks my view on this place.', '2011-12-26 01:46:17', 31);

        }*/

        private void setTipDetails(NpgsqlDataReader R)
        {
            // Update the values of each of the text boxes with values we retrieved from the reader.
            // TipText.Text = R.GetString(0);
        }

        private void TipText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string userTipText = TipText.Text.Replace("","`");
            //Console.WriteLine(TipText.Text);
            //string sqlStr = "INSERT INTO Tip(businessID, userID, tipText, tipDate, likes)VALUES(WHERE businessID = '" + this.bid + "', AND userID = '" + userID + "', '" + TipText + "', '" + todaysDate + "', '" + likes + "');";
            //executeQuery(sqlStr, setTipDetails);
        }

        private void TipText_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userTipText = TipText.Text;
            userTipText = userTipText.Replace("'", "`");
            DateTime todaysDate = DateTime.Now;

            //string sqlStr = "INSERT INTO Tip(businessID, userID, tipText, tipDate, likes)VALUES('5KheTjYPu1HcQzQFtm4_vw','jRyO2V1pA4CdVVqCIOPc1Q','This is Andrew :D', '"+ todaysDate + "', 0);";
            //Console.WriteLine("here");
            string sqlStr = "INSERT INTO Tip(businessID, userID, tipText, tipDate, likes)VALUES('" + this.bid + "','jRyO2V1pA4CdVVqCIOPc1Q','" + userTipText + "', '" + todaysDate + "', 0);";
            executeQuery(sqlStr, setTipDetails);

            this.Close();
        }
    }
}


// use case for querues
// 
//
//
//
//
//
//
//
//