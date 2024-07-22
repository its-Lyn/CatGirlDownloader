{
  description = "Catgirl Downloader";

  inputs = {
    flake-parts.url = "github:hercules-ci/flake-parts";
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
  };

  outputs = inputs:
    inputs.flake-parts.lib.mkFlake { inherit inputs; } {
      systems = [ "x86_64-linux" ];
      perSystem = { config, self', pkgs, system, ... }: {
	_module.args.pkgs = import inputs.nixpkgs {
	  inherit system;
	  overlays = [
	    inputs.self.overlays.default
	  ];
	};

	packages = {
	  default = pkgs.catgirl-downloader;
	  catgirl-downloader = self'.packages.catgirl-downloader;
	};

	formatter = pkgs.nixpkgs-fmt;
      };
      flake = {
	overlays.default = final: _prev: {
	  catgirl-downloader = final.callPackage ./nix/package.nix { };
	};
      };
    };
}
