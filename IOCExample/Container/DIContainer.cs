using IOCExample.Enums;

namespace IOCExample.Container
{
    public class DIContainer
    {
        private IDictionary<string, Registration> _dependenciesDictionary;

        public DIContainer()
        {
            _dependenciesDictionary = new Dictionary<string, Registration>();
        }

        public void Register<definitionObject, implementationObject>(LifeTime registerType, string name = "")
        {
            var definitionType = typeof(definitionObject);
            var implementationType = typeof(implementationObject);
            var registration = new Registration(definitionType, implementationType, registerType);
            _dependenciesDictionary.Add(definitionType.FullName + name, registration);
        }

        public T Resolve<T>(string name = "")
        {
            var registration = _dependenciesDictionary[(typeof(T).FullName) + name];
            if (registration.LifeofRegister == LifeTime.Transient)
            {
                return (T)Load(registration, true);
            }
            return (T)registration.CurrentObject;
        }

        public void LoadAll()
        {
            foreach (var dict in _dependenciesDictionary)
            {
                var registration = dict.Value;
                if (registration.LifeofRegister == LifeTime.Singleton)
                {
                    registration.CurrentObject = Load(registration);
                    _dependenciesDictionary[dict.Key] = registration;
                }
            }
        }

        #region private methods
        private Object Load(Registration registration, bool isTransient = false)
        {
            if (!isTransient && registration.CurrentObject!=null)
            {
                return registration.CurrentObject;
            }
            var constructors = registration.Implementation.GetConstructors()[0];
            var parameters = constructors.GetParameters();
            var newparameters = new List<Object>();
            foreach (var param in parameters)
            {
                var type = param.ParameterType;
                if (_dependenciesDictionary.ContainsKey(type.FullName))
                {
                    var paramobj = Load(_dependenciesDictionary[type.FullName], isTransient);
                    newparameters.Add(paramobj);
                }
                else
                {
                    newparameters.Add(Activator.CreateInstance(type));
                }
            }
            if (newparameters.Count > 0)
            {
                return constructors.Invoke(newparameters.ToArray());
            }
            return Activator.CreateInstance(registration.Implementation);
        }
        #endregion

    }
}
