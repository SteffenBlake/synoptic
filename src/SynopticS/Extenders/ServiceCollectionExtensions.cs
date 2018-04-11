using Microsoft.Extensions.DependencyInjection;

namespace SynopticS
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection Clone(this IServiceCollection inCollection)
        {
            IServiceCollection outCollection = new ServiceCollection();

            foreach (var service in inCollection)
            {
                outCollection.Add(service);
            }

            return outCollection;
        }
    }
}
