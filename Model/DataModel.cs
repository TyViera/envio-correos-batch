namespace envio_correos_batch.Model
{
    public class DataModel
    {
        public string FilePath { get; set; }
        public ServerMailModel ServerMail { get; set; }
        public FileRoutesModel FileRoutes { get; set; }
        public DataModel() : this("", new ServerMailModel(), new FileRoutesModel())
        {
        }
        public DataModel(string filePath, ServerMailModel serverMail, FileRoutesModel fileRoutes)
        {
            FilePath = filePath;
            ServerMail = serverMail;
            FileRoutes = fileRoutes;
        }
    }
}
