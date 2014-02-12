xcopy ..\..\build\thinktecture.identitymodel.owin.requireSsl.* lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.Owin.RequireSsl.nuspec -exclude *.cmd -OutputDirectory ..\