$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$htmlPath = Join-Path $root 'index.html'
$pdfPath = Join-Path $root 'resume-output.pdf'
$tempPdfPath = Join-Path $root 'resume-export.pdf'
$edgePath = 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe'

if (-not (Test-Path $edgePath)) {
	throw "未找到 Edge：$edgePath"
}

if (-not (Test-Path $htmlPath)) {
	throw "未找到简历文件：$htmlPath"
}

$htmlUri = [System.Uri]::new($htmlPath).AbsoluteUri

if (Test-Path $tempPdfPath) {
	Remove-Item $tempPdfPath -Force
}

$process = Start-Process -FilePath $edgePath -ArgumentList @(
	'--headless',
	'--disable-gpu',
	"--print-to-pdf=$tempPdfPath",
	'--print-to-pdf-no-header',
	$htmlUri
) -Wait -PassThru -WindowStyle Hidden

if ($process.ExitCode -ne 0) {
	throw "Edge 导出失败，退出码：$($process.ExitCode)"
}

if (-not (Test-Path $tempPdfPath)) {
	throw "PDF 导出失败：$tempPdfPath"
}

if (Test-Path $pdfPath) {
	Remove-Item $pdfPath -Force
}

Move-Item -Path $tempPdfPath -Destination $pdfPath -Force

if (-not (Test-Path $pdfPath)) {
	throw "PDF 导出失败：$pdfPath"
}

Write-Output "PDF 已导出：$pdfPath"

