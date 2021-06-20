using AutoResolver;

namespace AutoResolverTest
{
    public interface IResolveAsTransient : IResolveTransient
    {
        string GetMessage();
    }
}
