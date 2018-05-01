Param(
	[parameter(Mandatory=$True)]
	[string]$Org
)

$confirmation = Read-Host "This is a DESTRUCTIVE operation.  Are you sure you want to proceed?(y/n)"
if ($confirmation -ne 'y') {
  Exit
}

& cf target -o $Org
If ($? -ne "True") {Pause; Exit}

& cf spaces | select -Skip 3 | foreach {
	& cf delete-space $_ -f
}