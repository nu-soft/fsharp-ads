namespace System.Reflection

  [<AutoOpen>]
  module Extensions =
    open System.Runtime.InteropServices
    
    let maa = typedefof<MarshalAsAttribute>
    
    let GetCustomAttribute = 
      maa.GetMethods(BindingFlags.Static ||| BindingFlags.NonPublic) 
      |> List.ofArray 
      |> List.tryFind (fun mi -> 
        let parameters = mi.GetParameters()
        mi.Name = "GetCustomAttribute" 
        && parameters.Length = 1 
        && parameters.[0].ParameterType.Name = "RuntimeFieldInfo"
        )

    type FieldInfo with
      member self.GetMarshalAsAttribute () =
        match GetCustomAttribute with
          | Some(gcaMi) ->
            let attr = gcaMi.Invoke(null, [| self |])
            match attr with
            | null -> None
            | _ -> Some(attr :?> MarshalAsAttribute)
          | None -> None

namespace Ploeh.AutoFixture

  [<AutoOpen>]
  module TypeBuilders =

    open System.Reflection
    open System.Runtime.InteropServices
    open System
    open Ploeh.AutoFixture.Kernel

    let nospec () =
      new NoSpecimen() :> obj

    let MarshalAsString (context: ISpecimenContext) (field: FieldInfo) =
      let attr = field.GetMarshalAsAttribute()
      match attr with
        | Some(maa) when maa.Value = UnmanagedType.ByValTStr && maa.SizeConst > 0 -> 
            let result = context.Resolve(typedefof<string>).ToString()
            result.Substring(0,Math.Min(result.Length,maa.SizeConst-1)) :> obj
        | _ -> nospec()
    
    let MarshalAsArray (context: ISpecimenContext) (field: FieldInfo) =
      let attr = field.GetMarshalAsAttribute()
      match attr with
        | Some(maa) -> 
          let result = Array.CreateInstance(field.FieldType.GetElementType(), maa.SizeConst)
          Array.Copy(Array.init maa.SizeConst (fun _ -> context.Resolve(field.FieldType.GetElementType())), result,maa.SizeConst)
          result :> obj
        | _ -> nospec()


    let MarshalAsTypeBuilder =
      {
        new ISpecimenBuilder with
        member self.Create(request, context) =

          let getString = MarshalAsString context
          let getArray = MarshalAsArray context
          
          match request with
            | :? ParameterInfo as pi when pi.ParameterType.IsArray ->
              pi.Member.DeclaringType.GetField(pi.Name+"@", BindingFlags.Instance ||| BindingFlags.NonPublic)
              |> getArray
            | :? ParameterInfo as pi when pi.ParameterType = typedefof<string> ->
              pi.Member.DeclaringType.GetField(pi.Name+"@", BindingFlags.Instance ||| BindingFlags.NonPublic) 
              |> getString
          
            | :? FieldInfo as fi when fi.FieldType.IsArray -> 
              let attr = fi.GetMarshalAsAttribute()
              match attr with
                | Some(maa) -> 
                  let result = Array.CreateInstance(fi.FieldType.GetElementType(), maa.SizeConst)
                  Array.Copy(Array.init maa.SizeConst (fun _ -> context.Resolve(fi.FieldType.GetElementType())), result,maa.SizeConst)
                  result :> obj
                | _ -> nospec()
          
            | :? FieldInfo as fi when fi.FieldType = typedefof<string> ->
              let attr = fi.GetMarshalAsAttribute()
              match attr with
                | Some(maa) when maa.Value = UnmanagedType.ByValTStr && maa.SizeConst > 0 -> 
                    let result = context.Resolve(typedefof<string>).ToString()
                    result.Substring(0,Math.Min(result.Length,maa.SizeConst-1)) :> obj
                | _ -> nospec()
          
            | :? Type as t when 
              t.IsValueType 
              && t.IsPrimitive |> not
              && Array.contains t 
                [|
                  typedefof<string>;
                  typedefof<decimal>;
                  typedefof<DateTime>;
                  typedefof<DateTimeOffset>;
                  typedefof<TimeSpan>
                |] |> not ->
                  let instance = Activator.CreateInstance(t)
              
                  t.GetFields(BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Public)
                  |> Array.map (fun fi -> fi,context.Resolve(fi))
                  |> Array.iter (fun (fi, value) -> 
                    fi.SetValue(instance, value)
                  )
                  instance
            | _ -> nospec()
      }