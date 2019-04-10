set executable=ethminer.exe
set commandline= --farm-recheck 200 -U http://127.0.0.1:8080/
set runforseconds=14400
set restartinseconds=2
set /a counter=0

:start
start "Miner Window" %executable% %commandline%
echo:
echo The software will run for %runforseconds% seconds
timeout %runforseconds%
taskkill /f /im %executable%
echo:
echo Restarting the software in %restartinseconds% seconds (%counter%)
timeout %restartinseconds%
set /a counter+=1
echo:
echo:
goto start