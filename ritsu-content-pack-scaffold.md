# Ritsu content-pack scaffold

- Generated from source patterns after removing the BaseLib project dependency.
- This scaffold does not recreate BaseLib APIs; it only proposes RitsuLib registration calls.
- You still need to rewrite BaseLib-based model classes to native game or Ritsu-compatible types before this code can compile.

## Suggested registration skeleton

```csharp
RitsuLibFramework.CreateContentPack("StsModBloodywolf")
    .Character<StsModBloodywolf.Scripts.Char.Bloodywolf>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.AdvancedExperience>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.AxisOutput>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.BackStomp>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Blade>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.BoostBarrage>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.BringItOn>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.ByTheWay>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Chat>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.CloutImpact>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Competition>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.ConclusionFirst>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Controversy>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.CrossCompare>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.CupTheoryStrike>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.DefendWolf>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Discreet>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.DoOrDie>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.EndOfTunnel>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.FakeAccount>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.FakeNews>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.FinalAssault>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Flourishing>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.FoolForm>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.FrameCheck>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.FullAssault>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.GentleGuidance>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.GoodKarma>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.HighAndMighty>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.HoldBack>()
    .Card<TokenCardPool, StsModBloodywolf.Scripts.Cards.IBiteYou>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.IGetYou>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Infiltrate>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Landmine>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.LaughOutLoud>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Liquidate>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.LoveAndHate>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.NewGodArrives>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.NoStreamTonight>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Optimize>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Overrule>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.PlayDumb>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.PlayWithFire>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.PullTable>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Purge>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Rally>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Reciprocity>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.ReputationReverse>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.ResonantWords>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.RetreatAll>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Review>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.SentimentStorm>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.SharpJab>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.ShutUp>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Sing>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Slander>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.SpotLight>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Stick>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.StirUp>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.StrategicSetup>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.StrictlyBetter>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.StrikeWolf>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Tally>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.TestServer>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.TestYou>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.TourDeForce>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Troll>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Unpack>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Unranked>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Unrivaled>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Verdict>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.VerticalStrike>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.Veto>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.WeAreBuddies>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.WellMade>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.WhipYou>()
    .Card<StsModBloodywolf.Scripts.Pools.BloodywolfCardPool, StsModBloodywolf.Scripts.Cards.ZeroFrameDeploy>()
    .Relic<StsModBloodywolf.Scripts.Pools.BloodywolfRelicPool, StsModBloodywolf.Scripts.Relics.MartialSoul>()
    .Relic<StsModBloodywolf.Scripts.Pools.BloodywolfRelicPool, StsModBloodywolf.Scripts.Relics.RabbitHead>()
    .Potion<StsModBloodywolf.Scripts.Pools.BloodywolfPotionPool, StsModBloodywolf.Scripts.Potions.ExposurePotion>()
    .Potion<StsModBloodywolf.Scripts.Pools.BloodywolfPotionPool, StsModBloodywolf.Scripts.Potions.FluidSentimentPotion>()
    .Power<StsModBloodywolf.Scripts.Powers.CloutPower>()
    .Power<StsModBloodywolf.Scripts.Powers.ControversyPower>()
    .Power<StsModBloodywolf.Scripts.Powers.CupLossPower>()
    .Power<StsModBloodywolf.Scripts.Powers.DataPower>()
    .Power<StsModBloodywolf.Scripts.Powers.FakeAccountPower>()
    .Power<StsModBloodywolf.Scripts.Powers.FlourishingPower>()
    .Power<StsModBloodywolf.Scripts.Powers.FoolFormPower>()
    .Power<StsModBloodywolf.Scripts.Powers.FuckingTerriblePower>()
    .Power<StsModBloodywolf.Scripts.Powers.GoodKarmaPower>()
    .Power<StsModBloodywolf.Scripts.Powers.LandminePower>()
    .Power<StsModBloodywolf.Scripts.Powers.OverrulePower>()
    .Power<StsModBloodywolf.Scripts.Powers.RealAccountPower>()
    .Power<StsModBloodywolf.Scripts.Powers.ReciprocityPower>()
    .Power<StsModBloodywolf.Scripts.Powers.SentimentStormPower>()
    .Power<StsModBloodywolf.Scripts.Powers.ShutUpPower>()
    .Power<StsModBloodywolf.Scripts.Powers.StickPower>()
    .Power<StsModBloodywolf.Scripts.Powers.SuperChatPower>()
    .Power<StsModBloodywolf.Scripts.Powers.TemporaryFreeAttackPower>()
    .Power<StsModBloodywolf.Scripts.Powers.TestYouPower>()
    .Power<StsModBloodywolf.Scripts.Powers.UnrankedPower>()
    .Power<StsModBloodywolf.Scripts.Powers.UnrivaledPower>()
    .Apply();

var content = RitsuLibFramework.GetContentRegistry("StsModBloodywolf");
// No custom monster classes were detected.
```

