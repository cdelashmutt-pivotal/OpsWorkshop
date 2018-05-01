Param(
	[parameter(ValueFromPipeline=$True)]
	[string[]]$StudentId,
	[parameter(Mandatory=$True)]
	[string]$Org
)
Begin {
	& cf target -o $Org
	If ($? -ne "True") {Pause; Exit}
}
Process {
	$spaceName = "$StudentId-space"
	& cf create-space $spaceName
	If ($? -ne "True") {Pause; Exit}

	& cf set-space-role $StudentId $Org $spaceName SpaceManager
	If ($? -ne "True") {Pause; Exit}
}