using IOCExample.Interfaces;

namespace IOCExample.Classes
{
    public class Temp3 : ITemp3
    {
        public ITemp1 tempobj { get; set; }

        public Temp3(ITemp1 temp1, ITemp2 temp2)
        {
            tempobj = temp1;
        }
    }
}
