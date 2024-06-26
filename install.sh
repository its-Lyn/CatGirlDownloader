#!/bin/env bash

if [ "$1" == "--remove" ]; then
    rm -rfv $HOME/.local/share/CatGirlDownloader
    rm -v $HOME/.local/share/applications/CatGirlDownloader.desktop

    exit 0
fi

read -r -p "This script requires dotnet runtime 8.0 to run. Do you wish to continue? [Y/n] " RESPONSE
case $RESPONSE in
    [nN])
        exit 0
    ;;
esac

dotnet publish -c Release -p:PublishSingleFile=true --self-contained
echo ""

mkdir -pv $HOME/.local/share/applications
mkdir -pv $HOME/.local/share/CatGirlDownloader

cp -v CatGirlDownloader/bin/Release/net8.0/linux-x64/publish/* $HOME/.local/share/CatGirlDownloader
rm -v $HOME/.local/share/CatGirlDownloader/CatGirlDownloader.pdb
cp -v CatGirlDownloader/Assets/icon.ico $HOME/.local/share/CatGirlDownloader

cat << EOF > $HOME/.local/share/applications/CatGirlDownloader.desktop
[Desktop Entry]
Type=Application
Name=Catgirl Downloader
Icon=${HOME}/.local/share/CatGirlDownloader/icon.ico
Exec=${HOME}/.local/share/CatGirlDownloader/CatGirlDownloader
Categories=Utility
Comment=Best place to download kitties!
Terminal=false
EOF