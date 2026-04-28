$ErrorActionPreference = 'Stop'

$edgePath = 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe'
$htmlPath = Join-Path $PSScriptRoot 'resume.html'
$pdfPath = Join-Path $PSScriptRoot '房广重_简历_Unity研发技术主管.pdf'

if (-not (Test-Path $edgePath)) {
    throw "未找到 Microsoft Edge：$edgePath"
}

if (-not (Test-Path $htmlPath)) {
    throw "未找到简历 HTML：$htmlPath"
}

$uri = 'file:///' + (($htmlPath -replace '\\', '/'))

& $edgePath `
    --headless=new `
    --disable-gpu `
    --run-all-compositor-stages-before-draw `
    --print-to-pdf-no-header `
    "--print-to-pdf=$pdfPath" `
    $uri | Out-Null

Start-Sleep -Seconds 2

if (-not (Test-Path $pdfPath)) {
    throw "PDF 导出失败：$pdfPath"
}

Write-Host "PDF 已生成：$pdfPath"

