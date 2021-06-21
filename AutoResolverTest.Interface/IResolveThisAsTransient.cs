using AutoResolver;

namespace AutoResolverTest.Interface
{
    public interface IResolveThisAsTransient : IResolveTransient
    {
        string GetMessage();
    }
}
