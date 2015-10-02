// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;

namespace System.Globalization.Analyzers.UnitTests
{
    public sealed class CA1305SpecifyIFormatProviderTests : DiagnosticAnalyzerTestBase
    {
        [Fact]
        public void CA1305FormatProviderParamTests_StringFormatting()
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
    }
}",
            GetCA1305CSharpDefaultResultAt(9, 23, MessageAlternateString,
                                                 "string.Format(string, object)",
                                                 "C.M(string, string)",
                                                 "string.Format(IFormatProvider, string, params object[])"),
            GetCA1305CSharpDefaultResultAt(10, 23, MessageAlternateString,
                                                 "string.Format(string, object, object)",
                                                 "C.M(string, string)",
                                                 "string.Format(IFormatProvider, string, params object[])"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = string.Format(""Foo {0}"", strA)
        Dim str2 As String = string.Format(""Foo {0} {1}"", strA, strB)
    End Sub
End Module",
            GetCA1305BasicDefaultResultAt(6, 30, MessageAlternateString,
                                                 "Public Shared Overloads Function Format(format As String, arg0 As Object) As String",
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "string.Format(IFormatProvider, string, params object[])"),
            GetCA1305BasicDefaultResultAt(7, 30, MessageAlternateString,
                                                 "Public Shared Overloads Function Format(format As String, arg0 As Object, arg1 As Object) As String",
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "string.Format(IFormatProvider, string, params object[])"));
        }

        [Fact]
        public void CA1305IFormatProvider_Leading()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = strA;
    
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(str1);
    
        IFormatProviderOverloads.LeadingIFormatProvider(str1);
      
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

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }

    }
}",

            GetCA1305CSharpDefaultResultAt(11, 16, MessageAlternateString,
                                                 "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(System.IFormatProvider, string)"),
               GetCA1305CSharpDefaultResultAt(13, 9, MessageAlternate,
                                                 "C.IFormatProviderOverloads.LeadingIFormatProvider(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.LeadingIFormatProvider(System.IFormatProvider, string)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = strA

        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(str1)
        
        IFormatProviderOverloads.LeadingIFormatProvider(str1)       
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

    Friend Function LeadingIFormatProviderReturningString(format As String) As String
        Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
    End Function

    Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
        Return String.Format(provider, format)
    End Function

End Module",
            GetCA1305BasicDefaultResultAt(11, 16, MessageAlternateString,
                                                  "Friend Function LeadingIFormatProviderReturningString(format As String) As String",
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "Friend Function LeadingIFormatProviderReturningString(provider As System.IFormatProvider, format As String) As String"),
           GetCA1305BasicDefaultResultAt(13, 9, MessageAlternate,
                                                  "Friend Sub LeadingIFormatProvider(s As String)",
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "Friend Sub LeadingIFormatProvider(provider As System.IFormatProvider, s As String)"));
        }

        [Fact]
        public void CA1305IFormatProvider_Trailing()
        {

            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {

        string str2 = strB;
        
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2);
        
        IFormatProviderOverloads.TrailingIFormatProvider(str2);

         }

    internal static class IFormatProviderOverloads
    {
        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
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
           GetCA1305CSharpDefaultResultAt(12, 16, MessageAlternateString,
                                                 "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string, System.IFormatProvider)"),
           GetCA1305CSharpDefaultResultAt(14, 9, MessageAlternate,
                                                 "C.IFormatProviderOverloads.TrailingIFormatProvider(string)",
                                                 "C.M(string, string)",
                                                 "C.IFormatProviderOverloads.TrailingIFormatProvider(string, System.IFormatProvider)"));


            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String, strB As String)
       
        Dim str2 As String = strB
        
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2)
        
        IFormatProviderOverloads.TrailingIFormatProvider(str2)
    End Sub
End Module

