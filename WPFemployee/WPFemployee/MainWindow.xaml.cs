using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media.Animation;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class global
    {
        public static int flag = 0;
    }

    public partial class MainWindow : Window
    {
        String sqlCon = "Server=.\\SQLEXPRESS2008;Database=Suwanon;Trusted_Connection=True;";//MultipleActiveResultSets=True; เมื่อมีการ query ซ้ำซ้อนและ error
        //SqlConnection _conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=Suwanon;Trusted_Connection=True;"); 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                _conn.Open();
                //string query = "select * from employeeinfo where username = '" + this.txt_user_name.Text.Trim() + "' and password = '" + this.txt_password.Password + "'";
                SqlCommand createCommand = new SqlCommand("Login", _conn);
                createCommand.CommandType = CommandType.StoredProcedure;
                createCommand.Parameters.Add(new SqlParameter("@Username", this.txt_user_name.Text.Trim()));
                StringBuilder test = new StringBuilder();
                test.Append("Suwanon");
                test.Append("Choojan");
                createCommand.ExecuteNonQuery();
                SqlDataReader dr = createCommand.ExecuteReader();
                int count = 0;
                while (dr.Read())
                {
                    count++;
                }
                _conn.Close();

                if (count == 1)
                {
                    string eID = null;
                    string check = null;
                    laValid.Content = "";
                    _conn.Open();
                    //string query2 = "select eid, name, surname, position from employeeinfo where username = '" + txt_user_name.Text.Trim() + "' AND password = '" + txt_password.Password + "'";
                    SqlCommand com = new SqlCommand("CheckUser", _conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@Username", txt_user_name.Text.Trim()));
                    com.Parameters.Add(new SqlParameter("@Password", txt_password.Password));
                    SqlDataReader dread = com.ExecuteReader();
                    second sec = new second();
                    if(dread.Read())
                    {
                        string position = null;
                        check = dread.GetString(3);
                        if (check == "A")
                        {
                            position = "Admin";
                            global.flag = 1;
                        }
                        else if (check == "E")
                        {
                            position = "Employee";
                            global.flag = 2;
                        }
                        else
                        {
                            //เพิ่มเติม
                        }
                        //string test = Convert.ToString(dread.GetInt32(0));
                        eID = "Jellyfish";
                        sec.laName.Content = "Welcome : " + dread.GetString(1) + "  " + dread.GetString(2) + " ( "+ position + " ) ";
                        sec.image1.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(@"pack://application:,,,/TestWPF;component/Images/"+eID+".jpg"));
                    }
                    _conn.Close();  
                    this.Hide();
                    sec.Left = this.Left;
                    sec.Top = this.Top;
                    sec.ShowDialog();
                    this.Left = sec.Left;
                    this.Top = sec.Top;
                    this.Show();
                }
                else if (count > 1)
                {
                    laValid.Content = "*Duplicate Username and Password!";
                }
                else
                {
                    laValid.Content = "*Username or Password incorrect!";
                }
                txt_user_name.Clear();
                txt_password.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                global.flag = 10;
                second sec = new second();
                this.Hide();
                sec.laName.Content = "Welcome : Guest(Anonymous)";
                sec.Left = this.Left;
                sec.Top = this.Top;
                sec.ShowDialog();
                this.Left = sec.Left;
                this.Top = sec.Top;
                txt_user_name.Clear();
                txt_password.Clear();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
            img.BeginAnimation(Image.OpacityProperty, animation);
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            glow.Opacity = 100;
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            glow.Opacity = 0;
        }

        private void button2_MouseEnter(object sender, MouseEventArgs e)
        {
            glow2.Opacity = 100;
        }

        private void button2_MouseLeave(object sender, MouseEventArgs e)
        {
            glow2.Opacity = 0;
        }

        //ปุ่ม Login เก่า name=btn_1
        //private void btn_1_Click(object sender, RoutedEventArgs e)
        //{
        //    SqlConnection _conn = new SqlConnection(sqlCon);
        //    try
        //    {
        //        _conn.Open();
        //        string query = "select * from employeeinfo where username = '" + this.txt_user_name.Text.Trim() + "' and password = '" + this.txt_password.Password + "'";
        //        SqlCommand createCommand = new SqlCommand(query, _conn);
        //        createCommand.ExecuteNonQuery();
        //        SqlDataReader dr = createCommand.ExecuteReader();
        //        int count = 0;
        //        while (dr.Read())
        //        {
        //            count++;
        //        }
        //        if (count == 1)
        //        {
        //            MessageBox.Show("Username and Password is correct.");
        //            this.Hide();
        //            _conn.Close();
        //            second sec = new second();
        //            sec.ShowDialog();
        //        }
        //        else if (count > 1)
        //        {
        //            MessageBox.Show("Duplicate Username and Password, Please Try Again!");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Username and Password is incorrect.");
        //        }
        //        txt_user_name.Clear();
        //        txt_password.Clear();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Can't connect to DATABASE!");
        //    }
        //}
    }
}
