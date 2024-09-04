

namespace ecopriv.Models
{
    public class Sum
    {
        public string Type { get; set; }
        public int Value { get; set; }
        public string Assignment { get; set; }

        public Sum(string type, int value, string assignment)
        {
            Type = type;
            Value = value;
            Assignment = assignment;
        }

        public Sum() { }

    }
}
