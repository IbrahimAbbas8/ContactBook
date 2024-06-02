namespace ContactBook.API.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageNumber, int pageSize, IList<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IList<T> Data { get; set; }
    }
}
