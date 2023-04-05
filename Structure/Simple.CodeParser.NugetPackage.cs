using System.Text.RegularExpressions;

namespace Simple.CodeParser.Structure {

  /// <summary>
  /// Nuget package
  /// </summary>
  public sealed partial class NugetPackage : IEquatable<NugetPackage>, IComparable<NugetPackage> {
    #region Private Data

    [GeneratedRegex(pattern: @"^(?<prefix>.*)(?<version>[0-9]{1,9}(?:\.[0-9]{1,9}){1,3})(?<suffix>.*)$")]
    private static partial Regex VersionRegex();

    #endregion Private Data

    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="version">Version</param>
    /// <exception cref="ArgumentNullException">When Name is null</exception>
    public NugetPackage(string name, string version) {
      Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));

      if (string.IsNullOrWhiteSpace(version))
        Version = new Version();
      else {
        var match = VersionRegex().Match(version.Trim());

        if (Version.TryParse(match.Groups["version"].Value, out var ver))
          Version = ver;
        else
          Version = new Version();

        VersionPrefix = match.Groups["prefix"].Value;
        VersionSuffix = match.Groups["suffix"].Value;
      }
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

      result = left.Version.CompareTo(right.Version);

      if (result != 0)
        return result;

      result = StringComparer.OrdinalIgnoreCase.Compare(left.VersionPrefix, right.VersionPrefix);

      if (result != 0)
        return result;

      return StringComparer.OrdinalIgnoreCase.Compare(left.VersionSuffix, right.VersionSuffix);
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
    /// Version Prefix
    /// </summary>
    public string VersionPrefix { get; } = "";

    /// <summary>
    /// Version Suffix
    /// </summary>
    public string VersionSuffix { get; } = "";

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
