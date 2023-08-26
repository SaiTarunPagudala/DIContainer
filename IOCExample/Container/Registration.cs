using IOCExample.Enums;

namespace IOCExample.Container
{
    public class Registration
    {
        public Type Definition { get; private set; }
        public Type Implementation { get; private set; }

        public LifeTime LifeofRegister { get; private set; }

        public Object CurrentObject { get; set; }

        public Registration(Type Definition, Type Implementation, LifeTime LifeofRegister, object CurrentObject = null)
        {
            this.Definition = Definition;
            this.Implementation = Implementation;
            this.LifeofRegister = LifeofRegister;
            this.CurrentObject = CurrentObject;
        }
    }
}