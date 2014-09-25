xcopy ..\..\build\thinktecture.identitymodel.hawk*.* lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.Hawk.nuspec -exclude *.cmd -OutputDirectory ..\