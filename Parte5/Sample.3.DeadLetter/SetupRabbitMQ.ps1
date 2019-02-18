

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

Write-Host "Create Dead Letter Exchange"
$exchangeType = [RabbitMQ.Client.ExchangeType]::Fanout
$model.ExchangeDeclare("Module3.Sample9.DeadLetterExchange", $exchangeType, $true)

Write-Host "Creating Dead Letter Queue"
$model.QueueDeclare(“Module3.Sample9.DeadLetter”, $true, $false, $false, $null)
$model.QueueBind("Module3.Sample9.DeadLetter", "Module3.Sample9.DeadLetterExchange", "")

Write-Host "Creating Queue"
$args = @{"x-dead-letter-exchange"="Module3.Sample9.DeadLetterExchange";};
$model.QueueDeclare(“Module3.Sample9.Normal”, $true, $false, $false, $args)

Write-Host "Setup complete"