## Detected types

- Characters: 1
- Pool-bound cards: 76
- Pool-bound relics: 2
- Pool-bound potions: 2
- Powers: 21
- Monsters: 0
- Encounters: 0
- Ancients: 0

## Content model inventory

- These classes were detected as still inheriting BaseLib content abstractions or depending on BaseLib pool markers.
- Treat this as the rewrite backlog after dependency conversion and Harmony bootstrap migration.

### Shared Card Pools

- None detected.

### Shared Relic Pools

- None detected.

### Shared Potion Pools

- None detected.

### Characters

- StsModBloodywolf.Scripts.Char.Bloodywolf | base: PlaceholderCharacterModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Char\Bloodywolf.cs

### Cards

- StsModBloodywolf.Scripts.Cards.AdvancedExperience | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AdvancedExperience.cs
- StsModBloodywolf.Scripts.Cards.AxisOutput | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AxisOutput.cs
- StsModBloodywolf.Scripts.Cards.BackStomp | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BackStomp.cs
- StsModBloodywolf.Scripts.Cards.Blade | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Blade.cs
- StsModBloodywolf.Scripts.Cards.BoostBarrage | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BoostBarrage.cs
- StsModBloodywolf.Scripts.Cards.BringItOn | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BringItOn.cs
- StsModBloodywolf.Scripts.Cards.ByTheWay | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ByTheWay.cs
- StsModBloodywolf.Scripts.Cards.Chat | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Chat.cs
- StsModBloodywolf.Scripts.Cards.CloutImpact | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CloutImpact.cs
- StsModBloodywolf.Scripts.Cards.Competition | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Competition.cs
- StsModBloodywolf.Scripts.Cards.ConclusionFirst | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ConclusionFirst.cs
- StsModBloodywolf.Scripts.Cards.Controversy | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Controversy.cs
- StsModBloodywolf.Scripts.Cards.CrossCompare | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CrossCompare.cs
- StsModBloodywolf.Scripts.Cards.CupTheoryStrike | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CupTheoryStrike.cs
- StsModBloodywolf.Scripts.Cards.DefendWolf | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DefendWolf.cs
- StsModBloodywolf.Scripts.Cards.Discreet | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Discreet.cs
- StsModBloodywolf.Scripts.Cards.DoOrDie | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DoOrDie.cs
- StsModBloodywolf.Scripts.Cards.EndOfTunnel | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\EndOfTunnel.cs
- StsModBloodywolf.Scripts.Cards.FakeAccount | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeAccount.cs
- StsModBloodywolf.Scripts.Cards.FakeNews | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeNews.cs
- StsModBloodywolf.Scripts.Cards.FinalAssault | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FinalAssault.cs
- StsModBloodywolf.Scripts.Cards.Flourishing | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Flourishing.cs
- StsModBloodywolf.Scripts.Cards.FoolForm | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FoolForm.cs
- StsModBloodywolf.Scripts.Cards.FrameCheck | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FrameCheck.cs
- StsModBloodywolf.Scripts.Cards.FullAssault | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FullAssault.cs
- StsModBloodywolf.Scripts.Cards.GentleGuidance | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\GentleGuidance.cs
- StsModBloodywolf.Scripts.Cards.GoodKarma | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\GoodKarma.cs
- StsModBloodywolf.Scripts.Cards.HighAndMighty | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\HighAndMighty.cs
- StsModBloodywolf.Scripts.Cards.HoldBack | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\HoldBack.cs
- StsModBloodywolf.Scripts.Cards.IBiteYou | base: CustomCardModel | pool: TokenCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\IBiteYou.cs
- StsModBloodywolf.Scripts.Cards.IGetYou | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\IGetYou.cs
- StsModBloodywolf.Scripts.Cards.Infiltrate | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Infiltrate.cs
- StsModBloodywolf.Scripts.Cards.Landmine | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Landmine.cs
- StsModBloodywolf.Scripts.Cards.LaughOutLoud | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\LaughOutLoud.cs
- StsModBloodywolf.Scripts.Cards.Liquidate | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Liquidate.cs
- StsModBloodywolf.Scripts.Cards.LoveAndHate | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\LoveAndHate.cs
- StsModBloodywolf.Scripts.Cards.NewGodArrives | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\NewGodArrives.cs
- StsModBloodywolf.Scripts.Cards.NoStreamTonight | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\NoStreamTonight.cs
- StsModBloodywolf.Scripts.Cards.Optimize | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Optimize.cs
- StsModBloodywolf.Scripts.Cards.Overrule | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Overrule.cs
- StsModBloodywolf.Scripts.Cards.PlayDumb | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\PlayDumb.cs
- StsModBloodywolf.Scripts.Cards.PlayWithFire | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\PlayWithFire.cs
- StsModBloodywolf.Scripts.Cards.PullTable | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\PullTable.cs
- StsModBloodywolf.Scripts.Cards.Purge | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Purge.cs
- StsModBloodywolf.Scripts.Cards.Rally | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Rally.cs
- StsModBloodywolf.Scripts.Cards.Reciprocity | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Reciprocity.cs
- StsModBloodywolf.Scripts.Cards.ReputationReverse | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ReputaionReverse.cs
- StsModBloodywolf.Scripts.Cards.ResonantWords | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ResonantWords.cs
- StsModBloodywolf.Scripts.Cards.RetreatAll | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\RetreatAll.cs
- StsModBloodywolf.Scripts.Cards.Review | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Review.cs
- StsModBloodywolf.Scripts.Cards.SentimentStorm | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\SentimentStorm.cs
- StsModBloodywolf.Scripts.Cards.SharpJab | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\SharpJab.cs
- StsModBloodywolf.Scripts.Cards.ShutUp | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ShutUp.cs
- StsModBloodywolf.Scripts.Cards.Sing | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Sing.cs
- StsModBloodywolf.Scripts.Cards.Slander | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Slander.cs
- StsModBloodywolf.Scripts.Cards.SpotLight | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\SpotLight.cs
- StsModBloodywolf.Scripts.Cards.Stick | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Stick.cs
- StsModBloodywolf.Scripts.Cards.StirUp | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StirUp.cs
- StsModBloodywolf.Scripts.Cards.StrategicSetup | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StrategicSetup.cs
- StsModBloodywolf.Scripts.Cards.StrictlyBetter | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StrictlyBetter.cs
- StsModBloodywolf.Scripts.Cards.StrikeWolf | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StrikeWolf.cs
- StsModBloodywolf.Scripts.Cards.Tally | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Tally.cs
- StsModBloodywolf.Scripts.Cards.TestServer | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\TestServer.cs
- StsModBloodywolf.Scripts.Cards.TestYou | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\TestYou.cs
- StsModBloodywolf.Scripts.Cards.TourDeForce | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\TourDeForce.cs
- StsModBloodywolf.Scripts.Cards.Troll | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Troll.cs
- StsModBloodywolf.Scripts.Cards.Unpack | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Unpack.cs
- StsModBloodywolf.Scripts.Cards.Unranked | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Unranked.cs
- StsModBloodywolf.Scripts.Cards.Unrivaled | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\unrivaled.cs
- StsModBloodywolf.Scripts.Cards.Verdict | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Verdict.cs
- StsModBloodywolf.Scripts.Cards.VerticalStrike | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\VerticalStrike.cs
- StsModBloodywolf.Scripts.Cards.Veto | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Veto.cs
- StsModBloodywolf.Scripts.Cards.WeAreBuddies | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\WeAreBuddies.cs
- StsModBloodywolf.Scripts.Cards.WellMade | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\WellMade.cs
- StsModBloodywolf.Scripts.Cards.WhipYou | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\WhipYou.cs
- StsModBloodywolf.Scripts.Cards.ZeroFrameDeploy | base: CustomCardModel | pool: BloodywolfCardPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ZeroFrameDeploy.cs

