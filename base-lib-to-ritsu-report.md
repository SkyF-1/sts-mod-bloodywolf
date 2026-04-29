# BaseLib -> RitsuLib migration report

## Scope

- This converter removes the BaseLib project/package dependency and points the project to the STS2.RitsuLib NuGet package.
- It can generate project-local compatibility shims for commonly used BaseLib abstract/config/utils APIs.
- Migrated projects also get STS001/STS003 analyzer suppression because the stock analyzers do not understand the generated BaseLib compatibility surface.

## Dependency target

- Project root: D:\mod\ctf9\tools\sts-mod-bloodywolf-main
- Old library root: D:\mod\ctf9\tools\BaseLib-StS2-master
- RitsuLib package id: STS2.RitsuLib
- RitsuLib package version: 0.0.54
- RitsuLib metadata source: D:\mod\ctf9\tools\STS2-RitsuLib-main
- New manifest id: STS2-RitsuLib
- Project-local Sts2PathDiscovery.props: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Sts2PathDiscovery.props
- Safe C# rewrites enabled: True
- Legacy Harmony bootstrap requested: True
- Patch bootstrap rewrite enabled: True
- Migration support requested: True
- Migration support rewrite enabled: True
- Ritsu scaffold enabled: True
- Legacy Harmony bootstrap path: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyHarmonyPatchBootstrap.g.cs
- Migration support path: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyMigrationSupport.g.cs
- Compatibility support path: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyCompatibility.g.cs

## Changed files

- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyCompatibility.g.cs: generated BaseLib abstract/config compatibility support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyMigrationSupport.g.cs: generated BaseLib utils migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\ritsu-content-pack-scaffold.md: generated Ritsu content-pack scaffold
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\BloodywolfMerchant.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\BloodywolfRestSite.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AdvancedExperience.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AxisOutput.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BackStomp.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Blade.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BoostBarrage.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BringItOn.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ByTheWay.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Chat.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CloutImpact.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Competition.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ConclusionFirst.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Controversy.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CrossCompare.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CupTheoryStrike.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DefendWolf.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Discreet.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DoOrDie.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\EndOfTunnel.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeAccount.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeNews.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FinalAssault.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Flourishing.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FoolForm.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FrameCheck.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FullAssault.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\GentleGuidance.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\GoodKarma.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\HighAndMighty.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\HoldBack.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\IBiteYou.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\IGetYou.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Infiltrate.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Landmine.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\LaughOutLoud.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Liquidate.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\LoveAndHate.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\NewGodArrives.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\NoStreamTonight.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Optimize.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Overrule.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\PlayDumb.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\PlayWithFire.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\PullTable.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Purge.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Rally.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Reciprocity.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Redeploy.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ReputaionReverse.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ResonantWords.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\RetreatAll.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Review.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\SentimentStorm.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\SharpJab.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ShutUp.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Sing.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Slander.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\SpotLight.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Stick.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StirUp.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StrategicSetup.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StrictlyBetter.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\StrikeWolf.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Tally.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\TestServer.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\TestYou.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\TourDeForce.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Troll.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Unpack.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Unranked.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\unrivaled.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Verdict.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\VerticalStrike.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Veto.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\WeAreBuddies.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\WellMade.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\WhipYou.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ZeroFrameDeploy.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Char\Bloodywolf.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Config\BloodywolfModConfig.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\DynamicVars\CloutLossVar.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\DynamicVars\HotTakeVar.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\DynamicVars\RateVar.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Entry.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Pools\BloodywolfCardPool.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Pools\BloodywolfPotionPool.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Pools\BloodywolfRelicPool.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Potions\ExposurePotion.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Potions\FluidSentimentPotion.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\CloutPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\ControversyPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\CupLossPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\CustomTemporayStrengthPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\DataPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FakeAccountPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FlourishingPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FoolFormPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\FuckingTerriblePower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\GoodKarmaPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\LandminePower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\OverrulePower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\RealAccountPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\ReciprocityPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\SentimentStormPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\ShutUpPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\StickPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\SuperChatPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\TemporaryFreeAttackPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\TestYouPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\UnrankedPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\UnrivaledPower.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Relics\MartialSoul.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Relics\RabbitHead.cs: rewrote BaseLib.Utils usage to generated migration support
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Sts2PathDiscovery.props: generated project-local Sts2PathDiscovery.props for NuGet-based RitsuLib builds
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf.csproj: enabled deploy targets after Build when present; enabled sts2 publicizer for generated compatibility shims; ensured project-local Sts2PathDiscovery.props is imported; made hardcoded STS2 path properties overridable; removed incompatible package source exclusions for publicizer; suppressed BaseLib migration analyzer noise; updated project dependency references
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf.json: updated mod manifest dependencies
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_advanced_experience.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_axis_output.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_back_stomp.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_blade.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_boost_barrage.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_bring_it_on.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_by_the_way.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_chat.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_clout_impact.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_competition.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_conclusion_first.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_controversy.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_cross_compare.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_cup_theory_strike.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_defend_wolf.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_discreet.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_do_or_die.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_end_of_tunnel.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_fake_account.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_fake_news.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_final_assault.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_flourishing.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_fool_form.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_frame_check.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_full_assault.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_gentle_guidance.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_good_karma.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_high_and_mighty.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_hold_back.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_i_bite_you.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_i_get_you.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_infiltrate.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_landmine.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_laugh_out_loud.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_liquidate.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_love_and_hate.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_new_god_arrives.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_no_stream_tonight.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_optimize.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_overrule.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_play_dumb.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_play_with_fire.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_pull_table.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_purge.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_rally.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_reciprocity.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_redeploy.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_reputation_reverse.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_resonant_words.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_retreat_all.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_review.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_sentiment_storm.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_sharp_jab.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_shut_up.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_sing.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_slander.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_spot_light.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_stick.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_stir_up.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_strategic_setup.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_strictly_better.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_strike_wolf.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_tally.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_test_server.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_test_you.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_tour_de_force.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_troll.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_unpack.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_unranked.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_unrivaled.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_verdict.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_vertical_strike.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_veto.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_we_are_buddies.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_well_made.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_whip_you.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\cards\sts_mod_bloodywolf_card_zero_frame_deploy.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_clout_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_controversy_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_cup_loss_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_data_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_fake_account_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_flourishing_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_fool_form_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_fucking_terrible_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_good_karma_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_landmine_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_overrule_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_real_account_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_reciprocity_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_sentiment_storm_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_shut_up_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_stick_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_super_chat_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_temporary_free_attack_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_test_you_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_unranked_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\powers\sts_mod_bloodywolf_power_unrivaled_power.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\relics\sts_mod_bloodywolf_relic_martial_soul.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\images\relics\sts_mod_bloodywolf_relic_rabbit_head.png: added asset aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\localization\zhs\cards.json: added canonical localization aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\localization\zhs\characters.json: added canonical localization aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\localization\zhs\potions.json: added canonical localization aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\localization\zhs\powers.json: added canonical localization aliases for Ritsu public entries
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\StsModBloodywolf\localization\zhs\relics.json: added canonical localization aliases for Ritsu public entries

## Generated scaffold

- Wrote Ritsu registration scaffold: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\ritsu-content-pack-scaffold.md
- The scaffold now includes a per-type content model inventory for the remaining BaseLib abstract classes.

## Generated migration support

- Wrote project-local migration support file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyMigrationSupport.g.cs
- Rewritten C# files: 114
- Wrote project-local compatibility support file: D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Generated\BaseLibToRitsu\LegacyCompatibility.g.cs
- Scope: BaseLib.Utils / NodeFactory wrappers, project-local SpireField support, BaseLib.Abstracts / BaseLib.Config compatibility shims, and runtime registration / asset patches.

## Legacy Harmony bootstrap

- Detected legacy Harmony patch classes: 0
- TryPatchAll call sites found before rewrite: 0
- TryPatchAll call sites rewritten: 0
- No directly patchable Harmony classes were detected, so no bootstrap file was generated.
- No legacy Harmony patch classes were detected.

## Manual Harmony patch sites

- No direct harmony.Patch(...) sites were found.

## Remaining BaseLib code references

- Found 3 lines that still reference BaseLib namespaces or types.
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Potions\SuperChatPotion.cs:1 -> // using BaseLib.Abstracts;
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Potions\SuperChatPotion.cs:2 -> // using BaseLib.Utils;
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Powers\TemporaryFreeAttackPower.cs:9 -> using StsModBloodywolf.Scripts.Powers;using BaseLib.Abstracts;

## Migration buckets

### Config and settings

