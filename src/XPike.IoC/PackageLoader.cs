using System;
using System.Collections.Generic;

namespace XPike.IoC
{
    /// <summary>
    /// IDependencyCollection Extensions.
    /// </summary>
    public static class IDependencyCollectionExtensions
    {
        private static readonly List<Type> _loadedPackages = new List<Type>();

        public static void ResetPackages() => _loadedPackages.Clear();

        public static IDependencyCollection ResetPackages(this IDependencyCollection dependencyCollection)
        {
            ResetPackages();
            return dependencyCollection;
        }

        /// <summary>
        /// Can be optionally used to reduce the overhead of repeatedly registering the same packages once they are loaded.
        /// </summary>
        /// <param name="dependencyCollection">This instance.</param>
        /// <param name="package">The package to load.</param>
        public static IDependencyCollection LoadPackage(this IDependencyCollection dependencyCollection, IDependencyPackage package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            if (_loadedPackages.Contains(package.GetType()))
                return dependencyCollection;

            _loadedPackages.Add(package.GetType());
            package.RegisterPackage(dependencyCollection);

            return dependencyCollection;
        }
    }
}