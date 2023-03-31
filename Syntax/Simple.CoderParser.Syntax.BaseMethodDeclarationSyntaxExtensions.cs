using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace Simple.CodeParser.Syntax;

/// <summary>
///     Base Method Declaration Extensions
/// </summary>
public static class BaseMethodDeclarationSyntaxExtensions {
  /// <summary>
  ///     Identifier if exists
  /// </summary>
  /// <param name="declarationSyntax">Declaration Syntax</param>
  /// <returns>Identifier, null if declarationSyntax doesn't provide</returns>
  public static SyntaxToken? Identifier(this BaseMethodDeclarationSyntax? declarationSyntax) {
    if (declarationSyntax is null)
      return null;

    var prop = declarationSyntax
        .GetType()
        .GetProperty("Identifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    if (prop is null)
      return null;

    if (!prop.CanRead)
      return null;

    return prop.GetValue(declarationSyntax) is SyntaxToken result
        ? result
        : null;
  }
}
