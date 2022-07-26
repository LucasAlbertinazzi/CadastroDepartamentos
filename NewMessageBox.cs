namespace Sistema.Utils
{
    public static class NewMessageBox
    {
        // Resposta: Indica a ação:
        // Botão SIM: true
        // Botão NÃO: false
        public static bool Resposta { get; set; }

        // Opcao: Indica a ação:
        // Escolha de opcao: 1,2,3 ....
        public static int Opcao { get; set; }

        // Texto: Recebe o texto a ser exibido.
        // Tipo:
        // Tipo 1: - Pergunta
        // Tipo 2: - Aviso
        public static void AbreWpf(string Texto, int Tipo)
        {
            WpfDialogResult wpf = new WpfDialogResult(Texto, Tipo);
            wpf.ShowDialog();
        }

        public static void OpcaoAbreWpf(string Texto, string OpUm, string OpDois, int Tipo)
        {
            WpfDialogResult wpf = new WpfDialogResult(Texto, OpUm,OpDois, Tipo);
            wpf.ShowDialog();
        }
    }
}
