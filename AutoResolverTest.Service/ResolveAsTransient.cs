using AutoResolverTest.Interface;

namespace AutoResolverTest.Service
{
    public class ResolveAsTransient : IResolveThisAsTransient
    {
        public string GetMessage()
        {
            return "This message is from resolved service";
        }
    }
}
