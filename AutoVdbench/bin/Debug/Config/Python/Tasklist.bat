@ECHO OFF
::cd C:\Users\Administrator\Documents\Visual Studio 2015\Projects\AutoVdbench\AutoVdbench\bin\Debug\Config\Python
(FindStr /IC:"cmd" "..\..\New_taskkill.txt" >Nul && (Echo CMD open) || Echo CMD close) > New_taskkill_Result.txt


