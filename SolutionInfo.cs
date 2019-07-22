// --------------------------------------------------------------------------------------------------------------------
// <copyright company="o.s.i.s.a. GmbH" file="SolutionInfo.cs">
//    (c) 2014. See licence text in binary folder.
// </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Security;

//[assembly: AssemblyVersion("1.0.0.1")]
//[assembly: AssemblyFileVersion("1.0.0.1")]

[assembly: SecurityRules(SecurityRuleSet.Level1)]

internal static class SolutionInfo
{
    #region Constants

    /// <summary>
    ///     Solution wide company name for the assemblyInfo files.
    /// </summary>
    public const string Company = "open source";

    /// <summary>
    ///     Solution wide copyright description for the assemblyInfo files.
    /// </summary>
    public const string Copyright = "no copyrights";

    /// <summary>
    ///     Solution wide product description for the assemblyInfo files.
    /// </summary>
    public const string Product = "mi Nationalrot";

    /// <summary>
    ///     Solution wide trademark description for the assemblyInfo files.
    /// </summary>
    public const string Trademark = "miNationalrot";

    #endregion
}