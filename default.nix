{
  pkgs ? import (import ./lon.nix).nixpkgs { },
}:
let
  agile_octopus = pkgs.callPackage ./src/package.nix { };
in
{
  inherit agile_octopus;

  docker = pkgs.dockerTools.buildLayeredImage {
    name = "AgileOctopus";
    tag = "latest";

    config.Cmd = [ (pkgs.lib.getExe agile_octopus) ];
  };
}
