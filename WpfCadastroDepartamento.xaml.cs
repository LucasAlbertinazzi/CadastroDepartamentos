using Sist_Controle.Business.Utils;
using Sist_Controle.Utils;
using Sist_Controle_Dal;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Sist_Controle.Business.Utils.AutoCompleteBoxUserControl;

namespace Sist_Controle
{
    /// <summary>
    /// Interaction logic for WpfCadastroDepartamento.xaml
    /// </summary>
    public partial class WpfCadastroDepartamento : Window
    {
        #region Permissões
        private bool UsuarioTemPermissao()
        {
            return (App.Current as Sist_Controle.App).permissao == "G";
        }
        #endregion

        #region 1- Variaveis

        DAL obj = new DAL();

        public List<autoComplete> listUser = new List<autoComplete>();
        public List<autoComplete> listDep = new List<autoComplete>();
        List<GridDepartamentos> _adicionados = new List<GridDepartamentos>();

        #endregion

        #region 2- Classes

        public class GridDepartamentos
        {
            public string colUsuario { get; set; }
            public string colDep { get; set; }
            public string colFuncao { get; set; }
            public string colLoja { get; set; }
        }


        #endregion

        #region 3- Métodos Construtores

        public WpfCadastroDepartamento()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (UsuarioTemPermissao())
            {
                CarregaGridDep();
                CarregaUsuarios();
                CarregaDepartamento();
            }
            else
            {
                Close();
            }
        }

        #endregion

        #region 4- Métodos

        public void CarregaUsuarios()
        {
            listUser = new List<autoComplete>();
            DataTable dt = obj.RetornaTabela("SELECT usuario, codusuario FROM tbl_usuario WHERE ativo = 'S' AND coddep IS NULL ORDER BY usuario ASC");

            foreach (DataRow dr in dt.Rows)
            {
                listUser.Add(new autoComplete
                {
                    Nome = dr["usuario"].ToString(),
                    Id = dr["codusuario"].ToString()
                });
            }

            listboxUsuario.ItemsSource = listUser;

        }

        public void CarregaDepartamento()
        {
            listDep = new List<autoComplete>();

            DataTable dt = obj.RetornaTabela("SELECT nome_dep, id FROM tbl_departamento_usuarios ORDER BY nome_dep ASC");

            foreach (DataRow dr in dt.Rows)
            {
                listDep.Add(new autoComplete
                {
                    Nome = dr["nome_dep"].ToString(),
                    Id = dr["id"].ToString()
                });
            }

            listboxDep.ItemsSource = listDep;
        }

        public void CarregaGridDep()
        {
            _adicionados = new List<GridDepartamentos>();
            DataTable dt = new DataTable();

            dt = obj.RetornaTabela("SELECT u.usuario, u.codusuario, u.codloja, d.nome_dep, f.funcao " +
                                             "FROM tbl_usuario AS u " +
                                             "INNER JOIN tbl_funcao AS f ON u.codfuncao = f.codigo " +
                                             "INNER JOIN tbl_departamento_usuarios AS d ON u.coddep = d.id " +
                                             "WHERE ativo = 'S' AND coddep IS NOT NULL ORDER BY usuario ASC");

            if (dt == null || dt.Rows.Count == 0)
            {
                dgDepartamento.Visibility = Visibility.Collapsed;
                bdDep.Visibility = Visibility.Visible;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _adicionados.Add(new GridDepartamentos
                    {
                        colUsuario = dt.Rows[i]["usuario"].ToString(),
                        colDep = dt.Rows[i]["nome_dep"].ToString(),
                        colFuncao = dt.Rows[i]["funcao"].ToString(),
                        colLoja = dt.Rows[i]["codloja"].ToString()
                    });
                }

                dgDepartamento.ItemsSource = _adicionados;

                dgDepartamento.Visibility = Visibility.Visible;
                bdDep.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region 5- Eventos de controle

        private void btnAddDep_Click(object sender, RoutedEventArgs e)
        {
            if (txbDep.Text != string.Empty)
            {
                string resp = obj.RetornaValor("SELECT * FROM tbl_departamento_usuarios WHERE nome_dep = '" + txbDep.Text + "' ");

                if (resp == "0")
                {
                    obj.ComandoSql("INSERT INTO tbl_departamento_usuarios (nome_dep) VALUES ('" + txbDep.Text + "')");
                    txbDep.Text = string.Empty;
                    NewMessageBox.AbreWpf("DEPARTAMENTO cadastrado com sucesso!", 2);
                    CarregaDepartamento();
                }
                else
                {
                    NewMessageBox.AbreWpf("Já existe um DEPARTAMENTO com este nome!", 2);
                    txbDep.Text = string.Empty;
                }
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            autoComplete listaDep = new autoComplete();
            var item = listboxDep.SelectedItem;
            listaDep = (autoComplete)item;

            autoComplete listaUser = new autoComplete();
            var itemUser = listboxUsuario.SelectedItem;
            listaUser = (autoComplete)itemUser;

            if (txbDep.Text != string.Empty && txbUsuario.Text != string.Empty && txbDep.Text.Length > 3 && txbUsuario.Text.Length > 3)
            {
                if (listaDep.fullname != string.Empty && listaUser.fullname != string.Empty)
                {
                    obj.ComandoSql("UPDATE tbl_usuario SET " +
                                       "coddep = " + listaDep.Id + " " +
                                       "WHERE codusuario = " + listaUser.Id + "");

                    txbUsuario.Text = string.Empty;
                    NewMessageBox.AbreWpf("DEPARTAMENTO vinculado com sucesso!", 2);
                }

                else
                {
                    NewMessageBox.AbreWpf("Selecione um departamento e um usuário!", 2);
                }

                CarregaUsuarios();
                CarregaGridDep();
            }
        }

        private void listboxUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteBoxUserControl.SelectIndex(txbUsuario, popupUsuario, listboxUsuario, 1);
        }

        private void txbUsuario_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (txbUsuario.Text.Length > 0)
            {
                lblMarcaUser.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblMarcaUser.Visibility = Visibility.Visible;
            }

            AutoCompleteBoxUserControl.SelectContains(txbUsuario, popupUsuario, listboxUsuario, listUser, 1);

        }

        private void listboxDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteBoxUserControl.SelectIndex(txbDep, popupDep, listboxDep, 0);
        }

        private void txbDep_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(txbDep.Text.Length > 0)
            {
                lblMarcaDep.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblMarcaDep.Visibility = Visibility.Visible;
            }

            AutoCompleteBoxUserControl.SelectContains(txbDep, popupDep, listboxDep, listDep, 0);

        }

        #endregion
    }
}
