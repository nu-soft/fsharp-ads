namespace System.Reflection
  [<AutoOpen>]
  module Extensions =
    open System.Runtime.InteropServices

    type FieldInfo with
      /// <summary>
      /// returns <c>System.Runtime.InteropServices.MarshalAsAttribute</c> for field, if defined
      /// </summary>
      /// <returns>Some(MarshalAsAttribute) or None</returns>
      member GetMarshalAsAttribute : unit -> MarshalAsAttribute option

namespace Ploeh.AutoFixture

  [<AutoOpen>]
  module TypeBuilders =
    open Ploeh.AutoFixture.Kernel
    val MarshalAsTypeBuilder: ISpecimenBuilder