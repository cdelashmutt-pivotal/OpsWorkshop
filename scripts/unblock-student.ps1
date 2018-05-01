Param(
	[parameter(ValueFromPipeline=$True)]
	[string[]]$StudentId,
	[parameter(Mandatory=$True)]
	[string]$Org
)
Process {
	$spaceName = "$StudentId-space"
	& cf unbind-security-group block-internal $Org $spaceName
	If ($? -ne "True") {Pause; Exit}
}