Friend Module IFormatProviderOverloads
   Friend Sub TrailingIFormatProvider(format As String)
        TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
        Console.WriteLine(String.Format(provider, format))
    End Sub

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
           GetCA1305BasicDefaultResultAt(12, 16, MessageAlternateString,
                                                 "Friend Function TrailingIFormatProviderReturningString(format As String) As String",
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "Friend Function TrailingIFormatProviderReturningString(format As String, provider As System.IFormatProvider) As String"),
           GetCA1305BasicDefaultResultAt(14, 9, MessageAlternate,
                                                 "Friend Sub TrailingIFormatProvider(format As String)",
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "Friend Sub TrailingIFormatProvider(format As String, provider As System.IFormatProvider)"));
        }

        [Fact]
        public void CA1305ConvertToInt()
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
    }
}",
            GetCA1305CSharpDefaultResultAt(9, 19, MessageAlternate,
                                                  "System.Convert.ToInt32(string)",
                                                  "C.M(string)",
                                                  "System.Convert.ToInt32(string, System.IFormatProvider)"),
            GetCA1305CSharpDefaultResultAt(10, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "System.Convert.ToInt64(string)",
                                                  "C.M(string)",
                                                  "System.Convert.ToInt64(string, System.IFormatProvider)"));
            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String)
        Dim i0 As Integer = Convert.ToInt32(strA)
        Dim l0 As Long =    Convert.ToInt64(strA)
    End Sub
End Module",
           GetCA1305BasicDefaultResultAt(9, 29, MessageAlternate,
                                                "Public Shared Overloads Function ToInt32(value As String) As Integer",
                                                "Public Sub M(strA As String)",
                                                "Public Shared Overloads Function ToInt32(value As String, provider As System.IFormatProvider) As Integer"),
           GetCA1305BasicDefaultResultAt(10, 29, MessageAlternate,
                                                "Public Shared Overloads Function ToInt64(value As String) As Long",
                                                "Public Sub M(strA As String)",
                                                "Public Shared Overloads Function ToInt64(value As String, provider As System.IFormatProvider) As Long"));
        }

        [Fact]
        public void CA1305IntParse()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA)
    {
        

        int i1 =  Int32.Parse(strA);

        int i2 =  Int32.Parse(strA, NumberStyles.HexNumber);
    }
}",
            GetCA1305CSharpDefaultResultAt(11, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "int.Parse(string)",
                                                  "C.M(string)",
                                                  "int.Parse(string, System.IFormatProvider)"),
             GetCA1305CSharpDefaultResultAt(13, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "int.Parse(string, System.Globalization.NumberStyles)",
                                                  "C.M(string)",
                                                  "int.Parse(string, System.Globalization.NumberStyles, System.IFormatProvider)"));
            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String)
        

        Dim i1 As Integer = Int32.Parse(strA)
        
        Dim i2 As Integer = Int32.Parse(strA, NumberStyles.HexNumber)
    End Sub
