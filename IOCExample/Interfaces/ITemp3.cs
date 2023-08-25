namespace IOCExample.Interfaces
{
    public interface ITemp3
    {
        public string temp3Name { get; set; }
        public ITemp1 temp1obj { get; set; }

        public ITemp2 temp2obj { get; set; }
    }
}
