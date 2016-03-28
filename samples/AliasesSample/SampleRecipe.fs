namespace NuSoft.Ads.Experimental.Samples
open NuSoft.Ads.Experimental

#nowarn "9"

[<Measure>] type h 
[<Measure>] type ml
[<Measure>] type A

type SampleRecipe =
  struct
    val airCellLeakageLimitLo: DWORD<ml/h>
    val airCellLeakageLimitHi: DWORD<ml/h>
    val standbyCurrentLimitLo: LREAL<A>
    val standbyCurrentLimitHi: LREAL<A>
  end


