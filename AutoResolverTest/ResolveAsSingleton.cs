using AutoResolver;

namespace AutoResolverTest
{
    public class ResolveAsSingleton : IResolveSingleton
    {
        public string SingletonValue => "Singleton value";
    }
}
