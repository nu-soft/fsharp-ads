namespace NuSoft.Ads.Experimental

  /// <summary>An abbreviation for the CLI type <c>System.Boolean</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type BOOL.</remarks>
  type BOOL   = bool
  /// <summary>An abbreviation for the CLI type <c>System.Byte</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type BYTE.</remarks>
  type BYTE   = byte
  /// <summary>An abbreviation for the CLI type <c>System.UInt16</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type WORD.</remarks>
  type WORD   = uint16
  /// <summary>An abbreviation for the CLI type <c>System.UInt32</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type DWORD.</remarks>
  type DWORD  = uint32
  /// <summary>An abbreviation for the CLI type <c>System.SByte</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type SINT.</remarks>
  type SINT   = sbyte
  /// <summary>An abbreviation for the CLI type <c>System.Byte</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type USINT.</remarks>
  type USINT  = byte
  /// <summary>An abbreviation for the CLI type <c>System.Int16</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type INT.</remarks>
  type INT    = int16
  /// <summary>An abbreviation for the CLI type <c>System.UInt16</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type UINT.</remarks>
  type UINT   = uint16
  /// <summary>An abbreviation for the CLI type <c>System.Int32</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type DINT.</remarks>
  type DINT   = int32
  /// <summary>An abbreviation for the CLI type <c>System.UInt32</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type UDINT.</remarks>
  type UDINT  = uint32
  /// <summary>An abbreviation for the CLI type <c>System.Int64</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type LINT.</remarks>
  type LINT   = int64
  /// <summary>An abbreviation for the CLI type <c>System.UInt64</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type ULINT.</remarks>
  type ULINT  = uint64
  /// <summary>An abbreviation for the CLI type <c>System.Single</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type REAL.</remarks>
  type REAL   = float32
  /// <summary>An abbreviation for the CLI type <c>System.Double</c>.</summary>
  /// <remarks>An abbreviation for the IEC 61131 type LREAL.</remarks>
  type LREAL  = float

  [<AutoOpen>]
  module Operators =
  
    /// <summary>Converts the argument to unsigned 8-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Byte.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted BYTE</returns>
    [<CompiledName("ToBYTE")>]
    val inline BYTE : value:^T -> BYTE        when ^T : (static member op_Explicit : ^T -> BYTE)        
    
    /// <summary>Converts the argument to unsigned 16-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>UInt16.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted WORD</returns>
    [<CompiledName("ToWORD")>]
    val inline WORD : value:^T -> WORD        when ^T : (static member op_Explicit : ^T -> WORD)        
    
    /// <summary>Converts the argument to unsigned 32-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>UInt32.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted DWORD</returns>
    [<CompiledName("ToDWORD")>]
    val inline DWORD : value:^T -> DWORD        when ^T : (static member op_Explicit : ^T -> DWORD)        
    
    /// <summary>Converts the argument to signed 8-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>SByte.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted SINT</returns>
    [<CompiledName("ToSINT")>]
    val inline SINT : value:^T -> SINT        when ^T : (static member op_Explicit : ^T -> SINT)        
    
    /// <summary>Converts the argument to unsigned 8-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Byte.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted USINT</returns>
    [<CompiledName("ToUSINT")>]
    val inline USINT : value:^T -> USINT        when ^T : (static member op_Explicit : ^T -> USINT)        
    
    /// <summary>Converts the argument to signed 16-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Int16.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted INT</returns>
    [<CompiledName("ToINT")>]
    val inline INT : value:^T -> INT        when ^T : (static member op_Explicit : ^T -> INT)        
    
    /// <summary>Converts the argument to unsigned 16-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>UInt16.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted UINT</returns>
    [<CompiledName("ToUINT")>]
    val inline UINT : value:^T -> UINT        when ^T : (static member op_Explicit : ^T -> UINT)        
    
    /// <summary>Converts the argument to signed 32-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Int32.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted DINT</returns>
    [<CompiledName("ToDINT")>]
    val inline DINT : value:^T -> DINT        when ^T : (static member op_Explicit : ^T -> DINT)        
    
    /// <summary>Converts the argument to unsigned 32-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>UInt32.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted UDINT</returns>
    [<CompiledName("ToUDINT")>]
    val inline UDINT : value:^T -> UDINT        when ^T : (static member op_Explicit : ^T -> UDINT)        
    
    /// <summary>Converts the argument to signed 64-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Int64.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted LINT</returns>
    [<CompiledName("ToLINT")>]
    val inline LINT : value:^T -> LINT        when ^T : (static member op_Explicit : ^T -> LINT)        
    
    /// <summary>Converts the argument to unsigned 64-bit integer. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>UInt64.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted ULINT</returns>
    [<CompiledName("ToULINT")>]
    val inline ULINT : value:^T -> ULINT        when ^T : (static member op_Explicit : ^T -> ULINT)        
    
    /// <summary>Converts the argument to unsigned 32-bit float. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Single.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted REAL</returns>
    [<CompiledName("ToREAL")>]
    val inline REAL : value:^T -> REAL        when ^T : (static member op_Explicit : ^T -> REAL)        
    
    /// <summary>Converts the argument to unsigned 64-bit float. This is a direct conversion for all 
    /// primitive numeric types. For strings, the input is converted using <c>Double.Parse()</c>  
    /// with InvariantCulture settings. Otherwise the operation requires an appropriate
    /// static conversion method on the input type.</summary>
    /// <param name="value">The input value.</param>
    /// <returns>The converted LREAL</returns>
    [<CompiledName("ToLREAL")>]
    val inline LREAL : value:^T -> LREAL        when ^T : (static member op_Explicit : ^T -> LREAL)        

  
    /// <summary>The type of 8-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Byte</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type BYTE<[<Measure>] 'Measure>   = BYTE 

    /// <summary>The type of 16-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.UInt16</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type WORD<[<Measure>] 'Measure>   = WORD 

    /// <summary>The type of 32-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.UInt32</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type DWORD<[<Measure>] 'Measure>  = DWORD

    /// <summary>The type of 8-bit signed integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.SByte</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type SINT<[<Measure>] 'Measure>   = SINT 

    /// <summary>The type of 8-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Byte</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type USINT<[<Measure>] 'Measure>  = USINT

    /// <summary>The type of 16-bit signed integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Int16</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type INT<[<Measure>] 'Measure>    = INT  

    /// <summary>The type of 16-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.UInt16</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type UINT<[<Measure>] 'Measure>   = UINT 

    /// <summary>The type of 32-bit signed integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Int32</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type DINT<[<Measure>] 'Measure>   = DINT 

    /// <summary>The type of 32-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.UInt32</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type UDINT<[<Measure>] 'Measure>  = UDINT

    /// <summary>The type of 64-bit signed integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Int64</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type LINT<[<Measure>] 'Measure>   = LINT 

    /// <summary>The type of 32-bit signed integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.UInt64</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type ULINT<[<Measure>] 'Measure>  = ULINT

    /// <summary>The type of 64-bit unsigned integer numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Single</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type REAL<[<Measure>] 'Measure>   = REAL 

    /// <summary>The type of floating point numbers, annotated with a unit of measure. The unit
    /// of measure is erased in compiled code and when values of this type
    /// are analyzed using reflection. The type is representationally equivalent to 
    /// <c>System.Double</c>.</summary>
    [<MeasureAnnotatedAbbreviation>] type LREAL<[<Measure>] 'Measure>  = LREAL
    
  [<AutoOpen>]
  module MeasureOperators =

    /// <summary>Creates a BYTE value with units-of-measure</summary>
    /// <param name="BYTE">The input BYTE.</param>
    /// <returns>The BYTE with units-of-measure.</returns>
    val inline BYTEWithMeasure   : BYTE  -> BYTE<'Measure>
    
    /// <summary>Creates a BYTE value with units-of-measure</summary>
    /// <param name="WORD">The input WORD.</param>
    /// <returns>The WORD with units-of-measure.</returns>
    val inline WORDWithMeasure   : WORD  -> WORD<'Measure>
    
    /// <summary>Creates a BYTE value with units-of-measure</summary>
    /// <param name="DWORD">The input DWORD.</param>
    /// <returns>The DWORD with units-of-measure.</returns>
    val inline DWORDWithMeasure  : DWORD -> DWORD<'Measure> 
    
    /// <summary>Creates a BYTE value with units-of-measure</summary>
    /// <param name="SINT">The input SINT.</param>
    /// <returns>The SINT with units-of-measure.</returns>
    val inline SINTWithMeasure   : SINT  -> SINT<'Measure> 
    
    /// <summary>Creates a BYTE value with units-of-measure</summary>
    /// <param name="USINT">The input USINT.</param>
    /// <returns>The USINT with units-of-measure.</returns>
    val inline USINTWithMeasure  : USINT -> USINT<'Measure>
    
    /// <summary>Creates a INT value with units-of-measure</summary>
    /// <param name="INT">The input INT.</param>
    /// <returns>The INT with units-of-measure.</returns>
    val inline INTWithMeasure    : INT   -> INT<'Measure> 
    
    /// <summary>Creates a UINT value with units-of-measure</summary>
    /// <param name="UINT">The input UINT.</param>
    /// <returns>The UINT with units-of-measure.</returns>
    val inline UINTWithMeasure   : UINT  -> UINT<'Measure>
    
    /// <summary>Creates a DINT value with units-of-measure</summary>
    /// <param name="DINT">The input DINT.</param>
    /// <returns>The DINT with units-of-measure.</returns>
    val inline DINTWithMeasure   : DINT  -> DINT<'Measure>
    
    /// <summary>Creates a UDINT value with units-of-measure</summary>
    /// <param name="UDINT">The input UDINT.</param>
    /// <returns>The UDINT with units-of-measure.</returns>
    val inline UDINTWithMeasure  : UDINT -> UDINT<'Measure>
    
    /// <summary>Creates a LINT value with units-of-measure</summary>
    /// <param name="LINT">The input LINT.</param>
    /// <returns>The LINT with units-of-measure.</returns>
    val inline LINTWithMeasure   : LINT  -> LINT<'Measure>
    
    /// <summary>Creates a ULINT value with units-of-measure</summary>
    /// <param name="ULINT">The input ULINT.</param>
    /// <returns>The ULINT with units-of-measure.</returns> 
    val inline ULINTWithMeasure  : ULINT -> ULINT<'Measure>
    
    /// <summary>Creates a REAL value with units-of-measure</summary>
    /// <param name="REAL">The input REAL.</param>
    /// <returns>The REAL with units-of-measure.</returns>
    val inline REALWithMeasure   : REAL  -> REAL<'Measure> 
    
    /// <summary>Creates a LREAL value with units-of-measure</summary>
    /// <param name="LREAL">The input LREAL.</param>
    /// <returns>The LREAL with units-of-measure.</returns>
    val inline LREALWithMeasure  : LREAL -> LREAL<'Measure>

  [<AutoOpen>]
  module Workarounds =

    [<Struct(*;StructLayout(LayoutKind.Sequential, Pack=1)*)>]
    type BOOLSTRUCT  = 
      val value: BOOL

      new : BOOL -> BOOLSTRUCT

      
    [<CompiledName("ToBool")>]
    val inline BOOL : BOOL -> BOOLSTRUCT
    
    [<CompiledName("FromBool")>]
    val inline BOOLSTRUCT : BOOLSTRUCT -> BOOL