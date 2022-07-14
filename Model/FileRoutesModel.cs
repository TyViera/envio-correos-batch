namespace envio_correos_batch.Model
{
    public class FileRoutesModel
    {
        public string? XmlRoute { get; set; }
        public string? PdfRoute { get; set; }
        public FileRoutesModel()
        {
            XmlRoute = "";
            PdfRoute = "";
        }
    }
}
