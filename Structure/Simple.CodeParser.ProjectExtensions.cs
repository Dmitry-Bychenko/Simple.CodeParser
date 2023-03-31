using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml;

namespace Simple.CodeParser.Structure;

/// <summary>
///     Project Extensions
/// </summary>
public static class ProjectExtensions {

  /// <summary>
  ///     Get all Nuget package from project
  /// </summary>
  /// <param name="project">Project to parse</param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException">When project is null</exception>
  /// <exception cref="ArgumentException">When project is in unknown format</exception>
  public static IEnumerable<(Project Project, NugetPackage Package)> NugetPackages(this Project? project) {
    if (project is null)
      throw new ArgumentNullException(nameof(project));

    if (project.FilePath is null)
      yield break;

    XmlNodeList? nodes;

    try {
      var csproj = new XmlDocument();
      csproj.Load(project.FilePath);
      nodes = csproj.SelectNodes("//PackageReference[@Include and @Version]");
    }
    catch (Exception e) {
      throw new ArgumentException($"{e}", nameof(project), e);
    }

    if (nodes is null)
      yield break;

    foreach (XmlNode packageReference in nodes) {
      var nameAttributes = packageReference?.Attributes?["Include"];
      var versionAttributes = packageReference?.Attributes?["Version"];

      if (nameAttributes is null || versionAttributes is null)
        continue;

      var packageName = nameAttributes.Value;

      var packageVersion = Version.TryParse(versionAttributes.Value, out var version)
        ? version
        : new Version();

      yield return (project, new NugetPackage(packageName, packageVersion));
    }
  }

  /// <summary>
  ///     Parse project into Syntax Nodes
  /// </summary>
  /// <param name="project">Project to parse</param>
  /// <returns>Project, Source Files and corresponding Syntax</returns>
  /// <exception cref="ArgumentNullException"></exception>
  public static IEnumerable<(Project Project, string SourceFile, SyntaxNode Root)> ParseSourceFiles(this Project project) {
    if (project is null)
      throw new ArgumentNullException(nameof(project));

    foreach (var sourceDocument in project.Documents) {
      if (sourceDocument.FilePath is null)
        continue;

      var root = CSharpSyntaxTree
          .ParseText(File.ReadAllText(sourceDocument.FilePath))
          .GetRoot();

      yield return (project, sourceDocument.FilePath, root);
    }
  }

  /// <summary>
  ///     Parse Project into parsed methods
  /// </summary>
  /// <param name="project">Project to parse</param>
  /// <returns> Project, Source Files and all parsed methods</returns>
  /// <exception cref="ArgumentNullException">When project is null</exception>
  public static IEnumerable<(Project Project, string SourceFile, BaseMethodDeclarationSyntax method)> ParseSourceCodes(this Project project) {
    if (project is null)
      throw new ArgumentNullException(nameof(project));

    foreach (var sourceDocument in project.Documents) {
      if (sourceDocument.FilePath is null)
        continue;

      var root = CSharpSyntaxTree
          .ParseText(File.ReadAllText(sourceDocument.FilePath))
          .GetRoot();

      foreach (var method in root.DescendantNodes().OfType<BaseMethodDeclarationSyntax>())
        yield return (project, sourceDocument.FilePath, method);
    }
  }

  /// <summary>
  ///     Parse Project into Syntax Nodes
  /// </summary>
  /// <typeparam name="T">Syntax Nodes Type to Parse into</typeparam>
  /// <param name="project">Project to Parse</param>
  /// <returns>Project, Source File, Syntax Nodes</returns>
  /// <exception cref="ArgumentNullException">When project is null</exception>
  public static IEnumerable<(Project Project, string SourceFile, T Node)> ParseSyntaxNodes<T>(this Project project) where T : SyntaxNode {
    if (project is null)
      throw new ArgumentNullException(nameof(project));

    foreach (var sourceDocument in project.Documents) {
      if (sourceDocument.FilePath is null)
        continue;

      var root = CSharpSyntaxTree
          .ParseText(File.ReadAllText(sourceDocument.FilePath))
          .GetRoot();

      foreach (var method in root.DescendantNodes().OfType<T>())
        yield return (project, sourceDocument.FilePath, method);
    }
  }
}

