

param([String]$RabbitDllPath = "not specified")

$RabbitDllPath = Resolve-Path $RabbitDllPath 
Write-Host "Rabbit DLL Path: " 
Write-Host $RabbitDllPath -foregroundcolor green

set-ExecutionPolicy Unrestricted

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: " 
Write-Host $absoluteRabbitDllPath -foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

Write-Host "Setting up RabbitMQ Connection Factory"
$factory = new-object RabbitMQ.Client.ConnectionFactory
$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“HostName”)
$hostNameProp.SetValue($factory, “localhost”)

$usernameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“UserName”)
$usernameProp.SetValue($factory, “guest”)

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField(“Password”)
$passwordProp.SetValue($factory, “guest”)

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod(“CreateConnection”, [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, “instance,public”, $null, $null, $null)

Write-Host "Setting up RabbitMQ Model"
$model = $connection.CreateModel()

Write-Host "Creating Queue"
$model.QueueDeclare(“Module3.Sample8”, $true, $false, $false, $null)

Write-Host "Setup complete"