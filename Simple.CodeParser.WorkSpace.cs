using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace Simple.CodeParser;

/// <summary>
///     Workspace to start working with
/// </summary>
public static class WorkSpace {
  #region Algorithm

  private static void CoreLoad() {
    if (MSBuildLocator.IsRegistered)
      return;

    var studio = MSBuildLocator
        .QueryVisualStudioInstances()
        .Where(instance => instance.Name.Contains(".NET Core SDK"))
        .MaxBy(instance => instance.Version);

    MSBuildLocator.RegisterInstance(studio);

    VisualStudio = studio;
  }

  #endregion Algorithm

  #region Create

  static WorkSpace() {
    CoreLoad();

    BuildWorkSpace = MSBuildWorkspace.Create();
  }

  #endregion Create

  #region Public

  /// <summary>
  /// Visual Studio
  /// </summary>
  public static VisualStudioInstance? VisualStudio { get; private set; }

  public static MSBuildWorkspace BuildWorkSpace { get; }

  /// <summary>
  /// Open Solution
  /// </summary>
  /// <param name="solutionFileName">Solution File Name (.sln)</param>
  /// <returns>Solution Opened</returns>
  public static async Task<Solution> OpenSolutionAsync(string solutionFileName) =>
    await BuildWorkSpace.OpenSolutionAsync(solutionFileName);

  /// <summary>
  ///     Parse File
  /// </summary>
  /// <param name="filePath">File to Parse</param>
  /// <returns>Parsed File</returns>
  /// <exception cref="ArgumentNullException">When filePath is null</exception>
  public static SyntaxNode ParseFile(string filePath) => filePath is not null
    ? CSharpSyntaxTree.ParseText(File.ReadAllText(filePath)).GetRoot()
    : throw new ArgumentNullException(nameof(filePath));

  /// <summary>
  ///     Parse File
  /// </summary>
  /// <param name="filePath">File to Parse</param>
  /// <returns>Parsed File</returns>
  /// <exception cref="ArgumentNullException">When filePath is null</exception>
  public static async Task<SyntaxNode> ParseFileAsync(string filePath) {
    if (filePath is null)
      throw new ArgumentNullException(nameof(filePath));

    string text = await File.ReadAllTextAsync(filePath);

    return await Task.Run(() => CSharpSyntaxTree.ParseText(text).GetRoot());
  }

  /// <summary>
  ///     Parse File
  /// </summary>
  /// <param name="text">Text to Parse</param>
  /// <returns>Parsed File</returns>
  /// <exception cref="ArgumentNullException">When text is null</exception>
  public static SyntaxNode ParseText(string text) {
    if (text is null)
      throw new ArgumentNullException(nameof(text));

    return CSharpSyntaxTree.ParseText(text).GetRoot();
  }

  /// <summary>
  ///     Parse File
  /// </summary>
  /// <param name="text">Text to Parse</param>
  /// <returns>Parsed File</returns>
  /// <exception cref="ArgumentNullException">When text is null</exception>
  public static Task<SyntaxNode> ParseTextAsync(string text) {
    if (text is null)
      throw new ArgumentNullException(nameof(text));

    return Task.Run(() => CSharpSyntaxTree.ParseText(text).GetRoot());
  }

  #endregion Public
}

