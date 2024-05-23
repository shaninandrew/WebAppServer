@echo off
chcp 65001
echo Copy files...
copy  %1lib\*.* %2lib\*.*
copy  %1css\*.* %2css\*.*
echo Done!