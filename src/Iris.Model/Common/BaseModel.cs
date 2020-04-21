namespace Iris.Models.Common
{
    public class ListBase
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; }
    }
}
