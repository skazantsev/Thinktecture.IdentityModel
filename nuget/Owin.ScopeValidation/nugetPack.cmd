xcopy ..\..\build\thinktecture.identitymodel.owin.ScopeValidation.* lib\net45 /y
NuGet.exe pack Thinktecture.IdentityModel.Owin.ScopeValidation.nuspec -exclude *.cmd -OutputDirectory ..\