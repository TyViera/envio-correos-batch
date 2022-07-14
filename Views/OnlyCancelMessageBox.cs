namespace envio_correos_batch.Views
{
    public partial class OnlyCancelMessageBox : Form
    {
        public OnlyCancelMessageBox()
        {
            InitializeComponent();
        }

        private static OnlyCancelMessageBox _last;
        public static DialogResult Show(string message)
        {
            _last = new OnlyCancelMessageBox();
            _last.labelMessage.Text = message;
            return _last.ShowDialog();
        }

        public static void CloseLast()
        {
            if (_last != null)
            {
                if (_last.InvokeRequired)
                {
                    _last.Invoke(new MethodInvoker(delegate
                    {
                        _last.Close();
                    }));
                }
                else
                {
                    _last.Close();
                }
                _last = null;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
