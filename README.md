本模组为杀戮尖塔2模组，添加了可玩角色血狼破军。
已完成大部分内容，目前主要进行数值调整。现版本的卡牌有重做和移除的可能。

### 教学资源
- 杀戮尖塔2mod制作教程
https://glitchedreme.github.io/SlayTheSpire2ModdingTutorials/index.html

### 前置要求
本mod目前使用https://github.com/Alchyr/BaseLib-StS2 作为前置开发。如果你想要游玩此mod，请务必一同下载此前置，并放在游戏根目录的mods文件夹中。

目前在开发中使用的BaseLib版本为v3.1.0。如果你使用更高或者更低版本的BaseLib，可能有发生问题的风险。如果你遇到问题，请报告给我。

## 卡牌音效

卡牌音效会自动扫描，不需要在代码中逐张登记。文件名必须使用卡牌的
`Id.Entry` 小写形式：

```text
StsModBloodywolf/audio/cards/name/stsmodbloodywolf-strike_wolf.ogg
StsModBloodywolf/audio/cards/flavor/stsmodbloodywolf-strike_wolf.ogg
```

`name` 表示卡名语音，`flavor` 表示风味语音。每张卡可以只有其中一种，
也可以两种都有。新增音频后需要在 Godot 中重新导入并导出 PCK。

本mod将来有可能更换前置mod，可能导致更新后无法正常游玩。届时会更新README文档，特此告知。
