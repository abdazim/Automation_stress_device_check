@ECHO OFF
psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\vdbecnhWin_NEW.bat
psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult.bat