namespace TwinCAT.Ads

  type AdsResult<'a> =
    | Success of 'a
    | Error of string

  type Ams = {
    NetId: string
    Port: int
  }

  [<AutoOpen>]
  module AdsResult =
    val isSuccess: AdsResult<'a> -> bool
    val isError: AdsResult<'a> -> bool
    val ofSuccess: AdsResult<'a> -> 'a
  
  [<AutoOpen>]
  module Expressions =
    open System

    [<Class>]
    type TwinCatBuilder =
      member Bind: Ams * (TcAdsClient -> AdsResult<'c>) -> AdsResult<'c>
      member Bind: string * ('T -> AdsResult<TcAdsClient>) -> AdsResult<TcAdsClient>
      member Bind: string * (DateTime -> AdsResult<TcAdsClient>) -> AdsResult<TcAdsClient>
      member Bind: string * (TimeSpan -> AdsResult<TcAdsClient>) -> AdsResult<TcAdsClient>
      member Bind: string * (DateTime array -> AdsResult<TcAdsClient>) -> AdsResult<TcAdsClient>
      member Bind: string * (TimeSpan array -> AdsResult<TcAdsClient>) -> AdsResult<TcAdsClient>
      member Return: 'a -> AdsResult<'a>

    val twincat: TwinCatBuilder
