namespace NuSoft.Ads.Tests

#nowarn "9"

open Xunit
open TwinCAT.Ads


[<Collection("Tests agains TwinCAT runtime")>]
module ReadTests = 
  open Ploeh.AutoFixture
  open NuSoft.Ads.Experimental
  open System
  
  [<Fact>]
  let ``Let test connection fail on unexisting client`` () = 
    let client = twincat {
      let! c = {NetId ="123.156.189.12.1.1"; Port=801 }
      return c
    }
    client |> AdsResult.isError |> Assert.True

  [<Fact>]
  let ``Assert connection established to actual TwinCAT runtime`` () = 

    //Set ADS in Run
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    let client = twincat {
      let! c = {NetId="192.168.56.2.1.1";Port=801}
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True
    
    
    
  [<Fact>]
  let ``Assert connection fail when State is Stop to actual TwinCAT runtime`` () = 
    //Set ADS in Stop
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Stop then
      setupClient.WriteControl(new StateInfo(AdsState.Stop, currState.DeviceState))

    let client = twincat {
      let! c = {NetId="192.168.56.2.1.1";Port=801}
      return c
    }
    client |> AdsResult.isError |> Assert.True

    
    
  [<Fact>]
  let ``Try create handle to nonexisting symbolic name`` () = 

    let fixture = new Fixture()
    let expected = fixture.Create<bool>()
    
    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

      let! boolValue = ".nonexistingVar"

      Assert.Equal(expected, boolValue)
      return c
    }
    client |> AdsResult.isError |> Assert.True
  
  module Helpers =

    let writeOnce<'T when 'T : struct> (client:TcAdsClient) symName (value: 'T) =
      let boolHandle = client.CreateVariableHandle symName

      client.WriteAny(boolHandle, value)
      client.DeleteVariableHandle boolHandle
    let writeArrayOnce<'T when 'T : struct> (client:TcAdsClient) symName (value: 'T array) size=
      let boolHandle = client.CreateVariableHandle symName
      client.WriteAny(boolHandle, value)
      client.DeleteVariableHandle boolHandle
    let writeStringOnce (client:TcAdsClient) symName (value: string) size =
      let boolHandle = client.CreateVariableHandle symName
      client.WriteAny(boolHandle, value, [| size |])
      client.DeleteVariableHandle boolHandle
    let writeStringArrayOnce (client:TcAdsClient) symName (value: string array) strSize arrSize=
      let boolHandle = client.CreateVariableHandle symName
      client.WriteAny(boolHandle, value, [| arrSize; strSize |])
      client.DeleteVariableHandle boolHandle
    let writeDate (client:TcAdsClient) symName (value: DateTime)=
      let boolHandle = client.CreateVariableHandle symName

      use stream = new AdsStream(4)
      use writer = new AdsBinaryWriter(stream)

      writer.WritePlcType value
      client.Write(boolHandle, stream)
      client.DeleteVariableHandle boolHandle
    let writeTime (client:TcAdsClient) symName (value: TimeSpan)=
      let boolHandle = client.CreateVariableHandle symName

      use stream = new AdsStream(4)
      use writer = new AdsBinaryWriter(stream)

      writer.WritePlcType value
      client.Write(boolHandle, stream)
      client.DeleteVariableHandle boolHandle
    let writeDateArray (client:TcAdsClient) symName (value: DateTime array) size=
      let boolHandle = client.CreateVariableHandle symName

      use stream = new AdsStream(4*size)
      use writer = new AdsBinaryWriter(stream)

      value |> Array.iter writer.WritePlcType
      client.Write(boolHandle, stream)
      client.DeleteVariableHandle boolHandle
    let writeTimeArray (client:TcAdsClient) symName (value: TimeSpan array) size=
      let boolHandle = client.CreateVariableHandle symName

      use stream = new AdsStream(4*size)
      use writer = new AdsBinaryWriter(stream)

      value |> Array.iter writer.WritePlcType

      client.Write(boolHandle, stream)
      client.DeleteVariableHandle boolHandle
      

  [<Fact>]
  let ``primitive types read`` () = 

    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    let inline write symName  = Helpers.writeOnce setupClient symName 

    
    let boolExp = fixture.Create<BOOL>()
    let byteExp = fixture.Create<BYTE>()
    let wordExp = fixture.Create<WORD>()
    let dwordExp = fixture.Create<DWORD>()
    let sintExp = fixture.Create<SINT>()
    let intExp = fixture.Create<INT>()
    let dintExp = fixture.Create<DINT>()
    let realExp = fixture.Create<REAL>()
    let lrealExp = fixture.Create<LREAL>()

    write  ".boolVar"  boolExp 
    write  ".byteVar"  byteExp 
    write  ".wordVar"  wordExp 
    write  ".dwordVar" dwordExp
    write  ".sintVar"  sintExp 
    write  ".intVar"   intExp 
    write  ".dintVar"  dintExp 
    write  ".realVar"  realExp 
    write  ".lrealVar" lrealExp


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

      let! boolAct = ".boolVar"
      let! byteAct = ".byteVar"
      let! wordAct = ".wordVar"
      let! dwordAct = ".dwordVar"
      let! sintAct = ".sintVar"
      let! intAct = ".intVar"
      let! dintAct = ".dintVar"
      let! realAct = ".realVar"
      let! lrealAct = ".lrealVar"

      Assert.Equal(boolExp, boolAct)
      Assert.Equal(byteExp, byteAct)
      Assert.Equal(wordExp, wordAct)
      Assert.Equal(dwordExp, dwordAct)
      Assert.Equal(sintExp, sintAct)
      Assert.Equal(intExp, intAct)
      Assert.Equal(dintExp, dintAct)
      Assert.Equal(realExp, realAct)
      Assert.Equal(lrealExp, lrealAct)
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True

  [<Fact>]
  let ``arrays read`` () = 


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    let write symName  = Helpers.writeArrayOnce setupClient symName 


    
    let boolExp = fixture.CreateMany<BOOL>(21) |> Array.ofSeq
    let byteExp = fixture.CreateMany<BYTE>(1) |> Array.ofSeq
    let wordExp = fixture.CreateMany<WORD>(19) |> Array.ofSeq
    let dwordExp = fixture.CreateMany<DWORD>(18) |> Array.ofSeq
    let sintExp = fixture.CreateMany<SINT>(17) |> Array.ofSeq
    let usintExp = fixture.CreateMany<USINT>(16) |> Array.ofSeq
    let intExp = fixture.CreateMany<INT>(15) |> Array.ofSeq
    let uintExp = fixture.CreateMany<UINT>(14) |> Array.ofSeq
    let dintExp = fixture.CreateMany<DINT>(count=13) |> Array.ofSeq
    let udintExp = fixture.CreateMany<UDINT>(12) |> Array.ofSeq
    let realExp = fixture.CreateMany<REAL>(11) |> Array.ofSeq
    let lrealExp = fixture.CreateMany<LREAL>(10) |> Array.ofSeq

    write  ".arrboolVar"  boolExp 21
    write  ".arrbyteVar"  byteExp 1
    write  ".arrwordVar"  wordExp 19
    write  ".arrdwordVar" dwordExp 18
    write  ".arrsintVar"  sintExp 17
    write  ".arrusintVar"  usintExp 16
    write  ".arrintVar"   intExp 15
    write  ".arruintVar"   uintExp 14
    write  ".arrdintVar"  dintExp 13
    write  ".arrudintVar"  udintExp 13
    write  ".arrrealVar"  realExp 11
    write  ".arrlrealVar" lrealExp 10


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

      let! boolAct = ".arrboolVar"
      let! byteAct = ".arrbyteVar"
      let! wordAct = ".arrwordVar"
      let! dwordAct = ".arrdwordVar"
      let! sintAct = ".arrsintVar"
      let! usintAct = ".arrusintVar"
      let! intAct = ".arrintVar"
      let! uintAct = ".arruintVar"
      let! dintAct = ".arrdintVar"
      let! udintAct = ".arrudintVar"
      let! realAct = ".arrrealVar"
      let! lrealAct = ".arrlrealVar"

      
      Assert.Equal(boolExp, boolAct |> Seq.ofArray)
      Assert.Equal(byteExp, byteAct|> Seq.ofArray)
      Assert.Equal(wordExp, wordAct|> Seq.ofArray)
      Assert.Equal(dwordExp, dwordAct|> Seq.ofArray)
      Assert.Equal(sintExp, sintAct|> Seq.ofArray)
      Assert.Equal(usintExp, usintAct|> Seq.ofArray)
      Assert.Equal(intExp, intAct|> Seq.ofArray)
      Assert.Equal(uintExp, uintAct|> Seq.ofArray)
      Assert.Equal(dintExp, dintAct|> Seq.ofArray)
      Assert.Equal(udintExp, udintAct|> Seq.ofArray)
      Assert.Equal(realExp, realAct|> Seq.ofArray)
      Assert.Equal(lrealExp, lrealAct|> Seq.ofArray)
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True

    
  [<Fact>]
  let ``strings read`` () = 


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    let write symName  = Helpers.writeStringOnce setupClient symName 
    
    let str80Exp = System.String.Join("",fixture.CreateMany<char>(80))
    let str1Exp = System.String.Join("",fixture.CreateMany<char>(1))
    let str256Exp = System.String.Join("",fixture.CreateMany<char>(256))

    write  ".string80Var"  str80Exp 80
    write  ".string1Var"  str1Exp 1
    write  ".string256Var"  str256Exp 256


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

      let! str80Act = ".string80Var"
      let! str1Act = ".string1Var"
      let! str256Act = ".string256Var"

      
      Assert.Equal(str80Exp, str80Act )
      Assert.Equal(str1Exp, str1Act)
      Assert.Equal(str256Exp, str256Act)
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True

  [<Fact>]
  let ``string arrays read`` () = 


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    let write symName  = Helpers.writeStringArrayOnce setupClient symName 

    let str80Exp = Array.init 9 (fun _ -> System.String.Join("",fixture.CreateMany<char>(80)))
    let str1Exp =  Array.init 8 (fun _ -> System.String.Join("",fixture.CreateMany<char>(1)))
    let str256Exp =  Array.init 7 (fun _ -> System.String.Join("",fixture.CreateMany<char>(256)))

    write  ".arrstring80Var"  str80Exp 9 80
    write  ".arrstring1Var"  str1Exp 8 1
    write  ".arrstring256Var"  str256Exp 7 256


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

      let! str80Act = ".arrstring80Var"
      let! str1Act = ".arrstring1Var"
      let! str256Act = ".arrstring256Var"

      
      Assert.Equal(str80Exp, str80Act |> Seq.ofArray)
      Assert.Equal(str1Exp, str1Act |> Seq.ofArray)
      Assert.Equal(str256Exp, str256Act |> Seq.ofArray)
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True
    
  [<Fact>]
  let ``DT, TOD, T, D read`` () = 
    


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    
    let todVar = DateTime.Now.TimeOfDay
    let dateVar =  fixture.Create<DateTime>().Date
    let dtVar =  fixture.Create<DateTime>()
    let timeVar =  TimeSpan.FromMilliseconds(fixture.Create<float>())
    
    let arrtodVar = Array.init 5 (fun _ -> DateTime.Now.AddDays(fixture.Create<int>() |> float).TimeOfDay)
    let arrdateVar =  fixture.CreateMany<DateTime>(4) |> Seq.map (fun d-> d.Date) |> Array.ofSeq
    let arrdtVar =  fixture.CreateMany<DateTime>(3) |> Array.ofSeq
    let arrtimeVar =  Array.init 6 (fun _ -> TimeSpan.FromMilliseconds(fixture.Create<float>()))
    
    Helpers.writeTime setupClient ".todVar"  todVar
    Helpers.writeDate setupClient ".dateVar"  dateVar
    Helpers.writeDate setupClient ".dtVar"  dtVar
    Helpers.writeTime setupClient ".timeVar"  timeVar
    Helpers.writeTimeArray setupClient ".arrtodVar"  arrtodVar 5
    Helpers.writeDateArray setupClient ".arrdateVar"  arrdateVar 4
    Helpers.writeDateArray setupClient ".arrdtVar"  arrdtVar 3
    Helpers.writeTimeArray setupClient ".arrtimeVar"  arrtimeVar 6


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1"; Port=801}

      let! (todAct: TimeSpan) = ".todVar"
      let! dateAct = ".dateVar"
      let! (dtAct: DateTime) = ".dtVar"
      let! (timeAct: TimeSpan) = ".timeVar"

      
      let! (arrtodAct: TimeSpan array) = ".arrtodVar"
      let! arrdateAct = ".arrdateVar"
      let! (arrdtAct: DateTime array) = ".arrdtVar"
      let! (arrtimeAct: TimeSpan array) = ".arrtimeVar"

      let eps = 1.0//ms
      let todDiff = todVar.TotalMilliseconds - todAct.TotalMilliseconds

      Assert.True(todDiff < 1.0 )
      Assert.Equal(dateVar, dateAct )
      //Assert.Equal(dtVar, dtAct )
      Assert.Equal(timeVar,timeAct)
      

      Array.zip arrtodVar arrtodVar |> Array.iter (fun (exp,act) -> Assert.True(exp.TotalMilliseconds - act.TotalMilliseconds < eps))
      Assert.Equal(arrdateVar, arrdateAct|>Seq.ofArray )
      //Assert.Equal(dtVar, dtAct )
      Assert.Equal(arrtimeVar,arrtimeAct|>Seq.ofArray)
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True


    
  //[<Fact>]
  //let ``Struct read`` () = 
  //
  //
  //  let fixture = new Fixture()
  //  MarshalAsTypeBuilder |> fixture.Customizations.Add
  //  
  //  let setupClient = new TcAdsClient()
  //  setupClient.Connect("192.168.56.2.1.1", 801)
  //  let currState =setupClient.ReadState()
  //  if currState.AdsState <> AdsState.Run then
  //    setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))
  //
  //
  //  let handle = setupClient.CreateVariableHandle ".st1"
  //  
  //  
  //  let s_internal = fixture.Create<BASIC_STRUCT_INTERNAL>()
  //  
  //  let mutable stVat = new BASIC_STRUCT()
  //
  //  Helpers.writeOnce setupClient ".st1" s_internal
  //
  //  
  //
  //  
  //  let client = twincat {
  //  
  //    let! c = {NetId="192.168.56.2.1.1"; Port=801}
  //  
  //    let! (stAct: BASIC_STRUCT) = ".st1"
  //  
  //    //Assert.Equal(stVat,stAct)
  //    return c
  //  }
  //  client |> AdsResult.isSuccess |> Assert.True