End Module",
          GetCA1305BasicDefaultResultAt(11, 29, MessageAlternate,
                                                "Public Shared Overloads Function Parse(s As String) As Integer",
                                                "Public Sub M(strA As String)",
                                                "Public Shared Overloads Function Parse(s As String, provider As System.IFormatProvider) As Integer"),
          GetCA1305BasicDefaultResultAt(13, 29, MessageAlternate,
                                                "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles) As Integer",
                                                "Public Sub M(strA As String)",
                                                "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles, provider As System.IFormatProvider) As Integer"));

        }

        [Fact]
        public void CA1305LongParse()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA)
    {
        

        
        long l1 = Int64.Parse(strA);
        
        long l2 = Int64.Parse(strA, NumberStyles.HexNumber);

    }
}",
           GetCA1305CSharpDefaultResultAt(12, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "long.Parse(string)",
                                                  "C.M(string)",
                                                  "long.Parse(string, System.IFormatProvider)"),
           GetCA1305CSharpDefaultResultAt(14, 19, SystemGlobalizationAnalyzersResources.SpecifyIFormatProviderDiagnosisAlternate,
                                                  "long.Parse(string, System.Globalization.NumberStyles)",
                                                  "C.M(string)",
                                                  "long.Parse(string, System.Globalization.NumberStyles, System.IFormatProvider)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String)
        


        Dim l1 As Long =    Int64.Parse(strA)
        
        Dim l2 As Long =    Int64.Parse(strA, NumberStyles.HexNumber)

    End Sub
End Module",
            GetCA1305BasicDefaultResultAt(12, 29, MessageAlternate,
                                                 "Public Shared Overloads Function Parse(s As String) As Long",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function Parse(s As String, provider As System.IFormatProvider) As Long"),
            GetCA1305BasicDefaultResultAt(14, 29, MessageAlternate,
                                                 "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles) As Long",
                                                 "Public Sub M(strA As String)",
                                                 "Public Shared Overloads Function Parse(s As String, style As System.Globalization.NumberStyles, provider As System.IFormatProvider) As Long"));
        }

        [Fact]
        public void CA1305StringFormat()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = string.Format(Thread.CurrentThread.CurrentUICulture, ""Foo {0}"", strA);
        string str2 = string.Format(CultureInfo.InstalledUICulture, ""Foo {0} {1}"", strA, strB);
    }
 
}",
            GetCA1305CSharpDefaultResultAt(10, 23, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "Thread.CurrentThread.CurrentUICulture",
                                                  "string.Format(System.IFormatProvider, string, object)"),
            GetCA1305CSharpDefaultResultAt(11, 23, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "string.Format(System.IFormatProvider, string, object, object)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Threading


Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = string.Format(Thread.CurrentThread.CurrentUICulture, ""Foo {0}"", strA)
        Dim str2 As String = string.Format(CultureInfo.InstalledUICulture, ""Foo {0} {1}"", strA, strB)
    End Sub
End Module
",
            GetCA1305BasicDefaultResultAt(9, 30, MessageUICultureString,
                                                 "Public Sub M(strA As String, strB As String)",
                                                 "Thread.CurrentThread.CurrentUICulture",
                                                 "Public Shared Overloads Function Format(provider As System.IFormatProvider, format As String, arg0 As Object) As String"),
            GetCA1305BasicDefaultResultAt(10, 30, MessageUICultureString,
                                                  "Public Sub M(strA As String, strB As String)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "Public Shared Overloads Function Format(provider As System.IFormatProvider, format As String, arg0 As Object, arg1 As Object) As String"));
        }

        [Fact]
        public void CA1305IFormatProvider_LeadingCultureInfo()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = strA;
        string str2 = strB;
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.CurrentUICulture, str1);
         
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.CurrentUICulture, str1);
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

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }
    }
}",
            GetCA1305CSharpDefaultResultAt(12, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "CultureInfo.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(System.IFormatProvider, string)"),
            GetCA1305CSharpDefaultResultAt(14, 9, MessageUICulture,
                                                  "C.M(string, string)",
                                                  "CultureInfo.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.LeadingIFormatProvider(System.IFormatProvider, string)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Threading


Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = strA
        Dim str2 As String = strB
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.CurrentUICulture, str1)

        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.CurrentUICulture, str1)
    End Sub
End Module

Friend Module IFormatProviderOverloads
    Friend Sub LeadingIFormatProvider(s As String)
        LeadingIFormatProvider(CultureInfo.CurrentCulture, s)
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
    Friend Function LeadingIFormatProviderReturningString(format As String) As String
        Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
    End Function
    Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
        Return String.Format(provider, format)
    End Function
End Module",
            GetCA1305BasicDefaultResultAt(11, 16, MessageUICultureString,
                                        "Public Sub M(strA As String, strB As String)",
                                        "CultureInfo.CurrentUICulture",
                                        "Friend Function LeadingIFormatProviderReturningString(provider As System.IFormatProvider, format As String) As String"),
            GetCA1305BasicDefaultResultAt(13, 9, MessageUICulture,
                                        "Public Sub M(strA As String, strB As String)",
                                        "CultureInfo.CurrentUICulture",
                                        "Friend Sub LeadingIFormatProvider(provider As System.IFormatProvider, s As String)"));
        }
        [Fact]
        public void CA1305IFormatProvider_TrailingCultureInfo()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = strA;
        string str2 = strB;
       
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentUICulture);
        
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentUICulture);
    }

    internal static class IFormatProviderOverloads
    {

        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
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
           GetCA1305CSharpDefaultResultAt(13, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "CultureInfo.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string, System.IFormatProvider)"),
           GetCA1305CSharpDefaultResultAt(15, 9, MessageUICulture,
                                                  "C.M(string, string)",
                                                  "CultureInfo.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.TrailingIFormatProvider(string, System.IFormatProvider)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Threading


Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = strA
        Dim str2 As String = strB
        
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentUICulture)
        
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentUICulture)
    End Sub
End Module

