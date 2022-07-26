using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sistema.Utils
{
    public static class AutoCompleteBoxUserControl
    {
        public class autoComplete
        {
            public string Nome { get; set; }
            public string Id { get; set; }
            public string Descricao { get; set; }
            public string DescricaoGrupo { get; set; }

            public string IdGrupo { get; set; }
            public string IdCategoria { get; set; }
            public string Quantidade { get; set; }
            public string Valor { get; set; }
            public string Sku { get; set; }

            public string Preco { get; set; }

            public string fullname => Id + " - " + Nome;
        }

        public static void OpenSuggestionBox(Popup nomePoput, ListBox nomeLista)
        {
            try
            {
                nomePoput.Visibility = Visibility.Visible;
                nomePoput.IsOpen = true;
                nomeLista.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                NewMessageBox.AbreWpf("" + ex.Message + " Error", 2);
            }
        }

        public static void CloseAutoSuggestionBox(Popup nomePoput, ListBox nomeLista)
        {
            try
            {
                nomePoput.Visibility = Visibility.Collapsed;
                nomePoput.IsOpen = false;
                nomeLista.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                NewMessageBox.AbreWpf("" + ex.Message + " Error", 2);
            }
        }

        public static void SelectIndex(TextBox nameTextBox, Popup nomePoput, ListBox nomeLista)
        {
            try
            {
                if (nomeLista.SelectedIndex <= -1)
                {
                    AutoCompleteBoxUserControl.CloseAutoSuggestionBox(nomePoput, nomeLista);
                    return;
                }

                AutoCompleteBoxUserControl.CloseAutoSuggestionBox(nomePoput, nomeLista);

                int item = nomeLista.SelectedIndex;

                List<autoComplete> nwList = new List<autoComplete>();

                foreach (autoComplete i in nomeLista.Items)
                {
                    nwList.Add(i);
                }

                nameTextBox.Text = nwList[item].fullname;

                AutoCompleteBoxUserControl.CloseAutoSuggestionBox(nomePoput, nomeLista);

            }
            catch (Exception ex)
            {
                NewMessageBox.AbreWpf("" + ex.Message + " Error", 2);
            }
        }

        public static void SelectContains(TextBox nameTextBox, Popup nomePoput, ListBox nomeLista, List<autoComplete> _list)
        {
            try
            {
                if (string.IsNullOrEmpty(nameTextBox.Text))
                {
                    AutoCompleteBoxUserControl.CloseAutoSuggestionBox(nomePoput, nomeLista);
                    return;
                }

                AutoCompleteBoxUserControl.OpenSuggestionBox(nomePoput, nomeLista);
                nomeLista.ItemsSource = _list.Where(p => p.fullname.Contains(nameTextBox.Text)).ToList();
            }
            catch (Exception ex)
            {
                // Info.  
                NewMessageBox.AbreWpf("" + ex.Message + " Error", 2);
            }
        }
    }
}
