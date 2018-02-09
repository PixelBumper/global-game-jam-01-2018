#!/bin/sh

# All specific versions can be found here: https://netstorage.unity3d.com/unity/a9f86dcd79df/unity-2017.3.0f3-osx.ini
echo 'Downloading Unity 2017.3.0f3 pkg:'
curl --retry 5 -o Unity.pkg http://download.unity3d.com/download_unity/a9f86dcd79df/MacEditorTargetInstaller/UnitySetup-WebGL-Support-for-Editor-2017.3.0f3.pkg
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Run installer(s)
echo 'Installing Unity.pkg'
sudo installer -verbose -dumplog -package Unity.pkg -target /