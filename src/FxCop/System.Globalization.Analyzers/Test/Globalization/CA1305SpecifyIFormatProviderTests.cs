// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;   

namespace System.Globalization.Analyzers.UnitTests
{
    public sealed class CA1305SpecifyIFormatProviderTests : DiagnosticAnalyzerTestBase
    {

        
        [Fact]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_StringFormatting_CS()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = string.Format(""Foo {0}"", strA);
        string str2 = string.Format(""Foo {0} {1}"", strA, strB);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2);
        IFormatProviderOverloads.LeadingIFormatProvider(str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2);

        // The following calls are okay as far as CA1305 is concerned
        str1 = string.Format(CultureInfo.InvariantCulture, ""Foo {0}"", strA);
        str2 = string.Format(CultureInfo.CurrentCulture, ""Foo {0} {1}"", strA, strB);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InvariantCulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentCulture);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InvariantCulture, str1);
        IFormatProviderOverloads.LeadingIFormatProvider2(str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentCulture);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        strA = strA.ToString();
        strB = strB.ToString(CultureInfo.CurrentCulture);
        Console.WriteLine(true.ToString());
        Console.WriteLine('A'.ToString());
        Console.WriteLine(StringComparison.Ordinal.ToString());
        Console.WriteLine(new Guid().ToString());
    }

    internal static class IFormatProviderOverloads
    {
        public static void LeadingIFormatProvider(string s)
        {
            LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        }

        public static void LeadingIFormatProvider(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void LeadingIFormatProvider2(string s)
        {
            LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        }

        private static void LeadingIFormatProvider2(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }

        public static string TrailingIFormatProviderReturningString(string format)
        {
            return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture);
        }

        public static string TrailingIFormatProviderReturningString(string format, IFormatProvider provider)
        {
            return string.Format(provider, format);
        }

        // compilation error: default needs to be compile-time constant
        public static void TrailingIFormatProvider2(string format, IFormatProvider provider = CultureInfo.CurrentCulture)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        // compilation error: default needs to be compile-time constant
        public static string TrailingIFormatProviderReturningString2(string format, IFormatProvider provider = CultureInfo.CurrentCulture)
        {
            return string.Format(provider, format);
        }
    }
}",
            GetCA1305CSharpDefaultResultAt(9, 23, MessageAlternateString,
                                                 "string.Format(string, object)",
                                                 "C.M(string, string)",
                                                 "string.Format(IFormatProvider, string, params object[])"),
            GetCA1305CSharpDefaultResultAt(10, 23, MessageAlternateString,
                                                 "string.Format(string, object, object)",
                                                 "C.M(string, string)",
                                                 "string.Format(IFormatProvider, string, params object[])"),
            GetCA1305CSharpDefaultResultAt(11, 16, MessageAlternateString,
                                                 "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(System.IFormatProvider, string)"),
            GetCA1305CSharpDefaultResultAt(12, 16, MessageAlternateString,
                                                 "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(13, 9, MessageAlternate,
                                                 "C.IFormatProviderOverloads.LeadingIFormatProvider(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.LeadingIFormatProvider(System.IFormatProvider, string)"),
            GetCA1305CSharpDefaultResultAt(14, 9, MessageAlternate,
                                                 "C.IFormatProviderOverloads.TrailingIFormatProvider(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.TrailingIFormatProvider(string, System.IFormatProvider)"));
        }         
        
        [Fact]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_ConvertingParsing_CS()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA)
    {
        int i0 =  Convert.ToInt32(strA);
        long l0 = Convert.ToInt64(strA);
        int i1 =  Int32.Parse(strA);
        long l1 = Int64.Parse(strA);
        int i2 =  Int32.Parse(strA, NumberStyles.HexNumber);
        long l2 = Int64.Parse(strA, NumberStyles.HexNumber);

        // The following calls are okay as far as CA1305 is concerned
        if ((i0 != Convert.ToInt32(strA, CultureInfo.CurrentCulture))
            || (l0 != Convert.ToInt64(strA, CultureInfo.CurrentCulture))
            || (i1 !=  Int32.Parse(strA, CultureInfo.CurrentCulture))
            || (l1 != Int64.Parse(strA, CultureInfo.CurrentCulture))
            || (i2 !=  Int32.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture))
            || (l2 != Int64.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture)))
        {
        }
    }
}",
            GetCA1305CSharpDefaultResultAt(9, 19, MessageAlternate,
                                                  "System.Convert.ToInt32(string)",
                                                  "C.M(string)",
                                                  "System.Convert.ToInt32(string, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(10, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "System.Convert.ToInt64(string)",
                                                  "C.M(string)",
                                                  "System.Convert.ToInt64(string, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(11, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "int.Parse(string)",
                                                  "C.M(string)",
                                                  "int.Parse(string, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(12, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "long.Parse(string)",
                                                  "C.M(string)",
                                                  "long.Parse(string, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(13, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "int.Parse(string, System.Globalization.NumberStyles)",
                                                  "C.M(string)",
                                                  "int.Parse(string, System.Globalization.NumberStyles, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(14, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "long.Parse(string, System.Globalization.NumberStyles)",
                                                  "C.M(string)",
                                                  "long.Parse(string, System.Globalization.NumberStyles, System.IFormatProvider)"));
        }            

        
        [Fact(Skip = "TODO: fix expected results")]
        public void CA1305ShouldNotUseUICultureAsIFormatProviderParamTests_CS()
        {
            VerifyCSharp(@"using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = string.Format(Thread.CurrentThread.CurrentUICulture, ""Foo {0}"", strA);
        string str2 = string.Format(CultureInfo.InstalledUICulture, ""Foo {0} {1}"", strA, strB);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.CurrentUICulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentUICulture);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.CurrentUICulture, str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentUICulture);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(Thread.CurrentThread.CurrentUICulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, Thread.CurrentThread.CurrentUICulture);
        IFormatProviderOverloads.LeadingIFormatProvider(Thread.CurrentThread.CurrentUICulture, str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, Thread.CurrentThread.CurrentUICulture);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InstalledUICulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.InstalledUICulture);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InstalledUICulture, str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.InstalledUICulture);
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
    }

    internal static class IFormatProviderOverloads
    {
        public static void LeadingIFormatProvider(string s)
        {
            LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        }

        public static void LeadingIFormatProvider(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void LeadingIFormatProvider2(string s)
        {
            LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        }

        private static void LeadingIFormatProvider2(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }

        public static string TrailingIFormatProviderReturningString(string format)
        {
            return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture);
        }

        public static string TrailingIFormatProviderReturningString(string format, IFormatProvider provider)
        {
            return string.Format(provider, format);
        }

        // compilation error: default needs to be compile-time constant
        public static void TrailingIFormatProvider2(string format, IFormatProvider provider = CultureInfo.InstalledUICulture)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        // compilation error: default needs to be compile-time constant
        public static string TrailingIFormatProviderReturningString2(string format, IFormatProvider provider = Thread.CurrentThread.CurrentUICulture)
        {
            return string.Format(provider, format);
        }
    }
}",
            GetCA1305CSharpDefaultResultAt(9, 23, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "Thread.CurrentThread.CurrentUICulture",
                                                  "string.Format(System.IFormatProvider, string, object)"),
            GetCA1305CSharpDefaultResultAt(10, 23, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "string.Format(System.IFormatProvider, string, object, object)"),
            GetCA1305CSharpDefaultResultAt(11, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(12, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(13, 9, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(14, 9, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(15, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(16, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(17, 9, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(18, 9, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(19, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(20, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(21, 9, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""),
            GetCA1305CSharpDefaultResultAt(22, 9, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "",
                                                  ""));
        }  
        
        [Fact]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_StringFormatting_VB()
        {
            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = string.Format(""Foo {0}"", strA)
        Dim str2 As String = string.Format(""Foo {0} {1}"", strA, strB)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2)
        IFormatProviderOverloads.LeadingIFormatProvider(str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2)

        ' The following calls are okay as far as CA1305 is concerned
        str1 = string.Format(CultureInfo.InvariantCulture, ""Foo {0}"", strA)
        str2 = string.Format(CultureInfo.CurrentCulture, ""Foo {0} {1}"", strA, strB)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InvariantCulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentCulture)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InvariantCulture, str1)
        IFormatProviderOverloads.LeadingIFormatProvider2(str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentCulture)
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
        strA = strA.ToString()
        strB = strB.ToString(CultureInfo.CurrentCulture)
        Console.WriteLine(true.ToString())
        Console.WriteLine('A'.ToString())
        Console.WriteLine(StringComparison.Ordinal.ToString())
        Console.WriteLine(new Guid().ToString())
    End Sub
End Module

Friend Module IFormatProviderOverloads
    Friend Sub LeadingIFormatProvider(s As String)
        LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
    End Sub

    Friend Sub LeadingIFormatProvider(provider As IFormatProvider, s As String)
        Console.WriteLine(String.Format(provider, s))
    End Sub

    Friend Sub LeadingIFormatProvider2(s As String)
        LeadingIFormatProvider2(CultureInfo.CurrentCulture, s)
    End Sub

    Private Sub LeadingIFormatProvider2(provider As IFormatProvider, s As String)
        Console.WriteLine(String.Format(provider, s))
    End Sub

    Friend Sub TrailingIFormatProvider(format As String)
        TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    Friend Function LeadingIFormatProviderReturningString(format As String) As String
        Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
    End Function

    Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
        Return String.Format(provider, format)
    End Function

    Friend Function TrailingIFormatProviderReturningString(format As String) As String
        Return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture)
    End Function

    Friend Function TrailingIFormatProviderReturningString(format As String, provider As IFormatProvider) As String
        Return String.Format(provider, format)
    End Function

    ' compilation error: default needs to be compile-time constant
    Friend Sub TrailingIFormatProvider2(format As String, Optional provider As IFormatProvider = CultureInfo.InvariantCulture)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    ' compilation error: default needs to be compile-time constant
    Friend Function TrailingIFormatProviderReturningString2(format As String, Optional provider As IFormatProvider = CultureInfo.CurrentCulture) As String
        Return String.Format(provider, format)
    End Function
End Module",
            GetCA1305BasicDefaultResultAt(9, 30, MessageAlternateString,
                                                 "Public Shared Overloads Function Format(format As String, arg0 As Object) As String",
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "string.Format(IFormatProvider, string, params object[])"),
            GetCA1305BasicDefaultResultAt(10, 30, MessageAlternateString,
                                                 "Public Shared Overloads Function Format(format As String, arg0 As Object, arg1 As Object) As String",
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "string.Format(IFormatProvider, string, params object[])"),
            GetCA1305BasicDefaultResultAt(11, 16, MessageAlternateString,
                                                  "Friend Function LeadingIFormatProviderReturningString(format As String) As String",
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "Friend Function LeadingIFormatProviderReturningString(provider As System.IFormatProvider, format As String) As String"),
            GetCA1305BasicDefaultResultAt(12, 16, MessageAlternateString,
                                                  "Friend Function TrailingIFormatProviderReturningString(format As String) As String",
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "Friend Function TrailingIFormatProviderReturningString(format As String, provider As System.IFormatProvider) As String"),
            GetCA1305BasicDefaultResultAt(13, 9, MessageAlternate,
                                                  "Friend Sub LeadingIFormatProvider(s As String)",
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "Friend Sub LeadingIFormatProvider(provider As System.IFormatProvider, s As String)"),
            GetCA1305BasicDefaultResultAt(14, 9, MessageAlternate,
                                                  "Friend Sub TrailingIFormatProvider(format As String)",
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "Friend Sub TrailingIFormatProvider(format As String, provider As System.IFormatProvider)"));
        }    
        
        [Fact]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_ConvertingParsing_VB()
        {
            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String)
        Dim i0 As Integer = Convert.ToInt32(strA)
        Dim l0 As Long =    Convert.ToInt64(strA)
        Dim i1 As Integer = Int32.Parse(strA)
        Dim l1 As Long =    Int64.Parse(strA)
        Dim i2 As Integer = Int32.Parse(strA, NumberStyles.HexNumber)
        Dim l2 As Long =    Int64.Parse(strA, NumberStyles.HexNumber)

        // The following calls are okay as far as CA1305 is concerned
        If ((i0 <> Convert.ToInt32(strA, CultureInfo.CurrentCulture)) _
            Or (l0 <> Convert.ToInt64(strA, CultureInfo.CurrentCulture)) _
            Or (i1 <> Int32.Parse(strA, CultureInfo.CurrentCulture)) _
            Or (l1 <> Int64.Parse(strA, CultureInfo.CurrentCulture)) _
            Or (i2 <> Int32.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture)) _
            Or (l2 <> Int64.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture))) Then
        End If
    End Sub
End Module",
            GetCA1305BasicDefaultResultAt(9, 29, MessageAlternate,
                                                 "Public Shared Overloads Function ToInt32(value As String) As Integer",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function ToInt32(value As String, provider As System.IFormatProvider) As Integer"),
            GetCA1305BasicDefaultResultAt(10, 29, MessageAlternate,
                                                 "Public Shared Overloads Function ToInt64(value As String) As Long",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function ToInt64(value As String, provider As System.IFormatProvider) As Long"),
            GetCA1305BasicDefaultResultAt(11, 29, MessageAlternate,
                                                 "Public Shared Overloads Function Parse(s As String) As Integer",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function Parse(s As String, provider As System.IFormatProvider) As Integer"),
            GetCA1305BasicDefaultResultAt(12, 29, MessageAlternate,
                                                 "Public Shared Overloads Function Parse(s As String) As Long",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function Parse(s As String, provider As System.IFormatProvider) As Long"),
            GetCA1305BasicDefaultResultAt(13, 29, MessageAlternate,
                                                 "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles) As Integer",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles, provider As System.IFormatProvider) As Integer"),
            GetCA1305BasicDefaultResultAt(14, 29, MessageAlternate,
                                                 "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles) As Long",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles, provider As System.IFormatProvider) As Long"));
        }                                                          
        
        [Fact(Skip = "TODO: Fix expected results")]
        public void CA1305ShouldNotUseUICultureAsIFormatProviderParamTests_VB()
        {
            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Threading


Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = string.Format(Thread.CurrentThread.CurrentUICulture, ""Foo {0}"", strA)
        Dim str2 As String = string.Format(CultureInfo.InstalledUICulture, ""Foo {0} {1}"", strA, strB)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.CurrentUICulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentUICulture)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.CurrentUICulture, str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentUICulture)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(Thread.CurrentThread.CurrentUICulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, Thread.CurrentThread.CurrentUICulture)
        IFormatProviderOverloads.LeadingIFormatProvider(Thread.CurrentThread.CurrentUICulture, str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, Thread.CurrentThread.CurrentUICulture)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InstalledUICulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.InstalledUICulture)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InstalledUICulture, str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.InstalledUICulture)
        IFormatProviderOverloads.TrailingIFormatProvider2(str2)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2)
    End Sub

    Friend Module IFormatProviderOverloads
        Friend Sub LeadingIFormatProvider(s As String)
            LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        End Sub

        Friend Sub LeadingIFormatProvider(provider As IFormatProvider, s As String)
            Console.WriteLine(string.Format(provider, s));
        End Sub

        Friend Sub LeadingIFormatProvider2(s As String)
            LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        End Sub

        Private Sub LeadingIFormatProvider2(provider As IFormatProvider, s As String)
            Console.WriteLine(string.Format(provider, s))
        End Sub

        Friend Sub TrailingIFormatProvider(format As String)
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
        End Sub

        Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
            Console.WriteLine(string.Format(provider, format))
        End Sub

        Friend Function LeadingIFormatProviderReturningString(format As String) As String
            Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
        End Function

        Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
            Return string.Format(provider, format)
        End Function

        Friend Function TrailingIFormatProviderReturningString(format As String) As String
            Return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture)
        End Function

        Friend Function TrailingIFormatProviderReturningString(format As String, provider As IFormatProvider) As String
            Return string.Format(provider, format)
        End Function

        ' compilation error: default needs to be compile-time constant
        Friend Sub TrailingIFormatProvider2(format As String, Optional provider As IFormatProvider = Thread.CurrentThread.CurrentUICulture)
            Console.WriteLine(string.Format(provider, format))
        End Sub

        ' compilation error: default needs to be compile-time constant
        Friend Function TrailingIFormatProviderReturningString2(format As String, Optional provider As IFormatProvider = CultureInfo.InstalledUICulture) As String
            Return string.Format(provider, format)
        End Function
    End Sub
