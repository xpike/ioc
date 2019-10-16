using System;
using System.Collections.Generic;

namespace XPike.IoC
{
    /// <summary>
    /// A DependencyVerificationException is thrown when the container fails to verify.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "We want the class to encapsulate the message and not allow consumers to pass in the message.")]
    [Serializable]
    public class DependencyVerificationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyVerificationException"/> class.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        public DependencyVerificationException(Exception[] exceptions)
            : base ("The container failed to validate all required dependencies. You are likely missing a registration. See the Exceptions collection for details.")
        {
            Exceptions = exceptions;
        }

        /// <summary>
        /// Gets a list of exceptions that occurred during verification.
        /// </summary>
        /// <value>An array of exceptions.</value>
        /// <remarks>
        /// Different containers will throw different exceptions. 
        /// Don't assume these exceptions will be the same across DI implementations.
        /// DependencyVerificationException is the abstraction.</remarks>
        public IEnumerable<Exception> Exceptions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyVerificationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected DependencyVerificationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            :base(serializationInfo, streamingContext)
        {
            
        }
    }
}
