using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Simple.CodeParser.Syntax;

/// <summary>
///     Syntax Token Extensions
/// </summary>
public static class SyntaxTokenExtensions {
  /// <summary>
  ///     Parents (the token is excluded)
  /// </summary>
  /// <param name="token">token to obtain all parents for</param>
  /// <returns>Enumeration of Parents</returns>
  public static IEnumerable<SyntaxNode> Parents(this SyntaxToken token) {
    for (SyntaxNode? node = token.Parent; node is not null; node = node.Parent)
      yield return node;
  }

  /// <summary>
  ///     Identifier if exists
  /// </summary>
  /// <param name="token">Declaration Syntax</param>
  /// <returns>Identifier, null if declarationSyntax doesn't provide</returns>
  public static SyntaxToken? TryGetIdentifier(this SyntaxToken token) {
    var prop = token
        .GetType()
        .GetProperty("Identifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    if (prop is null)
      return null;

    if (!prop.CanRead)
      return null;

    return prop.GetValue(token) is SyntaxToken result
        ? result
        : null;
  }
}

