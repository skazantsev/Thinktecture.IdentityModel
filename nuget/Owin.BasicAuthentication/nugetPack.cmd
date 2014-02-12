xcopy ..\..\build\thinktecture.identitymodel.owin.BasicAuthentication.* lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.Owin.BasicAuthentication.nuspec -exclude *.cmd -OutputDirectory ..\