using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.CodeParser.Structure {
  
  /// <summary>
  /// Nuget package
  /// </summary>
  public sealed class NugetPackage : IEquatable<NugetPackage>, IComparable<NugetPackage> {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="version">Version</param>
    /// <exception cref="ArgumentNullException">When Name is null</exception>
    public NugetPackage(string name, Version version) { 
      Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
      Version = version ?? new Version();
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Compare
    /// </summary>
    public static int Compare(NugetPackage? left, NugetPackage? right) {
      if (ReferenceEquals(left, right))
        return 0;
      if (left is null)
        return -1;
      if (right is null)
        return +1;

      int result = StringComparer.OrdinalIgnoreCase.Compare(left.Name, right.Name);

      if (result != 0)
        return result;

      return left.Version.CompareTo(right.Version);
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Version
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// To String
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
      $"{Name} {Version}";

    #endregion Public

    #region IEquatable<NugetPackage>

    /// <summary>
    /// Equals
    /// </summary>
    public bool Equals(NugetPackage? other) => Compare(this, other) == 0;

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(object? obj) => Equals(obj as NugetPackage);

    /// <summary>
    /// Hash Code
    /// </summary>
    public override int GetHashCode() => 
      HashCode.Combine(Name.GetHashCode(StringComparison.OrdinalIgnoreCase), Version);

    #endregion IEquatable<NugetPackage>

    #region IComparable<NugetPackage>

    /// <summary>
    /// Compare To
    /// </summary>
    public int CompareTo(NugetPackage? other) => Compare(this, other);

    #endregion IComparable<NugetPackage>
  }
}