Friend Module IFormatProviderOverloads
    Friend Sub TrailingIFormatProvider(format As String)
        TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    Friend Function TrailingIFormatProviderReturningString(format As String) As String
        Return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture)
    End Function

    Friend Function TrailingIFormatProviderReturningString(format As String, provider As IFormatProvider) As String
        Return String.Format(provider, format)
    End Function

    ' compilation error: default needs to be compile-time constant
    Friend Sub TrailingIFormatProvider2(format As String, Optional provider As IFormatProvider = Thread.CurrentThread.CurrentUICulture)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    ' compilation error: default needs to be compile-time constant
    Friend Function TrailingIFormatProviderReturningString2(format As String, Optional provider As IFormatProvider = CultureInfo.InstalledUICulture) As String
        Return String.Format(provider, format)
    End Function
End Module",
            GetCA1305BasicDefaultResultAt(12, 16, MessageUICultureString,
                                    "Public Sub M(strA As String, strB As String)",
                                    "CultureInfo.CurrentUICulture",
                                    "Friend Function TrailingIFormatProviderReturningString(format As String, provider As System.IFormatProvider) As String"),
            GetCA1305BasicDefaultResultAt(14, 9, MessageUICulture,
                                    "Public Sub M(strA As String, strB As String)",
                                    "CultureInfo.CurrentUICulture",
                                    "Friend Sub TrailingIFormatProvider(format As String, provider As System.IFormatProvider)"));
        }

        [Fact]
        public void CA1305IFormatProvider_LeadingReturningString()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = strA;
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(Thread.CurrentThread.CurrentUICulture, str1);
        IFormatProviderOverloads.LeadingIFormatProvider(Thread.CurrentThread.CurrentUICulture, str1);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InstalledUICulture, str1);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InstalledUICulture, str1);
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

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }
    }
}",
           GetCA1305CSharpDefaultResultAt(11, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "Thread.CurrentThread.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(System.IFormatProvider, string)"),
           GetCA1305CSharpDefaultResultAt(12, 9, MessageUICulture,
                                                  "C.M(string, string)",
                                                  "Thread.CurrentThread.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.LeadingIFormatProvider(System.IFormatProvider, string)"),
           GetCA1305CSharpDefaultResultAt(13, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "C.IFormatProviderOverloads.LeadingIFormatProviderReturningString(System.IFormatProvider, string)"),
           GetCA1305CSharpDefaultResultAt(14, 9, MessageUICulture,
                                                  "C.M(string, string)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "C.IFormatProviderOverloads.LeadingIFormatProvider(System.IFormatProvider, string)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Threading

Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = strA
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(Thread.CurrentThread.CurrentUICulture, str1)
        IFormatProviderOverloads.LeadingIFormatProvider(Thread.CurrentThread.CurrentUICulture, str1)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InstalledUICulture, str1)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InstalledUICulture, str1)

    End Sub
End Module

Friend Module IFormatProviderOverloads
    Friend Sub LeadingIFormatProvider(s As String)
        LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        End Sub

    Friend Sub LeadingIFormatProvider(provider As IFormatProvider, s As String)
        Console.WriteLine(String.Format(provider, s));
        End Sub

    Friend Sub LeadingIFormatProvider2(s As String)
        LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        End Sub

    Private Sub LeadingIFormatProvider2(provider As IFormatProvider, s As String)
        Console.WriteLine(String.Format(provider, s))
    End Sub

    Friend Function LeadingIFormatProviderReturningString(format As String) As String
        Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
    End Function

    Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
        Return String.Format(provider, format)
    End Function

