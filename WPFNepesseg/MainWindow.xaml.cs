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

namespace WPFNepesseg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Telepules> telepulesek;
        List<Telepules> kivalasztottTelepulesek = new List<Telepules>();

        public MainWindow()
        {
            InitializeComponent();

            telepulesek = AdatokBetoltese("Adatok\\kozerdeku_lakossag_2022.csv");
            dgTelepulesek.ItemsSource = telepulesek;
            dgKivalasztott.ItemsSource = kivalasztottTelepulesek;

            cbMegyek.ItemsSource = telepulesek.Select(x => x.Megye).Distinct().ToList().Prepend("Nincs szűrve");
        }

        private List<Telepules> AdatokBetoltese(string fajlNeve)
        {
            List<Telepules> ujTelepulesek = new List<Telepules>();

            string[] csvSorok = File.ReadAllLines(fajlNeve);
            for (int i = 1; i < csvSorok.Length; i++)
            {
                string[] mezok = csvSorok[i].Split(';');
                Telepules ujTelepules = new Telepules(mezok[2],
                                                      mezok[3],
                                                      mezok[4],
                                                      int.Parse(mezok[5].Replace(" ", "")),
                                                      int.Parse(mezok[6].Replace(" ", "")));
                ujTelepulesek.Add(ujTelepules);
            }

            return ujTelepulesek;
        }

        private void cbMegyek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMegyek.SelectedItem.ToString() == "Nincs szűrve")
            {
                dgTelepulesek.ItemsSource = telepulesek;
            }
            else
            { 
                dgTelepulesek.ItemsSource = telepulesek.Where(x => x.Megye == cbMegyek.SelectedItem.ToString());
            }
        }

        private void dgTelepulesek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTelepulesek.SelectedItem == null)
            {
                return;
            }
            telepulesek.Remove(dgTelepulesek.SelectedItem as Telepules);
            kivalasztottTelepulesek.Add(dgTelepulesek.SelectedItem as Telepules);
        }
    }
}
