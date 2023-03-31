using Microsoft.CodeAnalysis;

namespace Simple.CodeParser.Structure;

/// <summary>
///     Extensions over Solution class
/// </summary>
public static class SolutionExtensions {
  #region Public

  /// <summary>
  ///     All Project References except Nuget
  /// </summary>
  /// <param name="solution"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  public static IEnumerable<ProjectReference> ProjectReferences(this Solution? solution) {
    if (solution is null)
      throw new ArgumentNullException(nameof(solution));

    foreach (Project project in solution.Projects)
      foreach (var item in project.ProjectReferences)
        yield return item;
  }

  /// <summary>
  ///     All Projects' Source Files
  /// </summary>
  /// <param name="solution"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  public static IEnumerable<Document> SourceFiles(this Solution? solution) {
    if (solution is null)
      throw new ArgumentNullException(nameof(solution));

    foreach (Project project in solution.Projects)
      foreach (var item in project.Documents)
        yield return item;
  }

  /// <summary>
  ///     Project Dependencies (except Nuget)
  /// </summary>
  public static IEnumerable<ProjectId> Dependencies(this Solution solution) {
    if (solution is null)
      throw new ArgumentNullException(nameof(solution));

    var graph = solution.GetProjectDependencyGraph();

    return graph.GetTopologicallySortedProjects();
  }

  /// <summary>
  ///     Solution Parsed
  /// </summary>
  /// <param name="solution"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  public static IEnumerable<(Project Project, string SourceFile, SyntaxNode Root)> ParseSourceFiles(this Solution solution) {
    if (solution is null)
      throw new ArgumentNullException(nameof(solution));

    return solution
        .Projects
        .SelectMany(project => project
            .ParseSourceFiles()
            .Select(item => (project, item.SourceFile, item.Root)));
  }

  #endregion Public
}