# Frequently asked questions #
If you have questions ask them at [xdadevelopers forum](http://forum.xda-developers.com/showthread.php?t=1084126) or add a comment to this wiki page.

---

## Will this work for device ??? ##
I suppose so, it uses ApkTool and Smali/Baksmali to do all the modification to framework and apps.<br />
For more information check out the dedicated forum for device ??? on [xdadevelopers](http://forum.xda-developers.com) or any other android forum dedicated to device ???.<br />
I'm not an expert on making roms, I just made this tool to make it easier.
If you want advices regarding rom making, you will find better threads on [xdadevelopers](http://forum.xda-developers.com).<br />
I have made a lot of mistakes creating my own roms but I have never totally bricked it and have always managed to recover it to a working device.<br />
I will not give you any guaranties that crcstudio will work for you.<br />
**Using crcstudio will be at your own risk.**


---

## How do I get this working with Linux? ##
The short answer is: As I'm no Linux expert; I don't know.<br />

The second time I got it working with the following setup:
  * Ubuntu 10.4 (64-bit)
  * Mono 2.10.5 from http://badgerports.org/
  * MonoDevelop, the latest version from http://badgerports.org/
For some reason I needed to install MonoDevelop, as I'm no Linux expert I don't really know why.<br />

This is how I got it working the first time I tried, but I can't tell you why more than that it requires at least Mono 2.10.2 to work.
I used Ubuntu 11.04 (64-bit) and Mono 2.10.2.<br />
I just followed the instructions in this blog post: [Install Mono 2.10.2 and MonoDevelop 2.6 Beta 3 on Ubuntu With a Bash Script](http://www.integratedwebsystems.com/2011/05/install-mono-2-10-2-and-monodevelop-2-6-beta-3-on-ubuntu-or-fedora-with-a-bash-script/)<br />
It might work with a later version than 2.10.2, here is an updated version of the install description: [Install Mono 2.10.6 from Source on Ubuntu with a Bash Script](http://www.integratedwebsystems.com/2011/08/install-mono-2-10-3-on-ubuntu-using-bash-script/)

---

## How to use CrcStudio with command line ##
CrcStudio takes only one command line parameter, a path to a file.
If the file is a solution or a project it will be opened in a new instance of CrcStudio.
When the file is a project CrcStudio will look for a solution containing that project in the same or the parent directory and open the solution, if no solution is found a new unsaved solution will be created.
All other files will be opened in a existing instance of CrcStudio or a new instance if no CrcStudio instance exists.

---

## Can I associate file types with CrcStudio? ##
Yes, .rssln, .rsproj and .apk can be associated with the application.<br />
MainMenu -> Help -> Register File Types

---

## Which information does ApkViewer show ##
<a href='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/apk_viewer_item.png'><img src='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/apk_viewer_item.png' width='800' /></a>

The information is different depending on the file type.<br />
Images are shown for png files
  * The left image is the one include in the apk file.
  * The right image is the one on disk, either new or decoded.
  * Image: label indicates the pixel size of the image _width_ x _height_
Image files may have different size and crc after the apk has been encode, this has to do with optimizations to png files by ApkTool.

---

## Can I compare an apk or jar file with a older version ##
Yes, if you have [WinMerge](http://winmerge.org) installed.<br />
For this to work you need to have projects with the two files you want to compare open in the same in solution.<br />
Download this [file](http://custom-rom-creator-studio.googlecode.com/svn/wiki/attachments/CrcStudio.flt) and save it to the \Filters folder in the folder [WinMerge](http://winmerge.org) is installed in.

The file: [CrcStudio.flt](http://custom-rom-creator-studio.googlecode.com/svn/wiki/attachments/CrcStudio.flt)

Now your set, in the context menu in the Solution Explorer you will have some thing like this if everything is right.
If the file to be compared is not decoded or decompiled this will be done.
<a href='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/file_compare.png'><img src='http://custom-rom-creator-studio.googlecode.com/svn/wiki/img/file_compare.png' width='800' /></a>

---
