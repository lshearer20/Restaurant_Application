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
    /// Interaction logic for Tips_window.xaml
    /// </summary>
    public partial class Tips_window : Window
    {
        private string bid = "";
        private string uid = "";
        public Tips_window(string bid, string uid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            this.uid = String.Copy(uid);
            addColumnsToGrids();
            loadBusinessTips();
            loadFriendsTips();
        }
        public class BusinessTip
        {
            //Date, User Name, Likes, Tip Text
            public string Date { get; set; }
            public string UserName { get; set; }

            public string Likes { get; set; }

            public string Text { get; set; }

            public string businessID { get; set; }

            public string userID { get; set; }

        }

        public void loadBusinessTips()
        {
            // build a query that selects name state city where id matchges
            string sqlStrexample = @"SELECT name, state, city, address, hoursday, hoursopen, hoursclose 
                              FROM business
                              join hours on business.businessid = hours.businessid
                              join categories on business.businessid = categories.businessid
                              join attributes on business.businessid = attributes.businessid 
                              WHERE business.businessid = '" + this.bid + "' AND hoursday = '" + DateTime.Now.DayOfWeek.ToString() + "';";


            string sqlStr = @"SELECT tip.tipdate, users.name, tip.likes, tip.tiptext, tip.businessID, tip.userID
                            FROM tip, users 
                            WHERE '" + this.bid + "' = tip.businessid AND users.userid = tip.userid ORDER BY tip.tipdate DESC";
            // Then pass this to executeQuery
            executeQuery(sqlStr, setBusinessTipsGrid);

        }

        public void loadFriendsTips()
        {
            // build a query that selects name state city where id matchges
            string sqlStrexample = @"SELECT name, state, city, address, hoursday, hoursopen, hoursclose 
                              FROM business
                              join hours on business.businessid = hours.businessid
                              join categories on business.businessid = categories.businessid
                              join attributes on business.businessid = attributes.businessid 
                              WHERE business.businessid = '" + this.bid + "' AND hoursday = '" + DateTime.Now.DayOfWeek.ToString() + "';";

            string sqlStr = @"Select tip.tipdate, users.name, tip.likes, tip.tiptext, tip.businessID, tip.userID From tip, users, (SELECT * FROM friends WHERE '" + this.uid + "' = friends.userid) as friendss WHERE friendss.friendid = tip.userID AND tip.businessID = '" + this.bid + "' and users.userID = friendss.friendID ORDER BY tip.tipdate DESC;";

            // Then pass this to executeQuery
            executeQuery(sqlStr, setFriendsTipsGrid);

        }


        private void setBusinessTipsGrid(NpgsqlDataReader R)
        {
            //query returns Date, User Name, Likes, Tip Text, businessID, userID
            DateTime datee = R.GetDateTime(0);
            string date_str = datee.ToString("MM/dd/yyyy HH:mm:ss");

            string un = R.GetString(1);
            string likess = R.GetValue(2).ToString();
            string textt = R.GetString(3);
            string busid = R.GetString(4);
            string useridd = R.GetString(5);

            Busniness_tips_grid.Items.Add(new BusinessTip() { Date = date_str, UserName = un, Likes = likess, Text = textt, businessID = busid, userID = useridd});
        }

        private void setFriendsTipsGrid(NpgsqlDataReader R)
        {
            //Console.Write(R);

            if (R.HasRows)
            {
                //query returns Date, User Name, Likes, Tip Text
                DateTime datee = R.GetDateTime(0);
                //datee = datee.ToString("MM/dd/yyyy HH:mm:ss");
                string date_str = datee.ToString("MM/dd/yyyy HH:mm:ss");

                string un = R.GetString(1);
                string likess = R.GetValue(2).ToString();
                string textt = R.GetString(3);
                string busid = R.GetString(4);
                string useridd = R.GetString(5);

                Friends_tips_grid.Items.Add(new BusinessTip() { Date = date_str, UserName = un, Likes = likess, Text = textt, businessID = busid, userID = useridd });
            }
        }

        private void addColumnsToGrids()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("Date");
            col1.Header = "Date";
            col1.Width = 150;
            Busniness_tips_grid.Columns.Add(col1);

            DataGridTextColumn col11 = new DataGridTextColumn();
            col11.Binding = new Binding("Date");
            col11.Header = "Date";
            col11.Width = 150;
            Friends_tips_grid.Columns.Add(col11);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("UserName");
            col2.Header = "User Name";
            col2.Width = 150;
            Busniness_tips_grid.Columns.Add(col2);

            DataGridTextColumn col22 = new DataGridTextColumn();
            col22.Binding = new Binding("UserName");
            col22.Header = "User Name";
            col22.Width = 150;
            Friends_tips_grid.Columns.Add(col22);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("Likes");
            col3.Header = "Likes";
            col3.Width = 60;
            Busniness_tips_grid.Columns.Add(col3);

            DataGridTextColumn col33 = new DataGridTextColumn();
            col33.Binding = new Binding("Likes");
            col33.Header = "Likes";
            col33.Width = 60;
            Friends_tips_grid.Columns.Add(col33);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("Text");
            col4.Header = "Text";
            col4.Width = 290;
            Busniness_tips_grid.Columns.Add(col4);

            DataGridTextColumn col44 = new DataGridTextColumn();
            col44.Binding = new Binding("Text");
            col44.Header = "Text";
            col44.Width = 290;
            Friends_tips_grid.Columns.Add(col44);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("businessID");
            col5.Header = "businessID";
            col5.Width = 0;
            Busniness_tips_grid.Columns.Add(col5);

            DataGridTextColumn col55 = new DataGridTextColumn();
            col55.Binding = new Binding("businessID");
            col55.Header = "businessID";
            col55.Width = 0;
            Friends_tips_grid.Columns.Add(col55);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("userID");
            col6.Header = "userID";
            col6.Width = 0;
            Busniness_tips_grid.Columns.Add(col6);

            DataGridTextColumn col66 = new DataGridTextColumn();
            col66.Binding = new Binding("userID");
            col66.Header = "userID";
            col66.Width = 0;
            Friends_tips_grid.Columns.Add(col66);

        }
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
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Port=5436; Database = milestone2db; password=liverpool20";
        }

        private void Add_Tip_Button_Click(object sender, RoutedEventArgs e)
        {
            //take text from box

            //insert text into database for that businessID and UserID

            string userTipText = Tip_text_box.Text;
            userTipText = userTipText.Replace("'", "`");
            DateTime todaysDate = DateTime.Now;

            string sqlStr = "INSERT INTO Tip VALUES('" + this.bid + "','" + this.uid+ "','"+ DateTime.Now + "','" + 0 + "','" + userTipText+"');";

            executeQuery(sqlStr, setTipDetails);
            //reload the grid with new tip?
            Busniness_tips_grid.Items.Clear();
            loadBusinessTips();
        }
        private void setTipDetails(NpgsqlDataReader R)
        {
            //not sure we do anything here, just ran the query to update the database

            //OLD: Update the values of each of the text boxes with values we retrieved from the reader.
            //OLD: TipText.Text = R.GetString(0);
        }


        private void Tip_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {//when user types into box this text box is changed

        }

        private void Tip_text_box_TextInput(object sender, TextCompositionEventArgs e)
        {//when there is a text input

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//business details selection grid that displays tips for that business
        //display date, User Name, Likes, Text

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //this is the friends data grid, dont think we need any functionality in it
        }

        private void Like_button_Click(object sender, RoutedEventArgs e)
        { // add like to selected tip!

            //get tip selected //help!
            if (Busniness_tips_grid.SelectedIndex > -1) // make sure selection is being made
            {
                // Look at the items of the business grid and get the items at selected index and pass to business.
                BusinessTip B = Busniness_tips_grid.Items[Busniness_tips_grid.SelectedIndex] as BusinessTip;

                // Make sure bid is not null
                if ((B.businessID != null) && (B.businessID.ToString().CompareTo("") != 0))
                {
                    //BusinessDetails businessWindow = new BusinessDetails(B.businessid.ToString());
                    //businessWindow.Show();
                    string tipbid = B.businessID.ToString();
                    string tipuid = B.userID.ToString();
                    string tiptext = B.Text.ToString();
                    //update with +1 like to database
                    string sqlStr = "UPDATE tip SET likes = likes + 1 WHERE tip.businessID = '" + tipbid + "' AND tip.userID = '" + tipuid + "' AND tip.tiptext = '" + tiptext + "';";
                    executeQuery(sqlStr, setTipDetails);
                    //reload the grid with new tip?
                    Busniness_tips_grid.Items.Clear();
                    loadBusinessTips();

                }
            }

        }

    }

    }
