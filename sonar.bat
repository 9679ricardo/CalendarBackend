dotnet sonarscanner begin /k:"Calendar" /d:sonar.login="b23806faa7476fab418c03ed07fb49261bfbec4b" /d:sonar.host.url="https://sonar-test.bit2bittest.com" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build --no-incremental CalendarBackend.sln
dotnet-coverage collect "dotnet test" -f xml  -o "coverage.xml"
dotnet sonarscanner end /d:sonar.login="b23806faa7476fab418c03ed07fb49261bfbec4b"
