namespace envio_correos_batch.Model
{
    public class ServerMailModel
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public ServerMailModel()
        {
            User = "";
            Password = "";
            Host = "smtp.outlook.com";
        }
    }
}
