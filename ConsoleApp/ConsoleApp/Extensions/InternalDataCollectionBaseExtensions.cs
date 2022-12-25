using System.Data;

namespace ConsoleApp.Extensions
{
    internal static class InternalDataCollectionBaseExtensions
    {
        internal static T[] ToArray<T>(this InternalDataCollectionBase collection)
        {
            var array = new T[collection.Count];

            collection.CopyTo(array, 0);

            return array;
        }
    }
}