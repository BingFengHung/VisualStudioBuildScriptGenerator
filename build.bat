
SET my_path=C:\Program Files (x86)\Microsoft Visual Studio\2019
SET msbuild_path=MSBuild\Current\Bin\MSBuild.exe
SET my_community="%my_path%\Community\%msbuild_path%"
SET my_professional="%my_path%\Professional\%msbuild_path%"
SET my_enterprise="%my_path%\Enterprise\%msbuild_path%"

IF EXIST %my_community% (%my_community% "VisualStudioBuildScriptGenerator.sln" /p:Configuration=Debug /p:Platform="Any CPU"
) ELSE IF EXIST %my_professional% (%my_professional% "VisualStudioBuildScriptGenerator.sln" /p:Configuration=Debug /p:Platform="Any CPU"
) ELSE IF EXIST %my_enterprise% (%my_enterprise% "VisualStudioBuildScriptGenerator.sln" /p:Configuration=Debug /p:Platform="Any CPU"
)

cmd /k
