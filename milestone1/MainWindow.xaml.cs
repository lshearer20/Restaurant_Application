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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string businessid { get; set; }
            public string name { get; set; }

            public string address { get; set; }

            public string city { get; set; }

            public string state { get; set; }

            public string distance { get; set; }

            public string ratings { get; set; }

            public string tips { get; set; }

            public string checkins { get; set; }

            //public string zipcode { get; set; }
            //public string category { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            addState();
            addColumnsToGrid();

            addColumnsToFriendsGrid();
            addColumnsToFriendsTipGrid();

            addSortList();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Port = 5436; Database = milestone2db; password=liverpool20";
        }

        private void addState()
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
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";

                    // Try/catch to handle errors so teh appli cation does crash.
                    try
                    {   // execute the query
                        var reader = cmd.ExecuteReader();
                        // Give query results to the reader.
                        while (reader.Read())
                            // Iterate over them.
                            stateList.Items.Add(reader.GetString(0));
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
            //   stateList.Items.Add("WA");
            //   stateList.Items.Add("CA");
            //   stateList.Items.Add("ID");
            //   stateList.Items.Add("NV");
        }

        private void addColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 150;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("address");
            col2.Header = "Address";
            col2.Width = 150;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 60;
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("state");
            col4.Header = "State";
            col4.Width = 60;
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("distance");
            col5.Header = "Distance";
            col5.Width = 60;
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("ratings");
            col6.Header = "Ratings";
            col6.Width = 70;
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("tips");
            col7.Header = "Tips";
            col7.Width = 80;
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("checkins");
            col8.Header = "Check-ins";
            col8.Width = 100;
            businessGrid.Columns.Add(col8);

            /*
            DataGridTextColumn col9 = new DataGridTextColumn();
            col4.Binding = new Binding("businessid");
            col4.Header = "Business ID";
            col4.Width = 255;
            businessGrid.Columns.Add(col4);
            */

            //businessGrid.Items.Add(new Business() { name = "business-1", state = "WA", city = "Pullman" });
            //businessGrid.Items.Add(new Business() { name = "business-2", state = "CA", city = "Pasadena" });
            //businessGrid.Items.Add(new Business() { name = "business-3", state = "NV", city = "Las Vegas" });
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

        // ANDREWS CODE
        // Locationbased filters

        private void addCity(NpgsqlDataReader R)
        {
            cityList.Items.Add(R.GetString(0));
        }

        private void StateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear business grid when new city is selected.
            cityList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                // Specify query to execute.                                         // Choose State from stateList and convert to string.
                string sqlStr = "SELECT distinct city FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' ORDER BY city";
                executeQuery(sqlStr, addCity);
            }
        }

        private void addGridRow(NpgsqlDataReader R)
        {
            businessGrid.Items.Add(new Business() { name = R.GetString(0), address = R.GetString(1), city = R.GetString(2), state = R.GetString(3), ratings = R.GetValue(4).ToString(), tips = R.GetValue(5).ToString(), checkins = R.GetValue(6).ToString() });
        }

        private void addGridRowWithoutDist(NpgsqlDataReader R)
        {//
            Console.Write("\n\nRatings:" + R.GetValue(5).ToString() + "\n\n");
            businessGrid.Items.Add(new Business() { name = R.GetValue(0).ToString(), address = R.GetValue(1).ToString(), city = R.GetString(2).ToString(), state = R.GetValue(3).ToString(), ratings = R.GetValue(4).ToString(), tips = R.GetValue(5).ToString(), checkins = R.GetValue(6).ToString(), businessid = R.GetValue(7).ToString() });
        }

        private void addGridRowWithDist(NpgsqlDataReader R)
        {//
            Console.Write("\n\nRatings:" + R.GetValue(5).ToString() + "\n\n");
            businessGrid.Items.Add(new Business() { name = R.GetValue(0).ToString(), address = R.GetValue(1).ToString(), city = R.GetString(2).ToString(), state = R.GetValue(3).ToString(), distance = R.GetValue(4).ToString(), ratings = R.GetValue(5).ToString(), tips = R.GetValue(6).ToString(), checkins = R.GetValue(7).ToString(), businessid = R.GetValue(8).ToString() });
        }

        private void addZipcode(NpgsqlDataReader R)
        {
            zipcodeList.Items.Add(R.GetValue(0)); // adds items to zipcodeList combo box.
        }



        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            // Populate zipcodeList
            zipcodeList.Items.Clear(); // clear zipcode
            if (cityList.SelectedIndex > -1)
            {
                Console.WriteLine(cityList.SelectedIndex);
                // Specify query to execute.                                         // Choose State from stateList and convert to string.
                string sqlStr = "SELECT distinct zipcode FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY zipcode";
                //string sqlStr = "SELECT zipcode FROM business;";
                executeQuery(sqlStr, addZipcode);
            }                       
        }

        private void addCategory(NpgsqlDataReader R)
        {
            categoryListBox.Items.Add(R.GetString(0)); // adds items to zipcodeList combo box.
        }

        // Changes the list of items in zipcode combobox when a certain city is selected.
        private void zipcodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoryListBox.Items.Clear();
            if (zipcodeList.SelectedIndex > -1)
            {
                // Specify query to execute.                                         // Choose State from stateList and convert to string.
                string sqlStr = "SELECT distinct categoriesname FROM business JOIN categories ON business.businessid = categories.businessid WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode = '" + zipcodeList.SelectedItem.ToString() + "' ORDER BY categoriesname";
                //string sqlStr = "SELECT zipcode FROM business;";
                executeQuery(sqlStr, addCategory);
            }
            
            
            // Clear business grid when new city is selected.
            businessGrid.Items.Clear();
            if (zipcodeList.SelectedIndex > -1)
            {
                string sqlStr = "";
                if (userList.SelectedItem != null) //if user has been selected, we can calculate the distance
                {
                    sqlStr = @"select b.name,b.address,b.city,b.state,dist.mydist,b.stars,b.numtips,b.numcheckins, b.businessid
                             from business b, 
                             (select myDist(ub.userlatitude, ub.userlongitude, ub.latitude, ub.longitude) as mydist, ub.businessID from(select u.userlatitude, u.userlongitude, b2.latitude, b2.longitude, b2.businessID from users u, business b2 where u.userID = '" + userList.SelectedItem + "' and b2.state = '"+ stateList.SelectedItem + "' and b2.city = '"+ cityList.SelectedItem + "' and b2.Zipcode = " + zipcodeList.SelectedItem + ") ub) dist where b.businessID = dist.businessID";
                    executeQuery(sqlStr, addGridRowWithDist);

                }
                else  //else we query without the distance.
                {
                    sqlStr = "SELECT name, address, city, state ,stars, numtips, numcheckins, businessid FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode = " + zipcodeList.SelectedItem + " ORDER BY name;";
                    executeQuery(sqlStr, addGridRowWithoutDist);
                }                
                
            }
        }

        // Event when something in the category list is selected.
        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter_By_Attributes(string.Empty);
        }


        private void BusinessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
