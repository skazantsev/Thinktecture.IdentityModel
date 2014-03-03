xcopy ..\..\build\thinktecture.identitymodel.owin.dll lib\net45 /y
xcopy ..\..\build\thinktecture.identitymodel.owin.pdb lib\net45 /y

NuGet.exe pack Thinktecture.IdentityModel.Owin.nuspec -exclude *.cmd -OutputDirectory ..\