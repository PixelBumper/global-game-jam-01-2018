#! /bin/sh

echo "Attempting tp build"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-logFile $(pwd)/unity.log \
	-executeMethod BuildScript.PerformBuild \
	-quit

rc=$?
echo "Build log"
cat $(pwd)/unity.log

exit $rc