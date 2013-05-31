$nuget = "..\tools\nuget\nuget.exe"
$apikey = read-host 'Api Key:'
& $nuget SetApiKey $apikey 
$packages = get-childitem ..\publish\Nuget | where {$_.extension -eq ".nupkg" -and !$_.Name.EndsWith("symbols.nupkg")}
foreach ($package in $packages)
{
	write-host $package
	$answer = read-host 'Upload package (y/n)?'
	if($answer -eq 'y')
	{
		write-host 'Pushing package' $package 
		& $nuget push $package.FullName
	}
}