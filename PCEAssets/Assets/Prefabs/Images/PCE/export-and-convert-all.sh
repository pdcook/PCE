#!/bin/bash

for f in `ls $1`
do
    echo "$f"
    cd $f &&
    for i in `ls *.svg`
    do
        ../layers2png.sh $i
    done &&
    cd ..
done
