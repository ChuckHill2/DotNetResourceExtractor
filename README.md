# .Net Resource Extractor
Extracts resources from .Net assemblies.

This utility extracts the embedded resources within one or an entire directory tree of .NET assemblies (exe’s, dll’s, and more). It is a handy way to access the images and other embedded objects. 

This will detect if it is an assembly by its content, not its extension so all non-assemblies are ignored. Duplicate assemblies, are ignored.

Tooltip help is provided for the input text boxes. 

Filename wildcards ('?', '*') are allowed although you will have to manually enter them in the text field. 

In addition, one may extract from a selected set of assemblies (but without wildcards). The filenames must be entered (really pasted) into the textbox delimited by vertical bars '|'. 

So many options for entering the inputs:
* Type it in
* Copy n' paste
* Click 'n drag from Windows Explorer
* Select from popup file dialog.
* Paste in a selected set of vertical bar-delimited names.

#### Details
When wildcards are added to the filename, it performs a recursive search through the entire directory tree.

Wildcards are not allowed when specifying multiple file names delimited by vertical bars.

Resources embedded in resx files lose their extension identity so upon extraction, the extension is detected by its content.

#### Use Cases
If there are multiple files with the same name, extracted resources may also have the same name. If the extracted resource content is identical, it is ignored. If not, it has a version number appended.
 
#### Build
Source may be found at https://github.com/ChuckHill2/DotNetResourceExtractor

This has been built with Microsoft Visual Studio 2019 with .Net Framework 4.8

#### Dependencies
https://github.com/ChuckHill2/ChuckHill2.ExtensionDetector
