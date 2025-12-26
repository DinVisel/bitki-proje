namespace Bitki.Core.Interfaces.Services
{
    /// <summary>
    /// Interface for caching operations
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get a cached value by key
        /// </summary>
        T? Get<T>(string key);

        /// <summary>
        /// Set a cached value with optional expiration
        /// </summary>
        void Set<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Remove a cached value by key
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// Get or create a cached value
        /// </summary>
        T GetOrCreate<T>(string key, Func<T> factory, TimeSpan? expiration = null);
    }
}
