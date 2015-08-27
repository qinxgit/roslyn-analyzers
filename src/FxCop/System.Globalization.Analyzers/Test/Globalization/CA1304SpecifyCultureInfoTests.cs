// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;   

namespace System.Globalization.Analyzers.UnitTests
{               
    public sealed class CA1304SpecifyCultureInfoTests : DiagnosticAnalyzerTestBase
    {
        [Fact]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_String_CS()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        string strAld = strA.ToLower();
        string strBud = strB.ToUpper();

        // The following calls are okay as far as CA1304 is concerned
        string strAlc = strA.ToLower(CultureInfo.CurrentCulture);
        string strBuc = strB.ToUpper(CultureInfo.InvariantCulture);
        if ((strAld == strAlc) != (strBud == strBuc))
        {
            Console.WriteLine(""Oops"");
        }
    }
}",
            GetCA1304CSharpDefaultResultAt(9, 25, callee: "string.ToLower()", caller: "C.M(string, string)", preferred: "string.ToLower(System.Globalization.CultureInfo)"),
            GetCA1304CSharpDefaultResultAt(10, 25, callee: "string.ToUpper()", caller: "C.M(string, string)", preferred: "string.ToUpper(System.Globalization.CultureInfo)"));
        } 
        
        [Fact]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_Misc_CS()
        {
            VerifyCSharp(@"
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

static class C
{
    static void T(string f, string s)
    {
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.InvariantCulture));
        CultureInfoOverloads.M2(f, s.ToUpper(CultureInfo.InvariantCulture));

        // The following calls are okay as far as CA1304 is concerned
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        CultureInfoOverloads.M2(CultureInfo.CurrentCulture, f, s.ToUpper(CultureInfo.CurrentCulture));
        CultureInfoOverloads.M3(f, s);
        CultureInfoOverloads.M1cc(f, s);

        char[] characters = { 'a', 'b', 'c', 'd', 'e', 'f' };
        object[][] arguments = new object[3][] { new object[] { characters },
                                                 new object[] { characters, 1, 4 },
                                                 new object[] { characters[1], 20 } };
        for (int ctr = 0; ctr <= arguments.GetUpperBound(0); ctr++) {
            object[] args = arguments[ctr];
            object result = Activator.CreateInstance(typeof(String), args);
            Console.WriteLine(""{0}: {1}"", result.GetType().Name, result);
        }

        var crm = new System.ComponentModel.ComponentResourceManager(typeof(C));
        crm.ApplyResources(f, s);
        Console.WriteLine(crm.GetString(s));
        Console.WriteLine(crm.GetObject(s).ToString());
        Console.WriteLine(crm.GetStream(s).ToString());

        PropertyInfo pinfo = typeof(string).GetProperty(""Chars"");
        pinfo.SetValue(null, null);
        pinfo.SetValue(null, null, null);
        Console.WriteLine(pinfo.GetValue(null, null));
    }

    internal static class CultureInfoOverloads
    {
        internal static void M1(string format, string content)
        {
            M1(format, content, CultureInfo.CurrentCulture);
        }

        internal static void M1(string format, string content, CultureInfo provider)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }

        // compilation error: default needs to be compile-time constant
        internal static void M1cc(string format, string content, CultureInfo provider = CultureInfo.CurrentCulture)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }

        internal static void M2(string format, string content)
        {
            M2(CultureInfo.CurrentCulture, format, content);
        }

        internal static void M2(CultureInfo provider, string format, string content)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }

        internal static void M3(string format, string content)
        {
            M3(format, CultureInfo.InvariantCulture, content);
        }

        private static void M3(string format, CultureInfo provider, string content)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }
    }
}",
            GetCA1304CSharpDefaultResultAt(12, 9, callee: "C.CultureInfoOverloads.M1(string, string)", 
                                                  caller: "C.T(string, string)", 
                                                  preferred: "C.CultureInfoOverloads.M1(string, string, System.Globalization.CultureInfo)"),
            GetCA1304CSharpDefaultResultAt(13, 9, callee: "C.CultureInfoOverloads.M2(string, string)", 
                                                  caller: "C.T(string, string)", 
                                                  preferred: "C.CultureInfoOverloads.M2(System.Globalization.CultureInfo, string, string)"));
        } 
        
        [Fact]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_String_VB()
        {
            VerifyBasic(@"
Imports System
Imports System.Globalization

Public Module C
    Public Sub M(strA As String, strB As String)
        Dim strAld As String = strA.ToLower()
        Dim strBud As String = strB.ToUpper()

        ' The following calls are okay as far as CA1304 is concerned
        Dim strAlc As String = strA.ToLower(CultureInfo.CurrentCulture)
        Dim strBuc As String = strB.ToUpper(CultureInfo.InvariantCulture)
        If ((strAld = strAlc) <> (strBud = strBuc)) Then
            Console.WriteLine(""Oops"")
        End If
    End Sub
End Module",
            GetCA1304BasicDefaultResultAt(7, 32, callee: "Public Overloads Function ToLower() As String",
                                                 caller: "Public Sub M(strA As String, strB As String)",
                                                 preferred: "Public Overloads Function ToLower(culture As System.Globalization.CultureInfo) As String"),
            GetCA1304BasicDefaultResultAt(8, 32, callee: "Public Overloads Function ToUpper() As String",
                                                 caller: "Public Sub M(strA As String, strB As String)",
                                                 preferred: "Public Overloads Function ToUpper(culture As System.Globalization.CultureInfo) As String"));
        }
        
        [Fact]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_Misc_VB()
        {
            VerifyBasic(@"
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports System.Resources

Public Module C
    Public Sub M(f As String, s As String)
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.InvariantCulture))
        CultureInfoOverloads.M2(f, s.ToUpper(CultureInfo.InvariantCulture))

        ' The following calls are okay as far as CA1304 is concerned
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture)
        CultureInfoOverloads.M2(CultureInfo.CurrentCulture, f, s.ToUpper(CultureInfo.CurrentCulture))
        CultureInfoOverloads.M3(f, s)
        CultureInfoOverloads.M1cc(f, s)

        Dim characters() As Char = New Char() {""a""c, ""b""c, ""c""c, ""d""c, ""e""c, ""f""c}
        Dim arguments()() As Object = New Object(2)() {New Object() {characters},
                                                        New Object() {characters, 1, 4},
                                                        New Object() {characters(1), 20}}
        For ctr = 0 To arguments.GetUpperBound(0)
            Dim args As Object = arguments(ctr)
            Dim result As Object = Activator.CreateInstance(GetType(String), args)
            Console.WriteLine(""{0}: {1}"", result.GetType().Name, result)
        Next

        Dim crm = New System.ComponentModel.ComponentResourceManager(GetType(C))
        crm.ApplyResources(Nothing, s)
        Console.WriteLine(crm.GetString(s))
        Console.WriteLine(crm.GetObject(s).ToString())
        Console.WriteLine(crm.GetStream(s).ToString())

        Dim pinfo As PropertyInfo = f.GetType().GetProperty(""Chars"")
        pinfo.SetValue(Nothing, Nothing)
        pinfo.SetValue(Nothing, Nothing, Nothing)
        Console.WriteLine(pinfo.GetValue(Nothing, Nothing))
    End Sub
