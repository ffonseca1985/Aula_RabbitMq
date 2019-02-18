param([String]$RabbitDllPath = "not specified")

$RabbitDllPath = Resolve-Path $RabbitDllPath 
Write-Host "Rabbit DLL Path: " 
Write-Host $RabbitDllPath -foregroundcolor green

set-ExecutionPolicy Unrestricted

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: " 
Write-Host $absoluteRabbitDllPath -foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

Write-Host "Setting up RabbitMQ Connection Factory" -foregroundcolor green
$factory = new-object RabbitMQ.Client.ConnectionFactory
$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“HostName”)
$hostNameProp.SetValue($factory, “localhost”)

$usernameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“UserName”)
$usernameProp.SetValue($factory, “guest”)

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField(“Password”)
$passwordProp.SetValue($factory, “guest”)

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod(“CreateConnection”, [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, “instance,public”, $null, $null, $null)

Write-Host "Setting up RabbitMQ Model" -foregroundcolor green
$model = $connection.CreateModel()


Write-Host "Creating Alternative Exchange" -foregroundcolor green
$exchangeType = [RabbitMQ.Client.ExchangeType]::Fanout
$model.ExchangeDeclare("Module3.Sample10.FailuresExchange", $exchangeType, $true)

Write-Host "Creating Failures Queue" -foregroundcolor green
$model.QueueDeclare(“Module3.Sample10.Failures”, $true, $false, $false, $null)
$model.QueueBind("Module3.Sample10.Failures", "Module3.Sample10.FailuresExchange", "")


Write-Host "Creating Exchange" -foregroundcolor green
$exchangeType = [RabbitMQ.Client.ExchangeType]::Topic
$args = @{"alternate-exchange"="Module3.Sample10.FailuresExchange";};
$model.ExchangeDeclare("Module3.Sample10.Exchange", $exchangeType, $true, $false, $args)

Write-Host "Creating Apples Queue" -foregroundcolor green
$model.QueueDeclare(“Module3.Sample10.Apples”, $true, $false, $false, $null)
$model.QueueBind("Module3.Sample10.Apples", "Module3.Sample10.Exchange", "apples")

Write-Host "Creating Oranges Queue" -foregroundcolor green
$model.QueueDeclare(“Module3.Sample10.Oranges”, $true, $false, $false, $null)
$model.QueueBind("Module3.Sample10.Oranges", "Module3.Sample10.Exchange", "oranges")







Write-Host "Setup complete"