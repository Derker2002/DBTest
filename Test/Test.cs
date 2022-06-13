using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }
        private DataBase db = new DataBase();
        private DataTable dtCex, dtO, dtPR,dtTr;

        private void addSklad_Click(object sender, EventArgs e)
        {
            if (O_Cex.Text!="" && O_Val.Text!="")
            {
                db.openConnnection();
                db.addSklad(Convert.ToInt32(O_Cex.Text), Convert.ToDecimal(O_Val.Text));
                db.closeConnnection();
                dtO = db.getData("O_Sklad");
                O_Sklad_V.DataSource = dtO;
                O_Sklad_V.Update();
                dtTr = db.getTrades();
                Trades_V.DataSource = dtTr;
                Trades_V.Update();
            }
            else
                MessageBox.Show("Заполните все поля!");

        }

        private void addTrade_Click(object sender, EventArgs e)
        {
            if (PR_Cex_O.Text!="" && PR_Cex_P.Text!="" && PR_Kol.Text!="")
            {
                db.openConnnection();
                db.addTrade(Convert.ToInt32(PR_Cex_O.Text), Convert.ToInt32(PR_Cex_P.Text), Convert.ToDecimal(PR_Kol.Text));
                db.closeConnnection();
                dtPR = db.getData("PR_Sklad");
                PR_Sklad_V.DataSource = dtPR;
                PR_Sklad_V.Update();
                dtTr = db.getTrades();
                Trades_V.DataSource = dtTr;
                Trades_V.Update();
            }
            else
                MessageBox.Show("Заполните все поля!");
        }

        private void get_Trades_Click(object sender, EventArgs e)
        {
            dtTr=db.getTrades();
            Trades_V.DataSource = dtTr;
            Trades_V.Update();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            db.openConnnection();
            dtCex=db.getData("S_Cex");
            dtO = db.getData("O_Sklad");
            dtPR = db.getData("PR_Sklad");
            db.closeConnnection();
            S_Cex_V.DataSource=dtCex;
            O_Sklad_V.DataSource=dtO;
            PR_Sklad_V.DataSource = dtPR;
            S_Cex_V.Update();
            O_Sklad_V.Update();
            PR_Sklad_V.Update();
        }

        private void addCex_Click(object sender, EventArgs e)
        {

            if (Cex_Numb.Text != "" && Cex_Name.Text != "")
            {
                db.openConnnection();
                db.addCex(Convert.ToInt32(Cex_Numb.Text), Cex_Name.Text);
                db.closeConnnection();
                dtCex = db.getData("S_Cex");
                S_Cex_V.DataSource = dtCex;
                S_Cex_V.Update();
            }
            else
                MessageBox.Show("Заполните все поля!");
            
        }
    }
}
