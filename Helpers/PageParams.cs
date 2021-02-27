namespace Estrutura.Web.API.Helpers
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string Filtro { get; set; } = string.Empty;

        public string Authorization { get; set; } = string.Empty;
    }
}