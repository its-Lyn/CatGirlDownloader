{ buildDotnetModule, dotnetCorePackages, fontconfig, xorg, gtk3 }:
buildDotnetModule rec {
  pname = "CatGirlDownloader";
  version = "0.1.0";

  dotnet-sdk = dotnetCorePackages.dotnet_8.sdk;
  dotnet-runtime = dotnetCorePackages.dotnet_8.runtime;

  src = ../.;
  projectFile = "CatGirlDownloader/CatGirlDownloader.csproj";
  nugetDeps = ./deps.nix;
  selfContainedBuild = true;
  runtimeDeps = [
    fontconfig
    xorg.libX11
    xorg.libICE
    xorg.libSM
    gtk3
  ];
}
