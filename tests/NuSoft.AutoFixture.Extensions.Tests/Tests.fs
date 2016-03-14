namespace NuSoft.Ads.Experimental

#nowarn "9"

module Tests = 
  

  open Xunit
  open Ploeh.AutoFixture
  open System.Runtime.InteropServices

  
  [<StructLayout(LayoutKind.Sequential, Pack=1)>]
  type StringTestStruct =
    struct
      [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=81)>]
      val string80Var:string
      [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=2)>]
      val string1Var:string
      [<MarshalAs(UnmanagedType.ByValTStr, SizeConst=257)>]
      val string256Var:string
    end
    
  
  [<StructLayout(LayoutKind.Sequential, Pack=1)>]
  type ArrayTestStruct =
    struct
      [<MarshalAs(UnmanagedType.ByValArray, SizeConst=21,ArraySubType=UnmanagedType.I1)>]
      val arrboolVar:bool array
      [<MarshalAs(UnmanagedType.ByValArray, SizeConst=1,ArraySubType=UnmanagedType.I1)>]
      val arrbyteVar:byte array
      [<MarshalAs(UnmanagedType.ByValArray, SizeConst=19,ArraySubType=UnmanagedType.I2)>]
      val arrwordVar:uint32 array
    end

  [<StructLayout(LayoutKind.Sequential, Pack=1)>]
  type NestedTestStruct =
    struct
      [<MarshalAs(UnmanagedType.Struct)>]
      val nestedStrings: StringTestStruct
      [<MarshalAs(UnmanagedType.ByValArray, SizeConst=67,ArraySubType=UnmanagedType.Struct)>]
      val nestedStringsArray: StringTestStruct array
      [<MarshalAs(UnmanagedType.Struct)>]
      val nestedArray: ArrayTestStruct
      [<MarshalAs(UnmanagedType.ByValArray, SizeConst=2,ArraySubType=UnmanagedType.Struct)>]
      val nestedArrayOfArray: ArrayTestStruct array
    end
  
  
  [<Fact>]
  let ``Test if strings are no longer than SizeConst`` () = 
    


    let fixture = new Fixture()
    MarshalAsTypeBuilder |> fixture.Customizations.Add

    let sut = fixture.Create<StringTestStruct>()
    
    Assert.True(sut.string1Var.Length <= 1)
    Assert.True(sut.string80Var.Length <= 80)
    Assert.True(sut.string256Var.Length <= 256)

    
    ()
  
  [<Fact>]
  let ``Test if arrays are of equal length as SizeConst`` () = 
    


    let fixture = new Fixture()
    MarshalAsTypeBuilder |> fixture.Customizations.Add

    let sut = fixture.Create<ArrayTestStruct>()
    
    Assert.Equal(21, sut.arrboolVar.Length)
    Assert.Equal(1, sut.arrbyteVar.Length)
    Assert.Equal(19, sut.arrwordVar.Length)

    
    ()
    
  
  [<Fact>]
  let ``Test works for nested structs`` () = 
    


    let fixture = new Fixture()
    MarshalAsTypeBuilder |> fixture.Customizations.Add

    let sut = fixture.Create<NestedTestStruct>()
    
    Assert.True(sut.nestedStrings.string1Var.Length <= 1)
    Assert.True(sut.nestedStrings.string80Var.Length <= 80)
    Assert.True(sut.nestedStrings.string256Var.Length <= 256)
    Assert.Equal(67, sut.nestedStringsArray.Length)

    sut.nestedStringsArray |> Array.iter (fun x->
      Assert.True(x.string1Var.Length <= 1)
      Assert.True(x.string80Var.Length <= 80)
      Assert.True(x.string256Var.Length <= 256)
    )

    
    Assert.Equal(21, sut.nestedArray.arrboolVar.Length)
    Assert.Equal(1, sut.nestedArray.arrbyteVar.Length)
    Assert.Equal(19, sut.nestedArray.arrwordVar.Length)
    Assert.Equal(2, sut.nestedArrayOfArray.Length)

    sut.nestedArrayOfArray |> Array.iter (fun x->
      Assert.Equal(21, x.arrboolVar.Length)
      Assert.Equal(1, x.arrbyteVar.Length)
      Assert.Equal(19, x.arrwordVar.Length)
    )
    
    ()