End Module",

            GetCA1305BasicDefaultResultAt(9, 16, MessageUICultureString,
                                    "Public Sub M(strA As String, strB As String)",
                                    "Thread.CurrentThread.CurrentUICulture",
                                    "Friend Function LeadingIFormatProviderReturningString(provider As System.IFormatProvider, format As String) As String"),
            GetCA1305BasicDefaultResultAt(10, 9, MessageUICulture,
                                    "Public Sub M(strA As String, strB As String)",
                                    "Thread.CurrentThread.CurrentUICulture",
                                    "Friend Sub LeadingIFormatProvider(provider As System.IFormatProvider, s As String)"),
            GetCA1305BasicDefaultResultAt(11, 16, MessageUICultureString,
                                    "Public Sub M(strA As String, strB As String)",
                                    "CultureInfo.InstalledUICulture",
                                    "Friend Function LeadingIFormatProviderReturningString(provider As System.IFormatProvider, format As String) As String"),
            GetCA1305BasicDefaultResultAt(12, 9, MessageUICulture,
                                    "Public Sub M(strA As String, strB As String)",
                                    "CultureInfo.InstalledUICulture",
                                    "Friend Sub LeadingIFormatProvider(provider As System.IFormatProvider, s As String)"));
        }
        [Fact]
        public void CA1305IFormatProvider_TrailingReturningString()
        {
            VerifyCSharp(@"
using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str2 = strB;
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, Thread.CurrentThread.CurrentUICulture);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, Thread.CurrentThread.CurrentUICulture);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.InstalledUICulture);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.InstalledUICulture);
        
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
    }

    internal static class IFormatProviderOverloads
    {
        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
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
           GetCA1305CSharpDefaultResultAt(11, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "Thread.CurrentThread.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string, System.IFormatProvider)"),
           GetCA1305CSharpDefaultResultAt(12, 9, MessageUICulture,
                                                  "C.M(string, string)",
                                                  "Thread.CurrentThread.CurrentUICulture",
                                                  "C.IFormatProviderOverloads.TrailingIFormatProvider(string, System.IFormatProvider)"),
           GetCA1305CSharpDefaultResultAt(13, 16, MessageUICultureString,
                                                  "C.M(string, string)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "C.IFormatProviderOverloads.TrailingIFormatProviderReturningString(string, System.IFormatProvider)"),
          GetCA1305CSharpDefaultResultAt(14, 9, MessageUICulture,
                                                  "C.M(string, string)",
                                                  "CultureInfo.InstalledUICulture",
                                                  "C.IFormatProviderOverloads.TrailingIFormatProvider(string, System.IFormatProvider)"));

            VerifyBasic(@"
Imports System
Imports System.Globalization
Imports System.Threading

Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str2 As String = strB
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, Thread.CurrentThread.CurrentUICulture)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, Thread.CurrentThread.CurrentUICulture)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.InstalledUICulture)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.InstalledUICulture)
        
        IFormatProviderOverloads.TrailingIFormatProvider2(str2)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2)
    End Sub
End Module

Friend Module IFormatProviderOverloads
    Friend Sub TrailingIFormatProvider(format As String)
        TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    Friend Function TrailingIFormatProviderReturningString(format As String) As String
        Return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture)
    End Function

    Friend Function TrailingIFormatProviderReturningString(format As String, provider As IFormatProvider) As String
        Return String.Format(provider, format)
    End Function

    ' compilation error: default needs to be compile-time constant
    Friend Sub TrailingIFormatProvider2(format As String, Optional provider As IFormatProvider = Thread.CurrentThread.CurrentUICulture)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    ' compilation error: default needs to be compile-time constant
    Friend Function TrailingIFormatProviderReturningString2(format As String, Optional provider As IFormatProvider = CultureInfo.InstalledUICulture) As String
        Return String.Format(provider, format)
    End Function
End Module",

           GetCA1305BasicDefaultResultAt(9, 16, MessageUICultureString,
                                    "Public Sub M(strA As String, strB As String)",
                                    "Thread.CurrentThread.CurrentUICulture",
                                    "Friend Function TrailingIFormatProviderReturningString(format As String, provider As System.IFormatProvider) As String"),
           GetCA1305BasicDefaultResultAt(10, 9, MessageUICulture,
                                    "Public Sub M(strA As String, strB As String)",
                                    "Thread.CurrentThread.CurrentUICulture",
                                    "Friend Sub TrailingIFormatProvider(format As String, provider As System.IFormatProvider)"),
           GetCA1305BasicDefaultResultAt(11, 16, MessageUICultureString,
                                    "Public Sub M(strA As String, strB As String)",
                                    "CultureInfo.InstalledUICulture",
                                    "Friend Function TrailingIFormatProviderReturningString(format As String, provider As System.IFormatProvider) As String"),
           GetCA1305BasicDefaultResultAt(12, 9, MessageUICulture,
                                    "Public Sub M(strA As String, strB As String)",
                                    "CultureInfo.InstalledUICulture",
                                    "Friend Sub TrailingIFormatProvider(format As String, provider As System.IFormatProvider)"));
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
