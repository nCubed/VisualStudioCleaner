VisualStudioCleaner
===================

Open Source project for deleting unnecessary Visual Studio files and folders, with an option for removing source control bindings as well.

Until documentation and associated downloaded file(s) are ready, VisualStudioCleaner can be used by:

1. Downloading the source.
2. Build the solution in Release mode.
2. Open a command line.
3. Configure the files and directories to be cleaned in the .config file.
4. Execute VSCleaner.exe with the appropriate command line options.

###Examples

The default is to delete unnecessary files and directories, but not remove source control bindings:

```bat
VSCleaner.exe -d "C:\Solution\Directory"
// Which is equivalent to:
VSCleaner.exe -d "C:\Solution\Directory" -o Default
```

To delete unnecessary files, directories, and remove source control bindings:

```bat
VSCleaner.exe -d "C:\Solution\Directory" -o All
```

To only remove source control bindings:

```bat
VSCleaner.exe -d "C:\Solution\Directory" -o RemoveSourceControlBindings
```

The options can be chained as well.

To delete unnecessary files, but not directories:

```bat
VSCleaner.exe -d "C:\Solution\Directory" -o Default|ExcludeDirectories
```

To delete unnecessary directories, but not files:

```bat
VSCleaner.exe -d "C:\Solution\Directory" -o Default|ExcludeFiles
```

To perform all actions, but not delete any directories:
```bat
VSCleaner.exe -d "C:\Solution\Directory" -o All|ExcludeDirectories
```


TODO: create full documentation
===================
