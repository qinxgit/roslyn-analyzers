// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Globalization.Analyzers.Common;

namespace System.Globalization.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CSharpCA1304DiagnosticAnalyzer : CA1304DiagnosticAnalyzer
    {
        private static readonly ImmutableArray<SyntaxKind> callNodeKindsOfInterest =
            ImmutableArray.Create(SyntaxKind.InvocationExpression,
                                  SyntaxKind.ObjectCreationExpression);
        private static readonly ImmutableArray<SyntaxKind> methodDeclNodeKindsOfInterest =
            ImmutableArray.Create(SyntaxKind.MethodDeclaration,
                                  SyntaxKind.ConstructorDeclaration);

        protected override void PrepareAnalyzer(CompilationStartAnalysisContext context)
        {
            var analysisSession = new AnalysisSession(context, CSharpSyntaxNodeHelper.Default);

            // Currently all the targeted types reside in mscorlib.dll, but it might be moved to seperate 
            // assemblies in the future. Need to revisit this check then
            Debug.Assert(analysisSession.ReferencesAnyTargetType());

            context.RegisterSyntaxNodeAction(analysisSession.AnalyzeCallNode,
                                                callNodeKindsOfInterest);
            context.RegisterSyntaxNodeAction(analysisSession.AnalyzeMethodDeclaration,
                                                methodDeclNodeKindsOfInterest);
        }
    }
}
