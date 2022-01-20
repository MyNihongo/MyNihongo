$srcDir = Get-Location
Set-Location ../
$rootDir = Get-Location
Set-Location ./proto

$webappDst = [IO.Path]::Combine($rootDir, "webapp", "src", "proto")
Remove-Item -Path "$webappDst\*" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $webappDst -Force

try {
	protoc messages/*.proto --js_out=import_style=commonjs:"$webappDst" --grpc-web_out=import_style=typescript,mode=grpcwebtext:"$webappDst"
	protoc *.proto --js_out=import_style=commonjs:"$webappDst" --grpc-web_out=import_style=typescript,mode=grpcwebtext:"$webappDst"

	# js generates empty messages in the root
	foreach ($item in Get-ChildItem -Path $webappDst -Filter *_pb.*) {
		Remove-Item -Path $item.FullName
		Write-Host "Deleted $($item.FullName)"
	}

	if ($LASTEXITCODE -ne 0) {
		exit -1
	}
} finally {
	Set-Location $srcDir
}
