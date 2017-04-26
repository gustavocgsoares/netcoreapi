@echo off

REM Get solution path
for %%i in ("%~dp0..") do set "@folder=%%~fi"
for %%i in ("%@folder%\..") do set "@folder=%%~fi"

SET @opencover_version=4.6.519
SET @unit_version=2.2.0
SET @report_version=2.5.6
SET @cobertura_version=0.2.6

SET @dotnet_exe="dotnet.exe" 
SET @opencover_exe="%userprofile%\.nuget\packages\OpenCover\%@opencover_version%\tools\OpenCover.Console.exe"
SET @unit_exe="%userprofile%\.nuget\packages\xunit.runner.console\%@unit_version%\tools\xunit.console.exe"
SET @report_exe="%userprofile%\.nuget\packages\ReportGenerator\%@report_version%\tools\ReportGenerator.exe"
SET @cobertura_exe="%userprofile%\.nuget\packages\OpenCoverToCoberturaConverter\%@cobertura_version%\tools\OpenCoverToCoberturaConverter.exe"

SET @targetargs="test -f netcoreapp1.1 -c Release %@folder%\test\Services.Web.Api.Tests"  
SET @filter="+[*].* -[*.Tests]* -[*]*.RouteConfig -[Castle.Core]* -[Moq]* -[FluentAssertions*]* -[FluentValidation]* -[xunit.*]*" 

SET @output_coverage_folder=%~dp0reports
SET @output_coverage_file=%@output_coverage_folder%\code-coverage-report.xml
SET @output_cobertura_file=%@output_coverage_folder%\code-coverage-cobertura-report.xml

REM Run code coverage analysis  
%@opencover_exe% ^
 -oldStyle ^
 -register:user ^
 -target:%@dotnet_exe% ^
 -output:%@output_coverage_file% ^
 -targetargs:%@targetargs% ^
 -filter:%@filter% ^
 -skipautoprops ^
 -mergeoutput ^
 -searchdirs:%@folder%\test\Services.Web.Api.Tests\bin\Release\netcoreapp1.1 ^
 -hideskipped:All

REM Convert opencover to cobertura report
%@cobertura_exe% ^
 -input:%@output_coverage_file% ^
 -output:%@output_cobertura_file% ^
 -sources:%@output_coverage_folder%
 
REM Generate the report generator 
%@report_exe% ^
 -targetdir:%@output_coverage_folder% ^
 -reporttypes:Html;Badges ^
 -reports:%@output_cobertura_file% ^
 -verbosity:Error

REM Open the report  
start "report" "%@output_coverage_folder%\index.htm"