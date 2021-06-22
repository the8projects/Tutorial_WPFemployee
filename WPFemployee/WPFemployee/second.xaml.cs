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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for second.xaml
    /// </summary>

    public partial class second : Window
    {
        string sqlCon = "Server=.\\SQLEXPRESS2008;Database=Suwanon;Trusted_Connection=True;";
        private string previousText;
        public second()
        {
            InitializeComponent();
            //Fill_Combo();
            //Fill_ListView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowData();
            if (global.flag == 1)
            {
                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnUpdate.IsEnabled = true;
                txtName.IsReadOnly = false;
                txtSurname.IsReadOnly = false;
                txtAge.IsReadOnly = false;
            }
            else if (global.flag == 2)
            {
                btnAdd.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                txtName.IsReadOnly = true;
                txtSurname.IsReadOnly = true;
                txtAge.IsReadOnly = true;
            }
            else if (global.flag == 10)
            {
                btnAdd.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                txtName.IsEnabled = false;
                txtSurname.IsEnabled = false;
                txtAge.IsReadOnly = true;
                txtID.Visibility = Visibility.Hidden;
                txtAge.Visibility = Visibility.Hidden;
                laID.Visibility = Visibility.Hidden;
                laAge.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Hidden;
                comboBox1.Visibility = Visibility.Hidden;
                laCategory.Visibility = Visibility.Hidden;
                txtName.Margin = new Thickness(118, 66, 0, 0);      //เลื่อนตำแหน่งชั่วคราว
                txtSurname.Margin = new Thickness(118, 103, 0, 0);
                laName2.Margin = new Thickness(42, 62, 0, 0);
                laSurname.Margin = new Thickness(42, 98, 0, 0);
                btnClear.Margin = new Thickness(153, 139, 0, 0);
                btnShow.Margin = new Thickness(153, 169, 0, 0);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (global.flag == 1)
            {
                SqlConnection _conn = new SqlConnection(sqlCon);
                try
                {
                    _conn.Open();
                    //string query = "select MAX(eid) from employeeinfo";
                    SqlCommand com = new SqlCommand("ShowMaxId", _conn);
                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dread = com.ExecuteReader();
                    while (dread.Read())
                    {
                        int check = dread.GetInt32(0);
                        txtID.Text = Convert.ToString(check + 1);
                    }
                    _conn.Close();

                    if (txtID.Text.Trim() == "" || txtName.Text.Trim() == "" || txtSurname.Text.Trim() == "" || txtAge.Text.Trim() == "")
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน");
                        return;
                    }
                    else
                    {
                        int eID = Convert.ToInt32(txtID.Text.Trim());
                        int age = Convert.ToInt32(txtAge.Text.Trim());
                        _conn.Open();
                        //string query3 = "insert into employeeinfo(eid, name, surname, age) values ('" + eID + "', '" + txtName.Text.Trim() + "', '" + txtSurname.Text.Trim() + "', '" + age + " ')";
                        SqlCommand comm = new SqlCommand("InsertEmp", _conn);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.Add(new SqlParameter("@eID", eID));
                        comm.Parameters.Add(new SqlParameter("@name", this.txtName.Text.Trim()));
                        comm.Parameters.Add(new SqlParameter("@surname", this.txtSurname.Text.Trim()));
                        comm.Parameters.Add(new SqlParameter("@age", age));
                        comm.ExecuteNonQuery();
                        _conn.Close();
                        MessageBox.Show("บันทึกเรียบร้อย");
                        ClearData();
                        ShowData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please contact your administrator.");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGrid1.SelectedItem == null){return;}
                DataRowView dr = dataGrid1.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;
                    if(global.flag == 10)
                    {
                        txtName.Text = Convert.ToString(dr1.ItemArray[0]);
                        txtSurname.Text = Convert.ToString(dr1.ItemArray[1]);
                    }
                    else
                    {
                        txtID.Text = Convert.ToString(dr1.ItemArray[0]);
                        txtName.Text = Convert.ToString(dr1.ItemArray[1]);
                        txtSurname.Text = Convert.ToString(dr1.ItemArray[2]);
                        txtAge.Text = Convert.ToString(dr1.ItemArray[3]);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(global.flag == 1)
            {
                SqlConnection _conn = new SqlConnection(sqlCon);
                try
                {
                    if (txtID.Text.Trim() == "")
                    {
                        MessageBox.Show("กรุณาเลือกพนักงานที่ต้องการลบข้อมูล"); return;
                    }
                    if (MessageBox.Show("ลบข้อมูลคุณ " + txtName.Text.Trim() + "  " + txtSurname.Text.Trim() + "?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        _conn.Open();
                        int eID = Convert.ToInt32(txtID.Text.Trim());
                        //String query = "Delete from employeeinfo where eid='" + eID + "'";
                        SqlCommand com = new SqlCommand("DeleteEmp", _conn);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@eID", eID));
                        com.ExecuteNonQuery();
                        MessageBox.Show("ลบข้อมูลเรียบร้อยแล้ว");
                        _conn.Close();
                        ClearData();
                        ShowData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please contact your administrator.");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(global.flag == 1)
            {
                SqlConnection _conn = new SqlConnection(sqlCon);
                try
                {
                    if (txtID.Text.Trim() != "" && txtName.Text.Trim() != "" && txtSurname.Text.Trim() != "" && txtAge.Text.Trim() != "")
                    {
                        if (MessageBox.Show("แก้ไขข้อมูล?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            _conn.Open();
                            int eID = Convert.ToInt32(txtID.Text.Trim());
                            int age = Convert.ToInt32(txtAge.Text.Trim());
                            //String query = "update employeeinfo set name='" + txtName.Text.Trim() + "', surname='" + txtSurname.Text.Trim() + "', age='" + age + "' where eid='" + eID + "'";
                            SqlCommand com = new SqlCommand("UpdateEmp", _conn);
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@eID", eID));
                            com.Parameters.Add(new SqlParameter("@name", this.txtName.Text.Trim()));
                            com.Parameters.Add(new SqlParameter("@surname", this.txtSurname.Text.Trim()));
                            com.Parameters.Add(new SqlParameter("@age", age));
                            com.ExecuteNonQuery();
                            _conn.Close();
                            MessageBox.Show("แก้ไขข้อมูลเรียบร้อยแล้ว");
                            ClearData();
                            ShowData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("กรุณาเลือกพนักงานที่ต้องการแก้ไขข้อมูล"); return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please contact your administrator.");
            }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            ShowData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (global.flag == 10)
            {
                SqlConnection _conn = new SqlConnection(sqlCon);
                _conn.Open();
                //string query = "select name, surname from employeeinfo where name LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%' OR surname LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%'";
                SqlCommand createCommand = new SqlCommand("SearchGuest", _conn);
                createCommand.CommandType = CommandType.StoredProcedure;
                createCommand.Parameters.Add(new SqlParameter("@txtSearch", txtSearch.Text.Trim()));
                createCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("employeeinfo");
                dataAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                _conn.Close();
            }
            else
            {
                if (comboBox1.Text.Trim() == "All" && txtSearch.Text.Trim() != "")
                {
                    searchAll();
                }

                else if (comboBox1.Text.Trim() == "eid" && txtSearch.Text.Trim() != "")
                {
                    searchEid();
                }

                else if (comboBox1.Text.Trim() == "name" && txtSearch.Text.Trim() != "")
                {
                    searchName();
                }

                else if (comboBox1.Text.Trim() == "surname" && txtSearch.Text.Trim() != "")
                {
                    searchSurname();
                }

                else if (comboBox1.Text.Trim() == "age" && txtSearch.Text.Trim() != "")
                {
                    searchAge();
                }

                else
                {
                    ShowData();
                }
            }
        }

        private void ClearData()
        {
            txtID.Clear();
            txtName.Clear();
            txtSurname.Clear();
            txtAge.Clear();
            txtSearch.Clear();
            comboBox1.Text = null;
            ShowData();
        }

        private void ShowData()
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                _conn.Open();
                SqlCommand createCommand = new SqlCommand("ShowData", _conn);
                createCommand.CommandType = CommandType.StoredProcedure;
                createCommand.Parameters.Add(new SqlParameter("@flag", global.flag));
                createCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("employeeinfo");
                dataAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                _conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchAll()
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                string Str = txtSearch.Text.Trim();
                int intSearch = Convert.ToInt32(txtSearch.Text.Trim());
                double Num;
                bool isNum = double.TryParse(Str, out Num);
                _conn.Open();
                //string query = "select eid, name, surname, age from employeeinfo where name LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%' OR surname LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%'";
                SqlCommand createCommand = new SqlCommand("SearchAll", _conn);
                createCommand.CommandType = CommandType.StoredProcedure;
                createCommand.Parameters.Add(new SqlParameter("@intSearch", intSearch));
                createCommand.Parameters.Add(new SqlParameter("@txtSearch", this.txtSearch.Text.Trim()));
                createCommand.Parameters.Add(new SqlParameter("@bool", isNum));
                createCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable("employeeinfo");
                dataAdp.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                _conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchEid()
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                string Str = txtSearch.Text.Trim();
                double Num;
                bool isNum = double.TryParse(Str, out Num);
                if (isNum)
                {
                    _conn.Open();
                    int eID = Convert.ToInt32(txtSearch.Text.Trim());
                    //string query = "select eid, name, surname, age from employeeinfo where eid LIKE '%' + '" + eID + "'";
                    SqlCommand createCommand = new SqlCommand("SearchEid", _conn);
                    createCommand.CommandType = CommandType.StoredProcedure;
                    createCommand.Parameters.Add(new SqlParameter("@eID", eID));
                    createCommand.ExecuteNonQuery();

                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("employeeinfo");
                    dataAdp.Fill(dt);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    dataAdp.Update(dt);
                    _conn.Close();
                }
                else
                {
                    MessageBox.Show("กรุณากรอกข้อมูลเป็นตัวเลข");
                    txtSearch.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchName()
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                string Str = txtSearch.Text.Trim();
                double Num;
                bool isNum = double.TryParse(Str, out Num);
                if (isNum)
                {
                    MessageBox.Show("กรุณากรอกข้อมูลเป็นตัวหนังสือ");
                    txtSearch.Clear();
                }
                else
                {
                    _conn.Open();
                    //string query = "select eid, name, surname, age from employeeinfo where name LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%'";
                    SqlCommand createCommand = new SqlCommand("SearchName", _conn);
                    createCommand.CommandType = CommandType.StoredProcedure;
                    createCommand.Parameters.Add(new SqlParameter("@name", this.txtSearch.Text.Trim()));
                    createCommand.ExecuteNonQuery();

                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("employeeinfo");
                    dataAdp.Fill(dt);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    dataAdp.Update(dt);
                    _conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchSurname()
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                string Str = txtSearch.Text.Trim();
                double Num;
                bool isNum = double.TryParse(Str, out Num);
                if (isNum)
                {
                    MessageBox.Show("กรุณากรอกข้อมูลเป็นตัวหนังสือ");
                    txtSearch.Clear();
                }
                else
                {
                    _conn.Open();
                    //string query = "select eid, name, surname, age from employeeinfo where surname LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%'";
                    SqlCommand createCommand = new SqlCommand("SearchSurname", _conn);
                    createCommand.CommandType = CommandType.StoredProcedure;
                    createCommand.Parameters.Add(new SqlParameter("@surname", this.txtSearch.Text.Trim()));
                    createCommand.ExecuteNonQuery();

                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("employeeinfo");
                    dataAdp.Fill(dt);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    dataAdp.Update(dt);
                    _conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchAge()
        {
            SqlConnection _conn = new SqlConnection(sqlCon);
            try
            {
                string Str = txtSearch.Text.Trim();
                double Num;
                bool isNum = double.TryParse(Str, out Num);
                if (isNum)
                {
                    int age = Convert.ToInt32(txtSearch.Text.Trim());
                    _conn.Open();
                    //string query = "select eid, name, surname, age from employeeinfo where name LIKE '%' + '" + txtSearch.Text.Trim() + "' + '%'";
                    SqlCommand createCommand = new SqlCommand("SearchAge", _conn);
                    createCommand.CommandType = CommandType.StoredProcedure;
                    createCommand.Parameters.Add(new SqlParameter("@age", age));
                    createCommand.ExecuteNonQuery();

                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable("employeeinfo");
                    dataAdp.Fill(dt);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    dataAdp.Update(dt);
                    _conn.Close();
                }
                else
                {
                    MessageBox.Show("กรุณากรอกข้อมูลเป็นตัวเลข");
                    txtSearch.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            previousText = txtName.Text;
            double num = 0;
            bool success = double.TryParse(((TextBox)sender).Text, out num);
            if (success & num >= 0)
                previousText = ((TextBox)sender).Text;
            else
                ((TextBox)sender).Text = previousText;
        }

        private void txtAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !onlyNumeric(e.Text);
        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !onlyAlphabet(e.Text);
        }

        private void txtSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !onlyAlphabet(e.Text);
        }

        public static bool onlyNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]"); //regex that allows numeric input only
            return !regex.IsMatch(text); // 
        }

        public static bool onlyAlphabet(string text)
        {
            Regex regex = new Regex("[^A-Z|^a-z|^ก-๙]"); //text only
            return !regex.IsMatch(text);
        }

        //=====================XAML==ListBox==ComboBox=====================
        //<ComboBox Height="24" HorizontalAlignment="Left" Margin="118,12,0,0" Name="comboBox1" VerticalAlignment="Top" Width="120" />
        //<ListView Height="61" HorizontalAlignment="Left" Margin="244,12,0,0" Name="listView1" VerticalAlignment="Top" Width="247" />
        
        //======================SourceCode==ComboBox======================
        //private void Fill_Combo()
        //{
        //    SqlConnection _conn = new SqlConnection(sqlCon);
        //    try
        //    {
        //        _conn.Open();
        //        string query = "select * from employeeinfo";
        //        SqlCommand createCommand = new SqlCommand(query, _conn);
        //        SqlDataReader dr = createCommand.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            string name = dr.GetString(1);
        //            comboBox1.Items.Add(name);
        //        }
        //        _conn.Close();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Something Error!");
        //    }
        //}
        //
        //======================SourceCode==ListBox======================
        //private void Fill_ListView()
        //{
        //    SqlConnection _conn = new SqlConnection(sqlCon);
        //    try
        //    {
        //        _conn.Open();
        //        string query = "select * from employeeinfo";
        //        SqlCommand createCommand = new SqlCommand(query, _conn);
        //        SqlDataReader dr = createCommand.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            string name = dr.GetString(1);
        //            string sname = dr.GetString(2);
        //            listView1.Items.Add(name + "." + sname);
        //        }
        //        _conn.Close();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Something Error!");
        //    }
        //}
    }
}
