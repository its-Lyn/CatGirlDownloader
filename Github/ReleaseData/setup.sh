#!/bin/sh

if [ "$1" == "--remove" ]; then
    rm -rfv $HOME/.local/share/CatGirlDownloader
    rm -v $HOME/.local/share/applications/CatGirlDownloader.desktop

    exit 0
fi

mkdir -pv $HOME/.local/share/applications
mkdir -pv $HOME/.local/share/CatGirlDownloader

cp -v ./CatGirlDownloader   $HOME/.local/share/CatGirlDownloader
cp -v ./libSkiaSharp.so     $HOME/.local/share/CatGirlDownloader
cp -v ./libHarfBuzzSharp.so $HOME/.local/share/CatGirlDownloader

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