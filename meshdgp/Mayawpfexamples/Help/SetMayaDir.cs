//方法三:
//用文字編輯器打開 maya.env 檔.
//maya.env 這個檔案的路徑:
//windows -> drive:\Documents and Settings\username\My Documents\maya\[版本]
//linux -> ~/maya/[版本]
//Mac OS X-> /Users/username/Library/Preferences/Autodesk/maya/[版本]
//在maya.env 裡加上這幾行:

//MAYA_SCRIPT_PATH = [mel檔的之料夾]
//MAYA_PLUG_IN_PATH = [外掛檔的之料夾]
//PYTHONPATH = [python檔的之料夾]
//例如:

//MAYA_SCRIPT_PATH = C:\zebruv\scripts;E:\test_scripts;
//MAYA_PLUG_IN_PATH = C:\zebruv\plugins;
//PYTHONPATH = C:\zebruv\scripts;


//如何在maya裡檢查目前存在的 maya plugin 目錄?
//-> 在maya裡打 getenv "MAYA_PLUG_IN_PATH";

//如何在maya裡檢查目前存在的 maya script 目錄?
//-> 在maya裡打 getenv "MAYA_SCRIPT_PATH";

//如何在maya裡檢查目前存在的 maya icon 目錄?
//-> 在maya裡打 getenv "XBMLANGPATH";



//if not exist "$(SolutionDir)assemblies" mkdir "$(SolutionDir)assemblies"
//copy "$(TargetPath)" "$(SolutionDir)assemblies\$(TargetName).nll.dll"