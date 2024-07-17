using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace AuthorsWPF;


public partial class MainWindow : Window
{
    private SqlConnection conn;
    private DataSet dataSet;
    private SqlDataAdapter adapter;
    private SqlCommandBuilder cmd;
    private SqlDataReader reader;
    private SqlCommand command;
    string cs = @"Data Source=COMPUTER01\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;TrustServerCertificate=True";
    public MainWindow()
    {
        InitializeComponent();
        conn = new();
        conn.ConnectionString = cs;

        dataSet = new();
        string query =
    @"
        SELECT FirstName, LastName
        FROM Authors
        ";
        adapter = new SqlDataAdapter(query, conn);
        cmd = new SqlCommandBuilder(adapter);
        adapter.Fill(dataSet, "Authors");
        comboBox.ItemsSource = dataSet.Tables["Authors"].DefaultView;

    }

    private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox)
        {
            ComboBox box = sender as ComboBox;
            try
            {
                DataRowView selectedRow = box.SelectedItem as DataRowView;
                if (selectedRow != null)

                dataSet = new();
                string query =
                    @$"
                SELECT Name
                FROM Books AS B 
                JOIN Authors AS A 
                ON B.Id_Author=A.Id AND A.FirstName='{selectedRow[0].ToString()}' AND A.LastName='{selectedRow[1].ToString()}'
                ";
                adapter = new SqlDataAdapter(query, conn);
                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(dataSet, "Authors");
                resultDataGrid.ItemsSource = dataSet.Tables["Authors"].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