- Recommendation: Rewrite to ModConfig-STS2 or native Ritsu settings. RitsuLib does not expose BaseLib.Config as a drop-in API.
- Match count: 4
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Entry.cs:25 -> ModConfigRegistry.Register("StsModBloodywolf", new BloodywolfModConfig());
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Config\BloodywolfModConfig.cs:6 -> public sealed class BloodywolfModConfig : SimpleModConfig
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Config\BloodywolfModConfig.cs:9 -> [ConfigSection("Audio Settings")]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Config\BloodywolfModConfig.cs:17 -> [ConfigHoverTip(false)]

### Patch bootstrap

- Recommendation: Prefer the generated legacy Harmony bootstrap for generic migration: replace TryPatchAll with CreateClassProcessor(...).Patch() calls per annotated class, then review any manual harmony.Patch(...) sites separately. Full Ritsu ModPatcher migration is still manual.
- No matches found.

### Godot node factories

- Recommendation: Prefer the generated LegacyNodeFactory wrappers for generic migration, or rewrite directly to STS2RitsuLib.Scaffolding.Godot.RitsuGodotNodeFactories.
- No matches found.

### Saved runtime fields

- Recommendation: Prefer the generated project-local SpireField/SavedSpireField helpers for generic migration, or replace them with another save strategy.
- No matches found.

### Content models and pool markers

- Recommendation: Rewrite BaseLib abstract models to vanilla or Ritsu-native model types, then register them through CreateContentPack / ModContentRegistry.
- Match count: 191
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AdvancedExperience.cs:11 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AdvancedExperience.cs:12 -> public sealed class AdvancedExperience : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AxisOutput.cs:12 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\AxisOutput.cs:13 -> public sealed class AxisOutput : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BackStomp.cs:15 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BackStomp.cs:16 -> public sealed class BackStomp : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Blade.cs:14 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Blade.cs:15 -> public sealed class Blade : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BoostBarrage.cs:15 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BoostBarrage.cs:16 -> public sealed class BoostBarrage : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BringItOn.cs:13 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\BringItOn.cs:14 -> public sealed class BringItOn : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ByTheWay.cs:15 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ByTheWay.cs:16 -> public sealed class ByTheWay : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Chat.cs:17 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Chat.cs:18 -> public sealed class Chat : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CloutImpact.cs:14 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CloutImpact.cs:15 -> public sealed class CloutImpact : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Competition.cs:13 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Competition.cs:14 -> public sealed class Competition : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ConclusionFirst.cs:13 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\ConclusionFirst.cs:14 -> public sealed class ConclusionFirst : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Controversy.cs:16 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Controversy.cs:17 -> public sealed class Controversy : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CrossCompare.cs:15 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CrossCompare.cs:16 -> public sealed class CrossCompare : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CupTheoryStrike.cs:16 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\CupTheoryStrike.cs:17 -> public sealed class CupTheoryStrike : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DefendWolf.cs:11 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DefendWolf.cs:12 -> public sealed class DefendWolf : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Discreet.cs:13 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\Discreet.cs:14 -> public sealed class Discreet : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DoOrDie.cs:13 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\DoOrDie.cs:14 -> public sealed class DoOrDie : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\EndOfTunnel.cs:16 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\EndOfTunnel.cs:17 -> public sealed class EndOfTunnel : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeAccount.cs:12 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeAccount.cs:13 -> public sealed class FakeAccount : CustomCardModel
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeNews.cs:13 -> [Pool(typeof(BloodywolfCardPool))]
- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\Scripts\Cards\FakeNews.cs:14 -> public sealed class FakeNews : CustomCardModel
- ... truncated 151 additional matches.

## Remaining documentation and manifest mentions

- D:\mod\ctf9\tools\sts-mod-bloodywolf-main\README.md:9 -> 本mod目前使用https://github.com/Alchyr/BaseLib-StS2 作为前置开发。如果你想要游玩此mod，请务必一同下载此前置，并放在游戏根目录的mods文件夹中。

## How to run

.\tools\Convert-BaseLibToRitsuLib.ps1 -ProjectRoot "D:\mod\ctf9\tools\sts-mod-bloodywolf-main" -Apply -RewriteSafeCode -RewritePatchBootstrap -GenerateMigrationSupport -RewriteMigrationSupportUsings -GenerateRitsuScaffold
