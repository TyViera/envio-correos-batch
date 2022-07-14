using envio_correos_batch.Model;
using envio_correos_batch.Services;

namespace envio_correos_batch.Views
{
    public partial class MainForm : Form
    {

        private DataModel _dataModel;

        public MainForm()
        {
            InitializeComponent();
            _dataModel = new DataModel();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            var fileResult = openFileToProcess.ShowDialog();
            if (fileResult == DialogResult.OK)
            {
                textBoxFileName.Text = openFileToProcess.FileName;
                _dataModel.FilePath = openFileToProcess.FileName;
                LogLoadFileEvent();
            }
            else
            {
                LogEvent("Carga de archivo cancelada");
            }
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            var errorMessage = ValidateProcessData();
            if (errorMessage != string.Empty)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //start processing
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                Start(cancellationTokenSource);
            });
            //task.Start();
            var t = OnlyCancelMessageBox.Show("Procesando");
            if (t == DialogResult.No)
            {
                cancellationTokenSource.Cancel();
                LogEvent("Proceso cancelado por el usuario");
            }

        }

        private void Start(CancellationTokenSource cancellationTokenSource)
        {
            try
            {

                var result = ProcessFile(cancellationTokenSource);
                OnlyCancelMessageBox.CloseLast();
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (!string.IsNullOrEmpty(result.Message))
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                OnlyCancelMessageBox.CloseLast();
                LogEvent(e.Message);
                MessageBox.Show("Ocurrió un error al procesar el archivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Result ProcessFile(CancellationTokenSource cancellationTokenSource)
        {
            LogEvent("Iniciando proceso...");
            ConnectToMailServer();
            var isFielValid = ValidateFile();

            if (!isFielValid)
            {
                return Result.OfError("Ocurrió un error al leer el archivo.");
            }
            var success = 0;
            var errors = 0;
            var rows = FileService.ReadRowsFromFile(_dataModel.FilePath).ToList();
            foreach (var row in rows)
            {
                CancellationToken token = cancellationTokenSource.Token;
                if (token.IsCancellationRequested)
                {
                    return Result.OfError(string.Empty);
                }
                LogReadLineEvent(row.Index);
                if (SendMail(row))
                {
                    success++;
                }
                else
                {
                    errors++;
                }
            }
            if (success == 0)
            {
                return Result.OfError("No se procesó ningún registro");
            }
            var messageTemplate = "Total de registros procesados: {0} " + Environment.NewLine +
                "Total de correos enviados: {1} " + Environment.NewLine +
                "Total de correos fallido: {2}";
            var message = string.Format(messageTemplate, (success + errors), success, errors);
            return Result.OfSuccess(message);
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm(_dataModel);
            var result = settingsForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                _dataModel = settingsForm.DataModel;
                LogEvent("¡Configuración actualizada!");
            }
        }

        private string ValidateProcessData()
        {
            if (string.IsNullOrEmpty(_dataModel.FilePath))
            {
                return "Debe seleccionar un archivo para procesar";
            }
            if (string.IsNullOrEmpty(_dataModel.ServerMail?.Host))
            {
                return "No se ha configurado el Host de origen del correo. Ej: smtp.google.com";
            }
            if (string.IsNullOrEmpty(_dataModel.ServerMail?.User))
            {
                return "No se ha configurado el usuario de la cuenta de correo.";
            }
            if (string.IsNullOrEmpty(_dataModel.ServerMail?.Password))
            {
                return "No se ha configurado la contraseña de la cuenta de correo.";
            }
            if (string.IsNullOrEmpty(_dataModel.FileRoutes?.XmlRoute))
            {
                return "No se ha configurado la ruta de los archivos xml.";
            }
            if (string.IsNullOrEmpty(_dataModel.FileRoutes?.PdfRoute))
            {
                return "No se ha configurado la ruta de los archivos pdf.";
            }
            return string.Empty;
        }

        private void LogLoadFileEvent()
        {
            var eventText = string.Format("Se ha cargado el archivo {0}", openFileToProcess.FileName);
            LogEvent(eventText);
        }

        private void LogReadLineEvent(int index)
        {
            var eventText = string.Format("Registro #{0}: Leído", index);
            LogEvent(eventText);
        }

        private void LogSendingMailEvent(string email)
        {
            var eventText = string.Format("Sending mail to {0}", email);
            LogEvent(eventText);
        }
        private void LogSuccessEvent(int index)
        {
            var eventText = string.Format("Registro #{0}: Correo enviado correctamente.", index);
            LogEvent(eventText);
        }
        private void LogErrorEvent(int index)
        {
            var eventText = string.Format("Registro #{0}: Ocurrió un error al enviar el correo.", index);
            LogEvent(eventText);
        }

        private void LogEvent(string eventText)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.Invoke(new MethodInvoker(delegate
                {
                    textBoxLog.Text += eventText + Environment.NewLine;
                }));
            }
            else
            {
                textBoxLog.Text += eventText + Environment.NewLine;
            }
        }

        private bool ValidateFile()
        {
            try
            {
                return FileService.IsValidCsvFile(_dataModel.FilePath);
            }
            catch (Exception e)
            {
                LogEvent(e.Message);
                return false;
            }
        }

        private bool ConnectToMailServer()
        {
            try
            {
                MailService.ConnectToServer(_dataModel.ServerMail);
                return true;
            }
            catch (Exception e)
            {
                LogEvent(e.Message);
                return false;
            }
        }

        private bool SendMail(CsvData<SendMailRow> row)
        {
            try
            {
                LogSendingMailEvent(row.Data.Email);
                MailService.SendMail(row.Data, _dataModel);
                LogSuccessEvent(row.Index);
                return true;
            }
            catch (Exception e)
            {
                LogErrorEvent(row.Index);
                LogEvent(e.Message);
                return false;
            }

        }
    }
}