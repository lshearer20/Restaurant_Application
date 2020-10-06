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
    /// Interaction logic for checkins_window.xaml
    /// </summary>
    public partial class checkins_window : Window
    {
        public class monthlycheckins
        {
            //Date, User Name, Likes, Tip Text
            public int January { get; set; }
            public int February { get; set; }

            public int March { get; set; }

            public int April { get; set; }

            public int May { get; set; }

            public int June { get; set; }
            public int July { get; set; }
            public int August { get; set; }
            public int September { get; set; }
            public int October { get; set; }
            public int November { get; set; }
            public int December { get; set; }


        }
        int counter = 0;
        private string bid = "";
        private int january = 0, february = 0, march = 0, april = 0, may = 0, june = 0, july = 0, august = 0, september = 0, october = 0, november= 0, december= 0;

        private void Checkin_button_click(object sender, RoutedEventArgs e)
        {
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string year = DateTime.Now.ToString("yyyy");
            string myTime1 = DateTime.Now.ToString("HH:mm:ss");
            // DateTime mytime = DateTime.ParseExact(time, "HH:mm:ss",
            //                           System.Globalization.CultureInfo.InvariantCulture);

            //DateTime t = Convert.ToDateTime(time);
            //DateTime universal = zone.ToUniversalTime(DateTime.Now);
            //date = R.GetTimeStamp(3).ToString();
            // String timeStamp = GetTimestamp(DateTime.Now);

            DateTime myDateTime = DateTime.Now;
            //DateTime myTime2 = default(DateTime).Add(myDateTime.TimeOfDay);
            TimeSpan TodayTime = DateTime.Now.TimeOfDay;

            string sqlStr = "INSERT INTO Checkin VALUES('" + this.bid + "','" + day + "','" + month + "','" + year + "','" + myTime1 + "', 0);";

            executeQuery(sqlStr, insertcheckin);
            counter = 0;
            Monthly_checkins.Items.Clear();
            loadBusinessCheckinNums();
            fillOutMonthlyCheckins();
        }
        public void insertcheckin(NpgsqlDataReader R)
        {
        }

        int[] months = new int[12];
        public checkins_window(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            loadBusinessCheckinNums();
            createGrid();
            fillOutMonthlyCheckins();
        }

        private void loadBusinessCheckinNums()
        {
            // build a query that selects name state city where id matchges
            string sqlStrex = @"SELECT name, state, city, address, hoursday, hoursopen, hoursclose 
                              FROM business
                              join hours on business.businessid = hours.businessid
                              join categories on business.businessid = categories.businessid
                              join attributes on business.businessid = attributes.businessid 
                              WHERE business.businessid = '" + this.bid + "' AND hoursday = '" + DateTime.Now.DayOfWeek.ToString() + "';";
            string sqlStr = "";
            string month = "";
            for(int i = 1; i < 13; i++)
            {
                month = i.ToString();
                if (i < 10)
                {
                    month = "0" + i.ToString();
                }

                sqlStr = "SELECT count(checkinmonth) FROM checkin, business WHERE business.businessID = '"+this.bid+"'AND business.businessID = checkin.businessID GROUP BY checkinmonth HAVING checkinmonth = '"+month+"';";
                executeQuery(sqlStr, getCheckinNums);
            }

            // Then pass this to executeQuerymonths[i]= 
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
        public void getCheckinNums(NpgsqlDataReader R)
        {
            string str = R.GetValue(0).ToString();

            months[counter] = Convert.ToInt32(str);
            //january = R.GetValue(0).ToString();
            counter += 1;
        }

        public void createGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("January");
            col1.Header = "January";
            col1.Width = 75;
            Monthly_checkins.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("February");
            col2.Header = "February";
            col2.Width =75;
            Monthly_checkins.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("March");
            col3.Header = "March";
            col3.Width = 75;
            Monthly_checkins.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("April");
            col4.Header = "April";
            col4.Width = 75;
            Monthly_checkins.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("May");
            col5.Header = "May";
            col5.Width = 75;
            Monthly_checkins.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("June");
            col6.Header = "June";
            col6.Width = 75;
            Monthly_checkins.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("July");
            col7.Header = "July";
            col7.Width = 75;
            Monthly_checkins.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("August");
            col8.Header = "August";
            col8.Width = 75;
            Monthly_checkins.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Binding = new Binding("September");
            col9.Header = "September";
            col9.Width = 75;
            Monthly_checkins.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn();
            col10.Binding = new Binding("October");
            col10.Header = "October";
            col10.Width = 75;
            Monthly_checkins.Columns.Add(col10);

            DataGridTextColumn col11 = new DataGridTextColumn();
            col11.Binding = new Binding("November");
            col11.Header = "November";
            col11.Width = 75;
            Monthly_checkins.Columns.Add(col11);

            DataGridTextColumn col12 = new DataGridTextColumn();
            col12.Binding = new Binding("December");
            col12.Header = "December";
            col12.Width =75;
            Monthly_checkins.Columns.Add(col12);

        }

        public void fillOutMonthlyCheckins()
        {
            Monthly_checkins.Items.Add(new monthlycheckins() { January = months[0], February = months[1], March= months[2], April= months[3], May= months[4], June = months[5], July= months[6], August= months[7], September= months[8],October= months[9],November= months[10], December = months[11] });
        }


    }
}
