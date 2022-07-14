namespace envio_correos_batch.Model
{
    public class CsvData<E>
    {
        public E Data { get; set; }
        public int Index { get; set; }
        public CsvData(E data, int index)
        {
            Data = data;
            Index = index;
        }
    }
}
