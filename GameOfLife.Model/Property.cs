namespace GameOfLife.Model
{
    public class Property
    {
        public PropertyState State { get; set; }
        public PropertyAddress Address { get; set; }
    }

    public enum PropertyState
    {
        Populated,
        Empty
    }

    public class PropertyAddress
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}