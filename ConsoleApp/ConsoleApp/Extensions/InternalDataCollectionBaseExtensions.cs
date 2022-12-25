using System.Data;

namespace ConsoleApp.Extensions
{
    internal static class InternalDataCollectionBaseExtensions
    {
        public static T[] ToArray<T>(this InternalDataCollectionBase collection)
            where T : class
        {
            var array = new T[collection.Count];

            collection.CopyTo(array, 0);

            return array;
        }
    }
}