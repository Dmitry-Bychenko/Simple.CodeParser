using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Simple.CodeParser.Syntax;

public static class SyntaxNodeExtensions {
  /// <summary>
  ///     Parents (the token is excluded)
  /// </summary>
  /// <param name="source">Node to obtain all parents for</param>
  /// <param name="includeSelf">Should we include self into parents</param>
  /// <returns>Enumeration of Parents</returns>
  /// <exception cref="ArgumentNullException">When source is null</exception>
  public static IEnumerable<SyntaxNode> Parents(this SyntaxNode? source, bool includeSelf = false) {
    if (source is null)
      throw new ArgumentNullException(nameof(source));

    if (includeSelf)
      yield return source;

    for (SyntaxNode? node = source.Parent; node is not null; node = node.Parent)
      yield return node;
  }

  /// <summary>
  ///     Identifier if exists
  /// </summary>
  /// <param name="node">Declaration Syntax</param>
  /// <returns>Identifier, null if declarationSyntax doesn't provide</returns>
  public static SyntaxToken? TryGetIdentifier(this SyntaxNode? node) {
    if (node is null)
      return null;

    var prop = node
        .GetType()
        .GetProperty("Identifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    if (prop is null)
      return null;

    if (!prop.CanRead)
      return null;

    return prop.GetValue(node) is SyntaxToken result
        ? result
        : null;
  }
}
