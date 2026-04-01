// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

/// <summary>
/// Indicates that a member is required and must be initialized during object initialization.
/// </summary>
/// <remarks>
/// This attribute is used to support scenarios where the `required` keyword is not natively available
/// or to enforce initialization rules for specific members in a class or struct. It signals to compilers
/// or tools that initialization of the attributed member is mandatory.
/// See https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
internal sealed class RequiredMemberAttribute : Attribute {}

/// <summary>
/// Identifies compiler features that are required for the usage of the attributed member, type, or resource.
/// </summary>
/// <remarks>
/// This attribute is primarily used to signal to compilers that specific features must be supported
/// for the correct use of the annotated element. This can include compiler-specific features or
/// language constructs that may not be supported universally across all compilers or runtime versions.
/// </remarks>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute {
    /// <summary>
    /// Gets the name of the compiler feature required for the usage of the attributed member, type, or resource.
    /// </summary>
    /// <remarks>
    /// This property contains the specific feature name that a compiler or runtime must support
    /// to ensure the correct use of the annotated element. The feature name is typically set during initialization
    /// and conveys the intended functionality or capability that is being relied upon.
    /// </remarks>
    public string FeatureName { get; } = featureName;

    /// <summary>
    /// Indicates whether the compiler feature specified by the attribute is optional for the correct use
    /// of the attributed member, type, or resource.
    /// </summary>
    /// <remarks>
    /// When set to <c>true</c>, this property informs the compiler that support for the associated feature
    /// is not strictly required, and the code may still function correctly even if the feature is unavailable.
    /// Conversely, when set to <c>false</c>, it signifies that the feature is mandatory and its absence
    /// may lead to compilation errors or runtime issues.
    /// </remarks>
    public bool IsOptional { get; init; }
}
