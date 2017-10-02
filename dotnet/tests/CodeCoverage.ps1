$dotnet="C:/Program Files/dotnet/dotnet.exe"
$opencover="$ENV:UserProfile\.nuget\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe"
$reportgenerator="$ENV:UserProfile\.nuget\packages\ReportGenerator\2.5.6\tools\ReportGenerator.exe"
$coveragedir="_coverage"
$filter="+[Chaffinch*]* -[*Unit*]* -[*Integration*]* -[*Functional*]*"  
$coveragefile="coverage.xml"

function CleanUp(){
    if (Test-Path $coveragefile) {
        Remove-Item $coveragefile
    }
}

function RunOpenCover  {
    Param([string]$project)
    Write-Output "Generating coverage for : $project"
    $targetargs="test $project"  

    $command="$opencover -oldStyle -register:user -target:""$dotnet"" -mergeoutput -output:""$coveragefile"" -targetargs:""$targetargs"" -filter:""$filter"" -skipautoprops -hideskipped:All"

    #Write-Output $command
    Invoke-Expression $command
}

function GenerateReport {
    $command="$reportgenerator -targetdir:""$coveragedir"" -reporttypes:""Html;Badges"" -reports:""$coveragefile"" -verbosity:""Error"""
    #Write-Output $command
    Invoke-Expression $command
}

function OpenReport{
    $command="start ""$coveragedir\index.htm"""
    Invoke-Expression $command
}

CleanUp
ForEach ($folder in (Get-ChildItem -Path . -Directory)) {   
    RunOpenCover($folder)
}
GenerateReport
OpenReport