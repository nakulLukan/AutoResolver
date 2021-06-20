namespace AutoResolverTest
{
    public class ResolveAsTransient : IResolveAsTransient
    {
        public string GetMessage()
        {
            return "This class has been resolved successfully";
        }
    }
}
