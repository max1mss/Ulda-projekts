using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Reflection.Emit;

namespace uldis_ladite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        int darba_samaksa = 15;
        int PVN = 21;


        static SQLiteConnection CreateConnection() // Programmas konnekcija ar SQL datubāzi
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source=Ulda_ladite.db; Version=3; New=True; Compress=True;");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                // Savienojuma kļūdu apstrāde
            }
            return sqlite_conn;
        }

        private void button1_Click(object sender, EventArgs e)
            {
            //Pārveido vēlējumu stringu uz int
            string varda_ga = tb_velt_teksts.Text;
            int gar = varda_ga.Length;

            // Pārbauda vai visas nepieciešamās vērtības ir 
            if (
                int.TryParse(tb_platums.Text, out int platums) &&
                int.TryParse(tb_garums.Text, out int garums) &&
                int.TryParse(tb_augstums.Text, out int augstums) &&
                int.TryParse(tb_cena.Text, out int materiala_cena))
            {
                //  saksaita characters tb_veltijums un reizina ar 1.2
                string inputText = tb_velt_teksts.Text;
                int charCount = inputText.Length;
                double multipliedCount = charCount * 1.2;

                // veic matemātiku
                double produkta_cena = (gar * 1.2) + ((platums / 100) * (augstums / 100) * (garums / 100)) / 3 * materiala_cena;
                double PVN_summa = (produkta_cena + darba_samaksa) * PVN / 100;
                double rekina_summa = produkta_cena + darba_samaksa + PVN_summa;

                // Parāda to RichTextBox
                richTextBox1.Text = $"Produkta cena: {produkta_cena:C}\n" +
                                   $"PVN summa: {PVN_summa:C}\n" +
                                   $"Rekina summa: {rekina_summa:C}\n";



          

            }
            else
            {
                // Kļūdas gadījumā programma izvada paziņojumu
                richTextBox1.Text = "Iavadiet lūdzu pareizi.";
            }



            }




        private void button2_Click(object sender, EventArgs e)
        {
            
            using (StreamWriter a = new StreamWriter("Cheks.txt"))
            {
                // Izveidojam jaunu mapi ar failu, kur saglabāsies informācija

                a.WriteLine(lb_vards.Text + " " + tb_vards.Text);
                a.WriteLine(lb_uzvards.Text + " " + tb_uzvards.Text);
                a.WriteLine(lb_platums.Text + " " + tb_platums.Text);
                a.WriteLine(lb_garums.Text + " " + tb_garums.Text);
                a.WriteLine(lb_augstums.Text + " " + tb_augstums.Text);
                a.WriteLine(lb_velt_teksts.Text + " " + tb_velt_teksts.Text);
                a.WriteLine(lb_cena.Text + " " + tb_cena.Text);

                
                a.Close();
                MessageBox.Show("Veiksmīgi saglabāts failā!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (AreFieldsFilled())
            {
                SQLiteConnection sqlite_conn;
                sqlite_conn = CreateConnection();

                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO Uldaizmaksas (Vards, Uzvards, Velejums, Laditesgarums, Laditesplatums, Laditesaugstums, Kokmaterialacena) " +
                                         "VALUES('" + tb_vards.Text + "', '" + tb_uzvards.Text + "', '" +
                                         tb_velt_teksts.Text + "', '" + tb_garums.Text + "', '" + tb_platums.Text + "', '"+tb_augstums.Text+"', '"+tb_cena.Text+"');";
                sqlite_cmd.ExecuteNonQuery();

                // Izvadām lodziņu, ja lietotājs ievadīja visu info.
                MessageBox.Show("Jūs veiksmīgi saglabājāt informāciju datubāzē!");
            }
            else
            {
                MessageBox.Show("Lūdzu ievadiet visus datus!");
            }
        }
        private bool AreFieldsFilled()
        {
            // Pārbaudām vai ir aizpildīti visi lodziņi
            return !string.IsNullOrEmpty(tb_vards.Text) &&
                   !string.IsNullOrEmpty(tb_uzvards.Text) &&
                   !string.IsNullOrEmpty(tb_velt_teksts.Text) &&
                   !string.IsNullOrEmpty(tb_garums.Text) &&
                   !string.IsNullOrEmpty(tb_platums.Text) &&
                   !string.IsNullOrEmpty(tb_augstums.Text) &&
                   !string.IsNullOrEmpty(tb_cena.Text);
        }
    }
}
