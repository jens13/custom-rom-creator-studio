# 1.2.0.0 #
2012-05-14
  * Changed compression stream to use DeflateStream from [DotNetZip](https://dotnetzip.codeplex.com), apk and jar files are now created with better compression ratio.
  * minor bugfixes.

---

# 1.1.0.0 #
2012-04-12
  * fixed [Issue 15](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=15) : Error when path to tool folder includes spaces.
  * fixed [Issue 16](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=16) : Text color in treeview does not change.

---

# 1.0.0.0 #
2012-03-06
  * fixed [Issue 13](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=13) : Bug with temporary files and FileSystemWatcher

---

# 0.9.5.2 #
2011-12-30
  * changed ProjectWizard dialog, when selecting name and location a description will appear to explain which actions will be executed

---

# 0.9.5.1 #
2011-12-14
  * fixed [Issue 12](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=12) : Exception when opening a file in texteditor
  * fixed CrcStudio for mono tries to load shell32.dll (a windows file)

---

# 0.9.5.0 #
2011-12-11
  * fixed [Issue 2](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=2) : Collection was modified; enumeration operation may not execute.
  * fixed [Issue 10](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=10) : Does not handle new option in smali/baksmali v1.3.0
  * fixed [Issue 11](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=11) : Archives containing files with zero bytes length

---

# 0.9.4.3 #
2011-11-13
  * fixed   [Issue 9](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=9)  : Non existing files are not copied when creating new project with a base rom

---

# 0.9.4.2 #
2011-11-09
  * fixed   [Issue 8](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=8)  : Recursion bug when creating a copy from base rom.
  * change: existing files where always replace, now a question will be asked about how to handle it.

---

# 0.9.4.1 #
2011-11-07
  * fixed png optimization menu options
  * hide File Association menu options on non windows OS
  * added function to add solutions and projects to "system recent documents", will affect taskbar jump list in windows 7
  * fixed menu event handlers for registering file types
  * changed Dispose and Finalize handling for some classes.
  * fixed minor bugs

---

# 0.9.4.0 #
2011-06-25
  * refactoring to support mono and linux
  * changed text editor component from ICSharpCode.AvalonEdit to ICSharpCode.TextEditor
  * modified ICSharpCode.TextEditor to work better with linux
  * added processing cancel when 10 consecutive errors occurs
  * added filetypes to only store in apk files
  * changed About dialog
  * fixed decompiling bugs
  * fixed recompiling bugs
  * fixed file disappearing when excluded after processing
  * fixed minor bugs

---

# 0.9.3.1 #
2011-06-14
  * fixed [Issue 6](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=6) : Icons can not be loaded by Windows XP.

---

# 0.9.3.0 #
2011-06-09
  * implemented [Issue 3](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=3) : Add commandline functionality
  * changed icons
  * implemented method to associate filetypes .rssln, .rsproj and .apk with CrcStudio
  * fixed logging for multiple processes
  * added load solution splash dialog
  * minor fixes

---

# 0.9.2.1 #
2011-06-01
  * fixed [Issue 2](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=2), again :)
  * fixed [Issue 5](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=5) : Spaces in path is save as %20 in project and solution files.
  * fixed Can not removing project from solution.
  * fixed minor issues with Solution Explorer refresh.

---

# 0.9.2.0 #
2011-05-29
  * Fixed [Issue 2](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=2) : Collection was modified; enumeration operation may not execute.
  * Added ApkViewer (shows a list of files in the apk file combined with decoded files.)
  * Fixed problems handling save
  * Changed binary files opened in text editor, now opens in external program
  * Fixed wrong order of files in recompiled or encoded apk/jar files
  * Fixed revert to original function
  * Added option for verbose logging from ApkTool
  * Some minor bug fixes

---

# 0.9.1.0 #
2011-05-19
  * Fixed [Issue 1](https://code.google.com/p/custom-rom-creator-studio/issues/detail?id=1) : Error handlig system folder paths when not using English version of windows
  * Fixed SolutionExplorer treeview refresh bugs.
  * Minor bug fixes.