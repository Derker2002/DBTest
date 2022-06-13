using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    class DataBase
    {
        private static MySqlConnection infcon = new MySqlConnection("server=localhost;port=3306;username=root;database=information_schema");
        private static MySqlConnection con = new MySqlConnection("server=localhost;port=3306;username=root;database=testdb");
        private MySqlCommand cmd = con.CreateCommand();
        private MySqlDataReader reader;
        private MySqlDataAdapter adapter = new MySqlDataAdapter();

        private bool tableExist(string table)
        {
            cmd.CommandText = "SELECT count(*) FROM `TABLES` WHERE `TABLE_SCHEMA`='testdb' and `TABLE_NAME`='"+table+"'";
            reader = cmd.ExecuteReader();
            reader.Read();
            if (Convert.ToInt32(reader[0]) == 0)
            {
                reader.Close();
                return false;
            }
            reader.Close();
            return true;
        }

        public DataBase()
        {
            openConnnection();
            cmd = infcon.CreateCommand();
            con.Open();
            infcon.Open();
            if (!tableExist("S_Cex"))
            {
                cmd = con.CreateCommand();
                cmd.CommandText = "Create table S_Cex (KodCex int, NamCex char(100))";
                cmd.ExecuteNonQuery();
                cmd = infcon.CreateCommand();
            }
            if (!tableExist("O_Sklad"))
            {
                cmd = con.CreateCommand();
                cmd.CommandText = "Create table O_Sklad (KodCex int, Kol numeric(18,3))";
                cmd.ExecuteNonQuery();
                cmd = infcon.CreateCommand();
            }
            if (!tableExist("PR_Sklad"))
            {
                cmd = con.CreateCommand();
                cmd.CommandText = "Create table PR_Sklad (KodCex_p int,KodCex_o int, Kol numeric(18,3))";
                cmd.ExecuteNonQuery();
                cmd = infcon.CreateCommand();
            }
            infcon.Close();
            con.Close();
            cmd = con.CreateCommand();
            closeConnnection();
        }
        public void openConnnection() 
        {
            try
            {
                if (con.State==System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("connection failed: " + ex.Message);
                throw;
            }
        }

        public MySqlConnection getConnection()
        {
            return con;
        }

        public DataTable getData(string table)
        {
            
            cmd.CommandText="Select * From "+table;
            DataTable dt = new DataTable();
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            return dt;
            
        }
        public void addCex(int Cex,string name)
        {
            cmd.CommandText = "Insert Into S_Cex Values ('"+Cex+"', '"+name+"')";
            cmd.ExecuteNonQuery();
        }
        public void addSklad(int Cex,decimal val)
        {
            cmd.CommandText = "Insert Into O_Sklad Values ('" + Cex + "', @val)";
            cmd.Parameters.Add("val",MySqlDbType.Decimal).Value = val;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        public void addTrade(int Cex_P,int Cex_O,decimal Kol)
        {
            cmd.CommandText = "Insert Into PR_Sklad Values ('" + Cex_P + "', '" + Cex_O + "',@val)";
            cmd.Parameters.Add("val", MySqlDbType.Decimal).Value = Kol;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        public DataTable getTrades()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            cmd.CommandText = "SELECT s_cex.KodCex AS 'Код Цеха',IF(sklad.Kol IS NULL,0,sklad.Kol) AS 'На складе',IF(pol.PSUM IS NULL,0,pol.PSUM) as 'Получил', IF(otp.OSUM IS NULL,0,otp.OSUM) AS 'Отправил',IF(sklad.Kol IS NULL,0,sklad.Kol)+IF(pol.PSUM IS NULL,0,pol.PSUM)-IF(otp.OSUM IS NULL,0,otp.OSUM) AS 'Осталось' FROM s_cex LEFT JOIN(SELECT `KodCex`, SUM(`Kol`) AS Kol FROM o_sklad GROUP BY `KodCex`) AS sklad ON s_cex.KodCex = sklad.KodCex LEFT JOIN(SELECT `KodCex_p`, SUM(`Kol`) AS PSUM FROM pr_sklad GROUP BY `KodCex_p`) AS pol ON s_cex.KodCex = pol.KodCex_p LEFT JOIN(SELECT `KodCex_o`, SUM(`Kol`) AS OSUM FROM pr_sklad GROUP BY `KodCex_o`) AS otp ON s_cex.KodCex = otp.KodCex_o; ";
            adapter.SelectCommand = cmd;
            adapter.Fill(dt1);
            return dt1;

        }
        public void closeConnnection()
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("connection failed: " + ex.Message);
                throw;
            }
        }
    }
}
//By Kucher Andrii