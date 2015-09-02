// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
              
using System.Collections.Immutable;
using System.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using System.Globalization.Analyzers.Common;

namespace System.Globalization.Analyzers
{
    public abstract class CA1304DiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string RuleId1304 = "CA1304";
        public const string RuleId1305 = "CA1305";
        public const string RuleId1307 = "CA1307";
        public const string RuleId1309 = "CA1309";          

        internal static readonly DiagnosticDescriptor SpecifyCultureInfoRule = CreateDiagnosticDescriptor(RuleId1304,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoDiagnosis)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoDescription)));

        internal static readonly DiagnosticDescriptor SpecifyIFormatProviderAlternateStringRule = CreateDiagnosticDescriptor(RuleId1305,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternateString)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDescription)));

        internal static readonly DiagnosticDescriptor SpecifyIFormatProviderAlternateRule = CreateDiagnosticDescriptor(RuleId1305,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDescription)));

        internal static readonly DiagnosticDescriptor SpecifyIFormatProviderUICultureStringRule = CreateDiagnosticDescriptor(RuleId1305,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisUICultureString)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDescription)));

        internal static readonly DiagnosticDescriptor SpecifyIFormatProviderUICultureRule = CreateDiagnosticDescriptor(RuleId1305,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisUICulture)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDescription)));

        internal static readonly DiagnosticDescriptor SpecifyStringComparisonRule = CreateDiagnosticDescriptor(RuleId1307,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyStringComparisonTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyStringComparisonDiagnosis)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.SpecifyStringComparisonDescription)));

        internal static readonly DiagnosticDescriptor UseOrdinalStringComparisonRule = CreateDiagnosticDescriptor(RuleId1309,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDiagnosis)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDescription)));

        internal static readonly DiagnosticDescriptor UseOrdinalStringComparerRule = CreateDiagnosticDescriptor(RuleId1309,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparerDiagnosis)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDescription)));

        internal static readonly DiagnosticDescriptor UseOrdinalStringComparisonAsDefaultRule = CreateDiagnosticDescriptor(RuleId1309,
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonTitle)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDefaultDiagnosis)),
                                                                                                            DiagnosticHelpers.GetLocalizableResourceString(nameof(SystemGlobalizationAnalyzersResources.UseOrdinalStringComparisonDescription)));

        private static readonly ImmutableArray<DiagnosticDescriptor> s_supportedDiagnostics =
            ImmutableArray.Create(SpecifyCultureInfoRule,
                                  SpecifyIFormatProviderAlternateRule,
                                  SpecifyIFormatProviderAlternateStringRule,
                                  SpecifyIFormatProviderUICultureRule,
                                  SpecifyIFormatProviderUICultureStringRule,
                                  SpecifyStringComparisonRule,
                                  UseOrdinalStringComparerRule,
                                  UseOrdinalStringComparisonRule,
                                  UseOrdinalStringComparisonAsDefaultRule);

        public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => s_supportedDiagnostics;   

        public sealed override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(PrepareAnalyzer);
        }

        private static DiagnosticDescriptor CreateDiagnosticDescriptor(string ruleId, LocalizableString title, LocalizableString message, LocalizableString description, string uri = null)
        {
            return new DiagnosticDescriptor(ruleId,
                                            title,
                                            message,
                                            DiagnosticCategory.Globalization,
                                            DiagnosticSeverity.Warning,
                                            isEnabledByDefault: true,
                                            description: description,
                                            helpLinkUri: uri,
                                            customTags: WellKnownDiagnosticTags.Telemetry);
        }

        protected abstract void PrepareAnalyzer(CompilationStartAnalysisContext context);

        private const string ActivatorTypeName = "System.Activator";
        private const string BasicDateTypeTypeName = "Microsoft.VisualBasic.CompilerServices.DateType";
        private const string CollectionsComparerTypeName = "System.Collections.Comparer";
        private const string CollectionsCaseInsensitiveComparerTypeName = "System.Collections.CaseInsensitiveComparer";
        private const string CollectionsIComparerTypeName = "System.Collections.IComparer";
        private const string ComponentResourceManagerTypeName = "System.ComponentModel.ComponentResourceManager";
        private const string CultureInfoTypeName = "System.Globalization.CultureInfo";
        private const string GuidTypeName = "System.Guid";
        private const string IFormatProviderTypeName = "System.IFormatProvider";
        private const string ResourceManagerTypeName = "System.Resources.ResourceManager";
        private const string StringComparisonTypeName = "System.StringComparison";
        private const string StringComparerTypeName = "System.StringComparer";

        private const string ActivatorCreateInstanceMethodName = "CreateInstance";
        private const string ApplyResourcesMethodName = "ApplyResources";
        private const string ResourceManagerGetObjectMethodName = "GetObject";
        private const string ResourceManagerGetStringMethodName = "GetString";
        private const string ResourceManagerGetStreamMethodName = "GetStream";

        protected sealed class AnalysisSession
        {
            internal readonly INamedTypeSymbol ActivatorTypeSym;
            internal readonly INamedTypeSymbol CultureInfoTypeSym;
            internal readonly INamedTypeSymbol ComponentResourceManagerTypeSym;
            internal readonly INamedTypeSymbol ResourceManagerTypeSym;
            internal readonly INamedTypeSymbol BasicDateTypeTypeSym;
            internal readonly INamedTypeSymbol StringComparisonTypeSym;
            internal readonly INamedTypeSymbol StringComparerTypeSym;
            internal readonly INamedTypeSymbol CollectionsComparerTypeSym;
            internal readonly INamedTypeSymbol CollectionsCaseInsensitiveComparerTypeSym;
            internal readonly INamedTypeSymbol CollectionsIComparerTypeSym;
            internal readonly INamedTypeSymbol GuidTypeSym;
            internal readonly INamedTypeSymbol IFormatProviderTypeSym;

            internal readonly SyntaxNodeHelper SymbolHelper;

#pragma warning disable RS1012 // Start action has no registered actions.
            public AnalysisSession(CompilationStartAnalysisContext context, SyntaxNodeHelper symbolHelper)
#pragma warning restore RS1012 // Start action has no registered actions.
            {
                SymbolHelper = symbolHelper;

                Compilation compilation = context.Compilation;
                ActivatorTypeSym = compilation.GetTypeByMetadataName(ActivatorTypeName);
                GuidTypeSym = compilation.GetTypeByMetadataName(GuidTypeName);
                StringComparerTypeSym = compilation.GetTypeByMetadataName(StringComparerTypeName);
                CollectionsComparerTypeSym = compilation.GetTypeByMetadataName(CollectionsComparerTypeName);
                CollectionsIComparerTypeSym = compilation.GetTypeByMetadataName(CollectionsIComparerTypeName);
                CollectionsCaseInsensitiveComparerTypeSym = compilation.GetTypeByMetadataName(CollectionsCaseInsensitiveComparerTypeName);
                ResourceManagerTypeSym = compilation.GetTypeByMetadataName(ResourceManagerTypeName);
                ComponentResourceManagerTypeSym = compilation.GetTypeByMetadataName(ComponentResourceManagerTypeName);
                BasicDateTypeTypeSym = compilation.GetTypeByMetadataName(BasicDateTypeTypeName);
                IFormatProviderTypeSym = compilation.GetTypeByMetadataName(IFormatProviderTypeName);
                CultureInfoTypeSym = compilation.GetTypeByMetadataName(CultureInfoTypeName);
                StringComparisonTypeSym = compilation.GetTypeByMetadataName(StringComparisonTypeName);
            }

            public void AnalyzeCallNode(SyntaxNodeAnalysisContext context)
            {
                new Analyzer(context).AnalyzeCallNode(this);
            }

            public void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
            {
                new Analyzer(context).AnalyzeMethodDeclaration(this);
            }

            public bool ReferencesAnyTargetType()
            {
                return StringComparerTypeSym != null
                    || CollectionsComparerTypeSym != null
                    || CollectionsCaseInsensitiveComparerTypeSym != null
                    || IFormatProviderTypeSym != null
                    || CultureInfoTypeSym != null
                    || StringComparisonTypeSym != null;
            }

        }

        private struct Analyzer
        {
            private const string PreferredStringCompareSignature = "string.Compare(string, string, StringComparison)";
            private const string PreferredStringFormatSignature = "string.Format(IFormatProvider, string, params object[])";

            private SyntaxNodeAnalysisContext context;
            private IMethodSymbol calleeMethodSym;
            private INamedTypeSymbol calleeOwnerTypeSym;
            private Location currentLocation;
            private string calleeDisplayString;
            private string callerDisplayString;

            public Analyzer(SyntaxNodeAnalysisContext context)
            {
                this.context = context;
                calleeMethodSym = null;
                calleeOwnerTypeSym = null;
                currentLocation = null;
                calleeDisplayString = null;
                callerDisplayString = null;
            }

            public void AnalyzeMethodDeclaration(AnalysisSession sessionInfo)
            {
                SyntaxNode node = this.context.Node;
                SemanticModel model = this.context.SemanticModel;
                Debug.Assert((node != null) && (model != null));

                ISymbol declSymbol = model.GetDeclaredSymbol(node);
                if ((declSymbol == null) || (declSymbol.Kind != SymbolKind.Method))
                {
                    return;
                }

                IMethodSymbol methodSymbol = (IMethodSymbol)declSymbol;
                this.calleeMethodSym = methodSymbol;

                ImmutableArray<IParameterSymbol> parameters = methodSymbol.Parameters;
                for (int i = parameters.Length; --i >= 0;)
                {
                    IParameterSymbol paramSym = parameters[i];
                    if (!paramSym.HasExplicitDefaultValue)
                    {
                        // optional parameters must be placed after all required parameters, and optional
                        // parameters must have default values
                        return;
                    }

                    SyntaxNode defaultValue = sessionInfo.SymbolHelper.GetDefaultValueForAnOptionalParameter(node, i);
                    CheckOptionalParamForMisusedCultureInfo(sessionInfo, paramSym, defaultValue);
                }
            }

            public void AnalyzeCallNode(AnalysisSession sessionInfo)
            {
                SyntaxNode node = this.context.Node;
                SemanticModel model = this.context.SemanticModel;
                Debug.Assert((node != null) && (model != null));

                ISymbol calleeSymbol = model.GetSymbolInfo(node).Symbol;
                if ((calleeSymbol == null) || (calleeSymbol.Kind != SymbolKind.Method))
                {
                    return;
                }

                var calleeMethod = (IMethodSymbol)calleeSymbol;
                INamedTypeSymbol calleeOwnerType = calleeMethod.ContainingType;
                this.calleeMethodSym = calleeMethod;
                this.calleeOwnerTypeSym = calleeOwnerType;

                // Check for special cases (e.g. string.ToString())
                if (IsExempt(sessionInfo, calleeMethod, calleeOwnerType))
                {
                    return;
                }

                // Easy special cases: calls to String.CompareTo() / String.Format()
                if (CheckForBadStringMethods(sessionInfo, calleeMethod))
                {
                    return;
                }

                // Check for CA1304, CA1305, and CA1307
                CheckForMissingCultureInfo(sessionInfo, calleeMethod);

                // Check for CA1305 and CA1309
                CheckForMisusedCultureInfo(sessionInfo, calleeMethod);

                // Check for "new System.Collections.Comparer(CultureInfo.InvariantCulture)" or
                // "new System.Collections.CaseInsensitiveComparer(CultureInfo.InvariantCulture)"
                // used in an expression except being passed as an argument (e.g. in an assignment),
                // and report CA1309 if applicable
                // REVIEW: legacy FXCop doesn't do this.  Dataflow analysis needed to improve the
                // accuracy.
                if (this.calleeMethodSym.MethodKind == MethodKind.Constructor)
                {
                }
            }

            private static bool IsExempt(AnalysisSession sessionInfo, IMethodSymbol calleeMethod, INamedTypeSymbol calleeOwnerType)
            {
                string calleeName = calleeMethod.Name;
                SpecialType typeEnum = calleeOwnerType.SpecialType;

                if (typeEnum != SpecialType.None)
                {
                    // CA1305 exceptions
                    if (calleeName == WellKnownMemberNames.ObjectToString)
                    {
                        if ((typeEnum == SpecialType.System_Boolean)
                            || (typeEnum == SpecialType.System_Char)
                            || (typeEnum == SpecialType.System_Enum)
                            || (typeEnum == SpecialType.System_String))
                        {
                            // The IFormatProvider argument is ignored anyway
                            return true;
                        }
                    }
                    // CA1307 exceptions
                    else if ((typeEnum == SpecialType.System_String) && (calleeName == WellKnownMemberNames.ObjectEquals))
                    {
                        ImmutableArray<IParameterSymbol> calleeParams = calleeMethod.Parameters;
                        Debug.Assert(calleeParams.Length > 0);
                        ITypeSymbol lastParamType = calleeParams[calleeParams.Length - 1].Type;
                        // String.Equals() performs an ordinal comparison by default
                        if (lastParamType != sessionInfo.StringComparisonTypeSym)
                        {
                            return true;
                        }
                    }

                    return false;
                }

                // CA1304 exceptions
                if (calleeOwnerType.IsDerivedFrom(sessionInfo.ResourceManagerTypeSym, baseTypesOnly: true))
                {
                    // sessionInfo.ResourceManager.GetString(), .GetObject(), GetStream()
                    Debug.Assert(ResourceManagerGetStringMethodName.Length == ResourceManagerGetObjectMethodName.Length);
                    Debug.Assert(ResourceManagerGetStringMethodName.Length == ResourceManagerGetStreamMethodName.Length);
                    if ((calleeName.Length == ResourceManagerGetStringMethodName.Length)
                        && ((calleeName == ResourceManagerGetStringMethodName)
                            || (calleeName == ResourceManagerGetObjectMethodName)
                            || (calleeName == ResourceManagerGetStreamMethodName)))
                    {
                        return true;
                    }

                    // ComponentResourceManager.ApplyResources()
                    if ((calleeOwnerType == sessionInfo.ComponentResourceManagerTypeSym) && (calleeName == ApplyResourcesMethodName))
                    {
                        return true;
                    }
                }
                else if (calleeOwnerType == sessionInfo.ActivatorTypeSym)
                {
                    // Activator.CreateInstance()
                    if (calleeName == ActivatorCreateInstanceMethodName)
                    {
                        return true;
                    }
                }
                else if (calleeOwnerType == sessionInfo.BasicDateTypeTypeSym)
                {
                    // VB emitted code
                    return true;
                }
                // CA1305 exceptions
                else if ((calleeName == WellKnownMemberNames.ObjectToString) && (calleeOwnerType == sessionInfo.GuidTypeSym))
                {
                    // The IFormatProvider argument is ignored anyway
                    return true;
                }

                return false;
            }

            private bool CheckForBadStringMethods(AnalysisSession sessionInfo, IMethodSymbol calleeMethod)
            {
                if (this.calleeOwnerTypeSym.SpecialType == SpecialType.System_String)
                {
                    string calleeName = calleeMethod.Name;
                    // CA1305
                    if (calleeName == "Format")
                    {
                        // Check whether the first parameter is IFormatProvider
                        var parameters = calleeMethod.Parameters;
                        Debug.Assert(parameters.Length > 0);
                        ITypeSymbol firstParamType = parameters[0].Type;
                        if (!firstParamType.IsDerivedFrom(sessionInfo.IFormatProviderTypeSym))
                        {
                            ReportMissingArg(sessionInfo, SpecifyIFormatProviderAlternateStringRule, PreferredStringFormatSignature);
                            return true;
                        }
                    }
                    // CA1307
                    else if (calleeName == "CompareTo")
                    {
                        ReportMissingArg(sessionInfo, SpecifyStringComparisonRule, PreferredStringCompareSignature);
                        return true;
                    }
                }

                return false;
            }

            private void CheckForMissingCultureInfo(AnalysisSession sessionInfo, IMethodSymbol calleeMethod)
            {
                if (calleeOwnerTypeSym.Kind != SymbolKind.NamedType)
                {
                    return;
                }

                // check each overload with an extra parameter that might be used to specify culture info
                string calleeName = calleeMethod.Name;
                ITypeSymbol calleeReturnType = calleeMethod.ReturnType;
                ImmutableArray<IParameterSymbol> calleeParams = calleeMethod.Parameters;
                int countParamsNeeded = calleeParams.Length + 1;    // need one more parameter
                foreach (ISymbol member in calleeOwnerTypeSym.GetMembers())
                {
                    if (member.Kind != SymbolKind.Method)
                    {
                        continue;
                    }

                    var candidate = (IMethodSymbol)member;
                    Debug.Assert(candidate != null);
                    ImmutableArray<IParameterSymbol> candidateParams = candidate.Parameters;
                    if ((candidateParams.Length != countParamsNeeded)
                        || (calleeName != candidate.Name)
                        || (calleeReturnType != candidate.ReturnType))
                    {
                        continue;
                    }

                    // find a promising overload; check if parameters match and find the extra one
                    int indexExtraParam = FindExtraParamPosition(calleeParams, candidateParams);
                    if (indexExtraParam < 0)
                    {
                        // There are too many mismatched parameters
                        continue;
                    }
                    // REVIEW: Legacy FXCop only checks the first and last parameter
                    if ((indexExtraParam != 0) && (indexExtraParam != countParamsNeeded - 1))
                    {
                        continue;   // for backward compatibility with legacy FXCop
                    }

                    // make sure the method is accessible at the callsite
                    SyntaxNode callSite = sessionInfo.SymbolHelper.GetCallTargetNode(this.context.Node);
                    Debug.Assert(callSite != null);
                    if (!this.context.SemanticModel.IsAccessible(callSite.SpanStart, candidate))
                    {
                        continue;   // candidate method cannot be accessed at the callsite
                    }

                    IParameterSymbol theParam = candidateParams[indexExtraParam];
                    ITypeSymbol theParamType = theParam.Type;
                    if (theParamType == sessionInfo.StringComparisonTypeSym)
                    {
                        // CA1307, specify StringComparison
                        ReportMissingArg(sessionInfo, SpecifyStringComparisonRule, candidate.ToDisplayString());
                        return;
                    }
                    else if (theParamType.IsDerivedFrom(sessionInfo.IFormatProviderTypeSym))
                    {
                        if (theParamType.IsDerivedFrom(sessionInfo.CultureInfoTypeSym, baseTypesOnly: true))
                        {
                            // CA1304, specify CultureInfo
                            ReportMissingArg(sessionInfo, SpecifyCultureInfoRule, candidate.ToDisplayString());
                            return;
                        }
                        else
                        {
                            // CA1305, specify IFormatProvider
                            DiagnosticDescriptor desc = (calleeReturnType.SpecialType == SpecialType.System_String) ?
                                SpecifyIFormatProviderAlternateStringRule : SpecifyIFormatProviderAlternateRule;
                            ReportMissingArg(sessionInfo, desc, candidate.ToDisplayString());
                            return;
                        }
                    }
                }
            }

            private static int FindExtraParamPosition(
                ImmutableArray<IParameterSymbol> seedParams,
                ImmutableArray<IParameterSymbol> matchParams)
            {
                Debug.Assert(seedParams.Length + 1 == matchParams.Length);

                int i = seedParams.Length;
                if (i <= 0)
                {
                    // the first param list is empty, so the extra is the only one on the second param list
                    return 0;
                }

                int indexExtraParam = -1;
                ITypeSymbol matchParamType;
                ITypeSymbol seedParamType;
                do
                {
                    matchParamType = matchParams[i].Type;
                    --i;
                    seedParamType = seedParams[i].Type;
                    if (!matchParamType.Equals(seedParamType))
                    {
                        indexExtraParam = i + 1;
                        break;
                    }
                }
                while (i > 0);

                if ((i == 0) && (indexExtraParam < 0))
                {
                    // the extra parameter is at the front
                    return 0;
                }

                Debug.Assert(indexExtraParam > 0);
                Debug.Assert(i >= 0);
                // the extra parameter is in the middle or rare partion; check the front portion matches
                do
                {
                    seedParamType = seedParams[i].Type;
                    matchParamType = matchParams[i].Type;
                    if (!matchParamType.Equals(seedParamType))
                    {
                        // found another unmatched parameter, so bail out
                        return -1;
                    }
                    --i;
                }
                while (i >= 0);

                return indexExtraParam;
            }

            private void PrepareCommonDiagnosticInfo(AnalysisSession sessionInfo)
            {
                if (this.currentLocation == null)
                {
                    this.currentLocation = this.context.Node.GetLocation();
                    this.calleeDisplayString = this.calleeMethodSym.ToDisplayString();
                    ISymbol callerSym =
                            sessionInfo.SymbolHelper.GetEnclosingConstructSymbol(this.context.Node, this.context.SemanticModel);
                    this.callerDisplayString = callerSym.ToDisplayString();
                }
            }

            private void ReportMissingArg(AnalysisSession sessionInfo, DiagnosticDescriptor desc, string preferred)
            {
                PrepareCommonDiagnosticInfo(sessionInfo);
                Location location = this.currentLocation;
                string calleeString = this.calleeDisplayString;
                string callerString = this.callerDisplayString;

                // {0} ..., replace this call in {1} with a call to {2}
                var diag = Diagnostic.Create(desc, location, calleeString, callerString, preferred);
                this.context.ReportDiagnostic(diag);
            }

            private void ReportMisusedArg(AnalysisSession sessionInfo, DiagnosticDescriptor desc, string misusedArg)
            {
                PrepareCommonDiagnosticInfo(sessionInfo);
                Location location = this.currentLocation;
                string calleeString = this.calleeDisplayString;
                string callerString = this.callerDisplayString;

                // {0} passes {1} to {2} ...
                var diag = Diagnostic.Create(desc, location, callerString, misusedArg, calleeString);
                this.context.ReportDiagnostic(diag);
            }

            private void CheckForMisusedCultureInfo(AnalysisSession sessionInfo, IMethodSymbol calleeMethod)
            {
                ImmutableArray<IParameterSymbol> calleeParams = calleeMethod.Parameters;
                int countParams = calleeParams.Length;
                if (countParams == 0)
                {
                    return;
                }

                int index = 0;
                SyntaxNode node = this.context.Node;
                SemanticModel model = this.context.SemanticModel;
                foreach (SyntaxNode argExpr in sessionInfo.SymbolHelper.GetCallArgumentExpressionNodes(node))
                {
                    IParameterSymbol paramSym = calleeParams[index++];
                    ITypeSymbol paramType = paramSym.Type;

                    // rule out parameters of no interest to us in a quick check
                    if (paramType.SpecialType != SpecialType.None)
                    {
                        continue;
                    }

                    // we're not interested in array-typed parameters in this check
                    if (paramType.TypeKind == TypeKind.Array)
                    {
                        if (paramSym.IsParams)
                        {
                            // the last parameter with a "params" keyword.  Most likely we don't care,
                            // even if it is an array of objects of types we may be interested in
                            Debug.Assert(index == countParams);
                            return;
                        }

                        continue;
                    }

                    CheckArgForMisusedCultureInfo(sessionInfo, argExpr, paramType, model);
                }

                // handle optional parameters with default constant values
                while (index < countParams)
                {
                    IParameterSymbol paramSym = calleeParams[index++];
                    // we may get here due to either no arguments passed to the last params parameter
                    // or an invalid default value being assigned to an optional parameter
                    if (!paramSym.HasExplicitDefaultValue)
                    {
                        if (paramSym.IsParams && (index == countParams))
                        {
                            return;
                        }

                        Debug.Assert(paramSym.IsOptional);
                        continue;
                    }

                    // unfortunately there doesn't appear to be a way to get the corresponding syntax node
                    // or retrieve the default value string.
                    CheckOptionalParamForMisusedCultureInfo(sessionInfo, paramSym, null);
                }
            }

            private void CheckArgForMisusedCultureInfo(AnalysisSession sessionInfo,
                SyntaxNode argExpr, ITypeSymbol paramType, SemanticModel model)
            {
                ISymbol argSym = model.GetSymbolInfo(argExpr).Symbol;
                if (argSym == null)
                {
                    return;
                }

                SymbolKind argKind = argSym.Kind;
                if ((argKind == SymbolKind.Field) && (paramType == sessionInfo.StringComparisonTypeSym))
                {
                    string enumValueString = argSym.Name;
                    if ((enumValueString == "InvariantCulture") || (enumValueString == "InvariantCultureIgnoreCase"))
                    {
                        // CA1309, use ordinal Stringcomparison
                        ReportMisusedArg(sessionInfo, UseOrdinalStringComparisonRule, argExpr.ToString());
                    }
                }
                else if ((argKind == SymbolKind.Field) || (argKind == SymbolKind.Property))
                {
                    string propertyName = argSym.Name;
                    ITypeSymbol argType = model.GetTypeInfo(argExpr).Type;
                    if (argType == sessionInfo.CultureInfoTypeSym)
                    {
                        // check for string methods involving string comparisons and with CultureInfo
                        // parameters (e.g. string.Compare, string.BeginsWith, string.EndsWith)
                        if ((this.calleeOwnerTypeSym.SpecialType == SpecialType.System_String)
                            && (this.calleeMethodSym.Parameters.Length > 1)
                            && paramType.IsDerivedFrom(sessionInfo.IFormatProviderTypeSym))
                        {
                            string calleeName = this.calleeMethodSym.Name;
                            if ((propertyName == "InvariantCulture") && ((calleeName == "Compare")
                                || (calleeName == "StartsWith") || (calleeName == "EndsWith")))
                            {
                                // CA1309, use ordinal Stringcomparison
                                ReportMisusedArg(sessionInfo, UseOrdinalStringComparisonRule, argExpr.ToString());
                            }
                        }
                        if ((paramType == sessionInfo.IFormatProviderTypeSym)
                            && ((propertyName == "CurrentUICulture") || (propertyName == "InstalledUICulture")))
                        {
                            // CA1305, specify IFormatProvider
                            ITypeSymbol calleeReturnType = this.calleeMethodSym.ReturnType;
                            DiagnosticDescriptor desc = (calleeReturnType.SpecialType == SpecialType.System_String) ?
                                SpecifyIFormatProviderUICultureStringRule : SpecifyIFormatProviderUICultureRule;
                            ReportMisusedArg(sessionInfo, desc, argExpr.ToString());
                        }
                    }
                    else if (argType.IsDerivedFrom(sessionInfo.StringComparerTypeSym, baseTypesOnly: true)
                        && ((propertyName == "InvariantCulture") || (propertyName == "InvariantCultureIgnoreCase")))
                    {
                        // CA1309, use ordinal Stringcomparison
                        ReportMisusedArg(sessionInfo, UseOrdinalStringComparerRule, argExpr.ToString());
                    }
                    else if ((propertyName == "DefaultInvariant")
                        && ((argType == sessionInfo.CollectionsComparerTypeSym) // Comparer is sealed
                            || argType.IsDerivedFrom(sessionInfo.CollectionsCaseInsensitiveComparerTypeSym, baseTypesOnly: true)))
                    {
                        // CA1309, use ordinal Stringcomparison
                        ReportMisusedArg(sessionInfo, UseOrdinalStringComparerRule, argExpr.ToString());
                    }
                }
                else if ((argKind == SymbolKind.Method)
                    && (((IMethodSymbol)argSym).MethodKind == MethodKind.Constructor)
                    && (paramType == sessionInfo.CollectionsIComparerTypeSym))
                {
                    foreach (SyntaxNode newObjArg in sessionInfo.SymbolHelper.GetObjectCreationArgumentExpressionNodes(argExpr))
                    {
                        ISymbol newObjArgSym = model.GetSymbolInfo(newObjArg).Symbol;
                        if ((newObjArgSym.Kind == SymbolKind.Property) && (newObjArgSym.Name == "InvariantCulture"))
                        {
                            ITypeSymbol newObjArgType = model.GetTypeInfo(newObjArg).Type;
                            if (newObjArgType == sessionInfo.CultureInfoTypeSym)
                            {
                                // CA1309, use ordinal Stringcomparison
                                ReportMisusedArg(sessionInfo, UseOrdinalStringComparisonRule, argExpr.ToString());
                            }
                        }
                    }
                }
            }

            private void CheckOptionalParamForMisusedCultureInfo(AnalysisSession sessionInfo,
                IParameterSymbol paramSym, SyntaxNode defaultValueNode)
            {
                Debug.Assert(paramSym.IsOptional && paramSym.HasExplicitDefaultValue);

                ITypeSymbol paramType = paramSym.Type;

                // rule out parameters of no interest to us in a quick check
                if (paramType.SpecialType != SpecialType.None)
                {
                    return;
                }

                // we're not interested in array-typed parameters in this check
                if (paramType.TypeKind == TypeKind.Array)
                {
                    return;
                }

                if (paramType == sessionInfo.StringComparisonTypeSym)
                {
                    // check for CA1309, use ordinal Stringcomparison
                    var defaultValue = paramSym.ExplicitDefaultValue;
                    Debug.Assert(defaultValue != null);
                    int value = (int)defaultValue;

                    // StringComparison.InvariantCulture and StringComparison.InvariantCultureIgnoreCase
                    // don't exist in portable class library, so we use hardcoded integers instead
                    if (value == 2 ||   //StringComparison.InvariantCulture
                        value == 3)     //StringComparison.InvariantCultureIgnoreCase
                    {
                        // Report accordingly based on whether this is at a call site or a method declaration
                        if (defaultValueNode == null)
                        {
                            // at a call site
                            string enumValueString = ((System.StringComparison)value).ToString();
                            ReportMisusedArg(sessionInfo, UseOrdinalStringComparisonRule, enumValueString);
                        }
                        else
                        {
                            // at a method declaration
                            string enumValueString = defaultValueNode.ToString();
                            ReportMisusedDefaultParam(UseOrdinalStringComparisonAsDefaultRule, enumValueString, defaultValueNode);
                        }
                    }
                }
            }

            private void ReportMisusedDefaultParam(DiagnosticDescriptor desc, string misusedParam, SyntaxNode defaultValueNode)
            {
                Location location = defaultValueNode.GetLocation();
                string methodName = this.calleeDisplayString ?? this.calleeMethodSym.ToDisplayString();

                // {0} uses {1} as the default argument ...
                var diag = Diagnostic.Create(desc, location, methodName, misusedParam);
                this.context.ReportDiagnostic(diag);
            }
        }
    }
}
