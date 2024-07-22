{ mkShell, dotnetCorePackages, csharp-ls, netcoredbg, csharpier }:
mkShell {
  name = "catgirldownloader-env";
  nativeBuildInputs = [
    dotnetCorePackages.dotnet_8.sdk
    dotnetCorePackages.dotnet_8.runtime
    csharp-ls
    netcoredbg
    csharpier
  ];
}
