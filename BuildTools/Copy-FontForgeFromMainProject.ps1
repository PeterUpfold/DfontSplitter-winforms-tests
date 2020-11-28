[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [String]$Path
)

Copy-Item -Path ($Path + "\ExternalTools\fontforge") -Destination "ExternalTools\fontforge" -Recurse