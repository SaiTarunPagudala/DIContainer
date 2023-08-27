using IOCExample.Interfaces;

namespace IOCExample.Classes
{
    public class Class3 : ITemp3
    {

        public ITemp1 temp1obj { get; set; }

        public ITemp2 temp2obj { get; set; }

        public string temp3Name { get; set; }

        public Class3(ITemp1 temp1, ITemp2 temp2)
        {
            temp1obj = temp1;
            temp2obj = temp2;
        }
    }
}
