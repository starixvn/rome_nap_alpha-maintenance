if not exist .git\hooks\pre-push copy build\pre-push .git\hooks\

"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe" build\BuildTasks.targets /fl /flp:logfile=BuildTask.log;verbosity=normal

exit
