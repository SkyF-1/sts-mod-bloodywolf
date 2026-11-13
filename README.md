本模组为杀戮尖塔2模组，添加了可玩角色血狼破军。
已完成大部分内容，目前主要进行数值调整。现版本的卡牌有重做和移除的可能。

### 教学资源
- 杀戮尖塔2mod制作教程
https://glitchedreme.github.io/SlayTheSpire2ModdingTutorials/index.html

### 双版本构建

在 VS Code 按 `Ctrl+Shift+B` 会运行默认任务 `Build Both Game Versions`：

- 分别编译游戏正式版和 beta
- 使用正式版 API 基线编译共享 Loader
- 为两个版本生成 Loader 内的独立 DLL artifact
- 导出一次共享 PCK
- 组装唯一的 Loader 工坊包
- 将完整 Loader 包部署到本地游戏目录
- 将同一份内容同步到 ModUploader 工作区

构建产物位于 `artifacts/workshop/StsModBloodywolf/`。游戏根据当前版本选择
`lib/` 下的 DLL。无需重新导出 PCK 时，可运行
`Build Loader Package With Existing PCK` 任务；本地测试通过后，可运行
`Upload Workshop` 任务上传。

### 前置要求
本mod使用 [STS2-RitsuLib](https://github.com/BAKAOLC/STS2-RitsuLib) 作为前置开发。游玩前请安装完整的 `STS2-RitsuLib` 运行时目录。当前开发版本对应 RitsuLib `0.6.11`，并同时维护游戏正式版 `0.107.1` 与 beta `0.113.0`。

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
