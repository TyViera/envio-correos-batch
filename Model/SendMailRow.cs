using CsvHelper.Configuration.Attributes;

namespace envio_correos_batch.Model
{
    public class SendMailRow
    {
        [Name("correo_destino")]
        public string Email { get; set; }
        [Name("nombre_archivo_xml")]
        public string XmlFileName { get; set; }
        [Name("nombre_archivo_pdf")]
        public string PdfFileName { get; set; }
        [Name("mensaje")]
        public string Message { get; set; }
        
    }
}
