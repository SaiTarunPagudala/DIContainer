using IOCExample.Interfaces;

namespace IOCExample.Classes
{
    public class Class1 : ITemp1
    {
        public ITemp3 temp3Obj { get; set; }
        public string temp1Name { get; set; }

        public Class1(ITemp3 temp3Obj, string temp1Name)
        {
            this.temp3Obj = temp3Obj;
            this.temp1Name = temp1Name;
        }
    }
}
