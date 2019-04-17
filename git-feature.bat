@echo off
git fetch upstream master
git checkout -b %1 upstream/master
git push -u origin %1

:exit