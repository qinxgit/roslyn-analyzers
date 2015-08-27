' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.Immutable
Imports System.Diagnostics

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic

Imports System.Globalization.Analyzers.Common

Namespace System.Globalization.Analyzers

    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public class VisualBasicCA1304DiagnosticAnalyzer 
        Inherits CA1304DiagnosticAnalyzer

        Private ReadOnly callNodeKindsOfInterest As ImmutableArray(Of SyntaxKind) =
            ImmutableArray.Create(SyntaxKind.InvocationExpression,
                                  SyntaxKind.ObjectCreationExpression)

        Private ReadOnly methodDeclNodeKindsOfInterest As ImmutableArray(Of SyntaxKind) =
            ImmutableArray.Create(SyntaxKind.FunctionBlock,
                                  SyntaxKind.SubBlock,
                                  SyntaxKind.ConstructorBlock)

        Protected Overrides Sub PrepareAnalyzer(context As CompilationStartAnalysisContext)

            Dim analysisSession As AnalysisSession = New AnalysisSession(context, BasicSyntaxNodeHelper.DefaultInstance)
            ' Currently all the targeted types reside in mscorlib.dll, but it might be moved to seperate 
            ' assemblies in the future. Need to revisit this check then
            Debug.Assert(analysisSession.ReferencesAnyTargetType())
            context.RegisterSyntaxNodeAction(AddressOf analysisSession.AnalyzeCallNode, callNodeKindsOfInterest)
            context.RegisterSyntaxNodeAction(AddressOf analysisSession.AnalyzeMethodDeclaration, methodDeclNodeKindsOfInterest)
        End Sub
    End Class
End Namespace