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
    public partial class User : MainWindow
    {
        public User()
        {
            InitializeComponent();
            //Console.Write("Here at Users");
            //userbutton1.Text = "LOL";
            //stateList.Items.Add("COCO");
        }

        public void Temp()
        {
            stateList.Items.Add("COCO");
        }
    }
}
