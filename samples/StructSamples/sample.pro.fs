namespace NuSoft.Ads.Experimental.Samples
open NuSoft.Ads.Experimental
open System.Runtime.InteropServices

#nowarn "9"

open System

type BOOL_WORKAROUND = 
  struct
    [<MarshalAs(UnmanagedType.I1)>]
    val value: BOOL
    new (value) = {
      value = value
    }
  end

[<StructLayout(LayoutKind.Sequential, Pack=1)>]
type STRING_1 =
  struct
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=2)>]
    val value: string
    new (value) = {
      value = value
    }
  end
[<StructLayout(LayoutKind.Sequential, Pack=1)>]
type STRING_256 =
  struct
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=257)>]
    val value: string
    new (value) = {
      value = value
    }
  end
[<StructLayout(LayoutKind.Sequential, Pack=1)>]
type STRING_80 =
  struct
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=81)>]
    val value: string
    new (value) = {
      value = value
    }
  end

type BASIC_STRUCT  =
  struct
    interface IAdsStruct
    val boolVar:BOOL
    val byteVar:BYTE
    val wordVar:WORD
    val dwordVar:DWORD
    val sintVar:SINT
    val intVar:INT
    val dintVar:DINT
    val realVar:REAL
    val lrealVar:LREAL
    val string80Var:string
    val string1Var:string
    val string256Var:string
    

    val timeVar:TimeSpan
    val todVar:TimeSpan
    val dateVar:DateTime
    val dtVar:DateTime
    
    val arrboolVar:bool array
    val arrbyteVar:BYTE array
    val arrwordVar:WORD array
    val arrdwordVar:DWORD array
    val arrsintVar:SINT array
    val arrintVar:INT array
    val arrdintVar:DINT array
    val arrrealVar:REAL array
    val arrlrealVar:LREAL array
    val arrstring80Var:string array
    val arrstring1Var:string array
    val arrstring256Var:string array
    val arrtimeVar:TimeSpan array
    val arrtodVar:TimeSpan  array
    val arrdateVar:DateTime array
    val arrdtVar:DateTime   array
  end

  
[<StructLayout(LayoutKind.Sequential, Pack=1)>]
type BASIC_STRUCT_INTERNAL =
  struct
    [<MarshalAs(UnmanagedType.I1)>]
    val boolVar:BOOL
    val byteVar:BYTE
    val wordVar:WORD
    val dwordVar:DWORD
    val sintVar:SINT
    val intVar:INT
    val dintVar:DINT
    val realVar:REAL
    val lrealVar:LREAL
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=81)>]
    val string80Var:string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=2)>]
    val string1Var:string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=257)>]
    val string256Var:string
    

    val mutable timeVar:WORD
    val mutable todVar:WORD
    val mutable dateVar:WORD
    val mutable dtVar:WORD
    
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=21,ArraySubType=UnmanagedType.Struct)>]
    val mutable arrboolVar:BOOL_WORKAROUND array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=1,ArraySubType=UnmanagedType.U1)>]
    val arrbyteVar:BYTE array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=19,ArraySubType=UnmanagedType.U2)>]
    val arrwordVar:WORD array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=18,ArraySubType=UnmanagedType.U4)>]
    val arrdwordVar:DWORD array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=17,ArraySubType=UnmanagedType.I1)>]
    val arrsintVar:SINT array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=15,ArraySubType=UnmanagedType.I2)>]
    val arrintVar:INT array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=13,ArraySubType=UnmanagedType.I4)>]
    val arrdintVar:DINT array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=11,ArraySubType=UnmanagedType.R4)>]
    val arrrealVar:REAL array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=10,ArraySubType=UnmanagedType.R8)>]
    val arrlrealVar:LREAL array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=9,ArraySubType=UnmanagedType.Struct)>]
    val mutable arrstring80Var:STRING_80 array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=8,ArraySubType=UnmanagedType.Struct)>]
    val mutable arrstring1Var:STRING_1 array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=7,ArraySubType=UnmanagedType.Struct)>]
    val mutable arrstring256Var:STRING_256 array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=6,ArraySubType=UnmanagedType.U4)>]
    val mutable arrtimeVar:WORD array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=5,ArraySubType=UnmanagedType.U4)>]
    val mutable arrtodVar:WORD  array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=4,ArraySubType=UnmanagedType.U4)>]
    val mutable arrdateVar:WORD array
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=3,ArraySubType=UnmanagedType.U4)>]
    val mutable arrdtVar:WORD   array
  end

[<StructLayout(LayoutKind.Sequential, Pack=1)>]
type CONTAINS_STRUCT =
  struct
    val arrStruct:BASIC_STRUCT
  end

[<StructLayout(LayoutKind.Sequential, Pack=1)>]
type CONTAINS_ARRAY_OF_STRUCTS =
  struct
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst=10,ArraySubType=UnmanagedType.Struct)>]
    val stArray:CONTAINS_STRUCT array;
  end