//deleted
        }





        //CARLOS CODE
        //USER CODE
        private void addUserList(NpgsqlDataReader R)
        {
            userList.Items.Add(R.GetValue(0)); // adds items to zipcodeList combo box.
        }

        private void findUser_Click(object sender, RoutedEventArgs e)
        {
            //Console.Write("\nhola\n\n\n");
            //Console.Write(nameInput.Text);

            //clear the userlist before the query
            userList.Items.Clear();
            string sqlStr = "SELECT distinct userID FROM Users where name = '" + nameInput.Text  + "'";
            //string sqlStr = "SELECT zipcode FROM business;";
            executeQuery(sqlStr, addUserList);
        }

        private void getUserInfo(NpgsqlDataReader R)
        {
            //Console.Write("\n\n");
            //Console.Write("value is:\n " + R.GetValue(1) + "\n");
            //Console.Write("\nFinished\n");


            //paste the values into the textbox
            starsTextBox.Text = R.GetValue(1).ToString();
            fansTextBox.Text = R.GetValue(2).ToString();            
            nameTextBox.Text = R.GetValue(3).ToString();
            coolTextBox.Text = R.GetValue(4).ToString();
            funnyTextBox.Text = R.GetValue(5).ToString();
            usefulTextBox.Text = R.GetValue(6).ToString();
            tipCountTextBox.Text = R.GetValue(7).ToString();
            totalTipLikesTextBox.Text = R.GetValue(8).ToString();
            yelpingSinceTextBox.Text = R.GetValue(9).ToString();
            latTextBox.Text = R.GetValue(10).ToString();
            longTextBox.Text = R.GetValue(11).ToString();
        }
        public class Friend
        {
            public string name { get; set; }
            public string totalLikes { get; set; }
            public string averageStars { get; set; }
            public string yelpingSince { get; set; }
        }

        public class FriendTip
        {
            public string user { get; set; }
            public string business { get; set; }
            public string city { get; set; }
            public string tipText { get; set; }
            public string tipDate { get; set; }
        }

        private void fillFriendsInfo(NpgsqlDataReader R)
        {
            //friendsGrid.Items.Add(new Business() { name = R.GetString(0), state = R.GetString(1), city = R.GetString(2), businessid = R.GetString(3) });
            /*Console.Write("\n\nHERE\n\n");
            Console.Write(R.GetValue(1).ToString());
            Console.Write(R.GetValue(2).ToString());
            Console.Write(R.GetValue(3).ToString());*/
            //friendsGrid.Items.Add(new Friend() { name = R.GetValue(0).ToString(), totalLikes = R.GetValue(1).ToString(), averageStars = R.GetValue(2).ToString(), yelpingSince = R.GetValue(3).ToString() } );
        }

        private void fillFriendsTipInfo(NpgsqlDataReader R)
        {
            //friendsTipGrid.Items.Add(new FriendTip() { user = R.GetValue(0).ToString(), business = R.GetValue(1).ToString(), city = R.GetValue(2).ToString(), tipText = R.GetValue(3).ToString(), tipDate = R.GetValue(4).ToString() });
        }

        private void getFriendsInfo(NpgsqlDataReader R)
        {   //we get the users friends and execute a query for each friend to find their info
            string sqlStr = "SELECT name,totallikes,averagestars,yelpingsince FROM Users where userID = '" + R.GetValue(1) + "'";
            executeQuery(sqlStr, fillFriendsInfo);


            //FRIENDSTIP CODE
            //we also find the users latest tip and fill out the tips grid            
            /*
            select u.name, bt.name, bt.city, bt.tiptext, bt.tipdate from
            (select t.userid,b.name,b.city,t.tiptext,t.tipdate from tip t join business b on b.businessid = t.businessid) bt     
            join users u on u.userid = bt.userid order by bt.tipdate desc;
            */
            string sqlStr2 = "select u.name, bt.name, bt.city, bt.tiptext, bt.tipdate from (select t.userid, b.name, b.city, t.tiptext, t.tipdate from tip t join business b on b.businessid = t.businessid) bt join users u on u.userid = bt.userid and u.userid ='" + R.GetValue(1) + "'order by bt.tipdate desc";
            executeQuery2(sqlStr2, fillFriendsTipInfo);
        }

        private void getInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Console.Write("\n\n\n");
            Console.Write(userList.SelectedItem);
            
            //Get user info and fill in the text boxes.
            string sqlStr = "SELECT * FROM Users where userID = '" + userList.SelectedItem + "'";            
            executeQuery(sqlStr, getUserInfo);

            //get friends info
            //clear the datagrid before processing.
           // friendsGrid.Items.Clear();
           // friendsGrid.Items.Refresh();
            //friendsTipGrid.Items.Clear();
            //friendsTipGrid.Items.Refresh();
            string sqlStr2 = "SELECT * FROM friends where userID = '" + userList.SelectedItem + "'";
            executeQuery(sqlStr2, getFriendsInfo);
            
        }

        private void addColumnsToFriendsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "Name";
            col1.Width = 120;
            //friendsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("totalLikes");
            col2.Header = "totalLikes";
            col2.Width = 70;
           // friendsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("averageStars");
            col3.Header = "averageStars";
            col3.Width = 80;
            //friendsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("yelpingSince");
            col4.Header = "yelpingSince";
            col4.Width = 150;
            //friendsGrid.Columns.Add(col4);
        }

        private void addColumnsToFriendsTipGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("user");
            col1.Header = "User Name";
            col1.Width = 80;
            //friendsTipGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("business");
            col2.Header = "Business";
            col2.Width = 80;
            //friendsTipGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 70;
            //friendsTipGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("tipText");
            col4.Header = "tipText";
            col4.Width = 450;
            //friendsTipGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("tipDate");
            col5.Header = "tipDate";
            col5.Width = 90;
            //friendsTipGrid.Columns.Add(col5);
        }

        private string executeQuery2(string sqlstr, Action<NpgsqlDataReader> myf)
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

                        if (reader.Read())
                            myf(reader);

                        /*while (reader.Read())
                            myf(reader);*/

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
                //Console.Write("\nHere3\n");
                return "\nfrom execquery2\n";
            }
        }

        private void nothing(NpgsqlDataReader R)
        {
            //nothing happens in this function, it is just used along with execute query to update/insert values onto the db
        }

        private void updateLatLongButton_Click(object sender, RoutedEventArgs e)
        {
            if (userList.SelectedItem == null || userList.SelectedItem == "")
            {
                Console.Write("\n\nplease choose an id\n\n");
            }
            else
            {
                //Update the users lat and long and post it onto the db
                Console.Write("\n\n" + userList.SelectedItem + "\n\n");
                string sqlStr = "update users set userlatitude = " + latTextBox.Text + ",userlongitude = " + longTextBox.Text + " where userid = '" + userList.SelectedItem + "'";
                executeQuery(sqlStr, nothing);
            }
         }


        //#6 sorting
        private void addSortList()
        {
            //sortListBox.Items.Add("Name(default)");
            //sortListBox.Items.Add("Highest rating (stars)");
            //sortListBox.Items.Add("Most number of tips");
            //sortListBox.Items.Add("Most check-ins");
            //sortListBox.Items.Add("Nearest");

            //sortListBox.SelectedItem = "Name(default)";
        }

        private void sortListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string orderBy = "";
            //if (sortListBox.SelectedItem == "Name(default)")
            //{
            //    Console.Write("\n\nSelected" + sortListBox.SelectedItem + "\n\n");
            //    orderBy = "name";
            //}
            //else if (sortListBox.SelectedItem == "Highest rating (stars)")
            //{
            //    Console.Write("\n\nSelected" + sortListBox.SelectedItem + "\n\n");
            //    orderBy = "stars";
            //}
            //else if (sortListBox.SelectedItem == "Most number of tips")
            //{
            //    Console.Write("\n\nSelected" + sortListBox.SelectedItem + "\n\n");
            //    orderBy = "numtips";
            //}
            //else if (sortListBox.SelectedItem == "Most check-ins")
            //{
            //    Console.Write("\n\nSelected" + sortListBox.SelectedItem + "\n\n");
            //    orderBy = "numcheckins";
            //}
            //else if (sortListBox.SelectedItem == "Nearest")
            //{
            //    Console.Write("\n\nSelected" + sortListBox.SelectedItem + "\n\n");
            //    orderBy = "distance";
            //}


        }


        /*DISTANCE FUNCTION. PLEASE ADD BELOW FUNCTION TO DB:
          CREATE OR REPLACE FUNCTION myDist
		        (lat1 double precision,long1 double precision,lat2 double precision,long2 double precision)
        returns double precision AS $$
        declare 
	        dlon double precision := (long2 * PI() / 180) - (long1 * PI() / 180);
	        dlat double precision := (lat2 * PI() / 180) - (lat1 * PI() / 180);
	        a double precision := ((sin((dlat)/2))^2) + cos(lat1 * PI() / 180) * cos(lat2 * PI() / 180) * ((sin((dlon)/2))^2);
	        c double precision := 2.0 * atan2(sqrt(a), sqrt(1.0-a));
	        d double precision := 3958.8 * c;

        begin	
	        return d;
        end;
        $$ LANGUAGE plpgsql;
         * 
         */


        //ANDREWS CODE

        Dictionary<int, string> attributeArray = new Dictionary<int, string>();

        // Will apply a dynamic query to the UI when filtering by attributes.
        private void Filter_By_Attributes(string categories)
        {
            string selectedAttributes = "";
            string selectedCategories = "";
            string comma = "";
            int count = 0;

            foreach (var item in categoryListBox.SelectedItems)
            {
                count++;
                if (count > 1) {
                    comma = ",";
                }
                selectedCategories += comma + "'" + item + "'";
            }

            if (selectedCategories.Length > 1)
            {
                selectedCategories = "AND categoriesname IN (" + selectedCategories + ")"; 
            }

            foreach (var clause in attributeArray)
            {
                selectedAttributes += clause.Value;
            }

            businessGrid.Items.Clear();

            string sqlStr = @"SELECT distinct name, address, city, state, stars, numtips, numcheckins 
                            FROM business 
                            JOIN categories ON business.businessid = categories.businessid 
                            WHERE state = '" + stateList.SelectedItem.ToString() + "' " +
                            "AND city = '" + cityList.SelectedItem.ToString() + "' " +
                            "AND zipcode = '" + zipcodeList.SelectedItem.ToString() + "' " +
                            selectedCategories +
                            selectedAttributes + "" +
                            "ORDER BY name;";

            //text1.Text = sqlStr;
            executeQuery(sqlStr, addGridRow);

        }

        private void Price1_Checked(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (price1.IsChecked.Value == true)
            {
                attributeArray.Add(1, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'RestaurantsPriceRange2' AND attributevalue = '1')");
                // whereclause = "AND attributename = 'RestaurantsPriceRange2' AND attributevalue = '1'";
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(1);
                Filter_By_Attributes("");
            }
        }

        private void Price2_Checked(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (price2.IsChecked.Value == true)
            {
                attributeArray.Add(2, "AND business.businessid in (select businessid from attributes where attributename = 'RestaurantsPriceRange2' AND attributevalue = '2')");
                //  whereclause = "AND attributename = 'RestaurantsPriceRange2' AND attributevalue = '2'";
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(2);
                Filter_By_Attributes("");
            }
        }

        private void Price3_Checked(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (price3.IsChecked.Value == true)
            {
                attributeArray.Add(3, "AND business.businessid in (select businessid from attributes where attributename = 'RestaurantsPriceRange2' AND attributevalue = '3')");
                //whereclause = "AND attributename = 'RestaurantsPriceRange2' AND attributevalue = '3'";
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(3);
                Filter_By_Attributes("");
            }
        }

        private void Price4_Checked(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (price4.IsChecked.Value == true)
            {
                attributeArray.Add(4, "AND business.businessid in (select businessid from attributes where attributename = 'RestaurantsPriceRange2' AND attributevalue = '4')");
                //whereclause = "AND attributename = 'RestaurantsPriceRange2' AND attributevalue = '4'";
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(4);
                Filter_By_Attributes("");
            }
        }

        private void AcceptsCreditCards_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (acceptsCreditCards.IsChecked.Value == true)
            {
                //attributeArray.Add(5, "AND attributename = 'BusinessAcceptsCreditCards' AND attributevalue = 'True'");
                attributeArray.Add(5, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'BusinessAcceptsCreditCards' AND attributevalue = 'True')");

                //whereclause = "AND attributename = 'BusinessAcceptsCreditCards' AND attributevalue = 'True'";
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(5);
                Filter_By_Attributes("");
            }
        }

        private void TakesReservations_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (takesReservations.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'RestaurantsReservations' AND attributevalue = 'True'";
                attributeArray.Add(6, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'RestaurantsReservations' AND attributevalue = 'True')");

                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(6);
                Filter_By_Attributes("");
            }
        }

        private void WheelChairAccessible_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (wheelChairAccessible.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'WheelchairAccessible' AND attributevalue = 'True'";
                attributeArray.Add(7, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'WheelchairAccessible' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(7);
                Filter_By_Attributes("");
            }
        }

        private void OutdoorSeating_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (outdoorSeating.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'OutdoorSeating' AND attributevalue = 'True'";
                attributeArray.Add(8, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'OutdoorSeating' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(8);
                Filter_By_Attributes("");
            }
        }

        private void GoodForKids_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (goodForKids.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'GoodForKids' AND attributevalue = 'True'";
                attributeArray.Add(9, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'GoodForKids' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(9);
                Filter_By_Attributes("");
            }
        }

        private void GoodForGroups_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (goodForGroups.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'RestaurantsGoodForGroups' AND attributevalue = 'True'";
                attributeArray.Add(10, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'RestaurantsGoodForGroups' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(10);
                Filter_By_Attributes("");
            }
        }

        private void Delivery_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (delivery.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'RestaurantsDelivery' AND attributevalue = 'True'";
                attributeArray.Add(11, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'RestaurantsDelivery' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(11);
                Filter_By_Attributes("");
            }
        }

        private void TakeOut_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (takeOut.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'RestaurantsTakeOut' AND attributevalue = 'True'";
                attributeArray.Add(12, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'RestaurantsTakeOut' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(12);
                Filter_By_Attributes("");
            }
        }

        private void Wifi_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (wifi.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'WiFi' AND attributevalue = 'free'";
                attributeArray.Add(13, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'WiFi' AND attributevalue = 'free')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(13);
                Filter_By_Attributes("");
            }
        }

        private void BikeParking_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (bikeParking.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'BikeParking' AND attributevalue = 'True'";
                attributeArray.Add(14, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'BikeParking' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(14);
                Filter_By_Attributes("");
            }
        }

        private void Breakfast_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (breakfast.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'breakfast' AND attributevalue = 'True'";
                attributeArray.Add(15, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'breakfast' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(15);
                Filter_By_Attributes("");
            }
        }

        private void Brunch_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (brunch.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'brunch' AND attributevalue = 'True'";
                attributeArray.Add(16, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'brunch' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(16);
                Filter_By_Attributes("");
            }
        }

        private void Lunch_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (lunch.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'lunch' AND attributevalue = 'True'";
                attributeArray.Add(17, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'lunch' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(17);
                Filter_By_Attributes("");
            }
        }

        private void Dinner_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (dinner.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'dinner' AND attributevalue = 'True'";
                attributeArray.Add(18, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'dinner' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(18);
                Filter_By_Attributes("");
            }
        }

        private void Dessert_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (dessert.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'dessert' AND attributevalue = 'True'";
                attributeArray.Add(19, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'dessert' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(19);
                Filter_By_Attributes("");
            }
        }

        private void Latenight_Click(object sender, RoutedEventArgs e)
        {
            string whereclause = "";
            if (latenight.IsChecked.Value == true)
            {
                //whereclause = "AND attributename = 'latenight' AND attributevalue = 'True'";
                attributeArray.Add(20, "AND business.businessid in (SELECT businessid FROM attributes WHERE attributename = 'latenight' AND attributevalue = 'True')");
                Filter_By_Attributes(whereclause);
            }
            else
            {
                attributeArray.Remove(20);
                Filter_By_Attributes("");
            }
        }

        private void BusinessGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }


        private void Show_Tips_Click(object sender, RoutedEventArgs e)
        {
            // Make sure business is being selected and make sure the data is gathered for the selected business.
            if (businessGrid.SelectedIndex > -1) // make sure selection is being made
            {
                // Look at the items of the business grid and get the items at selected index and pass to business.
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;

                // Make sure bid is not null
                if ((B.businessid != null) && (B.businessid.ToString().CompareTo("") != 0))
                {


                    Tips_window tipsWindow = new Tips_window(B.businessid.ToString(), userList.SelectedItem.ToString());
                    tipsWindow.Show();
                }
            }

        }

        private void Show_Checkins_Click(object sender, RoutedEventArgs e)
        {
            // Make sure business is being selected and make sure the data is gathered for the selected business.
            if (businessGrid.SelectedIndex > -1) // make sure selection is being made
            {
                // Look at the items of the business grid and get the items at selected index and pass to business.
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;

                // Make sure bid is not null
                if ((B.businessid != null) && (B.businessid.ToString().CompareTo("") != 0))
                {
                    checkins_window checkinwindow = new checkins_window(B.businessid.ToString());
                    checkinwindow.Show();

                }
            }
        }
    }
}

/*
ISSUES:
- If only state and city list is selected and list is populated, cannot populate businessdetails window. NULL OBJ REF ERROR.

- *** Need to make multiple selections. can filter list for $ AND $$. (DONE)
- *** How to get multiple categories and list them all. (in Business Details)

- Clicking an attribute filter gets rid of categories filter.
*/

