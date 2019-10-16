namespace XPike.IoC
{
    /// <summary>
    /// Implemented by dependency packages
    /// </summary>
    public interface IDependencyPackage
    {
        /// <summary>
        /// Registers the package with the provided <see cref="IDependencyCollection"/>.
        /// </summary>
        /// <param name="dependencyCollection">The dependency collection in which to perform the registrations.</param>
        void RegisterPackage(IDependencyCollection dependencyCollection);
    }
}