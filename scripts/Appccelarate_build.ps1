Properties {
  $projectName ="Appccelerate"
  
  $baseDir = Resolve-Path ..
  $binariesDir = "$baseDir\binaries"
  $publishDir = "$baseDir\publish"
  $sourceDir = "$baseDir\source"
  $scriptsDir = "$baseDir\scripts"
  $nugetDir = "$publishDir\NuGet"
  $packagesDir = "$sourceDir\packages"
  
  $slnFile = "$sourceDir\$projectName.sln"
  $nugetSymbolsFile = "$scriptsDir\nuget.symbols.txt"
  $licenseFile = "$sourceDir\License.txt"
    
  $versionFileName = "Version.txt"
  $dependenciesFileName = "Dependencies.txt"
  $assemblyInfoFileName = "VersionInfo.g.cs"

  $xunitRunner = "$packagesDir\xunit.runners.*\tools\xunit.console.clr4.x86.exe"
  $mspecConsole = "$packagesDir\Machine.Specifications.*\tools\mspec-clr4.exe"
  $nugetConsole = "$baseDir\tools\nuget\nuget.exe"
  
  $teamcity = $false
  $publish = $false
  $parallelBuild = $true
  
  $buildConfig = "Release"
  $buildNumber = 0
  $preVersion = "-alpha"
  
  $allVersions = @{}
  
  $projects = CoreProjects
}

FormatTaskName (("-"*70) + [Environment]::NewLine + "[{0}]"  + [Environment]::NewLine + ("-"*70))

Task default –depends Clean, Init, WriteAssemblyInfo, Build, CheckHintPaths, Test, CopyBinaries, ResetAssemblyInfo, Nuget

Task Clean { 

    Get-Childitem $sourceDir -Include bin, obj -Recurse | 
    Where {$_.psIsContainer -eq $true} | 
    Foreach-Object { 
        Write-Host "deleting" $_.fullname
        Remove-Item $_.fullname -Force -Recurse -ErrorAction SilentlyContinue
    }
    
    Write-Host "deleting" $publishDir
    Remove-Item $publishDir -Force -Recurse -ErrorAction SilentlyContinue
    
    Write-Host "deleting" $binariesDir
    Remove-Item $binariesDir -Force -Recurse -ErrorAction SilentlyContinue
}

Task Init -depends Clean {
    New-Item $binariesDir -type directory -force
    New-Item $publishDir -type directory -force
    New-Item $nugetDir -type directory -force
}

Task WriteAssemblyInfo -precondition { return $publish } -depends Clean, Init {
    $assemblyVersionPattern = 'AssemblyVersionAttribute\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$fileVersionPattern = 'AssemblyFileVersionAttribute\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'

    $projects | 
    Foreach-Object { 
		$project = $_.fullname
        $versionFile = "$project\$versionFileName"
        $assemblyInfoFile = "$project\Properties\$assemblyInfoFileName"
           
        $majorMinor = (Get-Content $versionFile).split(".")
        $major = $majorMinor[0]
        $minor = $majorMinor[1]
        $assemblyVersion = 'AssemblyVersionAttribute("' + (VersionNumber $major $minor 0 0 ) + '")'
        $fileVersion = 'AssemblyFileVersionAttribute("' + (VersionNumber $major $minor $buildNumber 0 ) + '")'
        
        Write-Host "updating" $assemblyInfoFile "with Assembly version:" (VersionNumber $major $minor 0 0 ) "Assembly file version:" (VersionNumber $major $minor $buildNumber 0)

        (Get-Content $assemblyInfoFile) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $assemblyInfoFile
		
		#temporary save all version numbers in a map
        $allVersions.Add($_.name, @((VersionNumber $major $minor 0 0), (VersionNumber (1+$major) $minor 0 0), (VersionNumber $major $minor $buildNumber 0 )))
    }
}

Task Build -depends Clean, WriteAssemblyInfo {
    Write-Host "building" $slnFile
    
    if($parallelBuild){
        $parallelBuildParam = "/m"
        $maxCpuCount = [Environment]::GetEnvironmentVariable("MAX_CPU_COUNT","User")
        if($maxCpuCount){
            $parallelBuildParam += ":$maxCpuCount"
        }
    }

    Write-Host "building using msbuild" $slnFile "/p:Configuration=$buildConfig" "/verbosity:minimal" "/fileLogger" "/fileLoggerParameters:LogFile=$baseDir/msbuild.log" $parallelBuildParam

    Exec { msbuild $slnFile "/p:Configuration=$buildConfig" "/verbosity:minimal" "/fileLogger" "/fileLoggerParameters:LogFile=$baseDir/msbuild.log" $parallelBuildParam}
    
}

