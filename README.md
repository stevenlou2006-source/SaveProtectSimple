# Save Protect Simple

轻量存档保护工具。仅提供最核心的存档保护功能，偏精简、稳定。

> 灵感来源：[SaveKeeper by Zichen](https://thunderstore.io/c/repo/p/Zichen/SaveKeeper/)，特别感谢 Zichen 协助测试。

> ⚠️ 本模组与其他存档保护类模组（如 No Save Delete、SaveKeeper）功能重叠，请只安装其中一个，避免补丁冲突导致存档异常。

版本：1.1.4

## 功能

- **死亡不删档**：拦截游戏在死亡/结算时自动删档的操作
- **存档清理模式**：手动开启 `AllowDelete` 后允许正常删档，清理完关闭即恢复保护
- **解除存档上限**：突破原版 10 个存档限制，默认上限 999
- **退出自动备份**：单人退出或多人房主退出时自动创建 `REPO_SAVE_AUTO_` 备份存档

## 与同类模组的区别

Save Protect Simple 偏精简，仅包含最核心的存档保护功能。如需更丰富的功能（F9 手动恢复、死亡匹配模式选择、决斗场跳过邀请界面等），推荐使用 [SaveKeeper](https://thunderstore.io/c/repo/p/Zichen/SaveKeeper/)。

## 安装

1. 安装 [BepInExPack](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
2. 将 `SaveProtectSimple.dll` 放入 `BepInEx/plugins/`
3. 启动游戏即可

## 配置

配置文件路径：`BepInEx/config/cn_xc.SaveProtectSimple.cfg`

| 配置项 | 默认值 | 说明 |
|---|---|---|
| PreventSaveDelete | true | 死亡时不删除存档 |
| AllowDelete | false | 存档清理模式，开启后允许手动删档 |
| RemoveSaveLimit | true | 移除存档数量上限 |
| MaxSaveSlots | 999 | 最大存档数量 |
| EnableAutoBackup | true | 退出房间时自动创建备份存档 |

## 常见问题

**Q：需要每个人都装吗？**
A：只需房主安装。自动备份功能也仅房主/单人触发。

**Q：装了之后死了从第一关开始？**
A：请检查是否同时安装了其他存档保护模组（如 No Save Delete、SaveKeeper）。这些模组修改的是同一批游戏方法，同时安装会导致补丁冲突。只保留一个即可恢复正常。

**Q：自动备份的存档文件在哪？**
A：和普通存档在同一个目录（`saves/`），文件名以 `REPO_SAVE_AUTO_` 开头。

## 致谢

- 特别感谢 **Zichen** 协助测试

## 模块问题交流

QQ群：573485890

---

## 许可证

本项目以 **GNU General Public License v3.0** 发布。

Copyright (C) 2026 cn_xc

完整条款见仓库根目录 [LICENSE](LICENSE)。
