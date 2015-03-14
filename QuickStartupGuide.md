# Introduction #

I would recommend having some knowledge of how to use apktool and smali/baksmali, but that's not necessary.<br />
After reading this you will be able to create your own deodexed rom with update.zip for e.g. clockwork mod


# Details #

## Create a new project ##

MainMenu -> File -> New Project...
The ProjectWizard dialog appears:
<a href='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/projectwizard.png'><img src='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/projectwizard.png' width='800' /></a>

Choose a project name e.g. I9000\_XWJVH and a project location e.g. C:\Android\Custom Roms.

Click OK button.

This will create an empty project file with the path C:\Android\Custom Roms\I9000\_XWJVH\I9000\_XWJVH.rsproj <br />and a solution file C:\Android\Custom Roms\I9000\_XWJVH\I9000\_XWJVH.rssln
<br /><br />
If you have a baserom i.e. a system folder for/from a device, you can add that path in the Location of base rom textbox.<br />
All files and sub folders in that folder will be copied recursively.

If you choose a baserom you will end up with something like this:
<a href='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/solution_properties.png'><img src='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/solution_properties.png' width='800' /></a>
_Solution properties_
<a href='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/project_properties.png'><img src='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/project_properties.png' width='800' /></a>
_Project properties_

## Select files to be excluded ##
If there is files you don't want to be included in the update.zip there is two ways to exclude them.
First you can load a predefined list with files to exclude from the project.<br />
This file should contain relative paths to files, like this (case insensitive):
  * /system/app/Maps.apk
  * system/app/Maps.apk
  * \system\app\Maps.apk
  * system\app\Maps.apk

The other way is to right click on the file you want to exclude in the Solution Explorer treeview and select menu item Exclude.
If you have excluded a file or created/copied a file outside of CrcStudio you can include them by right click on the file in the Solution Explorer treeview and select menu item Include.

## Deodex rom ##
To deodex the whole project i.e. all apk and jar files, just select MainMenu -> Project -> Deodex All Apk and Jar Files.
You will then get a question about if you want to optimize png files in Apk files.
If you choose to optimize png files, the process will take approximately three times longer.

When a Apk or Jar file is repacked the file structure will be according to the this (if the files exist):
  * META-INF/MANIFEST.MF
  * META-INF/CERT.SF
  * META-INF/CERT.RSA
  * AndroidManifest.xml
  * classes.dex
  * - all other files in alphabetical order
  * resources.arsc

Original META-INF files will be kept if not the Resign apk files checkbox is checked in Project properties.

## Create a template project ##
To simplify the creation of update.zip files, you can choose to create a template project.
A template project is a project with files for the update.zip, to use with several different rom projects.
Some thing like this
<a href='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/template_project.png'><img src='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/template_project.png' width='800' /></a>

## Creating update.zip ##
In Project properties check the checkbox Include in build for all the projects you want to be included in the update.zip.
In Solution properties you can choose build order and if later added files will overwrite files already added.