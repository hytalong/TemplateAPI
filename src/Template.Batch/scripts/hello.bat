@ECHO OFF 
d:
cd\Robos\ARQ_BTC

echo "Iniciando Exec Appsetting"

powershell -Command "&{ Start-Process powershell -ArgumentList '-File D:\Robos\ARQ_BTC\apply_appsettings.ps1' -Verb RunAs}"
powershell -command "Start-Sleep -s 5"

echo "Iniciando Exec Dotnet"

D:\Robos\ARQ_BTC\Template.Batch.exe Hello


echo "%ERRORLEVEL%"

pause