End Module

Friend Module CultureInfoOverloads
    Friend Sub M1(format As String, content As String)
        M1(format, content, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub M1(format As String, content As String, provider As CultureInfo)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub

    ' compilation error: default needs to be compile-time constant
    Friend Sub M1cc(format As String, content As String, Optional provider As CultureInfo = CultureInfo.CurrentCulture)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub

    Friend Sub M2(format As String, content As String)
        M2(CultureInfo.CurrentCulture, format, content)
    End Sub

    Friend Sub M2(provider As CultureInfo, format As String, content As String)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub

    Friend Sub M3(format As String, content As String)
        M3(format, CultureInfo.InvariantCulture, content)
    End Sub

    Private Sub M3(format As String, provider As CultureInfo, content As String)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub
End Module",
            GetCA1304BasicDefaultResultAt(10, 9, callee: "Friend Sub M1(format As String, content As String)", 
                                                 caller: "Public Sub M(f As String, s As String)", 
                                                 preferred: "Friend Sub M1(format As String, content As String, provider As System.Globalization.CultureInfo)"),
            GetCA1304BasicDefaultResultAt(11, 9, callee: "Friend Sub M2(format As String, content As String)", 
                                                 caller: "Public Sub M(f As String, s As String)", 
                                                 preferred: "Friend Sub M2(provider As System.Globalization.CultureInfo, format As String, content As String)")); 
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpCA1304DiagnosticAnalyzer();
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new VisualBasicCA1304DiagnosticAnalyzer();
        }

        internal static string CA1304Name = CA1304DiagnosticAnalyzer.RuleId1304;

        private static DiagnosticResult GetCA1304CSharpDefaultResultAt(int line, int column, string callee, string caller, string preferred)
        {
            // Because the behavior of '{0}' could vary based on the current user's locale settings, 
            // replace this call in '{1}' with a call to '{2}'. If the result of '{2}' will be displayed 
            // to the user, specify 'CultureInfo.CurrentCulture' as the 'CultureInfo' parameter. Otherwise, 
            // if the result will be stored and accessed by software, such as when it is persisted to disk or 
            // to a database, specify 'CultureInfo.InvariantCulture'.
            var message = string.Format(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoDiagnosis, callee, caller, preferred);
            return GetCSharpResultAt(line, column, CA1304Name, message);
        }

        private static DiagnosticResult GetCA1304BasicDefaultResultAt(int line, int column, string callee, string caller, string preferred)
        {
            // Because the behavior of '{0}' could vary based on the current user's locale settings, 
            // replace this call in '{1}' with a call to '{2}'. If the result of '{2}' will be displayed 
            // to the user, specify 'CultureInfo.CurrentCulture' as the 'CultureInfo' parameter. Otherwise, 
            // if the result will be stored and accessed by software, such as when it is persisted to disk or 
            // to a database, specify 'CultureInfo.InvariantCulture'.
            var message = string.Format(SystemGlobalizationAnalyzersResources.SpecifyCultureInfoDiagnosis, callee, caller, preferred);
            return GetBasicResultAt(line, column, CA1304Name, message);
        }
    }
}
