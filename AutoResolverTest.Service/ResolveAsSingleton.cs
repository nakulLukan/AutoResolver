using AutoResolver;

namespace AutoResolverTest.Service
{
    public class ResolveAsSingleton : IResolveSingleton
    {
        public string SingletonValue => "This message is from a class which implements 'IResolveSingleton'";
    }
}
