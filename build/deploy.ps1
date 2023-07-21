# deploy.ps1

# Load YAML module
Import-Module -Name PowerShell-yaml

# Load deploy settings from YAML file
$deploySettings = Get-YamlData -Path .\deploy_settings.yml

# Set FTP credentials
$ftpServer = $deploySettings.deployment.server
$ftpUsername = $deploySettings.deployment.username
$ftpPassword = $deploySettings.deployment.password

# Remote directory path
$remoteDirectory = $deploySettings.deployment.remote_directory

# Local directory path containing your .NET application files to be uploaded
$localDirectory = "path\to\your\.NET\app"

# Create FTP request object
$ftpRequest = [System.Net.FtpWebRequest]::Create("ftp://$ftpServer/$remoteDirectory/")
$ftpRequest.Method = [System.Net.WebRequestMethods+Ftp]::UploadFile
$ftpRequest.Credentials = New-Object System.Net.NetworkCredential($ftpUsername, $ftpPassword)

# Get the list of files to upload
$fileList = Get-ChildItem -Path $localDirectory -File -Recurse

# Upload each file to the FTP server
foreach ($file in $fileList) {
    $filePath = $file.FullName
    $relativePath = $filePath.Replace($localDirectory, '').TrimStart('\')
    $ftpFilePath = "ftp://$ftpServer/$remoteDirectory/$relativePath"

    $ftpRequest.UploadFile($ftpFilePath, $filePath)
}

Write-Host "Deployment completed!"