### Relics

- StsModBloodywolf.Scripts.Relics.MartialSoul | base: CustomRelicModel | pool: BloodywolfRelicPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Relics\MartialSoul.cs
- StsModBloodywolf.Scripts.Relics.RabbitHead | base: CustomRelicModel | pool: BloodywolfRelicPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Relics\RabbitHead.cs

### Potions

- StsModBloodywolf.Scripts.Potions.ExposurePotion | base: CustomPotionModel | pool: BloodywolfPotionPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Potions\ExposurePotion.cs
- StsModBloodywolf.Scripts.Potions.FluidSentimentPotion | base: CustomPotionModel | pool: BloodywolfPotionPool | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Potions\FluidSentimentPotion.cs

### Powers

- StsModBloodywolf.Scripts.Powers.CloutPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\CloutPower.cs
- StsModBloodywolf.Scripts.Powers.ControversyPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\ControversyPower.cs
- StsModBloodywolf.Scripts.Powers.CupLossPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\CupLossPower.cs
- StsModBloodywolf.Scripts.Powers.DataPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\DataPower.cs
- StsModBloodywolf.Scripts.Powers.FakeAccountPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FakeAccountPower.cs
- StsModBloodywolf.Scripts.Powers.FlourishingPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FlourishingPower.cs
- StsModBloodywolf.Scripts.Powers.FoolFormPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FoolFormPower.cs
- StsModBloodywolf.Scripts.Powers.FuckingTerriblePower | base: CustomTemporaryStrengthPower | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FuckingTerriblePower.cs
- StsModBloodywolf.Scripts.Powers.GoodKarmaPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\GoodKarmaPower.cs
- StsModBloodywolf.Scripts.Powers.LandminePower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\LandminePower.cs
- StsModBloodywolf.Scripts.Powers.OverrulePower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\OverrulePower.cs
- StsModBloodywolf.Scripts.Powers.RealAccountPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\RealAccountPower.cs
- StsModBloodywolf.Scripts.Powers.ReciprocityPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\ReciprocityPower.cs
- StsModBloodywolf.Scripts.Powers.SentimentStormPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\SentimentStormPower.cs
- StsModBloodywolf.Scripts.Powers.ShutUpPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\ShutUpPower.cs
- StsModBloodywolf.Scripts.Powers.StickPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\StickPower.cs
- StsModBloodywolf.Scripts.Powers.SuperChatPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\SuperChatPower.cs
- StsModBloodywolf.Scripts.Powers.TemporaryFreeAttackPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\TemporaryFreeAttackPower.cs
- StsModBloodywolf.Scripts.Powers.TestYouPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\TestYouPower.cs
- StsModBloodywolf.Scripts.Powers.UnrankedPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\UnrankedPower.cs
- StsModBloodywolf.Scripts.Powers.UnrivaledPower | base: CustomPowerModel | file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\UnrivaledPower.cs

### Monsters

- None detected.

### Encounters

- None detected.

### Ancients

- None detected.

