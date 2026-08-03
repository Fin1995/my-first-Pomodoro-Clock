#  黑猫番茄钟 (Black Cat Pomodoro)

一个小巧的桌面番茄钟应用：选择任务、开始专注、到点休息，让小黑猫陪你管理时间。

基于 C# / WinForms (.NET Framework 4.7.2) 开发，运行于 Windows。

## ✨ 功能特性

- **番茄钟计时**：专注 / 休息双阶段循环，支持自定义时长与循环次数
- **任务管理**：创建、编辑、删除任务，可为每个任务设置专注/休息时长、循环次数与备注
- **到期提醒**：系统托盘图标变化 + 气泡通知 / Toast 弹窗
- **声音提示**：支持系统提示音、自定义音频文件、静音三种模式（NAudio 可选依赖，不可用时自动降级为系统提示音）
- **主题切换**：内置多套主题配色
- **数据本地保存**：任务与设置自动持久化到本地

## 🖥️ 运行环境

- Windows 7 及以上
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)（Windows 10/11 通常已内置）

## 🚀 构建与运行

1. 克隆本仓库
2. 用 **Visual Studio 2019/2022**（或任意支持 C# 的 IDE）打开 `Black Cat Pomodoro Clock.sln`
3. 还原 NuGet 包（`NAudio`）后直接运行

> 不依赖 NAudio 时也完全可用：`AudioService` 会自动探测并降级为系统提示音。

## 📁 项目结构

```
Black Cat Pomodoro Clock.sln   # 解决方案
Cat1/
├── Program.cs                 # 程序入口
├── MainForm.cs                # 主窗体（计时、任务、托盘）
├── PomodoroService.cs         # 番茄钟核心计时逻辑
├── PomodoroTask.cs            # 任务数据模型
├── DataService.cs             # 任务数据持久化
├── ThemeService.cs            # 主题管理
├── AudioService.cs            # 声音播放（NAudio 可选）
├── TaskEditForm.cs            # 任务编辑窗体
├── ToastForm.cs               # Toast 通知窗体
└── Properties/                # 程序集信息与设置
```

## 📜 许可证

本项目采用 [MIT License](LICENSE) 开源。
