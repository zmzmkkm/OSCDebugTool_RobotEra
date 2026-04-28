# 简历 PDF 源文件

本目录包含基于 HTML/CSS 生成的中文简历：

- `resume.html`：简历正文
- `style.css`：样式文件
- `export-pdf.ps1`：使用 Microsoft Edge 无界面导出 PDF
- `房广重_简历_Unity研发技术主管.pdf`：导出的 PDF 成品

## 重新导出 PDF

在 PowerShell 中执行：

```powershell
Set-Location "E:\Project\OscDataLog\resume"
.\export-pdf.ps1
```

## 设计说明

- 当前版本为 **A4 一页式紧凑排版**。
- 尽量保留原始信息与事实，不改变关键成果口径，仅做结构重组与内容压缩。
- 前置突出：团队管理、机器人遥操作、数字孪生、VR/AR、低延迟通信。
- 通过双栏布局，将核心优势、技术栈、代表成果、核心经历集中在一页内便于 HR / 面试官快速扫描。

