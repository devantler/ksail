#!/bin/bash

## usage sh Notarize.sh "devemail" "Password1." "group.com.company" "DEVID" "./filename.zip"
## dev_account $1 "devemail"
## dev_Password $2 "Password1."
## GroupID $3 "group.com.company"
## dev_team $4 DEVID
## FileName $5 ./filename.zip

#xcrun altool --notarize-app --primary-bundle-id "$3" --username $1 --password "$2" --asc-provider "$4" --file "$5"
requestUUID=$(xcrun altool --notarize-app --primary-bundle-id "$3" --username "$1" --password "$2" --asc-provider "$4" --file "$5" >&1 | awk '/RequestUUID/ { print $NF; }')

echo "Notarization RequestUUID: $requestUUID"

if [[ $requestUUID == "" ]]; then
  echo "could not upload for notarization"
  exit 1
fi

# wait for status to be not "in progress" any more
request_status="in progress"
while [[ "$request_status" == "in progress" ]]; do
  echo -n "waiting... "
  sleep 10
  request_status=$(xcrun altool --notarization-info "$requestUUID" --username "$1" --password "$2" 2>&1 | awk -F ': ' '/Status:/ { print $2; }')
  echo "$request_status"
done

xcrun altool --notarization-info "$requestUUID" --username "$1" --password "$2"

if [[ $request_status != "success" ]]; then
  echo "## could not notarize $filepath"
  exit 1
fi
