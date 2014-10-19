VisualStudioCleaner
===================

Open Source project for cleaning Visual Studio directories and files.

Until documentation and associated downloaded file(s) are ready, VisualStudioCleaner can be used by:

1. Downloading the source.
2. Build the solution in Release mode.
2. Open a command line.
3. Configure the files and directories to be cleaned in the .config file.
4. Execute VSCleaner.exe with the appropriate command line options.

###Examples

To delete unnecessary files and directories, but not remove source control bindings:

```csharp
VSCleaner.exe -d "C:\Solution\Directory"
```

To delete unnecessary files, directories, and remove source control bindings:

```csharp
VSCleaner.exe -d "C:\Solution\Directory" -o All
```

To only remove source control bindings:

```csharp
VSCleaner.exe -d "C:\Solution\Directory" -o RemoveSourceControlBindings
```

The options can be chained as well.

To delete unnecessary files, but not directories:

```csharp
VSCleaner.exe -d "C:\Solution\Directory" -o Default|ExcludeDirectories
```

To delete unnecessary directories, but not files:

```csharp
VSCleaner.exe -d "C:\Solution\Directory" -o Default|ExcludeFiles
```

To perform all actions, but not delete any directories:
```csharp
VSCleaner.exe -d "C:\Solution\Directory" -o All|ExcludeDirectories
```


TODO: create full documentation
===================
