本模组为杀戮尖塔2模组，添加了可玩角色血狼破军。
已完成大部分内容，目前主要进行数值调整。现版本的卡牌有重做和移除的可能。

### 教学资源
- 杀戮尖塔2mod制作教程
https://glitchedreme.github.io/SlayTheSpire2ModdingTutorials/index.html

### 前置要求
本mod使用 [STS2-RitsuLib](https://github.com/BAKAOLC/STS2-RitsuLib) 作为前置开发。游玩前请安装完整的 `STS2-RitsuLib` 运行时目录。当前开发版本对应 RitsuLib `0.6.10` 和游戏 `0.111.0`。

迁移到 RitsuLib 后，内容 ID、旧存档和旧版独立音效配置键不再直接兼容。

## 卡牌音效

卡牌音效会自动扫描，不需要在代码中逐张登记。文件名沿用卡牌类名的蛇形写法和
`stsmodbloodywolf-` 前缀：

```text
StsModBloodywolf/audio/cards/name/stsmodbloodywolf-strike_wolf.ogg
StsModBloodywolf/audio/cards/flavor/stsmodbloodywolf-strike_wolf.ogg
```

`name` 表示卡名语音，`flavor` 表示风味语音。每张卡可以只有其中一种，
也可以两种都有。新增音频后需要在 Godot 中重新导入并导出 PCK。

本mod已从 BaseLib 迁移至 RitsuLib。旧版本存档、运行记录和旧音效覆盖设置不保证兼容。
