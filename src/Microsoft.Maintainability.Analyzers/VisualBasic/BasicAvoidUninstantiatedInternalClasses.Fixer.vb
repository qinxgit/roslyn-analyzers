' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Composition
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.Maintainability.Analyzers

Namespace Microsoft.Maintainability.VisualBasic.Analyzers
    ''' <summary>
    ''' CA1812: Avoid uninstantiated internal classes
    ''' </summary>
    <ExportCodeFixProvider(LanguageNames.VisualBasic), [Shared]>
    Public NotInheritable Class BasicAvoidUninstantiatedInternalClassesFixer
        Inherits AvoidUninstantiatedInternalClassesFixer

    End Class
End Namespace