Task CheckHintPaths -depends Clean, Init, WriteAssemblyInfo, Build {

    $exclude = "System*|Microsoft*"
    $startPatterns = @("..\packages\", ".\Libs\")
    
    $ns = @{ defaultNamespace = "http://schemas.microsoft.com/developer/msbuild/2003" }

    Get-ChildItem $sourceDir -Include "*.csproj" -Recurse | Foreach-Object{
        $projectFile = $_.fullname
        $projectDir = $_.fullname.replace($_.name, "")
        
        Write-Host "checking" $projectFile
        
        try{
            Select-Xml $projectFile -XPath "//defaultNamespace:Reference" -Namespace $ns | 
            Select -ExpandProperty Node | 
            Foreach-Object {
                if($_.Include -match $exclude){
                    return
                }
                
                $assemblyName = $_.Include.split(",")[0]
                $hintPath = $_.HintPath
                
                if(!$hintPath){
                    throw "The HintPath of the `"$assemblyName`" reference is missing."
                }

                $startOk = $startPatterns | Foreach-Object{
                    if($hintPath.StartsWith($_)){
                        return $true
                    }
                }
                
                if(!$startOk){
                    throw "The HintPath of the `"$assemblyName`" reference doesn't match one of the start patterns."
                }
                
                if(!$hintPath.Contains($assemblyName)){
                    throw "The HintPath of the `"$assemblyName`" reference doesn't contain the assembly name."
                }

                if(!(Test-Path($projectDir+$hintPath))){
                    throw "The HintPath of the `"$assemblyName`" reference is incorrect. The referenced file doesn't exist."
                }
            }
        }catch{
            throw "Reference-Error: " + $_
        }

    } 

}

Task Test -depends Clean, Init, Build, CheckHintPaths {
    RunUnitTest
    RunMSpecTest
}

Task CopyBinaries -precondition { return $publish } -depends Clean, Init, WriteAssemblyInfo, Build, CheckHintPaths, Test {

    $projects |  
    Foreach-Object { 
        $project = $_.fullname
        $projectName = $_.name
        $projectBinaries = "$project\bin\$buildConfig\" 
        $dependenciesFile = "$project\$dependenciesFileName"

        Get-Childitem $projectBinaries -Include "$projectName.dll", "$projectName.xml", "$projectName.pdb" -Recurse |
        Foreach-Object {
            $endpath = $_.fullname.Replace($projectBinaries, "").Replace($_.name, "")
            $destination = "$binariesDir\$buildConfig\$projectName\$endpath"
            Write-Host "copying" $_.fullname "to" $destination
			
            if (!(Test-Path -path $destination)){
				New-Item $destination -Type Directory
			}
				
            Copy-Item $_.fullname $destination -force
        }
        
         if(Test-Path $dependenciesFile){
		    Write-Host "copying additional dependencies:"
            (Get-Content $dependenciesFile) | ForEach-Object {
        		Get-Childitem $projectBinaries -Include $_ -Recurse |
                Foreach-Object {
                    $endpath = $_.fullname.Replace($projectBinaries, "").Replace($_.name, "")
                    $destination = "$binariesDir\$buildConfig\$projectName\$endpath"
                    Write-Host "copy" $_.fullname "to" $destination
					
                    if (!(Test-Path -path $destination)){
						New-Item $destination -Type Directory
					}
					
                    Copy-Item $_.fullname $destination -force
                }
        	}
         }
    }

}

Task ResetAssemblyInfo -precondition { return $publish -and !$teamcity } -depends Clean, WriteAssemblyInfo, Build, CheckHintPaths, Test, CopyBinaries {
    $projects | 
    Foreach-Object { 
       $assemblyInfoFile = $_.fullname + "\Properties\$assemblyInfoFileName"
       Write-Host "resetting" $assemblyInfoFile
       exec { cmd /c "git checkout $assemblyInfoFile" }
    }
    
}

Task Nuget -precondition { return $publish } -depends Clean, WriteAssemblyInfo, Build, CheckHintPaths, Test, CopyBinaries {
    
    $projects | 
    Foreach-Object { 
        $project = $_.fullname
        $projectName = $_.name
        $nuspecFile =  "$project\$projectName.nuspec"
        $destination = "$binariesDir\$buildConfig\$projectName"
        $newNuspecFile = "$destination\$projectName.nuspec"
        $versions = $allVersions.Get_Item($projectName)
        $fileVersion = $versions[2].Substring(0,$versions[2].Length-2)+$preVersion
        
        Write-Host "copying and updating" $nuspecFile "v.$fileVersion"
        
        $newNuspecContent = (Get-Content $nuspecFile) | ForEach-Object {$_ -replace ("%"+$projectName+"FileVersion%"), $fileVersion }
        $isBinaryPackage = -not ($newNuspecContent | Select-String "/files" -quiet)
        
        if($isBinaryPackage){
            $symbols = (Get-Content $nugetSymbolsFile) | ForEach-Object {
        		% {$_ -replace "%sourcefolder%", $projectName }
        	}
            
            $newNuspecContent = $newNuspecContent -replace "</package>", ($symbols+"</package>")
        }
        
        $allVersions.Keys | Foreach-Object {
            $newNuspecContent = $newNuspecContent -replace ("%"+$_+"%"), ("[" + $allVersions.Get_Item($_)[0] + "," + $allVersions.Get_Item($_)[1] + ")")
        }
        
        $newNuspecContent | Set-Content $newNuspecFile
        
        Copy-Item $licenseFile $destination -force 
        
        if($isBinaryPackage){
            Exec { & $nugetConsole "pack" $newNuspecFile "-symbols" }
        }else{
            Exec { & $nugetConsole "pack" $newNuspecFile }
        }
    }

    Get-Childitem $scriptsDir -Filter *.nupkg | 
    Foreach-Object{
       Write-Host "moving" $_.fullname "to" $nugetDir
       Move-Item $_.fullname $nugetDir
    }
}

Function RunUnitTest {
    #get newest xunit runner
    $newestRunner = GetNewest($xunitRunner)
    Write-Host "using" $newestRunner
    
    Get-Childitem $sourceDir -Recurse |
    Where{$_.fullname -like "*.Test\bin\$buildConfig\*Test.dll" } |
    Foreach-Object {
        $testFile = $_.fullname
        Write-Host "testing" $testFile 
        exec { cmd /c "$newestRunner $testFile" }
    }
}

Function RunMSpecTest {
    #get newest mspec runner
    $newestConsole = GetNewest($mspecConsole)
    Write-Host "using" $newestConsole
    
    Get-Childitem $sourceDir -Recurse |
    Where{$_.fullname -like "*.Specification\bin\$buildConfig\*Specification.dll" } |
    Foreach-Object {
        $testFile = $_.fullname
        
        if($teamcity){
            $htmlPath = $binariesDir +"\"+ $_.name + ".html"
            $additionalParams = "--html $htmlPath --teamcity"
        }
 	
        Write-Host "testing" $testFile 
        exec { cmd /c "$newestConsole $additionalParams $testFile" }
					
    }
}

Function CoreProjects {
    return Get-Childitem $sourceDir | 
    Where{$_.psIsContainer -eq $true -and 
    $_.name -like "$projectName.*" -and 
    $_.name -notlike "$projectName.*.Test" -and 
    $_.name -notlike "$projectName.*.Specification" -and 
    $_.name -notlike "$projectName.*.Sample" -and 
    $_.name -notlike "$projectName.*.Performance" -and 
    $_.name -notlike "\.*"}
}

Function VersionNumber([string] $n1,[string] $n2,[string] $n3,[string] $n4){
    "$n1.$n2.$n3.$n4"
}

Function GetNewest($path){
    $firstPart = $path.split("*")[0]
    $secondPart = $path.split("*")[1]
    
    $highestVersion = [version] "0.0"
    Get-Item $path | Foreach-Object {
        $currentVersion = [version] $_.fullname.Replace($firstPart, "").Replace($secondPart, "")
        if($currentVersion.CompareTo($highestVersion) -gt 0){
            $highestVersion = $currentVersion
            $newest = $_.fullname
        }
    }
    
    return $newest
}