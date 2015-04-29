Steps to create a Schedule in Task Scheduler (Windows 7):

1. Open Task Scheduler.
2. Click on Create task link on the right hand side of the window.
3. You will see a dialogue box opened. Enter the name, description and security option information under the General tab.
4. Click on the Triggers tab. Click New to create a trigger. Select the settings for a new Trigger according to your requirements. Once done, click ok.
5. Click on Actions tab. Click New to create an action.
6. A new dialogue will open. For action, select "Start a program option". For Program/Script write "powershell.exe". For Add arguments option, write the following:
   -file "path to your powershell script" eg. "C:\dev\25Live\TaskScheduler.ps1"
7. Click ok once you are done.

Note: You can manually run this scheduled task by selecting the task and clicking on Run on the right hand side of TaskScheduler window.


   