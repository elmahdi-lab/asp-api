#!/bin/bash

i=0
while [ $i -le 15 ]
do
	((i++))
	echo "$i WAITING FOR DELAY TO EXPIRE"
	sleep 1
done
  
echo "STARTING ..."

dotnet PomeloHealthApi.dll