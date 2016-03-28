namespace TwinCAT.Ads
  
  type AdsResult<'a> =
    | Success of 'a
    | Error of string

  module AdsResult =
    let isSuccess = function
      | Success _ -> true
      | Error _ -> false
    let isError a =  isSuccess a |> not
    let ofSuccess = function
      | Success e -> e
      | _ -> failwith "Not a success"

  type Ams = {
    NetId: string
    Port: int
  }
  type AdsHandleType =
    | Struct 
    | Array of int
    | StringArray of int * int
    | String of int 

  type AdsHandle = {
    Handle: int
    SymName: string
    Type: AdsHandleType
  }




  module Patterns = 
    open System.Text.RegularExpressions
    let re = new Regex(@"^ARRAY \[.*\] OF ([A-Z]+)(?:\((\d+)\))?$")
    let (|ByteSize|_|) input = 
      match re.Match input with
        | m when m.Success ->
          match [for g in m.Groups -> g.Value] |> List.tail with
            | "BOOL"::[""] | "BYTE"::[""] | "SINT"::[""]  | "USINT"::[""] -> Some 1
            | "WORD"::[""] | "INT"::[""] | "UINT"::[""]  -> Some 2
            | "DWORD"::[""] | "DINT"::[""] | "UDINT"::[""] | "REAL"::[""] | "TOD"::[""] | "DT"::[""] | "TIME"::[""] | "DATE"::[""] -> Some 4
            | "LREAL"::[""]  -> Some 8
            | "STRING"::len::[] -> len |> int |> (+) 1 |> Some
            | g -> None
        | m -> None
  
  module Internal =
    open System.Runtime.InteropServices

    type AdsType = 
      | Struct of AdsType list
      | Array of AdsType * int
      | String of int
      | Number of System.Type * UnmanagedType
      | Time 
      | Date 
  
  [<AutoOpen>]
  module Expressions =
    open TwinCAT.Ads
    open System.Collections.Generic
    open Patterns
    open TwinCAT.Ads.Internal
    open NuSoft.Ads.Experimental
    open System.Runtime.InteropServices

    type TwinCatBuilder() =

      let mutable storedClient:TcAdsClient = null
      let mutable infoLoader: TcAdsSymbolInfoLoader = null
      let handles = new Dictionary<string,AdsHandle>()
      let types = new Dictionary<System.Type,System.Type>()
      member self.Bind(ams, f) =
        try
          let client = new TcAdsClient()
          client.Connect(ams.NetId, ams.Port)
          match client.ReadState().AdsState with
            | AdsState.Run -> 
              storedClient <- client
              infoLoader <- storedClient.CreateSymbolInfoLoader()
              f client
            | e -> sprintf "Ads State is %A" e |> Error
        with 
          | :? AdsErrorException as adsErr when adsErr.ErrorCode=AdsErrorCode.TargetMachineNotFound -> sprintf "Target machine not found at %s:%i" ams.NetId ams.Port |> Error
          | :? AdsErrorException as adsErr -> sprintf "Unexpected ADS error %A" adsErr |> Error
          | e -> sprintf "Unexpected error %A" e |> Error

      member self.typeGen t (handle: AdsHandle) =
        
        let symInfo = infoLoader.FindSymbol handle.SymName
        let si = 
          [
            for i in symInfo.SubSymbols do
              yield 
                match i.Datatype,i.SubSymbolCount with
                  | AdsDatatypeId.ADST_BIT,0 -> Internal.AdsType.Number (typedefof<BOOL>,UnmanagedType.U1)
                  | AdsDatatypeId.ADST_INT16,0 -> Internal.AdsType.Number (typedefof<INT>,UnmanagedType.I2)
                  | AdsDatatypeId.ADST_INT32,0 -> Internal.AdsType.Number (typedefof<DINT>,UnmanagedType.I4)
                  | AdsDatatypeId.ADST_INT64,0 -> Internal.AdsType.Number (typedefof<LINT>,UnmanagedType.I8)
                  | AdsDatatypeId.ADST_INT8,0 -> Internal.AdsType.Number (typedefof<SINT>,UnmanagedType.I1)
                  | AdsDatatypeId.ADST_REAL32,0 -> Internal.AdsType.Number (typedefof<REAL>,UnmanagedType.R4)
                  | AdsDatatypeId.ADST_REAL64,0 -> Internal.AdsType.Number (typedefof<LREAL>,UnmanagedType.R8)
                  | AdsDatatypeId.ADST_UINT16,0 -> Internal.AdsType.Number (typedefof<WORD>,UnmanagedType.U2)
                  | AdsDatatypeId.ADST_UINT32,0 -> Internal.AdsType.Number (typedefof<DWORD>,UnmanagedType.U4)
                  | AdsDatatypeId.ADST_UINT64,0 -> Internal.AdsType.Number (typedefof<ULINT>,UnmanagedType.U8)
                  | AdsDatatypeId.ADST_UINT8,0 -> Internal.AdsType.Number (typedefof<BYTE>,UnmanagedType.U1)
                  
                  | AdsDatatypeId.ADST_BIT,s -> Internal.AdsType.Array(Internal.AdsType.Number(typedefof<BOOL>,UnmanagedType.U1),s)
                  | AdsDatatypeId.ADST_INT16,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<INT>,UnmanagedType.I2),s)
                  | AdsDatatypeId.ADST_INT32,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<DINT>,UnmanagedType.I4),s)
                  | AdsDatatypeId.ADST_INT64,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<LINT>,UnmanagedType.I8),s)
                  | AdsDatatypeId.ADST_INT8,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<SINT>,UnmanagedType.I1),s)
                  | AdsDatatypeId.ADST_REAL32,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<REAL>,UnmanagedType.R4),s)
                  | AdsDatatypeId.ADST_REAL64,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<LREAL>,UnmanagedType.R8),s)
                  | AdsDatatypeId.ADST_UINT16,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<WORD>,UnmanagedType.U2),s)
                  | AdsDatatypeId.ADST_UINT32,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<DWORD>,UnmanagedType.U4),s)
                  | AdsDatatypeId.ADST_UINT64,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<ULINT>,UnmanagedType.U8),s)
                  | AdsDatatypeId.ADST_UINT8,s -> Internal.AdsType.Array(Internal.AdsType.Number (typedefof<BYTE>,UnmanagedType.U1),s)
                  | AdsDatatypeId.ADST_STRING,s -> Internal.AdsType.Array(Internal.AdsType.String i.Size,s)
                  | AdsDatatypeId.ADST_BIGTYPE,s when i.Type = "TIME" || i.Type = "TOD" -> Internal.AdsType.Array(Internal.AdsType.Time,s)
                  | AdsDatatypeId.ADST_BIGTYPE,s when i.Type = "DATE" || i.Type = "DT" -> Internal.AdsType.Array(Internal.AdsType.Date,s)
          ] |> Internal.AdsType.Struct
        
        t

      member self.getInternalType t handle =
        match types.ContainsKey t with
          | true -> types.[t]
          | _ -> 
            let newt = self.typeGen t handle
            types.[t] <- newt
            
            newt

      member self.createHandle symName  =
        match handles.ContainsKey symName with
          | false -> 
            try
              let handle = storedClient.CreateVariableHandle symName
              let h={
                Handle=handle
                SymName=symName
                Type = 
                  match storedClient.ReadSymbolInfo symName with
                    | symInfo when symInfo.Type.Contains "ARRAY" && symInfo.Type.Contains "STRING" -> 
                      match symInfo.Type with
                        | ByteSize bytes -> StringArray (symInfo.Size/bytes, bytes-1)
                        | _ -> failwithf "Not supported type: %s" symInfo.Type
                    | symInfo when symInfo.Type.Contains "ARRAY" -> 
                      match symInfo.Type with
                        | ByteSize bytes -> symInfo.Size/bytes |> Array
                        | _ -> failwithf "Not supported type: %s" symInfo.Type
                    | symInfo when symInfo.Datatype = AdsDatatypeId.ADST_STRING ->
                      symInfo.Size - 1 |> String 
                    | symInfo -> Struct
              }
              handles.Add(symName,h)
              Success h
              
            with 
              | :? AdsErrorException as e when e.ErrorCode = AdsErrorCode.DeviceSymbolNotFound -> sprintf "Symbol %s could not be found " symName |> Error
              | :? AdsErrorException as e  -> sprintf "Unknown AdsError %A error while creating handle for %s" e symName |> Error
              | e -> sprintf "Unknown error %A while creating handle for %s" e symName |> Error

          | true -> handles.[symName] |> Success

      

      member self.Bind(symName, f: 'T -> AdsResult<TcAdsClient>) =
        let handle = self.createHandle symName
          
        match handle with
          | Success({Handle = h; Type = t}) ->
            try
              match t with
                | Struct when typedefof<IAdsStruct>.IsAssignableFrom(typedefof<'T>) -> 
                  let t = handle |> AdsResult.ofSuccess |> self.getInternalType typedefof<'T> 
                  storedClient.ReadAny(h,t) :?> 'T |> f
                | Struct -> storedClient.ReadAny(h,typedefof<'T>) :?> 'T |> f
                | String length -> storedClient.ReadAny(h, typedefof<'T>, [| length |]) :?> 'T |> f
                | StringArray (arrLength,strLength) -> storedClient.ReadAny(h, typedefof<'T>, [| strLength; arrLength |]) :?> 'T |> f
                | Array size -> storedClient.ReadAny(h, typedefof<'T>, [| size |]) :?> 'T |> f
                | _ -> 
                  sprintf "Attempt to read value at %s: %s as DateTime." "" "" |> Error
            with 
              | :? AdsErrorException as e -> sprintf "Unknown AdsError %A error while creating handle for %s" e symName |> Error
              | e -> sprintf "Unknown error %A while creating handle for %s" e symName |> Error
          | Error e ->  Error e
      
      member self.Bind(symName,f:System.DateTime -> AdsResult<TcAdsClient>) =
        let handle = self.createHandle symName
        match handle with
          | Success({Handle = h; Type = t}) ->
            try
              match t with
                | Struct -> 
                  storedClient.ReadAny(h,typedefof<uint32>) 
                    :?> uint32
                    |> int64
                    |> PlcOpenDateConverterBase.ToDateTime
                    |> f
                | _ -> 
                  sprintf "Attempt to read value at %s: %s as DateTime." "" "" |> Error
            with
              | :? AdsErrorException as e -> sprintf "Unknown AdsError %A error while creating handle for %s" e symName |> Error
              | e -> sprintf "Unknown error %A while creating handle for %s" e symName |> Error
          | Error e ->  Error e
      
      member self.Bind(symName,f:System.TimeSpan -> AdsResult<TcAdsClient>) =
        let handle = self.createHandle symName
        match handle with
          | Success({Handle = h; Type = t}) ->
            try
              match t with
                | Struct -> 
                  storedClient.ReadAny(h,typedefof<uint32>) 
                    :?> uint32
                    |> int64
                    |> PlcOpenTimeConverter.ToTimeSpan
                    |> f
                | _ -> 
                  sprintf "Attempt to read value at %s: %s as DateTime." "" "" |> Error
            with
              | :? AdsErrorException as e -> sprintf "Unknown AdsError %A error while creating handle for %s" e symName |> Error
              | e -> sprintf "Unknown error %A while creating handle for %s" e symName |> Error
          | Error e ->  Error e
          
      
      member self.Bind(symName,f:System.DateTime array -> AdsResult<TcAdsClient>) =
        let handle = self.createHandle symName
        match handle with
          | Success({Handle = h; Type = t}) ->
            try
              match t with
                | Array size -> 
                  storedClient.ReadAny(h,typedefof<uint32 array>, [| size |]) 
                    :?> uint32 array
                    |> Array.map int64
                    |> Array.map PlcOpenDateConverterBase.ToDateTime
                    |> f
                | _ -> 
                  sprintf "Attempt to read value at %s: %s as DateTime." "" "" |> Error
            with
              | :? AdsErrorException as e -> sprintf "Unknown AdsError %A error while creating handle for %s" e symName |> Error
              | e -> sprintf "Unknown error %A while creating handle for %s" e symName |> Error
          | Error e ->  Error e
      
      member self.Bind(symName,f:System.TimeSpan array -> AdsResult<TcAdsClient>) =
        let handle = self.createHandle symName
        match handle with
          | Success({Handle = h; Type = t}) ->
            try
              match t with
                | Array size -> 
                  storedClient.ReadAny(h,typedefof<uint32 array>, [| size |]) 
                    :?> uint32 array
                    |> Array.map int64
                    |> Array.map PlcOpenTimeConverter.ToTimeSpan
                    |> f
                | _ -> 
                  sprintf "Attempt to read value at %s: %s as DateTime." "" "" |> Error
            with
              | :? AdsErrorException as e -> sprintf "Unknown AdsError %A error while creating handle for %s" e symName |> Error
              | e -> sprintf "Unknown error %A while creating handle for %s" e symName |> Error
          | Error e ->  Error e

      member self.Return(x) =
        Success x

    let twincat = new TwinCatBuilder()
