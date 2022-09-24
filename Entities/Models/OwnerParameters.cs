
namespace Entities.Models
{
    public class OwnerParameters : QueryStringParameters
    {
        public OwnerParameters()
        {
            OrderBy = "Name";
        }
        public uint MinYearOfBirth { get; set; }
        public uint MaxYearOfBirth { get; set; } = (uint)DateTime.Now.Year;
        public bool isValidDate() => MaxYearOfBirth > MinYearOfBirth;
        public string Name { get; set; } = "";

    }
}
