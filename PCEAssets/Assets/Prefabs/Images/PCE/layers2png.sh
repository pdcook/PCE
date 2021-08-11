#!/bin/bash

readarray array <<< cut -d ' '  -f 3 <<< $(inkscape --actions="select-all:layers;select-list;" $1)

for i in "${array[@]}"
do
  echo $(cut -d ' ' -f 1 <<< $i)
  myLayer=$(cut -d ' ' -f 1 <<< $i)
  $(inkscape --actions="export-id:$myLayer;export-id-only;export-filename:$myLayer.png;export-dpi:500;export-do;" $1)
  echo 'Converting...'
  $(convert "$myLayer.png" -alpha extract -threshold 0 -transparent black "white-$myLayer.png")


done
