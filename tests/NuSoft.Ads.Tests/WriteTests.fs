namespace NuSoft.Ads.Tests

#nowarn "9"

open Xunit
open TwinCAT.Ads


[<Collection("Tests agains TwinCAT runtime")>]
module WriteTests = 
  open Ploeh.AutoFixture
  open NuSoft.Ads.Experimental
  open System
  open System.Diagnostics

  [<Fact>]
  let ``primitive types write`` () = 

    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))
    Trace.WriteLine "aaaa"

    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

      let boolExp = fixture.Create<BOOL>()
      let byteExp = fixture.Create<BYTE>()
      let wordExp = fixture.Create<WORD>()
      let dwordExp = fixture.Create<DWORD>()
      let sintExp = fixture.Create<SINT>()
      let intExp = fixture.Create<INT>()
      let dintExp = fixture.Create<DINT>()
      let realExp = fixture.Create<REAL>()
      let lrealExp = fixture.Create<LREAL>()
      
      do! (".boolVar",boolExp)
      do! (".byteVar",byteExp)
      do! (".wordVar",wordExp)
      do! (".dwordVar",dwordExp)
      do! (".sintVar",sintExp)
      do! (".intVar",intExp)
      do! (".dintVar",dintExp)
      do! (".realVar",realExp)
      do! (".lrealVar",lrealExp)

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
  let ``arrays write`` () = 


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}

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

      do!  (".arrboolVar"  ,boolExp  )
      do!  (".arrbyteVar"  ,byteExp )
      do!  (".arrwordVar"  ,wordExp )
      do!  (".arrdwordVar" ,dwordExp)
      do!  (".arrsintVar"  ,sintExp )
      do!  (".arrusintVar" , usintExp)
      do!  (".arrintVar"   ,intExp )
      do!  (".arruintVar"  , uintExp )
      do!  (".arrdintVar"  ,dintExp )
      do!  (".arrudintVar" , udintExp )
      do!  (".arrrealVar"  ,realExp   )
      do!  (".arrlrealVar" ,lrealExp)


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
  let ``strings write`` () = 


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))

    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}
      let str80Exp = System.String.Join("",fixture.CreateMany<char>(80))
      let str1Exp = System.String.Join("",fixture.CreateMany<char>(1))
      let str256Exp = System.String.Join("",fixture.CreateMany<char>(256))

      
      do! (".string80Var" , str80Exp )
      do! (".string1Var"  ,str1Exp )
      do! (".string256Var",  str256Exp )

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
  let ``string arrays write`` () = 


    let fixture = new Fixture()
    //Set ADS in Start
    let setupClient = new TcAdsClient()
    setupClient.Connect("192.168.56.2.1.1", 801)
    let currState =setupClient.ReadState()
    if currState.AdsState <> AdsState.Run then
      setupClient.WriteControl(new StateInfo(AdsState.Run, currState.DeviceState))


    let client = twincat {

      let! c = {NetId="192.168.56.2.1.1";Port=801}
      
      let str80Exp = Array.init 9 (fun _ -> System.String.Join("",fixture.CreateMany<char>(80)))
      let str1Exp =  Array.init 8 (fun _ -> System.String.Join("",fixture.CreateMany<char>(1)))
      let str256Exp =  Array.init 7 (fun _ -> System.String.Join("",fixture.CreateMany<char>(256)))
      
      do! (".arrstring80Var" , str80Exp )
      do! (".arrstring1Var"  ,str1Exp )
      do! (".arrstring256Var",  str256Exp )

      let! str80Act = ".arrstring80Var"
      let! str1Act = ".arrstring1Var"
      let! str256Act = ".arrstring256Var"

      
      Assert.Equal(str80Exp, str80Act |> Seq.ofArray)
      Assert.Equal(str1Exp, str1Act |> Seq.ofArray)
      Assert.Equal(str256Exp, str256Act |> Seq.ofArray)
      return c
    }
    client |> AdsResult.isSuccess |> Assert.True