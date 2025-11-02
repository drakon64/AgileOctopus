{
  pkgs ? import (import ./lon.nix).nixpkgs { },
}:
pkgs.mkShellNoCC {
  packages = with pkgs; [
    dotnetCorePackages.sdk_10_0
    lon
    nixfmt-rfc-style
  ];

  passthru.lon = pkgs.mkShellNoCC {
    packages = with pkgs; [ lon ];
  };
}
