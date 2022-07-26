using Sistema.Utils;
using System.Windows;

namespace Sistema
{
    public partial class WpfDialogResult : Window
    {
        #region 1- Variáveis
        private string _texto = string.Empty;
        private string _opUm = string.Empty;
        private string _opDois = string.Empty;
        private int _index;
        #endregion

        #region 2- Métodos Construtores
        public WpfDialogResult()
        {
            InitializeComponent();
        }
        public WpfDialogResult(string pergunta, int index)
        {
            InitializeComponent();
            _texto = pergunta;
            _index = index;
        }

        public WpfDialogResult(string pergunta, string opcaoUm, string opcaoDois, int index)
        {
            InitializeComponent();
            _texto = pergunta;
            _opUm = opcaoUm;
            _opDois = opcaoDois;
            _index = index;
        }
        #endregion

        #region 3- Eventos de Controle

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_index == 1)
            {
                gridPergunta.Visibility = Visibility.Visible;
                txbPergunta.Text = _texto;
            }

            if (_index == 2)
            {
                gridAviso.Visibility = Visibility.Visible;
                txbPergunta.Text = _texto;
            }

            if (_index == 3)
            {
                gridOpcao.Visibility = Visibility.Visible;
                txbPergunta.Text = _texto;
                btnUm.Content = _opUm;
                btnDois.Content = _opDois;
            }
        }

        private void btnSim_Click(object sender, RoutedEventArgs e)
        {
            NewMessageBox.Resposta = true;
            gridPergunta.Visibility = Visibility.Collapsed;
            gridAviso.Visibility = Visibility.Collapsed;
            Close();
        }

        private void btnNao_Click(object sender, RoutedEventArgs e)
        {
            NewMessageBox.Resposta = false;
            gridPergunta.Visibility = Visibility.Collapsed;
            gridAviso.Visibility = Visibility.Collapsed;
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            gridPergunta.Visibility = Visibility.Collapsed;
            gridAviso.Visibility = Visibility.Collapsed;
            Close();
        }

        private void btnUm_Click(object sender, RoutedEventArgs e)
        {
            NewMessageBox.Opcao = 1;
            gridOpcao.Visibility = Visibility.Collapsed;
            Close();
        }

        private void btnDois_Click(object sender, RoutedEventArgs e)
        {
            NewMessageBox.Opcao = 2;
            gridOpcao.Visibility = Visibility.Collapsed;
            Close();
        }

        #endregion
    }
}
