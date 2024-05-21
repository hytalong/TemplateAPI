@echo off
dotnet sonarscanner begin /k:"template"  /d:sonar.host.url="http://localhost:9999" /d:sonar.login="9cf72a8086418d0b3ec6a88be7492dcfcaaf2665" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.coverage.exclusions="Project.Tests/**,**/*Tests.cs" /d:sonar.cs.opencover.reportsPaths="%cd% ..\coverage\lcov.info"
::dotnet sonarscanner begin /k:"template" /d:sonar.host.url="http://localhost:9999" /d:sonar.login="9cf72a8086418d0b3ec6a88be7492dcfcaaf2665" /d:sonar.verbose=true
::dotnet sonarscanner begin /k:"company:project" /n:"Project" /v:"#.#.#" /o:"company" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="${token}" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.cs.opencover.reportsPaths="${dir}/lcov.opencover.xml"
dotnet restore
dotnet build
::dotnet test Project.Tests/Project.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet sonarscanner end /d:sonar.login="9cf72a8086418d0b3ec6a88be7492dcfcaaf2665"