using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Numerics;


namespace RSA
{
    /// <summary>
    /// Interaction logic for Algorytm.xaml
    /// </summary>
    public partial class Algorytm : UserControl
    {
        public Algorytm()
        {
            InitializeComponent();
        }

        int p = 0;
        int q = 0;
        int nwd;

        public static bool Pierwsza(int n)
        {
            if (n < 2)
                return false;

            for (int i = 2; i * i <= n; i++)
                if (n % i == 0)
                    return false;
            return true;
        }

        private void txt_p_OK_Click(object sender, RoutedEventArgs e)
        {
            int pom = Convert.ToInt32(txt_p.Text);
            if (Pierwsza(pom))
            {
                if ((pom / 1000) < 1)
                {
                    MessageBox.Show(txt_p.Text.ToString() + " nie jest liczbą czterocyfrową.");
                    p = 0;
                }
                else
                {
                    p = pom;
                }
            }
            else
            {
                MessageBox.Show(txt_p.Text.ToString() + " nie jest liczbą pierwszą.");
                p = 0;
            }
        }

        private void txt_q_OK_Click(object sender, RoutedEventArgs e)
        {
            int pom = Convert.ToInt32(txt_q.Text);
            if (Pierwsza(pom))
            {
                if ((pom / 1000) < 1)
                {
                    MessageBox.Show(txt_q.Text.ToString() + " nie jest liczbą czterocyfrową.");
                    q = 0;
                }
                else
                {
                    q = pom;
                }
            }
            else
            {
                MessageBox.Show(txt_q.Text.ToString() + " nie jest liczbą pierwszą.");
                q = 0;
            }
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static int Rand(int a)
        {
            lock (syncLock)
            {
                return random.Next(1, a);
            }
        }

        public static int NWD(int e, int a)
        {
            while (a != e)
            {
                if (a > e)
                    a -= e;
                else
                    e -= a;
            }

            return a;
        }

        private void Puliczny_wygeneruj_Click(object sender, RoutedEventArgs e)
        {
            if (p == 0 | q == 0)
            {
                MessageBox.Show("Liczby pierwsze nie zostały podane.");
            }
            else
            {
                int n = p * q;
                txt_n.Text = n.ToString();
                int a = (p - 1) * (q - 1);
                int e1;
                do
                {
                    e1 = Rand(a);
                    nwd = NWD(e1, a);

                } while (!(Pierwsza(e1)) && !(nwd == 1));

                txt_e.Text = e1.ToString();

            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo.SelectedIndex == 0)
            {
                txt_pom.Text = nwd.ToString();
            }
            if (combo.SelectedIndex == 1)
            {
                txt_pom.Text = ((p - 1) * (q - 1)).ToString();
            }
        }

        public static int wylicz_d(int e, int a)
        {
            int e0, a0, p0, p1, q, r, t;
            p0 = 0;
            p1 = 1;
            e0 = e;
            a0 = a;
            q = a0 / e0;
            r = a0 % e0;
            while (r > 0)
            {
                t = p0 - q * p1;
                if (t >= 0)
                {
                    t = t % a;
                }
                else
                {
                    t = a - ((-t) % a);
                }
                p0 = p1;
                p1 = t;
                a0 = e0;
                e0 = r;
                q = a0 / e0;
                r = a0 % e0;
            }

            return p1;
        }

        private void Prywatny_wygeneruj_Click(object sender, RoutedEventArgs e)
        {
            int d;
            //d x e mod a =1
            int a = (p - 1) * (q - 1);
            int e1 = Convert.ToInt32(txt_e.Text);
            d = wylicz_d(e1, a);
            txt_d.Text = d.ToString();
        }

        static String odczyt_z_pliku(String path)
        {
            string s = File.ReadAllText(path, Encoding.Default);
            return s;
        }

        private void wczytaj_tekst_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Wiadomość"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                do_zaszyfrowania.Text = odczyt_z_pliku(path);
            }
        }

        private void zapisz_tekst_Click(object sender, RoutedEventArgs e)
        {

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Wiadomość";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(do_zaszyfrowania.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

        private void wczytaj_zaszyfrowana_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Zaszyfrowana"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                zaszyfrowana.Text = odczyt_z_pliku(path);
            }
        }

        private void zapisz_zaszyfrowana_Click(object sender, RoutedEventArgs e)
        {

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Zaszyfrowana";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(zaszyfrowana.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

        Encoding ascii = Encoding.ASCII;
        private void Szyfruj_Click(object sender, RoutedEventArgs e)
        {
            string wiadomość = do_zaszyfrowania.Text;
            string c = null;
            Byte[] encodedBytes = ascii.GetBytes(wiadomość);
            double n = Convert.ToDouble(txt_n.Text);
            BigInteger big_n = new BigInteger(n);
            double e1 = Convert.ToDouble(txt_e.Text);
            BigInteger big_e = new BigInteger(e1);
            string m;
            double m1;
            BigInteger big_m;
            BigInteger big_c;
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                m = encodedBytes[i].ToString();
                m1 = Convert.ToDouble(m);
                big_m = new BigInteger(m1);

                if (c == null)
                {
                    big_c = BigInteger.ModPow(big_m, big_e, big_n);
                    c = big_c.ToString();
                }
                else
                {
                    big_c = BigInteger.ModPow(big_m, big_e, big_n);
                    c = c + " " + big_c.ToString();
                }
            }
            zaszyfrowana.Text = c;
        }

        private void Deszyfruj_Click(object sender, RoutedEventArgs e)
        {
            string wiadomość = zaszyfrowana.Text;
            string m = null;
            Char separator = ' ';
            string[] w = wiadomość.Split(separator);
            double[] wiad = new double[w.Length];
            for (int i = 0; i < w.Length; i++)
            {

                wiad[i] = Convert.ToDouble(w[i]);
            }
            double n = Convert.ToDouble(txt_n.Text);
            BigInteger big_n = new BigInteger(n);
            double d = Convert.ToDouble(txt_d.Text);
            BigInteger big_d = new BigInteger(d);
            BigInteger big_c;
            BigInteger big_m;
            char znak;
            for (int i = 0; i < wiad.Length; i++)
            {
                big_c = new BigInteger(wiad[i]);
                big_m = BigInteger.ModPow(big_c, big_d, big_n);
                znak = (char)(byte.Parse(big_m.ToString()));
                m = m + znak;
            }
            zdeszyfrowana.Text = m;

        }

        private void wczytaj_p_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "p"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                txt_p.Text = odczyt_z_pliku(path);
            }
        }

        private void wczytaj_q_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "q"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                txt_q.Text = odczyt_z_pliku(path);
            }
        }

        private void zapisz_p_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "p";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(txt_p.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

        private void zapisz_q_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "q";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(txt_q.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

        private void wczytaj_e_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "e"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                txt_e.Text = odczyt_z_pliku(path);
            }
        }

        private void wczytaj_n_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "n"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                txt_n.Text = odczyt_z_pliku(path);
            }
        }

        private void zapisz_e_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "e";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(txt_e.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

        private void zapisz_n_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "n";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(txt_n.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

        private void wczytaj_d_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "d"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string path = dlg.FileName;
                txt_d.Text = odczyt_z_pliku(path);
            }
        }

        private void zapisz_d_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "d";
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "txt files (.txt)|.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    byte[] buffer = Encoding.Default.GetBytes(txt_d.Text);
                    myStream.Write(buffer, 0, buffer.Length);

                    myStream.Close();
                }
            }
        }

    }
}
