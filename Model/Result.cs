namespace envio_correos_batch.Model
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public Result(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public static Result OfError(string message)
        {
            return new Result(message, false);
        }
        public static Result OfSuccess(string message)
        {
            return new Result(message, true);
        }
    }
}
