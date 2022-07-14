using envio_correos_batch.Model;

namespace envio_correos_batch.Views
{
    public partial class SettingsForm : Form
    {
        private readonly DataModel _dataModel;

        public DataModel DataModel
        {
            get { return _dataModel; }
        }


        public SettingsForm(DataModel dataModel)
        {
            _dataModel = dataModel;
            InitializeComponent();
            InitializeDataModel();
        }

        private void InitializeDataModel()
        {
            textBoxHost.Text = _dataModel.ServerMail?.Host;
            textBoxUser.Text = _dataModel.ServerMail?.User;
            textBoxPassword.Text = _dataModel.ServerMail?.Password;

            textBoxPdf.Text = _dataModel.FileRoutes?.PdfRoute;
            textBoxXml.Text = _dataModel.FileRoutes?.XmlRoute;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            RecoverDataFromForm();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void RecoverDataFromForm()
        {
            _dataModel.FileRoutes = new FileRoutesModel()
            {
                PdfRoute = textBoxPdf.Text,
                XmlRoute = textBoxXml.Text
            };
            _dataModel.ServerMail = new ServerMailModel()
            {
                User = textBoxUser.Text,
                Password = textBoxPassword.Text,
                Host = textBoxHost.Text
            };
        }

    }
}
