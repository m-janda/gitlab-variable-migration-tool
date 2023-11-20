# GitLab Variable Migration Tool

Exporting projects from GitLab does not transfer over CI/CD variables.
This is a tool that you can use to move CI/CD variables from one GitLab instance to another.

## Usage examples

For simple transfer of all CI/CD variables from one project to another, use the following

```
C:\Project> GitlabVariableMigrator.exe --sourceApiToken "<api token>" --sourceUrl "<gitlab instance url>" --sourceProjectId <project ID> --saveToDisk false --destinationApiToken "<api token>" --destinationUrl "<gitlab instance url>" --destinationProjectId <project ID>
```

If you need to make adjustments to the variables before import, you can first run the tool with the --saveToDisk flag (set to true by default):

```
C:\Project> GitlabVariableMigrator.exe --sourceApiToken "<api token>" --sourceUrl "<gitlab instance url>" --sourceProjectId <project ID>
```

Followed up with --loadFromDisk and --filePath flags for import:

```
C:\Project> GitlabVariableMigrator.exe --destinationApiToken "<api token>" --destinationUrl "<gitlab instance url>" --destinationProjectId <project ID> --loadFromDisk true --filePath "<filePath>" 
```

## Tool options

```
  --sourceApiToken          PRIVATE-TOKEN from the source GitLab instance. Not required when loading from disk.

  --sourceUrl               Source instance base URL. Not required when loading from disk.

  --sourceProjectId         Project ID from the source instance. Not required when loading from disk.

  --destinationApiToken     PRIVATE-TOKEN from the destination GitLab instance. Not required when saving to disk.

  --destinationUrl          Destination instance base URL. Not required when saving to disk.

  --destinationProjectId    Project ID from the destination instance. Not required when saving to disk.

  --saveToDisk              (Default: true) Determines whether the variables are automatically migrated or a JSON file is stored on the disk

  --loadFromDisk            (Default: false) Determines if the local file will be used as a source for importing data to destination

  --filePath                Path to the local JSON file

  --help                    Display this help screen.

  --version                 Display version information.
```