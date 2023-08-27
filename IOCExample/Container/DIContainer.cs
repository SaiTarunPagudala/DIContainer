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
            var registrationName = (typeof(T).FullName) + name;
            var registrationNamesSet = new HashSet<string>() { registrationName };
            var registration = _dependenciesDictionary[registrationName];
            if (registration.LifeofRegister == LifeTime.Transient)
            {
                return (T)Load(registration, registrationNamesSet, true);
            }
            return (T)registration.CurrentObject;
        }

        public List<Type> GetDependencies<T>(string name = "")
        {
            var registrationName = (typeof(T).FullName) + name;
            return _dependenciesDictionary[registrationName].ListOfDependencies;
        }

        public void LoadAll()
        {
            foreach (var dict in _dependenciesDictionary)
            {
                var set = new HashSet<string>() { dict.Key };
                var registration = dict.Value;
                if (registration.LifeofRegister == LifeTime.Singleton)
                {
                    registration.CurrentObject = Load(registration, set);
                    _dependenciesDictionary[dict.Key] = registration;
                }
                set = new HashSet<string>() { dict.Key };
                registration.ListOfDependencies = LoadDependencies(registration, set);
            }
        }

        #region private methods
        private Object Load(Registration registration, HashSet<string> registrationNamesSet, bool isTransient = false)
        {
            if (!isTransient && registration.CurrentObject != null)
            {
                return registration.CurrentObject;
            }
            var constructors = registration.Implementation.GetConstructors()[0];
            var parameters = constructors.GetParameters();
            var newparameters = new List<Object>();
            foreach (var param in parameters)
            {
                var type = param.ParameterType;
                if (registrationNamesSet.Contains(type.FullName))
                {
                    throw new Exception("Fail to Load due to Ciruclar dependency");
                }
                registrationNamesSet.Add(type.FullName);
                if (_dependenciesDictionary.ContainsKey(type.FullName))
                {
                    var paramobj = Load(_dependenciesDictionary[type.FullName], registrationNamesSet, isTransient);
                    newparameters.Add(paramobj);
                }
            }
            if (newparameters.Count > 0)
            {
                return constructors.Invoke(newparameters.ToArray());
            }
            return Activator.CreateInstance(registration.Implementation);
        }

        private List<Type> LoadDependencies(Registration registration, HashSet<string> registrationNamesSet)
        {
            if (registration.ListOfDependencies.Count > 0)
            {
                return registration.ListOfDependencies;
            }
            var constructors = registration.Implementation.GetConstructors()[0];
            var parameters = constructors.GetParameters();
            var newdependencies = new List<Type>();
            foreach (var param in parameters)
            {
                var type = param.ParameterType;
                if (registrationNamesSet.Contains(type.FullName))
                {
                    throw new Exception("Fail to Load due to Ciruclar dependency");
                }
                registrationNamesSet.Add(type.FullName);
                if (_dependenciesDictionary.ContainsKey(type.FullName))
                {
                    var paramobj = LoadDependencies(_dependenciesDictionary[type.FullName], registrationNamesSet);
                    newdependencies.Add(type);
                    newdependencies.AddRange(paramobj);
                }
            }
            return newdependencies;
        }
        #endregion

    }
}
