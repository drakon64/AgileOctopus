{
  pkgs,
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  stdenv,
  ...
}:
let
  fs = lib.fileset;
in
buildDotnetModule {
  pname = "AgileOctopus";
  version = "0.0.1";

  src = fs.toSource {
    root = ./.;

    fileset = fs.difference (./.) (
      fs.unions [
        (lib.fileset.maybeMissing ./bin)
        (lib.fileset.maybeMissing ./config)
        (lib.fileset.maybeMissing ./obj)
        ./package.nix
      ]
    );
  };

  projectFile = "AgileOctopus.csproj";

  dotnet-sdk = dotnetCorePackages.sdk_10_0;
  dotnet-runtime = null;

  executables = [ "AgileOctopus" ];

  selfContainedBuild = true;

  nativeBuildInputs = [ stdenv.cc ];

  meta = {
    license = lib.licenses.eupl12;
    mainProgram = "AgileOctopus";
    maintainers = with lib.maintainers; [ drakon64 ];
  };
}