End Module ' C",
            GetCA1305BasicDefaultResultAt(9, 30, MessageUICultureString,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "Thread.CurrentThread.CurrentUICulture",
                                                 "Public Shared Overloads Function Format(provider As System.IFormatProvider, format As String, arg0 As Object) As String"),
            GetCA1305BasicDefaultResultAt(10, 30, MessageUICultureString,
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "Public Shared Overloads Function Format(provider As System.IFormatProvider, format As String, arg0 As Object, arg1 As Object) As String"),
            GetCA1305BasicDefaultResultAt(11, 16, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(12, 16, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(13, 9, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(14, 9, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(15, 16, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(16, 16, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(17, 9, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(18, 9, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(19, 16, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(20, 16, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(21, 9, "", "", "", ""),
            GetCA1305BasicDefaultResultAt(22, 9, "", "", "", "")); 
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpCA1304DiagnosticAnalyzer();
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new VisualBasicCA1304DiagnosticAnalyzer();
        }

        internal static string CA1305Name = CA1304DiagnosticAnalyzer.RuleId1305;
        internal static string MessageAlternate = SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate;
        internal static string MessageAlternateString = SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternateString;
        internal static string MessageUICulture = SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisUICulture;
        internal static string MessageUICultureString = SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisUICultureString;

        private static DiagnosticResult GetCA1305CSharpDefaultResultAt(int line, int column, string messageFormat, params string[] arguments)
        {
            var message = string.Format(messageFormat, arguments);
            return GetCSharpResultAt(line, column, CA1305Name, message);
        }

        private static DiagnosticResult GetCA1305BasicDefaultResultAt(int line, int column, string messageFormat, params string[] arguments)
        {
            var message = string.Format(messageFormat, arguments);
            return GetBasicResultAt(line, column, CA1305Name, message);
        }
    }
}
