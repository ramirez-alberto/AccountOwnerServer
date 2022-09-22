
namespace Entities.Models
{
    public class OwnerParameters
    {
        const int maxSize = 40;
        public int PageCount { get; set; }
        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value < maxSize) ? value : maxSize;
            }
        }


    }
}
